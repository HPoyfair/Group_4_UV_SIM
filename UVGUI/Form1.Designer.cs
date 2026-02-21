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
            outputText = new TextBox();
            panel2 = new Panel();
            memoryGrid = new DataGridView();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
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
            btnLoad.Click += button3_Click;
            // 
            // btnStep
            // 
            btnStep.Location = new Point(84, 3);
            btnStep.Name = "btnStep";
            btnStep.Size = new Size(75, 23);
            btnStep.TabIndex = 1;
            btnStep.Text = "Step";
            btnStep.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(3, 3);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 0;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = true;
            // 
            // outputText
            // 
            outputText.Dock = DockStyle.Bottom;
            outputText.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            outputText.ForeColor = SystemColors.MenuText;
            outputText.Location = new Point(0, 410);
            outputText.Multiline = true;
            outputText.Name = "outputText";
            outputText.ReadOnly = true;
            outputText.ScrollBars = ScrollBars.Vertical;
            outputText.Size = new Size(617, 62);
            outputText.TabIndex = 1;
            outputText.Text = "<Console Output>";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaption;
            panel2.Controls.Add(memoryGrid);
            panel2.Controls.Add(textBox2);
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(ACCLabel);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(617, 472);
            panel2.TabIndex = 2;
            // 
            // memoryGrid
            // 
            memoryGrid.BackgroundColor = SystemColors.GradientInactiveCaption;
            memoryGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            memoryGrid.Location = new Point(32, 51);
            memoryGrid.Name = "memoryGrid";
            memoryGrid.Size = new Size(551, 320);
            memoryGrid.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(249, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(236, 25);
            textBox2.TabIndex = 3;
            textBox2.Text = "<Input>";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ButtonFace;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(528, 5);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(55, 18);
            textBox1.TabIndex = 4;
            textBox1.Text = "+0000";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // ACCLabel
            // 
            ACCLabel.AutoSize = true;
            ACCLabel.Font = new Font("Consolas", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ACCLabel.Location = new Point(495, 5);
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
            ClientSize = new Size(617, 472);
            Controls.Add(outputText);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "Form1";
            Text = "UV SIM Virtual Machine";
            Load += Form1_Load_1;
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
        private TextBox outputText;
        private Panel panel2;
        private Label ACCLabel;
        private TextBox textBox1;
        private TextBox textBox2;
        private DataGridView memoryGrid;
    }
}
