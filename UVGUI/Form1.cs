using Group_4_UV_SIM;

namespace UVGUI
{
    public partial class Form1 : Form
    {
        private CpuState cpu;

        public Form1(CpuState cpuState)
        {
            InitializeComponent();
            cpu = cpuState;

            InitializeMemoryGrid();
            RefreshMemoryDisplay();

        }

        private void RefreshMemoryDisplay()
        {
            for (int i = 0; i < 100; i++)
            {
                int row = i % 20;
                int pair = i / 20;
                int col = pair * 2 + 1;

                memoryGrid[col, row].Value =
                    cpu.Memory[i].ToString("+0000;-0000;0000");
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
                memoryGrid.Columns[col].Width = 45;

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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void memoryGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
