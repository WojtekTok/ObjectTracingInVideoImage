namespace ObjectTracingInVideoImage.App.Forms
{
    partial class BenchmarkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            benchmarkPlot = new ScottPlot.WinForms.FormsPlot();
            detectedPlot = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // benchmarkPlot
            // 
            benchmarkPlot.BackColor = SystemColors.Control;
            benchmarkPlot.DisplayScale = 1F;
            benchmarkPlot.Location = new Point(14, 10);
            benchmarkPlot.Name = "benchmarkPlot";
            benchmarkPlot.Size = new Size(530, 345);
            benchmarkPlot.TabIndex = 0;
            // 
            // detectedPlot
            // 
            detectedPlot.BackColor = SystemColors.Control;
            detectedPlot.DisplayScale = 1F;
            detectedPlot.Location = new Point(14, 348);
            detectedPlot.Name = "detectedPlot";
            detectedPlot.Size = new Size(530, 61);
            detectedPlot.TabIndex = 1;
            // 
            // BenchmarkForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(800, 450);
            Controls.Add(detectedPlot);
            Controls.Add(benchmarkPlot);
            Name = "BenchmarkForm";
            Text = "BenchmarkForm";
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot benchmarkPlot;
        private ScottPlot.WinForms.FormsPlot detectedPlot;
    }
}