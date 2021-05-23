using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Timer = System.Windows.Forms.Timer;

namespace SCOI_lab_1
{
    public class MyFigure
    {
        Point center;
        public Point Center
        {
            get => new Point(center.X - Radius / 2, center.Y - Radius / 2);
            set => center = value;
        }
        public Size Size { get; set; }
        public Size inSize { get; set; }
        public Size WindowSize { get; set; }
        public int Radius {
            get => (Size.Width == Size.Height) ? Size.Width : ((Size.Width + Size.Width) / 2);
            set => Size = new Size(value, value); 
        }
        public Rectangle Rect
        {
            get => new Rectangle(center.X - Size.Width / 2, center.Y - Size.Height / 2, Size.Width, Size.Height);
            set
            {
                center = value.Location;
                center.Offset(value.Width / 2, value.Height / 2);
                Size = value.Size;
            }
        }
        public Rectangle InsideRect
        {
            get => new Rectangle(center.X - inSize.Width / 2, center.Y - inSize.Height / 2, inSize.Width, inSize.Height);
            set
            {
                center = value.Location;
                center.Offset(value.Width / 2, value.Height / 2);
                Size = value.Size;
            }
        }
        public Rectangle SyRect
        {
            get => new Rectangle(WindowSize.Width - center.X - Size.Width / 2, WindowSize.Height - center.Y - Size.Height / 2, Size.Width, Size.Height);
        }
        public Rectangle InsideSyRect
        {
            get => new Rectangle(WindowSize.Width - center.X - inSize.Width / 2, WindowSize.Height - center.Y - inSize.Height / 2, inSize.Width, inSize.Height);
        }
        public MyFigure(Point Center, int Radius, int inRadius)
        {
            this.center = Center;
            this.Radius = Radius;
            this.inSize = new Size(inRadius, inRadius);
        }
        public MyFigure(Rectangle Rect, Size WindowSize)
        {
            this.Rect = Rect;
            this.WindowSize = WindowSize;
        }
        public MyFigure(MyFigure myFigure)
        {
            this.Rect = myFigure.Rect;
            this.WindowSize = myFigure.WindowSize;
        }

        public bool PointInFigure(Point mouse)
        {
            return Math.Sqrt(Math.Pow(mouse.X - center.X, 2) + Math.Pow(mouse.Y - center.Y, 2)) * 2 < Radius;
        }
        public MyFigure PointInFilter(Size newSize)
        {
            MyFigure newF = new MyFigure(this);
            double resizeX = (double)newSize.Width / this.WindowSize.Width;
            double resizeY = (double)newSize.Height / this.WindowSize.Height;
            newF.Center = new Point((int)Math.Round(newF.center.X * resizeX), (int)Math.Round(newF.center.Y * resizeY));
            newF.Size = new Size((int)Math.Round(this.Size.Width * resizeX), (int)Math.Round(this.Size.Height * resizeX));
            newF.inSize = new Size((int)Math.Round(this.inSize.Width * resizeX), (int)Math.Round(this.inSize.Height * resizeX));
            newF.WindowSize = newSize;
            return newF;
        }
    }

    public class MyFCanvas : Control
    {
        public List<MyFigure> figures = new List<MyFigure>() {};

        public Image img { get; set; }
        public PictureBox pchB { get; set; }
        public ComboBox Ftype { get; set; }
        public ComboBox Fname { get; set; }
        public NumericUpDown FN { get; set; }
        //Таймер для ее обновления
        private Timer timer;

        private Timer secmer;
        private double sec;

        //битмапы на которых будем рисовать в 2 слоя.
        //На первом будет само содержаение
        //На втором курсор.
        public Bitmap FourierTransform;
        private Bitmap layer1;
        private Bitmap layer2;

        //Графиксы для этих битмапов
        private Graphics g_FourierTransform;
        private Graphics g_layer1;
        private Graphics g_layer2;

        private bool painting_mode = false;
        private bool inside_painting_mode = false;
        public bool symmetry_mode = true;

        Pen pen = new Pen(Color.FromArgb(255, 100, 150, 100), 2);
        int r = 15;
        int rin = 0;

        Point draw_mouse_pos;

        public MyFCanvas()
        {
            //Включаем режим двойной буферизации, чтобы рисовка не мерцала.
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            //Опеределяем в нашей канве события
            this.Paint += MyFCanvas_Paint;
            this.MouseDown += MyFCanvas_MouseDown;
            this.MouseUp += MyFCanvas_MouseUp;
            this.MouseMove += MyFCanvas_MouseMove;
            this.MouseWheel += MyFCanvas_MouseWheel;

            this.SizeChanged += MyFCanvas_SizeChanged;

            //Запускаем таймер на перерисовку
            timer = new Timer();
            timer.Interval = 25;
            timer.Tick += (s, a) => this.Refresh();
            timer.Start();
        }
        ~MyFCanvas()
        {

            if (g_FourierTransform != null)
                FourierTransform.Dispose();
            if (g_layer1 != null)
                layer1.Dispose();
            if (g_layer2 != null)
                layer2.Dispose();

            if (FourierTransform != null)
                FourierTransform.Dispose();
            if (layer1 != null)
                layer1.Dispose();
            if (layer2 != null)
                layer2.Dispose();

            timer.Dispose();
            pen.Dispose();
        }

        private void MyFCanvas_SizeChanged(object sender, EventArgs e)
        {
            var _sender = sender as MyFCanvas;

            //При изменении размера у нас должны пересоздатся битмапы (так как нельзя изменить
            // размер битмапа во время работы)
            //По этому мы сначала создаем новые, если старые есть (при создании конвы их нет,
            //вот тут они и создадутся при первом отображении) - рисуем их содержимое на новых, удаляем старые.

            Bitmap new_FourierTransform = new Bitmap(_sender.Size.Width, _sender.Size.Height, PixelFormat.Format32bppArgb);
            Bitmap new_layer1 = new Bitmap(_sender.Size.Width, _sender.Size.Height, PixelFormat.Format32bppArgb);
            Bitmap new_layer2 = new Bitmap(_sender.Size.Width, _sender.Size.Height, PixelFormat.Format32bppArgb);
            Graphics new_g_FourierTransform = Graphics.FromImage(new_FourierTransform);
            Graphics new_g_layer1 = Graphics.FromImage(new_layer1);
            Graphics new_g_layer2 = Graphics.FromImage(new_layer2);

            if (g_FourierTransform != null)
            {
                new_g_FourierTransform.DrawImageUnscaled(FourierTransform, 0, 0);
                FourierTransform.Dispose();
            }
            if (FourierTransform != null)
                FourierTransform.Dispose();
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

            FourierTransform = new_FourierTransform;
            g_FourierTransform = new_g_FourierTransform;
            layer1 = new_layer1;
            g_layer1 = new_g_layer1;
            layer2 = new_layer2;
            g_layer2 = new_g_layer2;

        }
        public void NewFilterSize(Size Nsize)
        {
            Bitmap new_layer1 = new Bitmap(Nsize.Width, Nsize.Height, PixelFormat.Format32bppArgb);
            Graphics new_g_layer1 = Graphics.FromImage(new_layer1);

            if (g_layer1 != null)
            {
                new_g_layer1.DrawImageUnscaled(layer1, 0, 0);
                layer1.Dispose();
            }
            if (layer1 != null)
                layer1.Dispose();

            layer1 = new_layer1;
            g_layer1 = new_g_layer1;
        }
        private void MyFCanvas_Paint(object sender, PaintEventArgs e)
        {
            var mouse_pos = PointToClient(MousePosition);
            //if(Ftype.SelectedIndex == 0)
            //    g_layer1.Clear(Color.FromArgb(255, 0, 0, 0));
            //else
            //    g_layer1.Clear(Color.FromArgb(255, 255, 255, 255));
            g_layer2.Clear(Color.FromArgb(0, 0, 0, 0));

            if (!painting_mode)
            {
                draw_mouse_pos = mouse_pos;
            }

            MyFigure thisFigure = new MyFigure(draw_mouse_pos, r, rin);
            thisFigure.WindowSize = this.Size;

            g_layer2.DrawEllipse(pen, thisFigure.Rect);
            g_layer2.DrawEllipse(new Pen(Color.FromArgb(255, 200, 127, 0), 2), thisFigure.InsideRect);
            if (symmetry_mode)
            {
                g_layer2.DrawEllipse(pen, thisFigure.SyRect);
                g_layer2.DrawEllipse(new Pen(Color.FromArgb(255, 200, 127, 0), 2), thisFigure.InsideSyRect);
            }

            for (int i = 0; i < figures.Count; ++i)
            {
                g_layer2.DrawEllipse(pen, figures[i].Rect);
                g_layer2.DrawEllipse(new Pen(Color.FromArgb(255, 200, 127, 0), 2), figures[i].InsideRect);

                if (symmetry_mode)
                {
                    g_layer2.DrawEllipse(pen, figures[i].SyRect);
                    g_layer2.DrawEllipse(new Pen(Color.FromArgb(255, 200, 127, 0), 2), figures[i].InsideSyRect);
                }
            }

            e.Graphics.DrawImageUnscaled(FourierTransform, 0, 0);
            //e.Graphics.DrawImageUnscaled(layer1, 0, 0);
            e.Graphics.DrawImageUnscaled(layer2, 0, 0);

            pchB.Image = layer1;
        }
        private void MyFCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (((HandledMouseEventArgs)e).Delta > 0)
            { 
                if (r < this.Width)
                {
                    rin = rin <= 0 ? 0 : rin + 1;
                    r++;
                }
            }
            else if (r > 1)
            {
                rin = rin <= 0 ? 0 : rin - 1;
                r--;
            }
        }
        private void MyFCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            //при отпускании ЛКМ отключаем режим рисования
            if (e.Button == MouseButtons.Left)
            {
                painting_mode = false;
                figures.Add(new MyFigure(draw_mouse_pos, r, rin));
                figures.Last().WindowSize = this.Size;
                RefreshFurierObr();
                //g_layer2.FillEllipse(Brushes.Red, draw_mouse_pos.X - r / 2, draw_mouse_pos.Y - r / 2, r, r);

                sec = 0;
                secmer.Stop();
            }
            if (e.Button == MouseButtons.Right)
            {
                inside_painting_mode = false;
                sec = 0;
            }
        }
        private void MyFCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            //при нажании ЛКМ включаем режим рисования
            if (e.Button == MouseButtons.Left)
            {
                painting_mode = true;

                secmer = new Timer();
                secmer.Interval = 10;
                secmer.Tick += (s, a) => sec += 0.1d;
                secmer.Start();
            } else if(e.Button == MouseButtons.Right && !painting_mode)
            {
                var mouse_pos = PointToClient(MousePosition);
                foreach (MyFigure item in figures)
                {
                    MyFigure tmp = item;
                    if (symmetry_mode)
                        tmp = new MyFigure(item.SyRect, this.Size);
                    if (item.PointInFigure(mouse_pos) || tmp.PointInFigure(mouse_pos))
                    { 
                        figures.Remove(item);
                        RefreshFurierObr();
                        break; 
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
                inside_painting_mode = true;
        }
        private void MyFCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Если есть режим рисования, то нарисовать красный круг под мышкой.
            //ф-ция вызывается при движении мыши по канве
            if (painting_mode && sec >= 0.5)
            {
                var mouse_pos = PointToClient(MousePosition);
                int newR = (int)Math.Round(Math.Sqrt(Math.Pow(mouse_pos.X - draw_mouse_pos.X, 2) + Math.Pow(mouse_pos.Y - draw_mouse_pos.Y, 2))) * 2 + 1;

                if (inside_painting_mode)
                {
                    if (newR < r)
                        rin = newR;
                }
                else
                {
                    rin = rin <= 0 ? 0 : newR - r + rin;
                    r = newR;
                }
            }
        }
        public void RefreshFurierObr()
        {
            if (Ftype.SelectedIndex == 0)
                g_layer1.Clear(Color.FromArgb(255, 0, 0, 0));
            else
                g_layer1.Clear(Color.FromArgb(255, 255, 255, 255));
            for (int i = 0; i < figures.Count; ++i)
            {
                MyFigure new_tmp = figures[i].PointInFilter(layer1.Size);

                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(new_tmp.Rect);
                path.AddEllipse(new_tmp.InsideRect);

                GraphicsPath sypath = new GraphicsPath();
                sypath.AddEllipse(new_tmp.SyRect);
                sypath.AddEllipse(new_tmp.InsideSyRect);

                PathGradientBrush pthGrBrush = new PathGradientBrush(path);
                PathGradientBrush sypthGrBrush = new PathGradientBrush(sypath);

                pthGrBrush.WrapMode = WrapMode.Tile;
                pthGrBrush.InterpolationColors = GetColorBlend(Ftype.SelectedIndex, Fname.SelectedIndex, new_tmp.Size.Width, new_tmp.inSize.Width, (int)FN.Value);

                sypthGrBrush.WrapMode = WrapMode.Tile;
                sypthGrBrush.InterpolationColors = pthGrBrush.InterpolationColors;

                if (Ftype.SelectedIndex == 0)
                {
                    g_layer1.FillPath(Fname.SelectedIndex > 0 ? pthGrBrush : Brushes.White, path);
                }
                else
                {
                    g_layer1.FillPath(Fname.SelectedIndex > 0 ? pthGrBrush : Brushes.Black, path);
                }

                if (symmetry_mode)
                {
                    if (Ftype.SelectedIndex == 0)
                    {
                        g_layer1.FillPath(Fname.SelectedIndex > 0 ? sypthGrBrush : Brushes.White, sypath);
                    }
                    else
                    {
                        g_layer1.FillPath(Fname.SelectedIndex > 0 ? sypthGrBrush : Brushes.Black, sypath);
                    }
                }
            }
        }
        public static double Butter(double w, double wc, int n, double A = 1.0)
        {
            return A / (1 + Math.Pow(w / wc, 2 * n));
        }

        public static double Gauss(double w, double wc, double A = 1.0)
        {
            return A * Math.Exp(-(w * w / (2.0 * wc * wc)));
        }

        private ColorBlend GetColorBlend(int Ftype, int Fname, int outsideradius, int insideradius, int n)
        {
            List<Color> presetColors = new List<Color>() {};
            List<float> interpPositions = new List<float>() {};

            double wc = 0.4;

            if (insideradius == 0)
            {
                for(double i = 0; i < 1.01; i += 0.05)
                {
                    double b = Math.Abs((float)Ftype - ((Fname == 1) ? Butter(i, wc, n) : Gauss(i, wc)));
                    presetColors.Add(Color.FromArgb(255,
                        (int)(255 * b),
                        (int)(255 * b),
                        (int)(255 * b)));
                    interpPositions.Add((float)i);
                }
            } else
            {
                float h = (float)insideradius / outsideradius;
                float k = 1 - h;
                //wc = k;
                for (double i = 0; i < 1.0; i += 0.1)
                {
                    double b = Math.Abs((float)Ftype - ((Fname == 1) ? Butter(1 - i, wc, n) : Gauss(1 - i, wc)));
                    presetColors.Add(Color.FromArgb(255,
                        (int)(255 * b),
                        (int)(255 * b),
                        (int)(255 * b)));
                    interpPositions.Add(k * ((float)i/2));
                }
                for (double i = 0; i <= 1.01; i += 0.1)
                {
                    double b = Math.Abs((float)Ftype - ((Fname == 1) ? Butter(i, wc, n) : Gauss(i, wc)));
                    presetColors.Add(Color.FromArgb(255,
                        (int)(255 * b),
                        (int)(255 * b),
                        (int)(255 * b)));
                    interpPositions.Add(k * (0.5f + (float)i / 2));
                }
            }

            presetColors.Add(Color.FromArgb(255, Ftype * 255, Ftype * 255, Ftype * 255));
            interpPositions.Add(1.0f);

            presetColors.Reverse();
            //interpPositions.Reverse();

            ColorBlend color_blend = new ColorBlend();
            color_blend.Colors = presetColors.ToArray();
            color_blend.Positions = interpPositions.ToArray();

            return color_blend;
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

    }
}
