using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Mathcad;

namespace SCOI_lab_1
{
    public class MyCanvas : Control
    {
        public List<Point> points = new List<Point>() { new Point(0,0), new Point(255, 255) };

        private Point currentPoint;
        private bool PaintComplete = false;
        public bool cmbbChange = false;
        List<int> iter = new List<int>() { 0 };

        public Image img { get; set; }
        public PictureBox pchB { get; set; }
        public Chart chrt { get; set; }
        public ComboBox cmbb { get; set; }

        //Таймер для ее обновления
        private Timer timer;

        //битмапы на которых будем рисовать в 2 слоя.
        //На первом будет само содержаение
        //На втором курсор.
        private Bitmap layer1;
        private Bitmap layer2;

        //Графиксы для этих битмапов
        private Graphics g_layer1;
        private Graphics g_layer2;

        private bool painting_mode = false;

        Pen pen = new Pen(Color.FromArgb(255, 0, 80, 0), 2);

        public MyCanvas()
        {
            //Включаем режим двойной буферизации, чтобы рисовка не мерцала.
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            //Опеределяем в нашей канве события
            this.Paint += MyCanvas_Paint;
            this.MouseDown += MyCanvas_MouseDown;
            this.MouseUp += MyCanvas_MouseUp;
            this.MouseMove += MyCanvas_MouseMove;

            this.SizeChanged += MyCanvas_SizeChanged;

            //Запускаем таймер на перерисовку
            timer = new Timer();
            timer.Interval = 25;
            timer.Tick += (s, a) => this.Refresh();
            timer.Start();
        }
        ~MyCanvas()
        {

            if (g_layer1 != null)
                layer1.Dispose();
            if (g_layer2 != null)
                layer2.Dispose();

            if (layer1 != null)
                layer1.Dispose();
            if (layer2 != null)
                layer2.Dispose();

            timer.Dispose();
            pen.Dispose();
        }

        private void MyCanvas_SizeChanged(object sender, EventArgs e)
        {
            var _sender = sender as MyCanvas;

            //При изменении размера у нас должны пересоздатся битмапы (так как нельзя изменить
            // размер битмапа во время работы)
            //По этому мы сначала создаем новые, если старые есть (при создании конвы их нет,
            //вот тут они и создадутся при первом отображении) - рисуем их содержимое на новых, удаляем старые.

            Bitmap new_layer1 = new Bitmap(_sender.Size.Width, _sender.Size.Height, PixelFormat.Format32bppArgb);
            Bitmap new_layer2 = new Bitmap(_sender.Size.Width, _sender.Size.Height, PixelFormat.Format32bppArgb);
            Graphics new_g_layer1 = Graphics.FromImage(new_layer1);
            Graphics new_g_layer2 = Graphics.FromImage(new_layer2);

            if (g_layer1 != null)
            {
                new_g_layer1.DrawImageUnscaled(layer1, 0, 0);
                layer1.Dispose();
            }
            if (layer1 != null)
                layer1.Dispose();


            if (g_layer2 != null)
                layer2.Dispose();
            if (layer2 != null)
                layer2.Dispose();

            layer1 = new_layer1;
            g_layer1 = new_g_layer1;
            layer2 = new_layer2;
            g_layer2 = new_g_layer2;

            LinearGradientBrush linGrBrushVert = new LinearGradientBrush(
                new Point(0, this.Size.Height),
                new Point(this.Size.Width, this.Size.Height - 5),
                Color.FromArgb(255, 0, 0, 0),
                Color.FromArgb(255, 255, 255, 255));

            LinearGradientBrush linGrBrushGor = new LinearGradientBrush(
                new Point(0, this.Size.Height),
                new Point(5, 0),
                Color.FromArgb(255, 0, 0, 0),
                Color.FromArgb(255, 255, 255, 255));

            g_layer1.FillRectangle(linGrBrushVert, new Rectangle(0, this.Size.Height - 5, this.Size.Width, 5));
            g_layer1.FillRectangle(linGrBrushGor, new Rectangle(0, 0, 5, this.Size.Height));
        }
        private void MyCanvas_Paint(object sender, PaintEventArgs e)
        {

            //Всегда рисуем зеленый кружок под мышкой, чтобы видеть как будет рисоватся линия.

            var mouse_pos = PointToClient(MousePosition);
            int r = 2;
            g_layer2.Clear(Color.FromArgb(0, 0, 0, 0));
            g_layer2.DrawEllipse(pen, mouse_pos.X - r / 2, mouse_pos.Y - r / 2, r, r);

            points = points.OrderBy(p => p.X).ToList();
            for (int i = 0; i < points.Count - 1; ++i)
                if (points[i].X == points[i + 1].X)
                {

                    if (currentPoint == points[i + 1])
                    {
                        Point tmp = new Point(points[i + 1].X + 1, points[i + 1].Y);
                        currentPoint = new Point(tmp.X, tmp.Y);
                        points[i + 1] = tmp;
                    }
                    else
                    {
                        Point tmp = new Point(points[i].X - 1, points[i].Y);
                        currentPoint = new Point(tmp.X, tmp.Y);
                        points[i] = tmp;
                    }

                }

            List<int> Values = new List<int>();

            switch (cmbb.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < points.Count - 1; ++i)
                    {
                        Point one = PointToMine(points[i]);
                        Point two = PointToMine(points[i + 1]);
                        g_layer2.DrawLine(pen, one, two);
                    }
                    for (int i = 0; i < points.Count - 1; ++i)
                    {
                        float dy = (float)(points[i + 1].Y - points[i].Y) / (points[i + 1].X - points[i].X);
                        for (int v = 0; v < points[i + 1].X - points[i].X; ++v)
                        {
                            Values.Add(points[i].Y + (int)(dy * v));
                        }
                    }
                    Values.Add(points[points.Count - 1].Y);
                    break;

                case 1:
                    QuadProc(Values);
                    break;
                case 2:
                    CubProc(Values);
                    break;
                case 3:
                    LagProc(Values);
                    break;
                case 4:
                    NewtProc(Values);
                    break;
                case 5:
                    if (points.Count > 13)
                        for (int i = 1; i < points.Count - 12; ++i)
                            points.RemoveAt(i);
                    BezierProc(Values);
                    break;
            }

            Size pointSize = new Size(20, 20);

            for (int i = 0; i < points.Count; ++i)
            {
                Point point = PointToMine(points[i]);
                g_layer2.FillRectangle(Brushes.Red, new Rectangle(new Point(point.X - pointSize.Width / 2, point.Y - pointSize.Height / 2), pointSize));
            }

            e.Graphics.DrawImageUnscaled(layer1, 0, 0);
            e.Graphics.DrawImageUnscaled(layer2, 0, 0);

            if ((painting_mode || cmbbChange) && img != null && chrt != null)
            {

                (DataTable, Image) result;

                if (img.Width > 300)
                {
                    Size Probe = new Size(300, (int)Math.Round((decimal)img.Height * ((decimal)300.0/img.Width), 0));
                    result = MyImage.ProcessAndBarGraphData(new Bitmap(img, Probe), Values);
                }
                else result = MyImage.ProcessAndBarGraphData(new Bitmap(img), Values);

                pchB.Image.Dispose();
                pchB.Image = result.Item2;

                chrt.DataSource = result.Item1;
                chrt.DataBind();
            }

            if (((!painting_mode && !PaintComplete) || cmbbChange) && img != null && chrt != null)
            {
                PaintComplete = true;

                iter.Add(iter.Last() + 1);

                ProcessAsync(new Bitmap(img), Values, iter.Last());
            }
        }
        private async void ProcessAsync(Image img, List<int> list, int i)
        {
            var rez = await Task.Run(() => ProcessAndBarGraphData(img, list));

            img.Dispose();

            if (!painting_mode && i == iter.Last() || cmbbChange)
            {
                cmbbChange = false;
                pchB.Image.Dispose();
                pchB.Image = rez.Item2;

                chrt.DataSource = rez.Item1;
                chrt.DataBind();

                iter.Clear();
                iter.Add(0);
            }
        }
        private (DataTable, Image) ProcessAndBarGraphData(Image img, List<int> list)
        {
            var rezult = MyImage.ProcessAndBarGraphData(new Bitmap(img), list);
            return rezult;
        }
        private void MyCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            //при отпускании ЛКМ отключаем режим рисования
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                painting_mode = false;
        }
        private void MyCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            //при нажании ЛКМ включаем режим рисования
            if (e.Button == MouseButtons.Left)
            {
                PaintComplete = false;
                painting_mode = true;

                bool pointFinded = false;

                var mouse_pos = PointToClient(MousePosition);
                for (int i = 0; i < points.Count; ++i)
                {
                    if (PointInRectangle(mouse_pos, PointToMine(points[i]), new Size(20, 20)))
                    {
                        currentPoint = points[i];
                        pointFinded = true;
                        break;
                    }
                }
                if (!pointFinded && (cmbb.SelectedIndex != 5 || points.Count < 13))
                {
                    currentPoint = PointOutOfMine(mouse_pos);
                    points.Add(PointOutOfMine(mouse_pos));
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                PaintComplete = false;
                painting_mode = true;

                var mouse_pos = PointToClient(MousePosition);
                for (int i = 1; i < points.Count - 1; ++i)
                {
                    if (PointInRectangle(mouse_pos, PointToMine(points[i]), new Size(20, 20)))
                    {
                        points.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Если есть режим рисования, то нарисовать красный круг под мышкой.
            //ф-ция вызывается при движении мыши по канве
            if (painting_mode && e.Button != MouseButtons.Right)
            {
                var mouse_pos = PointToClient(MousePosition);

                try
                {
                    if (PointInRectangle(mouse_pos, new Point(173, 173), new Size(347, 348)))
                        if (points.IndexOf(currentPoint) == 0 || points.IndexOf(currentPoint) == points.Count - 1)
                        {
                            int index = points.IndexOf(currentPoint);
                            points[index] = new Point(points[index].X, PointOutOfMine(mouse_pos).Y);
                            currentPoint = new Point(points[index].X, PointOutOfMine(mouse_pos).Y);
                        }
                        else
                        {
                            int index = points.IndexOf(currentPoint);
                            points[index] = PointOutOfMine(mouse_pos);
                            currentPoint = PointOutOfMine(mouse_pos);
                        }
                }
                catch { }
                
            }
        }

        private Point PointToMine(Point pnt)
        {
            return new Point(
                (int)(pnt.X * (float)this.Size.Width / 255),
                (int)((float)this.Size.Height - pnt.Y * (float)this.Size.Height / 255));
        }
        private Point PointOutOfMine(Point pnt)
        {
            return new Point(
                (int)(((float)pnt.X / this.Size.Width) * 255),
                (int)(((float)(this.Size.Height - pnt.Y) / this.Size.Height) * 255));
        }

        private static bool PointInRectangle(Point pnt1, Point pnt2, Size size)
        {
            bool result = false;
            if (pnt1.X < pnt2.X + size.Width / 2 && pnt1.X > pnt2.X - size.Width / 2 &&
                pnt1.Y < pnt2.Y + size.Height / 2 && pnt1.Y > pnt2.Y - size.Height / 2)
                result = true;
            return result;
        }

        private void QuadProc(List<int> Values)
        {
            List<double> vx = new List<double>();
            List<double> vy = new List<double>();

            points.ForEach(item => { vx.Add(item.X); vy.Add(item.Y); });

            QuadraticSpline cubicSpline = new QuadraticSpline();

            cubicSpline.BuildSpline(vx.ToArray(), vy.ToArray(), vx.Count);

            List<Point> tmp = new List<Point>();

            for (int i = 0; i < 256; ++i)
            {
                int y = (int)Math.Round(cubicSpline.Interpolate(i), 0);
                if (y < 0 || y > 255)
                    if (y < 0)
                        y = 0;
                    else
                        y = 255;
                Values.Add(y);
                tmp.Add(new Point(i, y));
            }

            try
            {
                for (int i = 0; i < tmp.Count - 1; ++i)
                {
                    Point one = PointToMine(tmp[i]);
                    Point two = PointToMine(tmp[i + 1]);
                    g_layer2.DrawLine(pen, one, two);
                }
            }
            catch { }
        }

        private void CubProc(List<int> Values)
        {
            List<double> vx = new List<double>();
            List<double> vy = new List<double>();

            points.ForEach(item => { vx.Add(item.X); vy.Add(item.Y); });

            CubicSpline cubicSpline = new CubicSpline();

            cubicSpline.BuildSpline(vx.ToArray(), vy.ToArray(), vx.Count);

            List<Point> tmp = new List<Point>();

            for (int i = 0; i < 256; ++i)
            {
                int y = (int)Math.Round(cubicSpline.Interpolate(i), 0);
                if (y < 0 || y > 255)
                    if (y < 0)
                        y = 0;
                    else
                        y = 255;
                Values.Add(y);
                tmp.Add(new Point(i, y));
            }

            try
            {
                for (int i = 0; i < tmp.Count - 1; ++i)
                {
                    Point one = PointToMine(tmp[i]);
                    Point two = PointToMine(tmp[i + 1]);
                    g_layer2.DrawLine(pen, one, two);
                }
            }
            catch { }
        }

        private void LagProc(List<int> Values)
        {
            List<double> vx = new List<double>();
            List<double> vy = new List<double>();

            points.ForEach(item => { vx.Add(item.X); vy.Add(item.Y); });

            LagrangePolynomial cubicSpline = new LagrangePolynomial();

            List<Point> tmp = new List<Point>();

            for (int i = 0; i < 256; ++i)
            {
                int y = (int)Math.Round(cubicSpline.Interpolate(vx.ToArray(), vy.ToArray(), points.Count, i), 0);
                if (y < 0 || y > 255)
                    if (y < 0)
                        y = 0;
                    else
                        y = 255;
                Values.Add(y);
                tmp.Add(new Point(i, y));
            }

            try
            {
                for (int i = 0; i < tmp.Count - 1; ++i)
                {
                    Point one = PointToMine(tmp[i]);
                    Point two = PointToMine(tmp[i + 1]);
                    g_layer2.DrawLine(pen, one, two);
                }
            }
            catch { }
        }

        private void NewtProc(List<int> Values)
        {
            List<double> vx = new List<double>();
            List<double> vy = new List<double>();

            points.ForEach(item => { vx.Add(item.X); vy.Add(item.Y); });

            Newton_sPolynomial cubicSpline = new Newton_sPolynomial();

            cubicSpline.BuildSpline(vx.ToArray(), vy.ToArray(), points.Count);

            List<Point> tmp = new List<Point>();

            for (int i = 0; i < 256; ++i)
            {
                int y = (int)Math.Round(cubicSpline.Interpolate(i), 0);
                if (y < 0 || y > 255)
                    if (y < 0)
                        y = 0;
                    else
                        y = 255;
                Values.Add(y);
                tmp.Add(new Point(i, y));
            }

            try
            {
                for (int i = 0; i < tmp.Count - 1; ++i)
                {
                    Point one = PointToMine(tmp[i]);
                    Point two = PointToMine(tmp[i + 1]);
                    g_layer2.DrawLine(pen, one, two);
                }
            }
            catch { }
        }

        private void BezierProc(List<int> Values)
        {
            List<double> vx = new List<double>();
            List<double> vy = new List<double>();

            points.ForEach(item => { vx.Add(item.X); vy.Add(item.Y); });

            List<Point> tmp = new List<Point>();

            List<double> rez = BezierCurve.BuildSpline(vx.ToArray(), vy.ToArray(), points.Count);
            rez.ForEach(y =>
            {
                if (y < 0 || y > 255)
                    if (y < 0)
                        y = 0;
                    else
                        y = 255;
                y = Math.Round(y, 0);
                Values.Add((int)y);
                tmp.Add(new Point(tmp.Count, (int)y));
            });

            try
            {
                for (int i = 0; i < tmp.Count - 1; ++i)
                {
                    Point one = PointToMine(tmp[i]);
                    Point two = PointToMine(tmp[i + 1]);
                    g_layer2.DrawLine(pen, one, two);
                }
                List<List<Point>> layers = new List<List<Point>>();

                Pen tmppen = new Pen(Color.FromArgb(255, 0, 255, 255), 1);

                List<Point> newtmppnt = new List<Point>();
                for (int i = 0; i < points.Count - 1; ++i)
                {
                    Point one = PointToMine(points[i]);
                    Point two = PointToMine(points[i + 1]);

                    newtmppnt.Add(new Point((points[i].X + points[i + 1].X) / 2, (points[i].Y + points[i + 1].Y) / 2));
                    g_layer2.DrawLine(tmppen, one, two);
                }
                layers.Add(newtmppnt);

                while (layers.Last().Count > 1)
                {
                    int dc = 150 / (points.Count - 2);
                    dc *= (points.Count - layers.Last().Count);

                    List<Point> newnewtmppnt = new List<Point>();
                    Pen newtmppen = new Pen(Color.FromArgb(255, dc, 255 - dc, 255), 1);

                    for (int i = 0; i < layers.Last().Count - 1; ++i)
                    {
                        Point one = PointToMine(layers.Last()[i]);
                        Point two = PointToMine(layers.Last()[i + 1]);

                        newnewtmppnt.Add(new Point((layers.Last()[i].X + layers.Last()[i + 1].X) / 2, (layers.Last()[i].Y + layers.Last()[i + 1].Y) / 2));
                        g_layer2.DrawLine(newtmppen, one, two);
                    }
                    layers.Clear();
                    layers.Add(newnewtmppnt);
                }
                
            }
            catch { }
        }
    }
}
