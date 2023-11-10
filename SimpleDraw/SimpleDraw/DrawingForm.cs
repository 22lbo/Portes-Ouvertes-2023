using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleDraw
{
    public partial class DrawingForm : Form
    {
        HubConnection connection;

        List<Shape> Shapes = new();
        Shape SelectedShape;

        List<ToolStripButton> Tools = new List<ToolStripButton>();
        ToolStripButton SelectedTool;

        public DrawingForm()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:7081/DrawingHub")
                .WithAutomaticReconnect()
                .Build();

            if (connection != null)
                Debug.WriteLine("Connection successful");
            if (connection == null)
                Debug.WriteLine("Connection failed");

            // Add tools to list
            Tools.Add(LineTool);
            Tools.Add(RectangleTool);
            Tools.Add(EllipseTool);
            SelectTool(LineTool);
        }

        void SelectTool(ToolStripItem tool)
        {
            foreach (var t in Tools)
                if (t.Checked = t == tool)
                    SelectedTool = t;

            if (SelectedTool != null)
                SelectedTool.Checked = true;
        }


        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedTool == LineTool)
                    SelectedShape = new Line();
                else if (SelectedTool == RectangleTool)
                    SelectedShape = new Rectangle();
                else if (SelectedTool == EllipseTool)
                    SelectedShape = new Ellipse();
                Shapes.Add(SelectedShape);
                SelectedShape.X1 = SelectedShape.X2 = e.X;
                SelectedShape.Y1 = SelectedShape.Y2 = e.Y;
                SelectedShape.LineColor = FgColorButton.SelectedColor;
            }
            DrawingPanel.Invalidate();
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedShape != null)
                {
                    SelectedShape.X2 = e.X;
                    SelectedShape.Y2 = e.Y;
                }

                DrawingPanel.Invalidate();
            }
        }
        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (var s in Shapes)
            {
                s.Draw(e.Graphics);
            }

        }

        private void DrawingToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectTool(e.ClickedItem);
        }

    }
}
