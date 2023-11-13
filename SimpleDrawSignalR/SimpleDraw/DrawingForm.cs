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
using DrawLib;
using Microsoft.VisualBasic.ApplicationServices;

namespace SimpleDraw
{
    public partial class DrawingForm : Form
    {
        HubConnection connection;

        Dictionary<int, Shape> Shapes = new();
        Shape SelectedShape;

        List<ToolStripButton> Tools = new List<ToolStripButton>();
        ToolStripButton SelectedTool;

        public DrawingForm()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5076/DrawingHub")
                .WithAutomaticReconnect()
                .Build();

            if (connection != null)
            {
                Debug.WriteLine("Connection successful");

                connection.On<ShapeData>("ShapeAdded", (s) =>
                {
                    AddShape(s);
                    DrawingPanel.Invalidate();
                });

                connection.On<ShapeData>("ShapeUpdated", (s) =>
                {
                    if (Shapes.TryGetValue(s.Id, out var shape))
                    {
                        shape.Data = s;
                        DrawingPanel.Invalidate();
                    }
                });
            }
            else
                Debug.WriteLine("Connection failed");


            // Add tools to list
            Tools.Add(LineTool);
            Tools.Add(RectangleTool);
            Tools.Add(EllipseTool);
            SelectTool(LineTool);

            RetrieveShapes();
        }

        async void RetrieveShapes()
        {
            await connection.StartAsync();
            var shapes = await connection.InvokeAsync<IEnumerable<ShapeData>>("RetrieveShapes");
            foreach (var s in shapes)
                AddShape(s);
            DrawingPanel.Invalidate();
        }

        Shape AddShape(ShapeData s)
        {
            Shape shape = null;
            switch (s.Type)
            {
                case ShapeData.Types.Line:
                    shape = new Line();
                    break;
                case ShapeData.Types.Rectangle:
                    shape = new Rectangle();
                    break;
                case ShapeData.Types.Ellipse:
                    shape = new Ellipse();
                    break;
                default:
                    return null;
            }
            shape.Data = s;
            Shapes[shape.Id] = shape;
            return shape;
        }


        void SelectTool(ToolStripItem tool)
        {
            foreach (var t in Tools)
                if (t.Checked = t == tool)
                    SelectedTool = t;

            if (SelectedTool != null)
                SelectedTool.Checked = true;
        }


        private async void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedTool == LineTool)
                    SelectedShape = new Line();
                else if (SelectedTool == RectangleTool)
                    SelectedShape = new Rectangle();
                else if (SelectedTool == EllipseTool)
                    SelectedShape = new Ellipse();
                SelectedShape.X1 = SelectedShape.X2 = e.X;
                SelectedShape.Y1 = SelectedShape.Y2 = e.Y;
                SelectedShape.LineColor = FgColorButton.SelectedColor;
                DrawingPanel.Invalidate();
                var addedShape = SelectedShape;
                var id = await connection.InvokeAsync<int>("AddShape", addedShape.Data);
                addedShape.Id = id;
                Shapes.Add(id, addedShape);
            }
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedShape != null)
                {
                    SelectedShape.X2 = e.X;
                    SelectedShape.Y2 = e.Y;
                    if (SelectedShape.Id > 0)
                        connection.InvokeAsync<int>("UpdateShape", SelectedShape.Data);
                    DrawingPanel.Invalidate();
                }
            }
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {

            foreach (var s in Shapes)
            {
                s.Value.Draw(e.Graphics);
            }

            if (SelectedShape != null && SelectedShape.Id == 0)
                SelectedShape.Draw(e.Graphics);

        }

        private void DrawingToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectTool(e.ClickedItem);
        }
    }
}
