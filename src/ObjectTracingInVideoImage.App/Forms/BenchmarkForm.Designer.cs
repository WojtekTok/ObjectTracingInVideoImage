using System.Drawing.Drawing2D;

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
            SuspendLayout();
            // 
            // benchmarkPlot
            // 
            benchmarkPlot.BackColor = SystemColors.Control;
            benchmarkPlot.DisplayScale = 1F;
            benchmarkPlot.Location = new Point(12, 12);
            benchmarkPlot.Name = "benchmarkPlot";
            benchmarkPlot.Size = new Size(1111, 261);
            benchmarkPlot.TabIndex = 0;
            var path = new GraphicsPath();
            int radius = 20;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(benchmarkPlot.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(benchmarkPlot.Width - radius, benchmarkPlot.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, benchmarkPlot.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            benchmarkPlot.Region = new Region(path);
            // 
            // BenchmarkForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1135, 414);
            Controls.Add(benchmarkPlot);
            Name = "BenchmarkForm";
            Text = "BenchmarkForm";
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot benchmarkPlot;
    }
}