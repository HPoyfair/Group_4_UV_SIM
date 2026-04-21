using Group_4_UV_SIM;

namespace UVGUI
{
    public partial class Form1 : Form
    {
        private Theme theme;
        private int untitledCounter = 1;

        private static readonly string _themeConfigPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme.txt");

        private ProgramDocument? ActiveDocument => tabDocuments.SelectedTab?.Tag as ProgramDocument;
        private CpuState? ActiveCpu => ActiveDocument?.Cpu;
        private MemoryEditor? ActiveMemoryEditor => ActiveDocument?.MemoryEditor;

        public Form1(CpuState cpuState)
        {
            InitializeComponent();

            theme = ThemeService.LoadTheme(_themeConfigPath);

            var initialDocument = CreateBlankDocument(cpuState);
            AddDocumentTab(initialDocument);

            ApplyTheme();
            RefreshMemoryDisplay();
        }

        // ================= DOCUMENT HELPERS =================

        private ProgramDocument CreateBlankDocument(CpuState cpuState)
        {
            var document = new ProgramDocument(cpuState)
            {
                Title = $"Untitled {untitledCounter++}",
                IsDirty = false
            };

            ConfigureDocumentCallbacks(document);
            return document;
        }

        private ProgramDocument CreateLoadedDocument(CpuState cpuState, string filePath)
        {
            var document = new ProgramDocument(cpuState)
            {
                FilePath = filePath,
                Title = Path.GetFileName(filePath),
                IsDirty = false
            };

            ConfigureDocumentCallbacks(document);
            return document;
        }

        private void ConfigureDocumentCallbacks(ProgramDocument document)
        {
            document.Cpu.OnOutputMessage = message => WriteToOutput(document, message);
            document.Cpu.OnRequestInput = prompt => RequestInput(prompt);
            document.Cpu.StateChanged += () => HandleDocumentStateChanged(document);
        }

        private void HandleDocumentStateChanged(ProgramDocument document)
        {
            if (!ReferenceEquals(document, ActiveDocument))
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshMemoryDisplay));
                return;
            }

            RefreshMemoryDisplay();
        }

        private int RequestInput(string prompt)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "UVSim Input", "0");
            int.TryParse(input, out int result);
            return result;
        }

        private void AddDocumentTab(ProgramDocument document)
        {
            var tabPage = new TabPage(document.Title)
            {
                Tag = document
            };

            tabDocuments.TabPages.Add(tabPage);
            tabDocuments.SelectedTab = tabPage;
        }

        private void MarkDocumentDirty(ProgramDocument document)
        {
            if (document.IsDirty)
                return;

            document.IsDirty = true;
            UpdateTabTitle(document);
        }

        private void MarkDocumentSaved(ProgramDocument document, string? filePath = null)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                document.FilePath = filePath;
                document.Title = Path.GetFileName(filePath);
            }

            document.IsDirty = false;
            UpdateTabTitle(document);
        }

        private void UpdateTabTitle(ProgramDocument document)
        {
            foreach (TabPage page in tabDocuments.TabPages)
            {
                if (ReferenceEquals(page.Tag, document))
                {
                    page.Text = document.IsDirty ? $"{document.Title}*" : document.Title;
                    break;
                }
            }
        }

        // ================= CORE HELPERS =================

        private int GetVisibleMemorySize()
            => ActiveCpu == null ? 0 : FormatRules.GetMaxMemorySize(ActiveCpu.Format);

        private int GetGridRowCount()
            => ActiveCpu?.Format == ProgramFormat.Legacy4Digit ? 20 : 25;

        private int GetGridColumnPairs()
            => ActiveCpu?.Format == ProgramFormat.Legacy4Digit ? 5 : 10;

        private string FormatAddress(int address)
            => ActiveCpu?.Format == ProgramFormat.Legacy4Digit ? address.ToString("D2") : address.ToString("D3");

        private string FormatWord(int value)
            => ActiveCpu?.Format == ProgramFormat.Legacy4Digit
                ? value.ToString("+0000;-0000;0000")
                : value.ToString("+000000;-000000;000000");

        private int GetAddressFromCell(int col, int row)
        {
            if (col % 2 == 0 || ActiveCpu == null) return -1;

            int rows = GetGridRowCount();
            int pair = col / 2;
            int address = pair * rows + row;

            return address < GetVisibleMemorySize() ? address : -1;
        }

        private int GetSelectedAddress()
        {
            if (memoryGrid.CurrentCell == null || ActiveCpu == null) return -1;
            return GetAddressFromCell(memoryGrid.CurrentCell.ColumnIndex, memoryGrid.CurrentCell.RowIndex);
        }

        private bool TryGetSelectedBlock(out int start, out int count, out string error)
        {
            start = -1;
            count = 0;
            error = "";

            if (ActiveCpu == null)
            {
                error = "No open document.";
                return false;
            }

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
            int pairs = GetGridColumnPairs();

            memoryGrid.Columns.Clear();
            memoryGrid.Rows.Clear();

            memoryGrid.ColumnCount = pairs * 2;
            memoryGrid.RowCount = rows;

            memoryGrid.RowHeadersVisible = false;
            memoryGrid.ColumnHeadersVisible = false;

            memoryGrid.MultiSelect = true;
            memoryGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;

            memoryGrid.AllowUserToResizeColumns = false;
            memoryGrid.AllowUserToResizeRows = false;

            for (int c = 0; c < pairs * 2; c++)
            {
                if (c % 2 == 0)
                {
                    memoryGrid.Columns[c].Width = ActiveCpu?.Format == ProgramFormat.Legacy4Digit ? 30 : 40;
                    memoryGrid.Columns[c].ReadOnly = true;
                }
                else
                {
                    memoryGrid.Columns[c].Width = ActiveCpu?.Format == ProgramFormat.Legacy4Digit ? 80 : 95;
                }
            }
        }

        private void RefreshMemoryDisplay()
        {
            if (ActiveCpu == null)
            {
                accData.Text = "";
                txtOutput.Clear();
                return;
            }

            int max = GetVisibleMemorySize();
            int rows = GetGridRowCount();
            int pairs = GetGridColumnPairs();

            if (memoryGrid.RowCount != rows || memoryGrid.ColumnCount != pairs * 2)
            {
                InitializeMemoryGrid();
            }

            accData.Text = FormatWord(ActiveCpu.Accumulator);
            txtOutput.Text = ActiveDocument?.OutputLog.ToString() ?? string.Empty;

            for (int row = 0; row < memoryGrid.RowCount; row++)
            {
                for (int pair = 0; pair < pairs; pair++)
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
                    memoryGrid[col + 1, row].Value = FormatWord(ActiveCpu.Memory[address]);
                }
            }
        }

        private void memoryGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ActiveDocument == null || ActiveMemoryEditor == null) return;

            int address = GetAddressFromCell(e.ColumnIndex, e.RowIndex);
            if (address < 0) return;

            string raw = memoryGrid[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";

            if (!ActiveMemoryEditor.TryUpdateValue(address, raw, out string err))
            {
                MessageBox.Show(err);
            }
            else
            {
                MarkDocumentDirty(ActiveDocument);
            }

            RefreshMemoryDisplay();
        }

        // ================= FILE =================

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK) return;

            var loadedCpu = new CpuState();
            var sim = new Simulator(loadedCpu);
            sim.ReadFile(ofd.FileName);

            var document = CreateLoadedDocument(loadedCpu, ofd.FileName);
            AddDocumentTab(document);
            RefreshMemoryDisplay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ActiveDocument == null || ActiveCpu == null) return;

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = ActiveDocument.FilePath ?? ActiveDocument.Title;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            int visibleSize = FormatRules.GetMaxMemorySize(ActiveCpu.Format);
            int lastNonZero = -1;

            for (int i = visibleSize - 1; i >= 0; i--)
            {
                if (ActiveCpu.Memory[i] != 0)
                {
                    lastNonZero = i;
                    break;
                }
            }

            List<string> lines = new List<string>();

            if (lastNonZero == -1)
            {
                lines.Add(MemoryEditor.FormatValue(0, ActiveCpu.Format));
            }
            else
            {
                for (int i = 0; i <= lastNonZero; i++)
                {
                    lines.Add(MemoryEditor.FormatValue(ActiveCpu.Memory[i], ActiveCpu.Format));
                }
            }

            File.WriteAllLines(sfd.FileName, lines);
            MarkDocumentSaved(ActiveDocument, sfd.FileName);
        }

        // ================= EXECUTION =================

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (ActiveCpu == null || ActiveDocument == null) return;

            Simulator sim = new Simulator(ActiveCpu);

            await Task.Run(() =>
            {
                while (!ActiveCpu.Halted)
                {
                    sim.ExecuteNext();
                }
            });

            MessageBox.Show("Program Halted.");
        }

        // ================= MEMORY OPS =================

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (ActiveDocument == null || ActiveMemoryEditor == null) return;

            int addr = GetSelectedAddress();
            if (addr < 0) return;

            if (ActiveMemoryEditor.TryInsertValue(addr, 0, out string err))
            {
                MarkDocumentDirty(ActiveDocument);
                RefreshMemoryDisplay();
            }
            else
            {
                MessageBox.Show(err);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ActiveDocument == null || ActiveMemoryEditor == null) return;

            if (!TryGetSelectedBlock(out int start, out int count, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (!ActiveMemoryEditor.TryDeleteValue(start, out string deleteErr))
                {
                    MessageBox.Show(deleteErr);
                    return;
                }
            }

            MarkDocumentDirty(ActiveDocument);
            RefreshMemoryDisplay();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (ActiveMemoryEditor == null) return;

            if (!TryGetSelectedBlock(out int start, out int count, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            if (ActiveMemoryEditor.TryCopy(start, count, out string text, out string copyErr))
            {
                Clipboard.SetText(text);
            }
            else
            {
                MessageBox.Show(copyErr);
            }
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
            if (ActiveDocument == null || ActiveMemoryEditor == null) return;

            if (!TryGetSelectedBlock(out int start, out int count, out string err))
            {
                MessageBox.Show(err);
                return;
            }

            if (ActiveMemoryEditor.TryCut(start, count, out string text, out string cutErr))
            {
                Clipboard.SetText(text);
                MarkDocumentDirty(ActiveDocument);
                RefreshMemoryDisplay();
            }
            else
            {
                MessageBox.Show(cutErr);
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (ActiveDocument == null || ActiveMemoryEditor == null) return;

            int addr = GetSelectedAddress();
            if (addr < 0) return;

            if (!Clipboard.ContainsText()) return;

            if (ActiveMemoryEditor.TryPaste(addr, Clipboard.GetText(), out string err))
            {
                MarkDocumentDirty(ActiveDocument);
                RefreshMemoryDisplay();
            }
            else
            {
                MessageBox.Show(err);
            }
        }

        // ================= OUTPUT =================

        private void WriteToOutput(ProgramDocument document, string msg)
        {
            document.OutputLog.AppendLine(msg);

            if (!ReferenceEquals(document, ActiveDocument))
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => txtOutput.Text = document.OutputLog.ToString()));
                return;
            }

            txtOutput.Text = document.OutputLog.ToString();
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
            tabDocuments.BackColor = theme.OffColor;
            memoryGrid.BackgroundColor = theme.OffColor;

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

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            if (tabDocuments.TabPages.Count == 0 || tabDocuments.SelectedTab == null)
                return;

            var closingPage = tabDocuments.SelectedTab;
            tabDocuments.TabPages.Remove(closingPage);

            if (tabDocuments.TabPages.Count == 0)
            {
                var replacementCpu = new CpuState();
                var replacementDocument = CreateBlankDocument(replacementCpu);
                AddDocumentTab(replacementDocument);
            }

            RefreshMemoryDisplay();
        }

        private void tabDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeMemoryGrid();
            RefreshMemoryDisplay();
        }
    }
}
