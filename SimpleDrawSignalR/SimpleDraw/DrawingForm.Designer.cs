namespace SimpleDraw
{
    partial class DrawingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingForm));
            DrawingPanel = new PictureBox();
            ToolStripContainer = new ToolStripContainer();
            label1 = new Label();
            SizeUpDown = new NumericUpDown();
            ClearButton = new Button();
            DrawingToolStrip = new ToolStrip();
            SelectBtn = new ToolStripButton();
            BrushTool = new ToolStripButton();
            LineTool = new ToolStripButton();
            RectangleTool = new ToolStripButton();
            EllipseTool = new ToolStripButton();
            TriangleTool = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            FgColorButton = new ToolStripColorButton();
            EraserTool = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)DrawingPanel).BeginInit();
            ToolStripContainer.ContentPanel.SuspendLayout();
            ToolStripContainer.TopToolStripPanel.SuspendLayout();
            ToolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SizeUpDown).BeginInit();
            DrawingToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // DrawingPanel
            // 
            DrawingPanel.BackColor = Color.White;
            DrawingPanel.Dock = DockStyle.Fill;
            DrawingPanel.Location = new Point(0, 0);
            DrawingPanel.Name = "DrawingPanel";
            DrawingPanel.Size = new Size(800, 425);
            DrawingPanel.TabIndex = 1;
            DrawingPanel.TabStop = false;
            DrawingPanel.Paint += DrawingPanel_Paint;
            DrawingPanel.MouseDown += DrawingPanel_MouseDown;
            DrawingPanel.MouseMove += DrawingPanel_MouseMove;
            // 
            // ToolStripContainer
            // 
            // 
            // ToolStripContainer.ContentPanel
            // 
            ToolStripContainer.ContentPanel.AutoScroll = true;
            ToolStripContainer.ContentPanel.Controls.Add(label1);
            ToolStripContainer.ContentPanel.Controls.Add(SizeUpDown);
            ToolStripContainer.ContentPanel.Controls.Add(ClearButton);
            ToolStripContainer.ContentPanel.Controls.Add(DrawingPanel);
            ToolStripContainer.ContentPanel.Size = new Size(800, 425);
            ToolStripContainer.Dock = DockStyle.Fill;
            ToolStripContainer.Location = new Point(0, 0);
            ToolStripContainer.Name = "ToolStripContainer";
            ToolStripContainer.Size = new Size(800, 450);
            ToolStripContainer.TabIndex = 2;
            ToolStripContainer.Text = "toolStripContainer1";
            // 
            // ToolStripContainer.TopToolStripPanel
            // 
            ToolStripContainer.TopToolStripPanel.Controls.Add(DrawingToolStrip);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(118, 15);
            label1.TabIndex = 4;
            label1.Text = "Epaisseur du pinceau";
            // 
            // SizeUpDown
            // 
            SizeUpDown.Location = new Point(124, 0);
            SizeUpDown.Name = "SizeUpDown";
            SizeUpDown.Size = new Size(41, 23);
            SizeUpDown.TabIndex = 3;
            SizeUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(725, 0);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(75, 23);
            ClearButton.TabIndex = 3;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // DrawingToolStrip
            // 
            DrawingToolStrip.Dock = DockStyle.None;
            DrawingToolStrip.Items.AddRange(new ToolStripItem[] { SelectBtn, BrushTool, LineTool, RectangleTool, EllipseTool, TriangleTool, toolStripSeparator1, FgColorButton, EraserTool });
            DrawingToolStrip.Location = new Point(3, 0);
            DrawingToolStrip.Name = "DrawingToolStrip";
            DrawingToolStrip.Size = new Size(202, 25);
            DrawingToolStrip.TabIndex = 0;
            DrawingToolStrip.Text = "Drawing Tools";
            DrawingToolStrip.ItemClicked += DrawingToolStrip_ItemClicked;
            // 
            // SelectBtn
            // 
            SelectBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            SelectBtn.Image = (Image)resources.GetObject("SelectBtn.Image");
            SelectBtn.ImageTransparentColor = Color.Magenta;
            SelectBtn.Name = "SelectBtn";
            SelectBtn.Size = new Size(23, 22);
            SelectBtn.Text = "Pointer";
            SelectBtn.ToolTipText = "SelectBtn";
            SelectBtn.Click += SelectBtn_Click;
            // 
            // BrushTool
            // 
            BrushTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            BrushTool.Image = (Image)resources.GetObject("BrushTool.Image");
            BrushTool.ImageTransparentColor = Color.Magenta;
            BrushTool.Name = "BrushTool";
            BrushTool.Size = new Size(23, 22);
            BrushTool.Text = "Brush";
            BrushTool.ToolTipText = "Brush Tool";
            BrushTool.Click += BrushTool_Click;
            // 
            // LineTool
            // 
            LineTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            LineTool.Image = (Image)resources.GetObject("LineTool.Image");
            LineTool.ImageTransparentColor = Color.Magenta;
            LineTool.Name = "LineTool";
            LineTool.Size = new Size(23, 22);
            LineTool.Text = "Line";
            LineTool.ToolTipText = "Line Tool";
            LineTool.Click += LineTool_Click;
            // 
            // RectangleTool
            // 
            RectangleTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            RectangleTool.Image = (Image)resources.GetObject("RectangleTool.Image");
            RectangleTool.ImageTransparentColor = Color.Magenta;
            RectangleTool.Name = "RectangleTool";
            RectangleTool.Size = new Size(23, 22);
            RectangleTool.Text = "Rectangle";
            RectangleTool.ToolTipText = "Rectangle Tool";
            RectangleTool.Click += RectangleTool_Click;
            // 
            // EllipseTool
            // 
            EllipseTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            EllipseTool.Image = (Image)resources.GetObject("EllipseTool.Image");
            EllipseTool.ImageTransparentColor = Color.Magenta;
            EllipseTool.Name = "EllipseTool";
            EllipseTool.Size = new Size(23, 22);
            EllipseTool.Text = "Ellipse";
            EllipseTool.ToolTipText = "Ellipse Tool";
            EllipseTool.Click += EllipseTool_Click;
            // 
            // TriangleTool
            // 
            TriangleTool.BackColor = Color.Transparent;
            TriangleTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TriangleTool.Image = (Image)resources.GetObject("TriangleTool.Image");
            TriangleTool.ImageTransparentColor = Color.Magenta;
            TriangleTool.Name = "TriangleTool";
            TriangleTool.Size = new Size(23, 22);
            TriangleTool.Text = "Triangle";
            TriangleTool.ToolTipText = "Triangle Tool";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // FgColorButton
            // 
            FgColorButton.ColorRectangle = new System.Drawing.Rectangle(0, 13, 16, 3);
            FgColorButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            FgColorButton.Image = (Image)resources.GetObject("FgColorButton.Image");
            FgColorButton.ImageTransparentColor = Color.Magenta;
            FgColorButton.Name = "FgColorButton";
            FgColorButton.SelectedColor = Color.Black;
            FgColorButton.Size = new Size(23, 22);
            FgColorButton.Text = "toolStripColorButton1";
            // 
            // EraserTool
            // 
            EraserTool.DisplayStyle = ToolStripItemDisplayStyle.Image;
            EraserTool.Image = (Image)resources.GetObject("EraserTool.Image");
            EraserTool.ImageTransparentColor = Color.Magenta;
            EraserTool.Name = "EraserTool";
            EraserTool.Size = new Size(23, 22);
            EraserTool.Text = "Eraser";
            EraserTool.ToolTipText = "Eraser Tool";
            EraserTool.Click += EraserTool_Click;
            // 
            // DrawingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ToolStripContainer);
            Name = "DrawingForm";
            Text = "Simple Draw";
            ((System.ComponentModel.ISupportInitialize)DrawingPanel).EndInit();
            ToolStripContainer.ContentPanel.ResumeLayout(false);
            ToolStripContainer.ContentPanel.PerformLayout();
            ToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            ToolStripContainer.TopToolStripPanel.PerformLayout();
            ToolStripContainer.ResumeLayout(false);
            ToolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SizeUpDown).EndInit();
            DrawingToolStrip.ResumeLayout(false);
            DrawingToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox DrawingPanel;
        private ToolStripContainer ToolStripContainer;
        private ToolStrip DrawingToolStrip;
        private ToolStripButton LineTool;
        private ToolStripButton RectangleTool;
        private ToolStripButton EllipseTool;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripColorButton FgColorButton;
        private ToolStripButton SelectBtn;
        private ToolStripButton EraserTool;
        private ToolStripButton TriangleTool;
        private ToolStripButton BrushTool;
        private Button ClearButton;
        private Label label1;
        private NumericUpDown SizeUpDown;
    }
}