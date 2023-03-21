using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppLab3
{
  

    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer moveTimer = new System.Windows.Forms.Timer();

        // Все про точки
        List<Point> points = new List<Point>();
        public List<Point> pointsOffset = new List<Point>();
        public List<Point> pointsTrack = new List<Point>();

        public Size PointSize { get; set; } = new Size(5, 5);
        public Color PointColor { get; set; } = Color.DarkRed;

        public bool flagDraw = false;
        public bool flagDragnDrop = false;
        public int numberPoint;
        public bool flagMoove = false;
        public bool flagTrack = false;
        public int pictureBoxWidth { get; set; }


        public LineType LineTypePointConnect; 
        public int LineWidth { get; set; } = 3;      
        public enum LineType { None, Curve, Bezier, Polygone, FilledCurve}; 
        


        public Form1()
        {
            InitializeComponent();
            DoubleBuffered= true;
            Paint += Form1_Paint;


            moveTimer.Interval = 30;
            moveTimer.Tick += MoveTimer_Tick;
            pictureBoxWidth = pictureBox1.Width;

            MouseClick += Form1_MouseClick;

            MouseMove += Form1_MouseMove;
            MouseUp += Form1_MouseUp;
            MouseDown += Form1_MouseDown;

            KeyPreview = true;
            KeyDown += Form1_KeyDown;

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                    {
                        int x = 0, y = 0;
                        for (int i = 0; i < pointsOffset.Count; i++)
                        {
                            if (pointsOffset[i].X > 0)
                                x = 1;
                            if (pointsOffset[i].X < 0)
                                x = -1;
                            if (pointsOffset[i].Y > 0)
                                y = 1;
                            if (pointsOffset[i].Y < 0)
                                y = -1;
                            pointsOffset[i] = new Point(pointsOffset[i].X + x, pointsOffset[i].Y + y);
                        }
                        e.Handled = true;
                    }
                    break;
                case Keys.Subtract:
                    {
                        int x = 0, y = 0;
                        for (int i = 0; i < pointsOffset.Count; i++)
                        {
                            if (pointsOffset[i].X > 1)
                                x = -1;
                            if (pointsOffset[i].X < -1)
                                x = 1;
                            if (pointsOffset[i].Y > 1)
                                y = -1;
                            if (pointsOffset[i].Y < -1)
                                y = 1;
                            pointsOffset[i] = new Point(pointsOffset[i].X + x, pointsOffset[i].Y + y);
                        }
                        e.Handled = true;
                    }
                    break;
                case Keys.Escape:
                    button8.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Space:
                    button7.PerformClick();
                    e.Handled = true;
                    break;
               default: 
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool ret = false;
            if (LineTypePointConnect != LineType.None && flagMoove)
            {
                switch (keyData)
                {
                    case Keys.Left:
                        {     
                            for (int i = 0; i < pointsOffset.Count; i++)
                            {
                                if (pointsOffset.Min(a => a.X) == pictureBoxWidth)
                                    break;
                                pointsOffset[i] = new Point(pointsOffset[i].X - 1, pointsOffset[i].Y);
                            }
                            Refresh();
                        }
                        ret = true;
                        break;
                    case Keys.Right:
                        {
                            for (int i = 0; i < pointsOffset.Count; i++)
                            {
                                if (pointsOffset.Max(a => a.X) == this.ClientRectangle.Width - LineWidth)
                                    break;
                                pointsOffset[i] = new Point(pointsOffset[i].X + 1, pointsOffset[i].Y);
                            }
                            Refresh();
                        }
                        ret = true;
                        break;
                    case Keys.Up:
                        {       
                            for (int i = 0; i < pointsOffset.Count; i++)
                            {
                                if (pointsOffset.Min(a => a.Y) == 0)
                                    break;
                                pointsOffset[i] = new Point(pointsOffset[i].X, pointsOffset[i].Y - 1);
                            }
                            Refresh();
                        }
                        ret = true;
                        break;
                    case Keys.Down:
                        {
                            
                            for (int i = 0; i < pointsOffset.Count; i++)
                            {
                                if (pointsOffset.Max(a => a.Y) == this.ClientRectangle.Height - LineWidth)
                                    break;
                                pointsOffset[i] = new Point(pointsOffset[i].X, pointsOffset[i].Y + 1);
                            }
                            Refresh();
                        }
                        ret = true;
                        break;
                    default:
                        ret = false;
                        break;
                }
            }
            return ret;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (IsLocated(points[i], e.Location))
                {
                    flagDragnDrop = true;
                    numberPoint = i;
                    break;
                }
            }
        }

        bool IsLocated(Point list, Point loc )
        {
            if (loc.X >= list.X - PointSize.Width &&
               loc.X <= list.X + PointSize.Width &&
               loc.Y >= list.Y - PointSize.Height &&
               loc.Y <= list.Y + PointSize.Height)
                return true;
            else
                return false;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            flagDragnDrop = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (flagDragnDrop)
            {
                points[numberPoint] = new Point(e.Location.X, e.Location.Y);
                Refresh();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p;
            if (e.X > pictureBox1.Left && !flagDragnDrop)
            {
                p = e.Location;
                p.X -= PointSize.Width / 2; p.Y -= PointSize.Height / 2;
                if (flagDraw)
                {
                    points.Add(p);
                    //pointsTrack.Add(p);
                    LineTypePointConnect = LineType.None;

                    button5.Enabled = points.Count > 3 && (points.Count - 1) % 3 == 0 ? true : false;

                    if (points.Count >= 3)
                    {
                        button3.Enabled = true;
                        button6.Enabled = true;
                    }

                    if (points.Count >= 2)
                    {
                        button4.Enabled = true;
                    }

                    button7.Enabled = true;
                    button8.Enabled = true;
                    Refresh();
                }
            }

            if (flagDragnDrop) flagDragnDrop = !flagDragnDrop;
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            GetOffsetPoints();
            Refresh();
        }

        void GetOffsetPoints()
        {
            int x, y, xTr, yTr;
            for (int i = 0; i < points.Count; i++)
            {
                x = points[i].X + pointsOffset[i].X;
                xTr = points[i].X - pointsOffset[i].X;
                if (x >= this.ClientRectangle.Width || x <= pictureBox1.Left)
                {
                    pointsOffset[i] = new Point(-pointsOffset[i].X, pointsOffset[i].Y);
                    x = points[i].X + pointsOffset[i].X;
                    xTr = points[i].X - pointsOffset[i].X;
                }

                y = points[i].Y + pointsOffset[i].Y;
                yTr = points[i].Y - pointsOffset[i].Y;
                if (y >= this.ClientRectangle.Height || y <= 0)
                {
                    pointsOffset[i] = new Point(pointsOffset[i].X, -pointsOffset[i].Y);
                    y = points[i].Y + pointsOffset[i].Y;
                    yTr = points[i].Y - pointsOffset[i].Y;
                }
                //pointsTrack[i] = new Point(points[i].X, points[i].Y);
                
                pointsTrack.Add(new Point(points[i].X -1, points[i].Y-1));
                points[i] = new Point(x, y);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (points.Count > 0)
            {
                DrawPoints(g);
                if (LineTypePointConnect != LineType.None)
                {
                    if (flagTrack && flagMoove)
                    {
                        ShowLine(g, LineType.Polygone, pointsTrack, Color.DarkBlue);
                    }
                    ShowLine(g, LineTypePointConnect, points, Color.LightGreen);
                }
            }
        }

        void ShowLine(Graphics g, LineType line, List<Point> pList, Color LineColor)
        {
            Pen pen = new Pen(new SolidBrush(LineColor), LineWidth);
            
            switch (line)
            {
                case LineType.Bezier:
                    if ((pList.Count - 1) % 3 == 0 ? true : false)
                    { g.DrawBeziers(pen, pList.ToArray()); }
                    break;
                case LineType.Curve:
                    g.DrawClosedCurve(pen, pList.ToArray());
                    break;
                case LineType.FilledCurve:
                    g.FillClosedCurve(new SolidBrush(LineColor), pList.ToArray());
                    break;
                case LineType.Polygone:
                    g.DrawPolygon(pen, pList.ToArray());
                    break;
                default:
                    break;
            }
            
        }

        private void DrawPoints(Graphics g)
        {
            foreach (var p in points)
                g.FillEllipse(new SolidBrush(PointColor), p.X, p.Y, PointSize.Width, PointSize.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            flagDraw = !flagDraw; 
            flagDragnDrop = false;
            flagTrack = false;

            if (flagMoove)
            {
                moveTimer.Stop();
                flagMoove = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 paramsForm = new Form2(this);
            paramsForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LineTypePointConnect = LineTypePointConnect != LineType.Curve ? LineType.Curve : LineType.None;
            Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LineTypePointConnect = LineTypePointConnect != LineType.Polygone ? LineType.Polygone : LineType.None;
            Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            moveTimer.Stop();
            flagDraw = false;
            flagDragnDrop = false;
            flagMoove = false;
            points.Clear();
            pointsOffset.Clear();
            pointsTrack.Clear();
            flagTrack = false;
            PointColor = Color.DarkRed;
            LineWidth = 3;
            LineTypePointConnect = LineType.None;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (points.Count > 3 && (points.Count - 1) % 3 == 0 ? true : false)
            LineTypePointConnect = LineTypePointConnect != LineType.Bezier ? LineType.Bezier : LineType.None;
            Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LineTypePointConnect = LineTypePointConnect != LineType.FilledCurve ? LineType.FilledCurve : LineType.None;
            Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button9.Enabled = true;
            if (flagDraw)
                button1.PerformClick();
            flagMoove = !flagMoove;
            
            if (flagMoove)
            {
                pointsOffset = new List<Point>();
                int x, y;
                Random rnd = new Random((int)DateTime.Now.Ticks);
                x = rnd.Next(1, 5);
                y = rnd.Next(1, 5);
                for (int i = 0; i < points.Count; i++)
                {
                    pointsOffset.Add(new Point(x, y));
                }
                moveTimer.Start();
            }
            else
                moveTimer.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            flagTrack = !flagTrack;

            pointsTrack.Clear();

        }
    }
}
