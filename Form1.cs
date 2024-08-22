using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace 寻路
{
    public partial class Form1 : Form
    {
        Point start;
        Point end;
        int sleep = 0;
        int size = 10;

        int[,] map = new int[,] {
                { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,0},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0 ,3} };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        void run()
        {
            for (int x = 0; x < map.GetUpperBound(1) + 1; x++)
            {
                for (int y = 0; y < map.GetUpperBound(0) + 1; y++)
                {
                    switch (map[y, x])
                    {
                        case 1:
                            draw(x, y, Color.Gray);
                            break;
                        case 2:
                            draw(x, y, Color.Red);
                            start = new Point(x, y);
                            break;
                        case 3:
                            draw(x, y, Color.Yellow);
                            end = new Point(x, y);
                            break;
                        default:
                            draw(x, y, Color.White);
                            break;
                    }
                }
            }
        }
        void w()
        {
            Graphics g = this.CreateGraphics();
            Pen pen = new Pen(Color.Black, 1);
            for (int i = 0; i < map.GetLength(0) + 1; i++)
            {
                g.DrawLine(pen, new Point(0, i * size), new Point(map.GetLength(1) * size, i * size));
            }
            for (int i = 0; i < map.GetLength(1) + 1; i++)
            {
                g.DrawLine(pen, new Point(i * size, 0), new Point(i * size, map.GetLength(0) * size));
            }
        }
        public void draw(int x, int y, Color color)
        {
            Graphics g = this.CreateGraphics();
            SolidBrush pen = new SolidBrush(color);
            Rectangle rect = new Rectangle(x * size, y * size, size, size);
            g.FillRectangle(pen, rect);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            run();
            w();
        }

        #region 广度优先
        private void button2_Click(object sender, EventArgs e)
        {
            run();
            w();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//跨线程访问

            Thread thread = new Thread(breadfirst);
            thread.IsBackground = true;
            thread.Start();

        }

        public void breadfirst()
        {
            Queue<Point> queue = new Queue<Point>();
            //HashSet<Point> set = new HashSet<Point>();
            Dictionary<Point, Point> camefrom = new Dictionary<Point, Point>();
            queue.Enqueue(start);
            camefrom[start] = new Point(-1, -1);
            bool hasroute = false;

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();
                Thread.Sleep(sleep);
                draw(current.X, current.Y, Color.Blue);

                if (current == end)
                {
                    hasroute = true;
                    break;
                }
                //四个方向
                foreach (Point offset in new Point[] { new Point(-1, 0), new Point(0, 1), new Point(1, 0), new Point(0, -1) })
                {
                    Point newpos = new Point(current.X + offset.X, current.Y + offset.Y);
                    if (newpos.X < 0 || newpos.Y < 0 || newpos.X >= map.GetUpperBound(1) + 1 || newpos.Y >= map.GetUpperBound(0) + 1)
                    {
                        continue;
                    }
                    if (camefrom.ContainsKey(newpos))
                    {
                        continue;
                    }
                    if (map[newpos.Y, newpos.X] == 1)
                    {
                        continue;
                    }
                    queue.Enqueue(newpos);
                    camefrom[newpos] = current;
                }
            }
            if (hasroute)
            {
                Stack<Point> trace = new Stack<Point>();
                Point pos = end;
                while (camefrom.ContainsKey(pos))
                {
                    trace.Push(pos);
                    pos = camefrom[pos];
                }
                while (trace.Count > 0)
                {
                    Point p = trace.Pop();
                    Thread.Sleep(sleep);

                    draw(p.X, p.Y, Color.Green);
                }
            }
            w();
        }

        #endregion


        #region 迪杰斯特拉
        private void button3_Click(object sender, EventArgs e)
        {
            run();
            w();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//跨线程访问

            Thread thread = new Thread(dijkstra);
            thread.IsBackground = true;
            thread.Start();

        }

        public int manhattan(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public void dijkstra()
        {
            //Queue<Point> queue = new Queue<Point>();
            //HashSet<Point> set = new HashSet<Point>();
            List<Point> sortlist = new List<Point>();
            Dictionary<Point, Point> camefrom = new Dictionary<Point, Point>();
            sortlist.Add(start);
            camefrom[start] = new Point(-1, -1);
            bool hasroute = false;

            while (sortlist.Count > 0)
            {
                sortlist.Sort((Point a, Point b) =>
                {
                    return manhattan(start, a) - manhattan(start, b);
                });
                Point current = sortlist[0];
                sortlist.RemoveAt(0);

                Thread.Sleep(sleep);
                draw(current.X, current.Y, Color.Blue);

                if (current == end)
                {
                    hasroute = true;
                    break;
                }
                //四个方向
                foreach (Point offset in new Point[] { new Point(-1, 0), new Point(0, 1), new Point(1, 0), new Point(0, -1) })
                {
                    Point newpos = new Point(current.X + offset.X, current.Y + offset.Y);
                    if (newpos.X < 0 || newpos.Y < 0 || newpos.X >= map.GetUpperBound(1) + 1 || newpos.Y >= map.GetUpperBound(0) + 1)
                    {
                        continue;
                    }
                    if (camefrom.ContainsKey(newpos))
                    {
                        continue;
                    }
                    if (map[newpos.Y, newpos.X] == 1)
                    {
                        continue;
                    }
                    sortlist.Add(newpos);
                    camefrom[newpos] = current;
                }
            }
            if (hasroute)
            {
                Stack<Point> trace = new Stack<Point>();
                Point pos = end;
                while (camefrom.ContainsKey(pos))
                {
                    trace.Push(pos);
                    pos = camefrom[pos];
                }
                while (trace.Count > 0)
                {
                    Point p = trace.Pop();
                    Thread.Sleep(sleep);

                    draw(p.X, p.Y, Color.Green);
                }
            }
            w();
        }

        #endregion


        #region A星
        private void button4_Click(object sender, EventArgs e)
        {
            run();
            w();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//跨线程访问

            Thread thread = new Thread(A);
            thread.IsBackground = true;
            thread.Start();

        }

        public int manhattans(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public void A()
        {
            //Queue<Point> queue = new Queue<Point>();
            //HashSet<Point> set = new HashSet<Point>();
            List<Point> sortlist = new List<Point>();
            Dictionary<Point, Point> camefrom = new Dictionary<Point, Point>();
            sortlist.Add(start);
            camefrom[start] = new Point(-1, -1);
            bool hasroute = false;

            while (sortlist.Count > 0)
            {
                sortlist.Sort((Point a, Point b) =>
                {
                    return (manhattans(start, a) + manhattans(end, a)) - (manhattans(start, b) + manhattans(end, b));
                });
                Point current = sortlist[0];
                sortlist.RemoveAt(0);

                Thread.Sleep(sleep);
                draw(current.X, current.Y, Color.Blue);

                if (current == end)
                {
                    hasroute = true;
                    break;
                }
                //四个方向
                foreach (Point offset in new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, 1), new Point(0, -1) })
                {
                    Point newpos = new Point(current.X + offset.X, current.Y + offset.Y);
                    if (newpos.X < 0 || newpos.Y < 0 || newpos.X >= map.GetUpperBound(1) + 1 || newpos.Y >= map.GetUpperBound(0) + 1)
                    {
                        continue;
                    }
                    if (camefrom.ContainsKey(newpos))
                    {
                        continue;
                    }
                    if (map[newpos.Y, newpos.X] == 1)
                    {
                        continue;
                    }
                    sortlist.Add(newpos);
                    camefrom[newpos] = current;
                }
            }
            if (hasroute)
            {
                Stack<Point> trace = new Stack<Point>();
                Point pos = end;
                while (camefrom.ContainsKey(pos))
                {
                    trace.Push(pos);
                    pos = camefrom[pos];
                }
                while (trace.Count > 0)
                {
                    Point p = trace.Pop();
                    Thread.Sleep(sleep);

                    draw(p.X, p.Y, Color.Green);
                }
            }
            w();
        }

        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            run();
            w();
        }

        private void down(object sender, MouseEventArgs e)
        {
            int x = (int)Math.Floor((double)(e.X / size));
            int y = (int)Math.Floor((double)(e.Y / size));
            if (x <= map.GetLength(0)-1 && y <= map.GetLength(1)-1)
            {
                if (map[y, x] == 0)
                    map[y, x] = 1;
                else if (map[y, x] == 1)
                    map[y, x] = 0;
                run();
                w();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
