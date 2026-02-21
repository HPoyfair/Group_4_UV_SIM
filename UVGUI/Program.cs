using System;
using System.Windows.Forms;
using Group_4_UV_SIM;

namespace UVGUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CpuState cpu = new CpuState();

            Application.Run(new Form1(cpu));
        }
    }
}