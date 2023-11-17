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
using System.Drawing.Drawing2D;

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
            var ServerUrl = Properties.Settings.Default.ServerUrl;

            connection = new HubConnectionBuilder()
                .WithUrl($"{ServerUrl}/DrawingHub")
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

                connection.On<ShapeData>("ShapeRemoved", (s) =>
                {
                    Shapes.Remove(s.Id);
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
            Tools.Add(TriangleTool);
            Tools.Add(BrushTool);
            Tools.Add(SelectBtn);
            Tools.Add(EraserTool);
            SelectTool(LineTool);

            RetrieveShapes();
        }

        async void RetrieveShapes()
        {
            DrawingPanel.BackgroundImage = Image.FromFile("../../../Assets/loading.gif");
            DrawingPanel.BackgroundImageLayout = ImageLayout.Center;
            await connection.StartAsync();
            var shapes = await connection.InvokeAsync<IEnumerable<ShapeData>>("RetrieveShapes");
            foreach (var s in shapes)
                AddShape(s);
            DrawingPanel.Invalidate();
            DrawingPanel.BackgroundImage = null;
        }

        Shape AddShape(ShapeData s)
        {
            Shape shape = null;
            switch (s.Type)
            {
                case ShapeData.Types.Line:
                    if (SelectedTool == LineTool)
                        shape = new Line();
                    else if (SelectedTool == TriangleTool)
                        shape = new Triangle();
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

        private int prevMouseX = 0;
        private int prevMouseY = 0;

        public int SelectedPoint { get; set; } = 0;

        private bool IsHandleHit(int px, int py, int x, int y)
        {
            int ofs = (handleSize / 2) + 1;
            return (Math.Abs(x - px) <= ofs) && (Math.Abs(y - py) <= ofs);
        }

        private void MoveShape(int x, int y, int point)
        {
            if (point == 1)
            {
                SelectedShape.X1 += x;
                SelectedShape.Y1 += y;
            }
            if (point == 2)
            {
                SelectedShape.X2 += x;
                SelectedShape.Y2 += y;
            }

        }

        private async void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedTool == SelectBtn)
                {
                    SelectedPoint = 0;
                    if (SelectedShape != null)
                    {
                        if (IsHandleHit(SelectedShape.X1, SelectedShape.Y1, e.X, e.Y))
                            SelectedPoint = 1;
                        else if (IsHandleHit(SelectedShape.X2, SelectedShape.Y2, e.X, e.Y))
                            SelectedPoint = 2;
                    }
                    if (SelectedPoint == 0)
                        SelectedShape = SelectShapeModify(e.X, e.Y);
                    prevMouseX = e.X;
                    prevMouseY = e.Y;
                }
                else if (SelectedTool == EraserTool)
                {
                    SelectedShape = SelectShapeRemove(e.X, e.Y);
                    if (SelectedShape != null)
                    {
                        var removedShape = SelectedShape;
                        connection.InvokeAsync("RemoveShape", removedShape.Data);
                        Shapes.Remove(removedShape.Id, out removedShape);
                        DrawingPanel.Invalidate();
                    }
                }
                else
                {
                    if (SelectedTool == LineTool)
                        SelectedShape = new Line();
                    else if (SelectedTool == RectangleTool)
                        SelectedShape = new Rectangle();
                    else if (SelectedTool == EllipseTool)
                        SelectedShape = new Ellipse();
                    else if (SelectedTool == TriangleTool)
                        SelectedShape = new Triangle();
                    else if (SelectedTool == BrushTool)
                        SelectedShape = new BrushStroke();

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
        }

        private async void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedShape != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (SelectedTool == SelectBtn)
                        {
                            MoveShape(e.X - prevMouseX, e.Y - prevMouseY, SelectedPoint);
                            prevMouseX = e.X;
                            prevMouseY = e.Y;
                        }
                        else if (SelectedTool == BrushTool)
                        {
                            SelectedShape = new BrushStroke();
                            SelectedShape.X1 = e.X;
                            SelectedShape.Y1 = e.Y;
                            var addedShape = SelectedShape;
                            var id = await connection.InvokeAsync<int>("AddShape", addedShape.Data);
                            addedShape.Id = id;
                            Shapes.Add(id, addedShape);
                        }
                        else
                        {
                            SelectedShape.X2 = e.X;
                            SelectedShape.Y2 = e.Y;
                        }
                        if (SelectedShape.Id > 0)
                            connection.InvokeAsync<int>("UpdateShape", SelectedShape.Data);
                        DrawingPanel.Invalidate();
                    }
                }
            }
        }

        const int handleSize = 8;
        private void DrawHandle(Graphics g, int x, int y)
        {
            var hx = x - handleSize / 2;
            var hy = y - handleSize / 2;
            g.FillRectangle(Brushes.White, hx, hy, handleSize, handleSize);
            g.DrawRectangle(Pens.Black, hx, hy, handleSize, handleSize);
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var s in Shapes)
                s.Value.Draw(e.Graphics);

            if (SelectedShape != null && SelectedShape.Id == 0)
                SelectedShape.Draw(e.Graphics);

            if (SelectedShape != null && SelectedTool != BrushTool)
            {
                DrawHandle(e.Graphics, SelectedShape.X1, SelectedShape.Y1);
                DrawHandle(e.Graphics, SelectedShape.X2, SelectedShape.Y2);
            }
        }

        private void DrawingToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectTool(e.ClickedItem);
        }

        private Shape SelectShapeModify(int x, int y)
        {
            for (int i = Shapes.Count - 1; i >= 0; i--)
            {
                var item = Shapes.Values.ElementAt(i);
                if (item.IsHit(x, y))
                    return item;
            }
            return null;
        }

        private Shape SelectShapeRemove(int x, int y)
        {
            foreach (var s in Shapes.Values)
                if (s.IsHit(x, y))
                    return s;
            return null;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            foreach (var s in Shapes.Values)
            {
                Shapes.Remove(s.Id);
                connection.InvokeAsync("RemoveShape", s.Data);
            }
            DrawingPanel.Invalidate();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void EraserTool_Click(object sender, EventArgs e)
        {
            Cursor erase = new Cursor("../../../Assets/Eraser.cur");
            Cursor = erase;
        }

        private void LineTool_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void RectangleTool_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void EllipseTool_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void BrushTool_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }
    }
}
