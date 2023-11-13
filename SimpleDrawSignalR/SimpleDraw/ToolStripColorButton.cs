using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDraw
{
    public class ToolStripColorButton : ToolStripButton
    {
		private Color selectedColor;

		public Color SelectedColor
		{
			get { return selectedColor; }
			set
            { 
                selectedColor = value;
                UpdateImage();
            }
		}

		public System.Drawing.Rectangle ColorRectangle { get; set; } = new(0, 13, 16, 3);

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            var dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                SelectedColor = dlg.Color;
        }

        private void UpdateImage()
        {
            Graphics g = Graphics.FromImage(Image);
            g.FillRectangle(new SolidBrush(SelectedColor), ColorRectangle);
            Invalidate();
        }

    }
}
