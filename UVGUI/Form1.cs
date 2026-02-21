using Group_4_UV_SIM;
using Xunit.Sdk;

namespace UVGUI
{
    public partial class Form1 : Form
    {
        private CpuState cpu;

        public Form1(CpuState cpuState)
        {
            InitializeComponent();
            cpu = cpuState;

            cpu.OnOutputMessage = WriteToOutput;

            cpu.OnRequestInput = (prompt) =>
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(prompt, "UVSim Input", "0");
                int.TryParse(input, out int result);
                return result;
            };

            cpu.StateChanged += HandleCpuStateChanged;
            InitializeMemoryGrid();
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
                    memoryGrid[col, row].Style.BackColor = Color.LightBlue;
                    memoryGrid[col - 1, row].Style.BackColor = Color.LightBlue;
                }
                else
                {
                    memoryGrid[col, row].Style.BackColor = Color.White;
                    memoryGrid[col - 1, row].Style.BackColor = Color.White;
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

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (cpu.Halted)
            {
                MessageBox.Show("CPU is halted. Reload program to restart.");
                return;
            }

            Simulator sim = new Simulator(cpu);
            sim.ExecuteNext();

            RefreshMemoryDisplay();
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (cpu.Halted) return;

            btnRun.Enabled = false;
            btnStep.Enabled = false;

            Simulator sim = new Simulator(cpu);

            await Task.Run(() =>
            {
                while (!cpu.Halted)
                {
                    sim.ExecuteNext();
                }
            });

            btnRun.Enabled = true;
            btnStep.Enabled = true;
            MessageBox.Show("Program Halted.");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files|*.txt|All Files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Simulator sim = new Simulator(this.cpu);

                    Array.Clear(cpu.Memory, 0, cpu.Memory.Length);

                    sim.ReadFile(ofd.FileName);

                    cpu.InstructionPointer = 0;
                    cpu.Halted = false;

                    RefreshMemoryDisplay();

                    MessageBox.Show($"Loaded program: {Path.GetFileName(ofd.FileName)}");
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
    }
}
