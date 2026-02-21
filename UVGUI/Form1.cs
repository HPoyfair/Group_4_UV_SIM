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
                int row = i / 10;
                int col = i % 10;

                memoryGrid[col, row].Value =
                    cpu.Memory[i].ToString("+0000;-0000;0000");
            }
        }
        private void InitializeMemoryGrid()
        {
            memoryGrid.ColumnCount = 10;
            memoryGrid.RowCount = 10;

            for (int col = 0; col < 10; col++)
            {
                memoryGrid.Columns[col].Width = 60;
                memoryGrid.Columns[col].SortMode =
                    DataGridViewColumnSortMode.NotSortable;
            }

            for (int row = 0; row < 10; row++)
            {
                memoryGrid.Rows[row].HeaderCell.Value =
                    (row * 10).ToString("D2");
            }

            memoryGrid.AllowUserToAddRows = false;
            memoryGrid.AllowUserToDeleteRows = false;
            memoryGrid.AllowUserToResizeRows = false;
            memoryGrid.AllowUserToResizeColumns = false;

            memoryGrid.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            memoryGrid.DefaultCellStyle.Font = new Font("Consolas", 10);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
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
    }
}
