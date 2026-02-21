namespace UVGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnLoad = new Button();
            btnStep = new Button();
            btnRun = new Button();
            txtOutput = new TextBox();
            panel2 = new Panel();
            memoryGrid = new DataGridView();
            accData = new TextBox();
            ACCLabel = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoryGrid).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDarkDark;
            panel1.Controls.Add(btnLoad);
            panel1.Controls.Add(btnStep);
            panel1.Controls.Add(btnRun);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(243, 28);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(165, 3);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 2;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnStep
            // 
            btnStep.Location = new Point(84, 3);
            btnStep.Name = "btnStep";
            btnStep.Size = new Size(75, 23);
            btnStep.TabIndex = 1;
            btnStep.Text = "Step";
            btnStep.UseVisualStyleBackColor = true;
            btnStep.Click += btnStep_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(3, 3);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 0;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // txtOutput
            // 
            txtOutput.Dock = DockStyle.Bottom;
            txtOutput.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtOutput.ForeColor = SystemColors.MenuText;
            txtOutput.Location = new Point(0, 563);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = ScrollBars.Vertical;
            txtOutput.Size = new Size(502, 62);
            txtOutput.TabIndex = 1;
            txtOutput.Text = "<Console Output>\r\n";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaption;
            panel2.Controls.Add(memoryGrid);
            panel2.Controls.Add(accData);
            panel2.Controls.Add(ACCLabel);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(502, 625);
            panel2.TabIndex = 2;
            // 
            // memoryGrid
            // 
            memoryGrid.BackgroundColor = SystemColors.GradientInactiveCaption;
            memoryGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            memoryGrid.Location = new Point(32, 53);
            memoryGrid.Name = "memoryGrid";
            memoryGrid.Size = new Size(433, 480);
            memoryGrid.TabIndex = 3;
            memoryGrid.CellContentClick += memoryGrid_CellContentClick;
            // 
            // accData
            // 
            accData.BackColor = SystemColors.ButtonFace;
            accData.BorderStyle = BorderStyle.None;
            accData.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            accData.Location = new Point(323, 5);
            accData.Name = "accData";
            accData.ReadOnly = true;
            accData.Size = new Size(55, 18);
            accData.TabIndex = 4;
            accData.Text = "+0000";
            accData.TextAlign = HorizontalAlignment.Center;
            accData.TextChanged += textBox1_TextChanged;
            // 
            // ACCLabel
            // 
            ACCLabel.AutoSize = true;
            ACCLabel.Font = new Font("Consolas", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ACCLabel.Location = new Point(290, 5);
            ACCLabel.Name = "ACCLabel";
            ACCLabel.Size = new Size(32, 18);
            ACCLabel.TabIndex = 3;
            ACCLabel.Text = "ACC";
            ACCLabel.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(502, 625);
            Controls.Add(txtOutput);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "Form1";
            Text = "UV SIM Virtual Machine";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)memoryGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button btnLoad;
        private Button btnStep;
        private Button btnRun;
        private TextBox txtOutput;
        private Panel panel2;
        private Label ACCLabel;
        private TextBox accData;
        private DataGridView memoryGrid;
    }
}
