using Group_4_UV_SIM;

namespace UVGUI
{
    public partial class Form1 : Form
    {
        private CpuState cpu;
        private Theme theme;
        private MemoryEditor memoryEditor;

        private static readonly string _themeConfigPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.txt");

        public Form1(CpuState cpuState)
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

            cpu.StateChanged += () => RefreshMemoryDisplay();

            InitializeMemoryGrid();
            ApplyTheme();
            RefreshMemoryDisplay();
        }

        // ================= CORE HELPERS =================

        private int GetVisibleMemorySize() => FormatRules.GetMaxMemorySize(cpu.Format);
        private int GetGridRowCount() => cpu.Format == ProgramFormat.Legacy4Digit ? 20 : 50;

        private string FormatAddress(int address)
            => cpu.Format == ProgramFormat.Legacy4Digit ? address.ToString("D2") : address.ToString("D3");

        private string FormatWord(int value)
            => cpu.Format == ProgramFormat.Legacy4Digit
                ? value.ToString("+0000;-0000;0000")
                : value.ToString("+000000;-000000;000000");

        private int GetAddressFromCell(int col, int row)
        {
            if (col % 2 == 0) return -1;

            int rows = GetGridRowCount();
            int pair = col / 2;
            int address = pair * rows + row;

            return address < GetVisibleMemorySize() ? address : -1;
        }

        private int GetSelectedAddress()
        {
            if (memoryGrid.CurrentCell == null) return -1;
            return GetAddressFromCell(memoryGrid.CurrentCell.ColumnIndex, memoryGrid.CurrentCell.RowIndex);
        }

        private bool TryGetSelectedBlock(out int start, out int count, out string error)
        {
            start = -1;
            count = 0;
            error = "";

            var list = memoryGrid.SelectedCells
                .Cast<DataGridViewCell>()
                .Select(c => GetAddressFromCell(c.ColumnIndex, c.RowIndex))
                .Where(a => a >= 0)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            if (list.Count == 0)
            {
                error = "Select memory cells.";
                return false;
            }

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] != list[i - 1] + 1)
                {
                    error = "Selection must be contiguous.";
                    return false;
                }
            }

            start = list[0];
            count = list.Count;
            return true;
        }

        // ================= GRID =================

        private void InitializeMemoryGrid()
        {
            int rows = GetGridRowCount();

            memoryGrid.Columns.Clear();
            memoryGrid.Rows.Clear();

            memoryGrid.ColumnCount = 10;
            memoryGrid.RowCount = rows;

            memoryGrid.RowHeadersVisible = false;
            memoryGrid.ColumnHeadersVisible = false;

            memoryGrid.MultiSelect = true;
            memoryGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;

            for (int c = 0; c < 10; c++)
            {
                if (c % 2 == 0)
                {
                    memoryGrid.Columns[c].Width = 30;
                    memoryGrid.Columns[c].ReadOnly = true;
                }
                else
                {
                    memoryGrid.Columns[c].Width = 80;
                }
            }
        }

        private void RefreshMemoryDisplay()
        {
            int max = GetVisibleMemorySize();
            int rows = GetGridRowCount();

            accData.Text = FormatWord(cpu.Accumulator);

            for (int row = 0; row < memoryGrid.RowCount; row++)
            {
                for (int pair = 0; pair < 5; pair++)
                {
                    int address = pair * rows + row;
                    int col = pair * 2;

                    if (address >= max)
                    {
                        memoryGrid[col, row].Value = "";
                        memoryGrid[col + 1, row].Value = "";
                        continue;
                    }

                    memoryGrid[col, row].Value = FormatAddress(address);
                    memoryGrid[col + 1, row].Value = FormatWord(cpu.Memory[address]);
                }
            }
        }

        private void memoryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int address = GetAddressFromCell(e.ColumnIndex, e.RowIndex);
            if (address < 0) return;

            string raw = memoryGrid[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";

            if (!memoryEditor.TryUpdateValue(address, raw, out string err))
            {
                MessageBox.Show(err);
            }

            RefreshMemoryDisplay();
        }

        // ================= FILE =================

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK) return;

            Simulator sim = new Simulator(cpu);
            Array.Clear(cpu.Memory, 0, cpu.Memory.Length);

            sim.ReadFile(ofd.FileName);

            memoryEditor = new MemoryEditor(cpu);

            InitializeMemoryGrid();
            RefreshMemoryDisplay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog();

            if (sfd.ShowDialog() != DialogResult.OK) return;

            SaveFileService.SaveMemoryToFile(cpu.Memory, sfd.FileName);
        }

        // ================= EXECUTION =================

        private async void btnRun_Click(object sender, EventArgs e)
        {
            Simulator sim = new Simulator(cpu);

            await Task.Run(() =>
            {
                while (!cpu.Halted)
                {
                    sim.ExecuteNext();
                }
            });

            MessageBox.Show("Program Halted.");
        }

        // ================= MEMORY OPS =================

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int addr = GetSelectedAddress();
            if (addr < 0) return;

            memoryEditor.TryInsertValue(addr, 0, out _);
            RefreshMemoryDisplay();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int addr = GetSelectedAddress();
            if (addr < 0) return;

            memoryEditor.TryDeleteValue(addr, out _);
            RefreshMemoryDisplay();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedBlock(out int start, out int count, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            memoryEditor.TryCopy(start, count, out string text, out _);
            Clipboard.SetText(text);
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedBlock(out int start, out int count, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            memoryEditor.TryCut(start, count, out string text, out _);
            Clipboard.SetText(text);
            RefreshMemoryDisplay();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            int addr = GetSelectedAddress();
            if (addr < 0) return;

            if (!Clipboard.ContainsText()) return;

            memoryEditor.TryPaste(addr, Clipboard.GetText(), out _);
            RefreshMemoryDisplay();
        }

        // ================= OUTPUT =================

        private void WriteToOutput(string msg)
        {
            txtOutput.AppendText(msg + Environment.NewLine);
        }

        // ================= THEME =================

        private static Color GetContrastColor(Color bg)
        {
            double b = (bg.R * 299 + bg.G * 587 + bg.B * 114) / 1000;
            return b > 128 ? Color.Black : Color.White;
        }

        private void ApplyTheme()
        {
            panel1.BackColor = theme.PrimaryColor;
            panel2.BackColor = theme.OffColor;
            RefreshMemoryDisplay();
        }

        private void Form1_Load(object sender, EventArgs e)
{
}

private void label1_Click(object sender, EventArgs e)
{
}

private void textBox1_TextChanged(object sender, EventArgs e)
{
}

private void panel1_Paint(object sender, PaintEventArgs e)
{
}

private void memoryGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
{
}

private void btnChangeTheme_Click(object sender, EventArgs e)
{
    MessageBox.Show(
        "Choose the PRIMARY COLOR — used for buttons and the toolbar.",
        "Change Theme: Step 1 of 2",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information
    );

    using var primaryDialog = new ColorDialog();
    primaryDialog.Color = theme.PrimaryColor;
    if (primaryDialog.ShowDialog() != DialogResult.OK) return;

    MessageBox.Show(
        "Choose the SECONDARY COLOR — used for panel backgrounds.",
        "Change Theme: Step 2 of 2",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information
    );

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