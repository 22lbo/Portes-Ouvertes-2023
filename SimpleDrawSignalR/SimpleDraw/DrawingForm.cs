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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace SimpleDraw
{
    public partial class DrawingForm : Form
    {
        HubConnection connection;

        Mutex mutex = new Mutex();

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
                connection.On<ShapeData>("ShapeAdded", (s) =>
                {
                    AddShape(s);
                    DrawingPanel.Invalidate();
                });

                connection.On<ShapeData>("ShapeRemoved", (s) =>
                {
                    mutex.WaitOne();
                    Shapes.Remove(s.Id);
                    mutex.ReleaseMutex();
                    DrawingPanel.Invalidate();
                });

                connection.On<ShapeData>("ShapeUpdated", (s) =>
                {
                    mutex.WaitOne();
                    if (Shapes.TryGetValue(s.Id, out var shape))
                    {
                        shape.Data = s;
                        DrawingPanel.Invalidate();
                    }
                    mutex.ReleaseMutex();
                });
            }

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
            DrawingPanel.Image = Image.FromFile("../../../Assets/loading.gif");
            DrawingPanel.SizeMode = PictureBoxSizeMode.CenterImage;
            await connection.StartAsync();
            var shapes = await connection.InvokeAsync<IEnumerable<ShapeData>>("RetrieveShapes");
            mutex.WaitOne();
            foreach (var s in shapes)
                AddShape(s);
            mutex.ReleaseMutex();
            DrawingPanel.Invalidate();
            DrawingPanel.Image = null;
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
                case ShapeData.Types.Triangle:
                    shape = new Triangle();
                    break;
                case ShapeData.Types.BrushStroke:
                    shape = new BrushStroke();
                    break;
                default:
                    return null;
            }
            shape.Data = s;
            mutex.WaitOne();
            Shapes[shape.Id] = shape;
            mutex.ReleaseMutex();
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

        bool keyPressed = false;
        bool shapeToRemoveSelected = false;
        private void DrawingPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                keyPressed = true;

            if (shapeToRemoveSelected == true)
            {
                if (SelectedShape != null)
                {
                    var removedShape = SelectedShape;
                    connection.InvokeAsync("RemoveShape", removedShape.Data);
                    mutex.WaitOne();
                    Shapes.Remove(removedShape.Id, out removedShape);
                    mutex.ReleaseMutex();
                    DrawingPanel.Invalidate();
                }
                keyPressed = false;
            }

        }

        private void DrawingPanel_MouseEnter(object sender, EventArgs e)
        {
            DrawingPanel.Focus();
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
                    if (keyPressed == true)
                    {
                        SelectedShape = SelectShapeModify(e.X, e.Y);
                        shapeToRemoveSelected = true;
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
                    else if (SelectedTool == BrushTool || SelectedTool == EraserTool)
                        SelectedShape = new BrushStroke();

                    SelectedShape.X1 = SelectedShape.X2 = e.X;
                    SelectedShape.Y1 = SelectedShape.Y2 = e.Y;
                    if (SelectedTool == EraserTool)
                        SelectedShape.LineColor = Color.White;
                    else
                        SelectedShape.LineColor = FgColorButton.SelectedColor;
                    if (SelectedTool == EllipseTool || SelectedTool == RectangleTool || SelectedTool == TriangleTool)
                        SelectedShape.FillColor = BucketColorButton.SelectedColor;
                    DrawingPanel.Invalidate();
                    var addedShape = SelectedShape;
                    var id = await connection.InvokeAsync<int>("AddShape", addedShape.Data);
                    addedShape.Id = id;
                    mutex.WaitOne();
                    Shapes.Add(id, addedShape);
                    mutex.ReleaseMutex();
                }
            }
        }

        private async void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedShape != null && e.Button == MouseButtons.Left)
            {
                if (SelectedTool == SelectBtn)
                {
                    MoveShape(e.X - prevMouseX, e.Y - prevMouseY, SelectedPoint);
                    prevMouseX = e.X;
                    prevMouseY = e.Y;
                }
                else if (SelectedTool == BrushTool || SelectedTool == EraserTool)
                {
                    SelectedShape = new BrushStroke();
                    SelectedShape.X1 = e.X;
                    SelectedShape.Y1 = e.Y;
                    if (SelectedTool == EraserTool)
                        SelectedShape.LineColor = Color.White;
                    else
                        SelectedShape.LineColor = FgColorButton.SelectedColor;
                    SelectedShape.SZ = Convert.ToInt32(SizeUpDown.Value);
                    var addedShape = SelectedShape;
                    var id = await connection.InvokeAsync<int>("AddShape", addedShape.Data);
                    addedShape.Id = id;
                    mutex.WaitOne();
                    Shapes.Add(id, addedShape);
                    mutex.ReleaseMutex();
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
            mutex.WaitOne();
            foreach (var s in Shapes)
                s.Value.Draw(e.Graphics);
            mutex.ReleaseMutex();

            if (SelectedShape != null)
            {
                if (SelectedShape.Id == 0)
                    SelectedShape.Draw(e.Graphics);

                if (SelectedTool != BrushTool && SelectedTool != EraserTool && SelectedTool != TriangleTool)
                {
                    DrawHandle(e.Graphics, SelectedShape.X1, SelectedShape.Y1);
                    DrawHandle(e.Graphics, SelectedShape.X2, SelectedShape.Y2);
                }
            }
        }

        private void DrawingToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SelectTool(e.ClickedItem);
        }

        private Shape SelectShapeModify(int x, int y)
        {
            Shape item = null;
            mutex.WaitOne();
            for (int i = Shapes.Count - 1; i >= 0; i--)
            {
                item = Shapes.Values.ElementAt(i);
                if (item.IsHit(x, y))
                    break;
            }
            mutex.ReleaseMutex();
            return item;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            mutex.WaitOne();
            foreach (var s in Shapes.Values)
            {
                Shapes.Remove(s.Id);
                connection.InvokeAsync("RemoveShape", s.Data);
            }
            mutex.ReleaseMutex();
            Shapes.Clear();
            DrawingPanel.Invalidate();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void EraserTool_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
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
