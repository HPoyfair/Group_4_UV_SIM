using Group_4_UV_SIM;


namespace UVGUI
{
    public partial class Form1 : Form
    {
        private CpuState cpu;
        private Theme theme;
        private MemoryEditor memoryEditor;

        public Form1(CpuState cpuState)
        {
            InitializeComponent();
            cpu = cpuState;
            // Initialize the memory editor with the CPU's memory reference
            memoryEditor = new MemoryEditor(cpu.Memory);

            // Set a default theme
            theme = new Theme
        {
            PrimaryColor = Color.FromArgb(76, 114, 29),
            OffColor = Color.White
        };

            
            cpu.OnOutputMessage = WriteToOutput;

            cpu.OnRequestInput = (prompt) =>
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "UVSim Input", "0");
                int.TryParse(input, out int result);
                return result;
            };

            cpu.StateChanged += HandleCpuStateChanged;
            InitializeMemoryGrid();
            //apply theme here after initializing components
            ApplyTheme(); 
            RefreshMemoryDisplay();
        }

        private void HandleCpuStateChanged()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshMemoryDisplay));
            }
            else
            {
                RefreshMemoryDisplay();
            }
        }
        private void WriteToOutput(string message)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action(() => WriteToOutput(message)));
            }
            else
            {
                if (txtOutput.Lines.Length > 1000)
                {
                    txtOutput.Clear();
                }

                txtOutput.AppendText(message + Environment.NewLine);
                txtOutput.SelectionStart = txtOutput.Text.Length;
                txtOutput.ScrollToCaret();
            }
        }
        private void RefreshMemoryDisplay()
        {
            accData.Text = cpu.Accumulator.ToString("+0000;-0000;0000");
            for (int i = 0; i < 100; i++)
            {
                int row = i % 20;
                int pair = i / 20;
                int col = (pair * 2) + 1;

                string newValue = cpu.Memory[i].ToString("+0000;-0000;0000");
                if (memoryGrid[col, row].Value?.ToString() != newValue)
                {
                    memoryGrid[col, row].Value = newValue;
                }
                if (i == cpu.InstructionPointer)
                {
                    memoryGrid[col, row].Style.BackColor = theme.PrimaryColor;
                    memoryGrid[col - 1, row].Style.BackColor = theme.PrimaryColor;

                    memoryGrid[col, row].Style.ForeColor = Color.White;
                    memoryGrid[col - 1, row].Style.ForeColor = Color.White;
                }
                else
                {
                    memoryGrid[col, row].Style.BackColor = theme.OffColor;
                    memoryGrid[col - 1, row].Style.BackColor = theme.OffColor;

                    memoryGrid[col, row].Style.ForeColor = Color.Black;
                    memoryGrid[col - 1, row].Style.ForeColor = Color.Black;
                }
            }

        }
        private void InitializeMemoryGrid()
        {
            memoryGrid.Columns.Clear();
            memoryGrid.Rows.Clear();

            memoryGrid.AllowUserToAddRows = false;
            memoryGrid.AllowUserToDeleteRows = false;
            memoryGrid.AllowUserToResizeRows = false;
            memoryGrid.AllowUserToResizeColumns = false;

            memoryGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            memoryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            memoryGrid.RowTemplate.Height = 22;

            memoryGrid.ColumnCount = 10;
            memoryGrid.RowCount = 20;

            memoryGrid.RowHeadersVisible = false;
            memoryGrid.ColumnHeadersVisible = false;

            memoryGrid.DefaultCellStyle.Font = new Font("Consolas", 10);

            for (int col = 0; col < 10; col++)
            {
                memoryGrid.Columns[col].Width = 60;

                if (col % 2 == 0)
                {
                    memoryGrid.Columns[col].ReadOnly = true;
                    memoryGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    memoryGrid.Columns[col].Width = 25;
                }
                else
                {
                    memoryGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
            }

            for (int row = 0; row < 20; row++)
            {
                for (int pair = 0; pair < 5; pair++)
                {
                    int address = pair * 20 + row;
                    int col = pair * 2;

                    memoryGrid[col, row].Value = address.ToString("D2");

                    memoryGrid[col + 1, row].Value = "0000";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

            private void btnSave_Click(object sender, EventArgs e)
    {
        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Filter = "Text Files|*.txt|All Files|*.*";
            sfd.Title = "Save UVSim Program";
            sfd.FileName = "program.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    SaveFileService.SaveMemoryToFile(cpu.Memory, sfd.FileName);

                    MessageBox.Show(
                        $"Program saved to:\n{sfd.FileName}",
                        "Save Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Save failed: {ex.Message}",
                        "Save Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }
    }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (cpu.Halted) return;

            btnRun.Enabled = false;
            

            Simulator sim = new Simulator(cpu);

            await Task.Run(() =>
            {
                while (!cpu.Halted)
                {
                    sim.ExecuteNext();
                }
            });

            btnRun.Enabled = true;
            
            MessageBox.Show("Program Halted.");
        }

        private void btnLoad_Click(object sender, EventArgs e)
{
    using (OpenFileDialog ofd = new OpenFileDialog())
    {
        ofd.Filter = "Text Files|*.txt|All Files|*.*";

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                Simulator sim = new Simulator(this.cpu);

                Array.Clear(cpu.Memory, 0, cpu.Memory.Length);

                sim.ReadFile(ofd.FileName);

                cpu.InstructionPointer = 0;
                cpu.Halted = false;

                RefreshMemoryDisplay();

                MessageBox.Show($"Loaded program: {Path.GetFileName(ofd.FileName)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load failed: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void memoryGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        // ================== THEME METHODS ==================
        private void ApplyTheme()
    {
        panel1.BackColor = theme.PrimaryColor;
        panel2.BackColor = theme.OffColor;

        btnRun.BackColor = theme.PrimaryColor;
        btnRun.ForeColor = Color.White;
        btnRun.UseVisualStyleBackColor = false;

        btnSave.BackColor = theme.PrimaryColor;
        btnSave.ForeColor = Color.White;
        btnSave.UseVisualStyleBackColor = false;

        btnLoad.BackColor = theme.PrimaryColor;
        btnLoad.ForeColor = Color.White;
        btnLoad.UseVisualStyleBackColor = false;

        txtOutput.BackColor = Color.White;
        txtOutput.ForeColor = Color.Black;

        accData.BackColor = Color.White;
        accData.ForeColor = Color.Black;

        ACCLabel.ForeColor = Color.Black;

        memoryGrid.BackgroundColor = theme.OffColor;
        memoryGrid.GridColor = theme.PrimaryColor;

        memoryGrid.DefaultCellStyle.SelectionBackColor = theme.PrimaryColor;
        memoryGrid.DefaultCellStyle.SelectionForeColor = Color.White;

        btnInsert.BackColor = theme.PrimaryColor;
        btnInsert.ForeColor = Color.White;
        btnInsert.UseVisualStyleBackColor = false;

        btnDelete.BackColor = theme.PrimaryColor;
        btnDelete.ForeColor = Color.White;
        btnDelete.UseVisualStyleBackColor = false;

        btnCopy.BackColor = theme.PrimaryColor;
        btnCopy.ForeColor = Color.White;
        btnCopy.UseVisualStyleBackColor = false;

        btnCut.BackColor = theme.PrimaryColor;
        btnCut.ForeColor = Color.White;
        btnCut.UseVisualStyleBackColor = false;

        btnPaste.BackColor = theme.PrimaryColor;
        btnPaste.ForeColor = Color.White;
        btnPaste.UseVisualStyleBackColor = false;

        RefreshMemoryDisplay();
    }
// ================== MEMORY EDITING METHODS ==================
    private int GetSelectedMemoryAddress()
{
    if (memoryGrid.CurrentCell == null)
        return -1;

    int col = memoryGrid.CurrentCell.ColumnIndex;
    int row = memoryGrid.CurrentCell.RowIndex;

    if (col % 2 == 0)
        return -1;

    int pair = col / 2;
    int address = pair * 20 + row;

    return address;
}

private void memoryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
{
    if (e.ColumnIndex % 2 == 0) return;

    int pair = e.ColumnIndex / 2;
    int address = pair * 20 + e.RowIndex;

    string raw = memoryGrid[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim() ?? "";

    if (memoryEditor.TryUpdateValue(address, raw, out string errorMessage))
    {
        memoryGrid[e.ColumnIndex, e.RowIndex].Value = MemoryEditor.FormatValue(cpu.Memory[address]);
        RefreshMemoryDisplay();
    }
    else
    {
        MessageBox.Show(errorMessage, "Invalid Instruction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        memoryGrid[e.ColumnIndex, e.RowIndex].Value = MemoryEditor.FormatValue(cpu.Memory[address]);
    }
}



// insert handler
private void btnInsert_Click(object sender, EventArgs e)
{
    int address = GetSelectedMemoryAddress();
    if (address < 0)
    {
        MessageBox.Show("Select a memory value cell first.");
        return;
    }

    if (!memoryEditor.TryInsertValue(address, 0, out string errorMessage))
    {
        MessageBox.Show(errorMessage, "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    RefreshMemoryDisplay();
}

// delete handler
private void btnDelete_Click(object sender, EventArgs e)
{
    int address = GetSelectedMemoryAddress();
    if (address < 0)
    {
        MessageBox.Show("Select a memory value cell first.");
        return;
    }

    if (!memoryEditor.TryDeleteValue(address, out string errorMessage))
    {
        MessageBox.Show(errorMessage, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    RefreshMemoryDisplay();
}
// copy handler
private void btnCopy_Click(object sender, EventArgs e)
{
    int address = GetSelectedMemoryAddress();
    if (address < 0)
    {
        MessageBox.Show("Select a memory value cell first.");
        return;
    }

    if (!memoryEditor.TryCopy(address, 1, out string copiedText, out string errorMessage))
    {
        MessageBox.Show(errorMessage, "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    Clipboard.SetText(copiedText);
}
//cut handler
private void btnCut_Click(object sender, EventArgs e)
{
    int address = GetSelectedMemoryAddress();
    if (address < 0)
    {
        MessageBox.Show("Select a memory value cell first.");
        return;
    }

    if (!memoryEditor.TryCut(address, 1, out string cutText, out string errorMessage))
    {
        MessageBox.Show(errorMessage, "Cut Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    Clipboard.SetText(cutText);
    RefreshMemoryDisplay();
}
//paste handler
private void btnPaste_Click(object sender, EventArgs e)
{
    int address = GetSelectedMemoryAddress();
    if (address < 0)
    {
        MessageBox.Show("Select a memory value cell first.");
        return;
    }

    if (!Clipboard.ContainsText())
    {
        MessageBox.Show("Clipboard does not contain text.");
        return;
    }

    string clipboardText = Clipboard.GetText();

    if (!memoryEditor.TryPaste(address, clipboardText, out string errorMessage))
    {
        MessageBox.Show(errorMessage, "Paste Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    RefreshMemoryDisplay();
}
    }
}
