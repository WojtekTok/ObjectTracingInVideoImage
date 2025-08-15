using System.Drawing.Drawing2D;
using ObjectTracingInVideoImage.App.UIHelpers;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BenchmarkForm));
            benchmarkPlot = new ScottPlot.WinForms.FormsPlot();
            resultsGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)resultsGrid).BeginInit();
            SuspendLayout();
            // 
            // benchmarkPlot
            // 
            benchmarkPlot.BackColor = SystemColors.Control;
            benchmarkPlot.DisplayScale = 1F;
            benchmarkPlot.Location = new Point(12, 12);
            benchmarkPlot.Name = "benchmarkPlot";
            benchmarkPlot.Size = new Size(1111, 327);
            benchmarkPlot.TabIndex = 0;
            benchmarkPlot.Plot.Title($"IoU over Time – {Path.GetFileName(_csvPath)}");
            benchmarkPlot.Plot.Axes.Left.Label.Text = "IoU";
            benchmarkPlot.Plot.Axes.Bottom.Label.Text = "Frame";
            benchmarkPlot.Plot.Legend.IsVisible = true;
            benchmarkPlot.Plot.Legend.Alignment = ScottPlot.Alignment.LowerLeft;
            ControlStyler.RoundControlCorners(benchmarkPlot, 20);
            // 
            // resultsGrid
            // 
            resultsGrid.BackgroundColor = SystemColors.Control;
            resultsGrid.BorderStyle = BorderStyle.None;
            resultsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resultsGrid.Location = new Point(12, 359);
            resultsGrid.Name = "resultsGrid";
            resultsGrid.Size = new Size(1111, 210);
            resultsGrid.TabIndex = 1;
            ControlStyler.RoundControlCorners(resultsGrid, 20);
            // 
            // BenchmarkForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1135, 583);
            Controls.Add(resultsGrid);
            Controls.Add(benchmarkPlot);
            Name = "BenchmarkForm";
            Text = "Test Result Data";
            ((System.ComponentModel.ISupportInitialize)resultsGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot benchmarkPlot;
        private DataGridView resultsGrid;
    }
}