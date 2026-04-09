using Group_4_UV_SIM;


namespace UVGUI
{
    public partial class Form1 : Form
    {
        private CpuState cpu;
        private Theme theme;
        private MemoryEditor memoryEditor;
        private static readonly string _themeConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.txt");

        private int GetVisibleMemorySize()
{
    return FormatRules.GetMaxMemorySize(cpu.Format);
}

private int GetGridRowCount()
{
    return cpu.Format == ProgramFormat.Legacy4Digit ? 20 : 50;
}

private string FormatAddress(int address)
{
    return cpu.Format == ProgramFormat.Legacy4Digit
        ? address.ToString("D2")
        : address.ToString("D3");
}

private string FormatWord(int value)
{
    return cpu.Format == ProgramFormat.Legacy4Digit
        ? value.ToString("+0000;-0000;0000")
        : value.ToString("+000000;-000000;000000");
}

        public Form1(CpuState cpuState)
{
    try
    {
        InitializeComponent();
        cpu = cpuState;

        memoryEditor = new MemoryEditor(cpu);

        theme = ThemeService.LoadTheme(_themeConfigPath);

        cpu.OnOutputMessage = WriteToOutput;

        cpu.OnRequestInput = (prompt) =>
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "UVSim Input", "0");
            int.TryParse(input, out int result);
            return result;
        };

        cpu.StateChanged += HandleCpuStateChanged;

        InitializeMemoryGrid();
        ApplyTheme();
        RefreshMemoryDisplay();
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            $"Form startup failed:\n\n{ex}",
            "Startup Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
        throw;
    }
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
        //REFRESH MEMORY DISPLAY
        private void RefreshMemoryDisplay()
{
    int visibleMemory = GetVisibleMemorySize();
    int rowsPerPair = GetGridRowCount();

    accData.Text = FormatWord(cpu.Accumulator);

    for (int row = 0; row < memoryGrid.RowCount; row++)
    {
        for (int pair = 0; pair < 5; pair++)
        {
            int address = pair * rowsPerPair + row;
            int addressCol = pair * 2;
            int valueCol = addressCol + 1;

            if (address >= visibleMemory)
            {
                memoryGrid[addressCol, row].Value = "";
                memoryGrid[valueCol, row].Value = "";

                memoryGrid[addressCol, row].Style.BackColor = theme.OffColor;
                memoryGrid[valueCol, row].Style.BackColor = theme.OffColor;

                memoryGrid[addressCol, row].Style.ForeColor = GetContrastColor(theme.OffColor);
                memoryGrid[valueCol, row].Style.ForeColor = GetContrastColor(theme.OffColor);

                memoryGrid[valueCol, row].ReadOnly = true;
                continue;
            }

            memoryGrid[addressCol, row].Value = FormatAddress(address);

            string newValue = FormatWord(cpu.Memory[address]);
            if (memoryGrid[valueCol, row].Value?.ToString() != newValue)
            {
                memoryGrid[valueCol, row].Value = newValue;
            }

            memoryGrid[valueCol, row].ReadOnly = false;

            if (address == cpu.InstructionPointer)
            {
                Color highlightText = GetContrastColor(theme.PrimaryColor);

                memoryGrid[valueCol, row].Style.BackColor = theme.PrimaryColor;
                memoryGrid[addressCol, row].Style.BackColor = theme.PrimaryColor;

                memoryGrid[valueCol, row].Style.ForeColor = highlightText;
                memoryGrid[addressCol, row].Style.ForeColor = highlightText;
            }
            else
            {
                Color cellText = GetContrastColor(theme.OffColor);

                memoryGrid[valueCol, row].Style.BackColor = theme.OffColor;
                memoryGrid[addressCol, row].Style.BackColor = theme.OffColor;

                memoryGrid[valueCol, row].Style.ForeColor = cellText;
                memoryGrid[addressCol, row].Style.ForeColor = cellText;
            }
        }
    }
}

        // INIT MEMORY GRID
        private void InitializeMemoryGrid()
{
    int rowsPerPair = GetGridRowCount();

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
    memoryGrid.RowCount = rowsPerPair;

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
            memoryGrid.Columns[col].Width = cpu.Format == ProgramFormat.Legacy4Digit ? 25 : 35;
        }
        else
        {
            memoryGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            memoryGrid.Columns[col].Width = cpu.Format == ProgramFormat.Legacy4Digit ? 60 : 80;
        }
    }

    for (int row = 0; row < rowsPerPair; row++)
    {
        for (int pair = 0; pair < 5; pair++)
        {
            int address = pair * rowsPerPair + row;
            int col = pair * 2;

            if (address < GetVisibleMemorySize())
            {
                memoryGrid[col, row].Value = FormatAddress(address);
                memoryGrid[col + 1, row].Value = FormatWord(0);
            }
            else
            {
                memoryGrid[col, row].Value = "";
                memoryGrid[col + 1, row].Value = "";
                memoryGrid[col + 1, row].ReadOnly = true;
            }
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


                memoryEditor = new MemoryEditor(cpu);
                InitializeMemoryGrid();
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
        private static Color GetContrastColor(Color background)
        {
            // Perceived brightness formula — returns black or white for maximum readability
            double brightness = (background.R * 299 + background.G * 587 + background.B * 114) / 1000.0;
            return brightness > 128 ? Color.Black : Color.White;
        }

        private void ApplyTheme()
    {
        panel1.BackColor = theme.PrimaryColor;
        panel2.BackColor = theme.OffColor;

        Color buttonText = GetContrastColor(theme.PrimaryColor);

        btnRun.BackColor = theme.PrimaryColor;
        btnRun.ForeColor = buttonText;
        btnRun.UseVisualStyleBackColor = false;

        btnSave.BackColor = theme.PrimaryColor;
        btnSave.ForeColor = buttonText;
        btnSave.UseVisualStyleBackColor = false;

        btnLoad.BackColor = theme.PrimaryColor;
        btnLoad.ForeColor = buttonText;
        btnLoad.UseVisualStyleBackColor = false;

        txtOutput.BackColor = Color.White;
        txtOutput.ForeColor = Color.Black;

        accData.BackColor = Color.White;
        accData.ForeColor = Color.Black;

        ACCLabel.ForeColor = Color.Black;

        memoryGrid.BackgroundColor = theme.OffColor;
        memoryGrid.GridColor = theme.PrimaryColor;

        memoryGrid.DefaultCellStyle.SelectionBackColor = theme.PrimaryColor;
        memoryGrid.DefaultCellStyle.SelectionForeColor = buttonText;

        btnInsert.BackColor = theme.PrimaryColor;
        btnInsert.ForeColor = buttonText;
        btnInsert.UseVisualStyleBackColor = false;

        btnDelete.BackColor = theme.PrimaryColor;
        btnDelete.ForeColor = buttonText;
        btnDelete.UseVisualStyleBackColor = false;

        btnCopy.BackColor = theme.PrimaryColor;
        btnCopy.ForeColor = buttonText;
        btnCopy.UseVisualStyleBackColor = false;

        btnCut.BackColor = theme.PrimaryColor;
        btnCut.ForeColor = buttonText;
        btnCut.UseVisualStyleBackColor = false;

        btnPaste.BackColor = theme.PrimaryColor;
        btnPaste.ForeColor = buttonText;
        btnPaste.UseVisualStyleBackColor = false;

        btnChangeTheme.BackColor = theme.PrimaryColor;
        btnChangeTheme.ForeColor = buttonText;
        btnChangeTheme.UseVisualStyleBackColor = false;

        btnResetTheme.BackColor = theme.PrimaryColor;
        btnResetTheme.ForeColor = buttonText;
        btnResetTheme.UseVisualStyleBackColor = false;

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

    int rowsPerPair = GetGridRowCount();
    int pair = col / 2;
    int address = pair * rowsPerPair + row;

    if (address >= GetVisibleMemorySize())
        return -1;

    return address;
}

private void memoryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
{
    if (e.ColumnIndex % 2 == 0) return;

    int rowsPerPair = GetGridRowCount();
    int pair = e.ColumnIndex / 2;
    int address = pair * rowsPerPair + e.RowIndex;

    if (address >= GetVisibleMemorySize())
        return;

    string raw = memoryGrid[e.ColumnIndex, e.RowIndex].Value?.ToString()?.Trim() ?? "";

    if (memoryEditor.TryUpdateValue(address, raw, out string errorMessage))
    {
        memoryGrid[e.ColumnIndex, e.RowIndex].Value = FormatWord(cpu.Memory[address]);
        RefreshMemoryDisplay();
    }
    else
    {
        MessageBox.Show(errorMessage, "Invalid Instruction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        memoryGrid[e.ColumnIndex, e.RowIndex].Value = FormatWord(cpu.Memory[address]);
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

private void btnChangeTheme_Click(object sender, EventArgs e)
{
    MessageBox.Show("Choose the PRIMARY COLOR — used for buttons and the toolbar.", "Change Theme: Step 1 of 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
    using var primaryDialog = new ColorDialog();
    primaryDialog.Color = theme.PrimaryColor;
    if (primaryDialog.ShowDialog() != DialogResult.OK) return;

    MessageBox.Show("Choose the SECONDARY COLOR — used for panel backgrounds.", "Change Theme: Step 2 of 2", MessageBoxButtons.OK, MessageBoxIcon.Information);
    using var offDialog = new ColorDialog();
    offDialog.Color = theme.OffColor;
    if (offDialog.ShowDialog() != DialogResult.OK) return;

    theme.PrimaryColor = primaryDialog.Color;
    theme.OffColor = offDialog.Color;
    ThemeService.SaveTheme(theme, _themeConfigPath);
    ApplyTheme();
}

private void btnResetTheme_Click(object sender, EventArgs e)
{
    theme = ThemeService.DefaultTheme();
    if (File.Exists(_themeConfigPath))
        File.Delete(_themeConfigPath);
    ApplyTheme();
}
    }
}
