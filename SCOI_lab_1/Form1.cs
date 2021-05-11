using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SCOI_lab_1
{
    public partial class Form1 : Form
    {
        List<Layer> layers = new List<Layer>();
        decimal[] kk = { 0, 0, -0.2m, 0.2m, 0.5m, 0.15m };
        decimal max = 10, min = -10;
        (Image, double[], double[]) filter;
        Image tmpImage;
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 0;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeBackgroundWorker();

            chart1.Series[0].XValueMember = "Brightness";
            chart1.Series[0].YValueMembers = "Amount";

            var canvas1 = new MyCanvas();
            canvas1.Name = "canvas1";
            canvas1.pchB = pictureBox1;
            canvas1.chrt = chart1;
            canvas1.cmbb = comboBox1;
            panel1.Controls.Add(canvas1);
            canvas1.Dock = DockStyle.Fill;

            var canvas2 = new MyFCanvas();
            canvas2.Name = "Fcanvas";
            canvas2.pchB = FourierTransform;
            canvas2.Ftype = FilterType;
            canvas2.Fname = FilterName;
            canvas2.FN = nUDN;
            canvas2.symmetry_mode = SymmetryChB.Checked;
            panel2.Controls.Add(canvas2);
            canvas2.Dock = DockStyle.Fill;

            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += new EventHandler(this.CBSelectedIndexChanged);

            tableLayoutPanel1.AllowDrop = true;
            tableLayoutPanel1.DragDrop += new DragEventHandler(this.TLP_DragDrop);
            tableLayoutPanel1.DragEnter += new DragEventHandler(this.TLP_DragEnter);

            pictureBox1.AllowDrop = true;
            pictureBox1.MouseDown += new MouseEventHandler(this.PCHB_MouseDown);
            pictureBox1.DragDrop += new DragEventHandler(this.PCHB_DragDrop);
            pictureBox1.DragEnter += new DragEventHandler(this.PCHB_DragEnter);

            tabControl1.SelectedIndexChanged += new EventHandler(this.TabControl_SelectedIndexChanged);

            KR1.CheckedChanged += new EventHandler(this.KR_Checked);
            KR2.CheckedChanged += new EventHandler(this.KR_Checked);
            KR3.CheckedChanged += new EventHandler(this.KR_Checked);
            KR4.CheckedChanged += new EventHandler(this.KR_Checked);
            KR5.CheckedChanged += new EventHandler(this.KR_Checked);
            KR6.CheckedChanged += new EventHandler(this.KR_Checked);

            KRtT.SetToolTip(KR1, "Критерий Гаврилова");
            KRtT.SetToolTip(KR2, "Критерий Отсу");
            KRtT.SetToolTip(KR3, "Критерий Ниблека");
            KRtT.SetToolTip(KR4, "Критерий Сауволы");
            KRtT.SetToolTip(KR5, "Критерий Кристиана Вульфа");
            KRtT.SetToolTip(KR6, "Критерий Брэдли-Рота");

            comboBox2.SelectedIndex = 0;
            FilterType.SelectedIndex = 0;
            FilterName.SelectedIndex = 0;
        }
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файлы изображений|*.bmp;*.png;*.jpg";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                layers.Add(new Layer(Image.FromFile(openDialog.FileName), tableLayoutPanel1.Width));

                layers.Last().cmb.Enabled = false;
                if(layers.Count > 1)
                    layers[layers.Count - 2].cmb.Enabled = true;

                layers.Last().tckb.ValueChanged += new System.EventHandler(this.TrackBar_ValueChange);
                layers.Last().tckb.MouseWheel += new MouseEventHandler(trackBar_MouseWheel);
                layers.Last().Rchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().Gchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().Bchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().clsbtn.Click += new System.EventHandler(this.CloseButton_Click);
                layers.Last().cmb.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
                layers.Last().up.Click += new System.EventHandler(this.UpButton_Click);
                layers.Last().dwn.Click += new System.EventHandler(this.DownButton_Click);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 117));
                ++tableLayoutPanel1.RowCount;
                tableLayoutPanel1.Controls.Add(layers.Last().pctb, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(layers.Last().FLP, 1, tableLayoutPanel1.RowCount - 1);
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Ошибка чтения картинки");
                return;
            }
        }
        class LayerValue
        {
            public Image image;
            public bool Rchnl, Gchnl, Bchnl;
            public float tckb;
            public int cmb;
            public LayerValue(Layer value)
            {
                image = value.image;
                Rchnl = value.Rchnl.Checked;
                Gchnl = value.Gchnl.Checked;
                Bchnl = value.Bchnl.Checked;
                tckb = value.tckb.Value;
                cmb = value.cmb.SelectedIndex;
            }
        }
        class Layer
        {
            public Image image;
            public PictureBox pctb;
            public Label lbl;
            public Button clsbtn, up, dwn;
            public TrackBar tckb;
            public ComboBox cmb;
            public CheckBox Rchnl, Gchnl, Bchnl;
            public FlowLayoutPanel FLP;

            public Layer(Layer tmp)
            {
                image = tmp.image;
                pctb = tmp.pctb;
                lbl = tmp.lbl;
                clsbtn = tmp.clsbtn;
                up = tmp.up;
                dwn = tmp.dwn;
                tckb = tmp.tckb;
                cmb = tmp.cmb;
                Rchnl = tmp.Rchnl;
                Gchnl = tmp.Gchnl;
                Bchnl = tmp.Bchnl;
                FLP = tmp.FLP;
            }
            public Layer(Image image, int width)
            {
                this.image = image;

                // Создаем и настраиваем пикчербокс
                pctb = new PictureBox();
                pctb.SizeMode = PictureBoxSizeMode.Zoom;
                pctb.Size = new Size(width / 2, 115);
                pctb.Image = (Image)image.Clone();
                pctb.BackColor = Color.Transparent;

                // Создаем и настраиваем лейбл
                lbl = new Label();
                lbl.Text = "Прозрачность 0%";
                lbl.AutoSize = true;


                // Создаем и настраиваем трекбар
                tckb = new TrackBar();
                tckb.Maximum = 100;
                tckb.Minimum = 0;
                tckb.Height = 20;
                tckb.Width = 80;
                tckb.TickStyle = TickStyle.Both;

                // Создаем и настраиваем комбобокс
                cmb = new ComboBox();
                String[] items = { "Нет", "Сложение", "Разница", "Умножение", "Среднее значение", "Минимум", "Максимум", "Наложение" };
                cmb.Items.AddRange(items);
                cmb.SelectedIndex = 0;
                cmb.Height = 20;
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;

                // Создаем и разукрашиваем чекбоксы цветовых каналов
                Rchnl = new CheckBox();
                Rchnl.BackColor = Color.Red;
                Rchnl.AutoSize = true;
                Rchnl.Checked = true;

                Gchnl = new CheckBox();
                Gchnl.BackColor = Color.Green;
                Gchnl.AutoSize = true;
                Gchnl.Checked = true;

                Bchnl = new CheckBox();
                Bchnl.BackColor = Color.Blue;
                Bchnl.AutoSize = true;
                Bchnl.Checked = true;

                // Объединяем чекбоксы в панельку
                FlowLayoutPanel Channels = new FlowLayoutPanel();
                Channels.Controls.Add(Rchnl);
                Channels.Controls.Add(Gchnl);
                Channels.Controls.Add(Bchnl);

                clsbtn = new Button();
                clsbtn.Text = "X";
                clsbtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                clsbtn.AutoSize = true;
                clsbtn.BackColor = Color.Red;

                up = new Button();
                up.Text = "↑";
                up.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                up.AutoSize = true;

                dwn = new Button();
                dwn.Text = "↓";
                dwn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                dwn.AutoSize = true;

                FlowLayoutPanel UpDwnB = new FlowLayoutPanel();
                UpDwnB.Controls.Add(up);
                UpDwnB.Controls.Add(dwn);
                UpDwnB.Margin = new Padding(0);
                UpDwnB.Width = up.Width;
                UpDwnB.Height = up.Height + dwn.Height + 6;


                // Объединаем элементы управления в панель и настраиваем ее
                FLP = new FlowLayoutPanel();
                FLP.Controls.Add(tckb);
                FLP.Controls.Add(clsbtn);
                FLP.Controls.Add(UpDwnB);
                FLP.Controls.Add(lbl);
                FLP.Controls.Add(cmb);
                FLP.Controls.Add(Channels);
                FLP.Height = 115;
                FLP.Dock = DockStyle.Fill;
                FLP.Margin = new Padding(0);

            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 1)
                (panel1.Controls[0] as MyCanvas).AllRefreshPoc();
        }
        private void TLP_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(Bitmap)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void TLP_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Bitmap)))
            {
                var img = e.Data.GetData(typeof(Bitmap)) as Image;
                layers.Add(new Layer(new Bitmap(img), tableLayoutPanel1.Width));

                layers.Last().cmb.Enabled = false;
                if (layers.Count > 1)
                    layers[layers.Count - 2].cmb.Enabled = true;

                layers.Last().tckb.ValueChanged += new System.EventHandler(this.TrackBar_ValueChange);
                layers.Last().tckb.MouseWheel += new MouseEventHandler(trackBar_MouseWheel);
                layers.Last().Rchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().Gchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().Bchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                layers.Last().clsbtn.Click += new System.EventHandler(this.CloseButton_Click);
                layers.Last().cmb.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
                layers.Last().up.Click += new System.EventHandler(this.UpButton_Click);
                layers.Last().dwn.Click += new System.EventHandler(this.DownButton_Click);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 117));
                ++tableLayoutPanel1.RowCount;
                tableLayoutPanel1.Controls.Add(layers.Last().pctb, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(layers.Last().FLP, 1, tableLayoutPanel1.RowCount - 1);
            }
            else
            try
            {
                String[] a = (String[])e.Data.GetData(DataFormats.FileDrop);

                foreach(string path in a)
                {
                    layers.Add(new Layer(Image.FromFile(path), tableLayoutPanel1.Width));

                    layers.Last().cmb.Enabled = false;
                    if (layers.Count > 1)
                        layers[layers.Count - 2].cmb.Enabled = true;

                    layers.Last().tckb.ValueChanged += new System.EventHandler(this.TrackBar_ValueChange);
                    layers.Last().tckb.MouseWheel += new MouseEventHandler(trackBar_MouseWheel);
                    layers.Last().Rchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                    layers.Last().Gchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                    layers.Last().Bchnl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
                    layers.Last().clsbtn.Click += new System.EventHandler(this.CloseButton_Click);
                    layers.Last().cmb.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
                    layers.Last().up.Click += new System.EventHandler(this.UpButton_Click);
                    layers.Last().dwn.Click += new System.EventHandler(this.DownButton_Click);

                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 117));
                    ++tableLayoutPanel1.RowCount;
                    tableLayoutPanel1.Controls.Add(layers.Last().pctb, 0, tableLayoutPanel1.RowCount - 1);
                    tableLayoutPanel1.Controls.Add(layers.Last().FLP, 1, tableLayoutPanel1.RowCount - 1);
                }
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Ошибка чтения картинки");
                return;
            }
        }
        private void PCHB_MouseDown(object sender, MouseEventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                var pictureBox = (PictureBox)sender;
                pictureBox.DoDragDrop(pictureBox.Image, DragDropEffects.Move);
            }
        }
        private void PCHB_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void PCHB_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                String[] a = (String[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string path in a)
                {
                    if(pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();
                    pictureBox1.Image = Image.FromFile(path);
                }

                (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);
                if(tabControl1.SelectedIndex == 1)
                    (panel1.Controls[0] as MyCanvas).AllRefreshPoc();
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Ошибка чтения картинки");
                return;
            }
        }
        private void CBSelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
                (panel1.Controls[0] as MyCanvas).AllRefreshPoc();
        }
        private void TrackBar_ValueChange(object sender, EventArgs e)
        {
            // Получаем наш трекбар
            var trackbar = (TrackBar)sender;
            // Находим соответствующий ему слой
            Layer desired_layer = layers.Find(x => x.tckb == trackbar);
            // меняем значение лейбла
            desired_layer.lbl.Text = "Прозрачность " + trackbar.Value + "%";

            int R = desired_layer.Rchnl.Checked ? 1 : 0;
            int G = desired_layer.Gchnl.Checked ? 1 : 0;
            int B = desired_layer.Bchnl.Checked ? 1 : 0;

            desired_layer.pctb.Image.Dispose();
            desired_layer.pctb.Image = MyImage.SetImgChannelValue(desired_layer.image, (1 - (float)trackbar.Value / 100), R, G, B);
        }
        private void trackBar_MouseWheel(object sender, EventArgs e)
        {
            // Получаем наш трекбар
            var trackbar = (TrackBar)sender;
            ((HandledMouseEventArgs)e).Handled = true;
            if (((HandledMouseEventArgs)e).Delta > 0)
            {
                if (trackbar.Value < trackbar.Maximum)
                {
                    trackbar.Value++;
                }
            }
            else
            {
                if (trackbar.Value > trackbar.Minimum)
                {
                    trackbar.Value--;
                }
            }
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            Layer desired_layer = layers.Find(x => x.Rchnl == sender || x.Gchnl == sender || x.Bchnl == sender);

            int R = desired_layer.Rchnl.Checked ? 1 : 0;
            int G = desired_layer.Gchnl.Checked ? 1 : 0;
            int B = desired_layer.Bchnl.Checked ? 1 : 0;

            desired_layer.pctb.Image.Dispose();
            desired_layer.pctb.Image = MyImage.SetImgChannelValue(desired_layer.image, (1 - (float)desired_layer.tckb.Value / 100), R, G, B);
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Layer desired_layer = layers.Find(x => x.clsbtn == button);

            layers.Remove(desired_layer);

            if (layers.Count > 0)
            {
                layers.Last().cmb.Enabled = false;
                layers.Last().cmb.SelectedIndex = 0;
                if (layers.Count > 1)
                    layers[layers.Count - 2].cmb.Enabled = true;
            }
            RefreshTableLayoutPanel();
        }
        private void UpButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Layer desired_layer = layers.Find(x => x.up == button);
            if (layers.IndexOf(desired_layer) > 0)
            {
                Layer prew_layer = layers[layers.IndexOf(desired_layer) - 1];

                layers[layers.IndexOf(desired_layer)] = new Layer(prew_layer);
                layers[layers.IndexOf(prew_layer)] = new Layer(desired_layer);

                layers.Last().cmb.Enabled = false;
                layers.Last().cmb.SelectedIndex = 0;
                if (layers.Count > 1)
                    layers[layers.Count - 2].cmb.Enabled = true;

                RefreshTableLayoutPanel();
            }
        }
        private void DownButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Layer desired_layer = layers.Find(x => x.dwn == button);
            if (layers.IndexOf(desired_layer) < layers.Count - 1)
            {
                Layer next_layer = layers[layers.IndexOf(desired_layer) + 1];

                layers[layers.IndexOf(desired_layer)] = new Layer(next_layer);
                layers[layers.IndexOf(next_layer)] = new Layer(desired_layer);

                layers.Last().cmb.Enabled = false;
                layers.Last().cmb.SelectedIndex = 0;
                if (layers.Count > 1)
                    layers[layers.Count - 2].cmb.Enabled = true;

                RefreshTableLayoutPanel();
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            Layer desired_layer = layers.Find(x => x.cmb == comboBox);

            if (comboBox.SelectedIndex == 7)
            {
                desired_layer.tckb.Value = 0;
                desired_layer.tckb.Enabled = false;
            }
            else
                desired_layer.tckb.Enabled = true;
        }
        private void RefreshTableLayoutPanel()
        {
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 0;

            foreach(var item in layers)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 117));
                ++tableLayoutPanel1.RowCount;
                tableLayoutPanel1.Controls.Add(item.pctb, 0, tableLayoutPanel1.RowCount - 1);
                tableLayoutPanel1.Controls.Add(item.FLP, 1, tableLayoutPanel1.RowCount - 1);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            this.progressBar1.Value = 0;

            List<LayerValue> tmp = new List<LayerValue>();
            foreach (var item in layers)
                tmp.Add(new LayerValue(item));

            backgroundWorker1.RunWorkerAsync(tmp);
        }
        private void GetTheResult(List<LayerValue> layers, BackgroundWorker worker, DoWorkEventArgs e)
        {
            if(layers.Any())
            {
                int R = layers.Last().Rchnl ? 1 : 0;
                int G = layers.Last().Gchnl ? 1 : 0;
                int B = layers.Last().Bchnl ? 1 : 0;

                MyImage result = new MyImage(MyImage.SetImgChannelValue(layers.Last().image, (1 - layers.Last().tckb / 100), R, G, B));

                for (int i = layers.Count - 2; i >= 0; --i)
                {
                    R = layers[i].Rchnl ? 1 : 0;
                    G = layers[i].Gchnl ? 1 : 0;
                    B = layers[i].Bchnl ? 1 : 0;

                    result.CPP_LUBitsAndChange(MyImage.SetImgChannelValue(layers[i].image, (1 - (float)layers[i].tckb / 100), R, G, B), layers[i].cmb);

                    worker.ReportProgress((int)((float)(layers.Count - 1 - i) / (layers.Count - 1) * 100));
                }
                if(pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();
                pictureBox1.Image = result.image;
            }
            else
                MessageBox.Show("Добавьте слои!");
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            GetTheResult((List<LayerValue>)e.Argument, worker, e);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Обработка остановлена");
            }
            else
            {
                this.button3.Enabled = true;
                if (layers.Any())
                {
                    //MessageBox.Show("Обработка завершена");
                    this.progressBar1.Value = 100;

                    chart1.DataSource = MyImage.CPU_BarGraphData(pictureBox1.Image);
                    chart1.DataBind();
                    if((panel1.Controls[0] as MyCanvas).img != null)
                        (panel1.Controls[0] as MyCanvas).img.Dispose();
                    (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);
                }
            }

            this.button2.Enabled = true;
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            (panel1.Controls[0] as MyCanvas).points.Clear();
            (panel1.Controls[0] as MyCanvas).points.AddRange(new Point[] { new Point(0,0), new Point(255,255) });

            //(panel1.Controls[0] as MyCanvas).cmbbChange = true;
            (panel1.Controls[0] as MyCanvas).RefreshValues();
            (panel1.Controls[0] as MyCanvas).AllRefreshPoc();
        }

        private void KR_Refresh()
        {
            Bitmap tmp = new Bitmap(
                BinarPchB.Image,
                250, (int)Math.Round((decimal)BinarPchB.Image.Height * ((decimal)250.0 / BinarPchB.Image.Width)));

            if (KR1.BackgroundImage != null)
                KR1.BackgroundImage.Dispose();
            KR1.BackgroundImage = MyImage.CPP_GlobalBinarize(tmp, 0);

            if (KR2.BackgroundImage != null)
                KR2.BackgroundImage.Dispose();
            KR2.BackgroundImage = MyImage.CPP_GlobalBinarize(tmp, 1);

            if (KR3.BackgroundImage != null)
                KR3.BackgroundImage.Dispose();
            KR3.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 0, (int)BinarNUD1.Value, (float)kk[2]);

            if (KR4.BackgroundImage != null)
                KR4.BackgroundImage.Dispose();
            KR4.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 1, (int)BinarNUD1.Value, (float)kk[3]);

            if (KR5.BackgroundImage != null)
                KR5.BackgroundImage.Dispose();
            KR5.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 2, (int)BinarNUD1.Value, (float)kk[4]);

            if (KR6.BackgroundImage != null)
                KR6.BackgroundImage.Dispose();
            KR6.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 3, (int)BinarNUD1.Value, (float)kk[5]);

            tmp.Dispose();
        }
        private void KR_Checked(object sender, EventArgs e)
        {
            var item = sender as CheckBox;
            if (item.Checked)
            {
                if (KR1.Name != item.Name)
                    KR1.Checked = false;
                if (KR2.Name != item.Name)
                    KR2.Checked = false;
                if (KR3.Name != item.Name)
                    KR3.Checked = false;
                if (KR4.Name != item.Name)
                    KR4.Checked = false;
                if (KR5.Name != item.Name)
                    KR5.Checked = false;
                if (KR6.Name != item.Name)
                    KR6.Checked = false;

                switch (item.Name)
                {
                    case "KR1":
                    case "KR2":
                        BinarNUD1.Enabled = false;
                        BinarNUD2.Enabled = false;
                        break;
                    case "KR3":
                        BinarNUD1.Enabled = true;
                        BinarNUD2.Enabled = true;
                        BinarNUD2.Maximum = 10;
                        BinarNUD2.Minimum = -10;
                        max = 10;
                        min = -10;
                        BinarNUD2.Value = kk[2];
                        break;
                    case "KR4":
                        BinarNUD1.Enabled = true;
                        BinarNUD2.Enabled = true;
                        BinarNUD2.Maximum = 0.5m;
                        BinarNUD2.Minimum = 0.2m;
                        max = 0.5m;
                        min = 0.2m;
                        BinarNUD2.Value = kk[3];
                        break;
                    case "KR5":
                        BinarNUD1.Enabled = true;
                        BinarNUD2.Enabled = true;
                        BinarNUD2.Maximum = 1;
                        BinarNUD2.Minimum = 0;
                        max = 1;
                        min = 0;
                        BinarNUD2.Value = kk[4];
                        break;
                    case "KR6":
                        BinarNUD1.Enabled = true;
                        BinarNUD2.Enabled = true;
                        BinarNUD2.Maximum = 1;
                        BinarNUD2.Minimum = 0;
                        max = 1;
                        min = 0;
                        BinarNUD2.Value = kk[5];
                        break;
                }

            }
        }
        private void BinarLoad_Click(object sender, EventArgs e)
        {
            if (BinarPchB.Image != null)
                BinarPchB.Image.Dispose();
            else if(pictureBox1.Image != null)
            {
                BinarizeButton.Enabled = true;
                KR1.Enabled = true;
                KR2.Enabled = true;
                KR3.Enabled = true;
                KR4.Enabled = true;
                KR5.Enabled = true;
                KR6.Enabled = true;
                (sender as Button).Text = "Изменить";
            }
            if (pictureBox1.Image != null)
                if (BinarGrayCHB.Checked)
                    BinarPchB.Image = MyImage.CastingToGray(pictureBox1.Image);
                else
                    BinarPchB.Image = new Bitmap(pictureBox1.Image);

            KR_Refresh();
        }
        private void BinarizeButton_Click(object sender, EventArgs e)
        {
            BinarizeAsync(new CheckBox[] { KR1, KR2, KR3, KR4, KR5, KR6 }.FirstOrDefault(item => item.Checked));
        }
        private async void BinarizeAsync(CheckBox item)
        {
            if(item != null)
            {
                progressBar1.Value = 0;
                this.BinarizeButton.Enabled = false;
                Image tmp = null;
                switch (item.Name)
                {
                    case "KR1":
                        tmp = await Task.Run(() => MyImage.CPP_GlobalBinarize(BinarPchB.Image, 0));
                        break;
                    case "KR2":
                        tmp = await Task.Run(() => MyImage.CPP_GlobalBinarize(BinarPchB.Image, 1));
                        break;
                    case "KR3":
                        tmp = await Task.Run(() => MyImage.CPP_LocalBinarize(BinarPchB.Image, 0, (int)BinarNUD1.Value, (float)BinarNUD2.Value));
                        break;
                    case "KR4":
                        tmp = await Task.Run(() => MyImage.CPP_LocalBinarize(BinarPchB.Image, 1, (int)BinarNUD1.Value, (float)BinarNUD2.Value));
                        break;
                    case "KR5":
                        tmp = await Task.Run(() => MyImage.CPP_LocalBinarize(BinarPchB.Image, 2, (int)BinarNUD1.Value, (float)BinarNUD2.Value));
                        break;
                    case "KR6":
                        tmp = await Task.Run(() => MyImage.CPP_LocalBinarize(BinarPchB.Image, 3, (int)BinarNUD1.Value, (float)BinarNUD2.Value));
                        break;

                    default:
                        break;
                }
                if (tmp != null)
                {
                    progressBar1.Value = 100;

                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = tmp;

                    if ((panel1.Controls[0] as MyCanvas).img != null)
                        (panel1.Controls[0] as MyCanvas).img.Dispose();
                    (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);
                }
                this.button3.Enabled = true;
                this.BinarizeButton.Enabled = true;
            }
        }

        private void BinarNUD1_ValueChanged(object sender, EventArgs e)
        {
            KR_Refresh();
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            try
            {
                List<double> Core = new List<double>();
                int a = Matrix_textBox.Lines.Length;
                int b = Matrix_textBox.Lines[0].Split(' ').Length;
                bool error = false;
                foreach (string str in Matrix_textBox.Lines)
                {
                    if(b != str.Split(' ').Length)
                        error = true;
                    foreach (string item in str.Split(' '))
                    {
                        if (item.Contains('/'))
                        {
                            Fraction tmp = new Fraction(item);
                            Core.Add(tmp.toDouble() * (double)LineKoef.Value);
                        }
                        else
                            Core.Add(Convert.ToDouble(item));
                    }
                }

                if(!error)
                {
                    FMediaBut.Enabled = false;
                    (sender as Button).Enabled = false;
                    button3.Enabled = false;
                    FilterAsync(1, a, b, Core);
                }
            }
            catch
            {

            }
        }
        private void FLoadBut_Click(object sender, EventArgs e)
        {
            if (FilterPb.Image != null)
                FilterPb.Image.Dispose();
            else if (pictureBox1.Image != null)
            {
                Filter.Enabled = true;
            }
            if (pictureBox1.Image != null)
                FilterPb.Image = new Bitmap(pictureBox1.Image);
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    LineKoef.Value = 1;
                    break;
                case 1:
                    LineKoef.Value = (decimal)1 / 3;
                    Matrix_textBox.Clear();
                    Matrix_textBox.Lines = new string[3] { "-1 -1 -1", "0 0 0", "1 1 1" };
                    break;
                case 2:
                    LineKoef.Value = (decimal)1 / 4;
                    Matrix_textBox.Clear();
                    Matrix_textBox.Lines = new string[3] { "-1 -2 -1", "0 0 0", "1 2 1" };
                    break;
                default:
                    break;
            }
        }
        private void GenGauss_Click(object sender, EventArgs e)
        {
            double[] rez = new double[(int)MHeight.Value * (int)MWight.Value];
            MyImage.GetGauss(rez, (double)Sig.Value, (int)MHeight.Value, (int)MWight.Value);

            List<string> str = new List<string>();
            for(int i = 0; i < (int)MHeight.Value; ++i)
            {
                string tmp = "";
                for (int j = 0; j < (int)MWight.Value; ++j)
                {
                    tmp += Math.Round(rez[i * (int)MWight.Value + j], 4).ToString();
                    if (j != (int)MWight.Value - 1)
                        tmp += " ";
                }
                str.Add(tmp);
            }
            SumLbl.Text = "Сумма: " + rez.Sum();
            Matrix_textBox.Clear();
            Matrix_textBox.Lines = str.ToArray();
        }
        private void FMediaBut_Click(object sender, EventArgs e)
        {
            (sender as Button).Enabled = false;
            Filter.Enabled = false;
            button3.Enabled = false;
            FilterAsync(2, (int)MHeight.Value, (int)MWight.Value);
        }
        private void BinarNUD2_ValueChanged(object sender, EventArgs e)
        {
            if(max == (sender as NumericUpDown).Maximum && min == (sender as NumericUpDown).Minimum)
            {
                Bitmap tmp = new Bitmap(
                BinarPchB.Image,
                250, (int)Math.Round((decimal)BinarPchB.Image.Height * ((decimal)250.0 / BinarPchB.Image.Width)));

                var num = (sender as NumericUpDown).Value;

                switch (new CheckBox[] { KR1, KR2, KR3, KR4, KR5, KR6 }.First(item => item.Checked).Name)
                {
                    case "KR1":
                    case "KR2":
                        break;
                    case "KR3":
                        KR3.BackgroundImage.Dispose();
                        KR3.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 0, (int)BinarNUD1.Value, (float)num);
                        kk[2] = num;
                        break;
                    case "KR4":
                        KR4.BackgroundImage.Dispose();
                        KR4.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 1, (int)BinarNUD1.Value, (float)num);
                        kk[3] = num;
                        break;
                    case "KR5":
                        KR5.BackgroundImage.Dispose();
                        KR5.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 2, (int)BinarNUD1.Value, (float)num);
                        kk[4] = num;
                        break;
                    case "KR6":
                        KR6.BackgroundImage.Dispose();
                        KR6.BackgroundImage = MyImage.CPP_LocalBinarize(tmp, 3, (int)BinarNUD1.Value, (float)num);
                        kk[5] = num;
                        break;

                    default:
                        break;
                }
                tmp.Dispose();
            }
        }

        private void ChFLoadBut_Click(object sender, EventArgs e)
        {
            if (ChFilterPb.Image != null)
                ChFilterPb.Image.Dispose();
            if (pictureBox1.Image != null)
            {
                ChFilterPb.Image = new Bitmap(pictureBox1.Image);
                (ChFilterPb.Image as Bitmap).SetResolution(pictureBox1.Image.HorizontalResolution, pictureBox1.Image.VerticalResolution);

                int width = ChFilterPb.Image.Width;
                int height = ChFilterPb.Image.Height;

                var p = Math.Log(width, 2);
                if (p != Math.Floor(p))
                    width = (int)Math.Pow(2, Math.Ceiling(p));
                p = Math.Log(height, 2);
                if (p != Math.Floor(p))
                    height = (int)Math.Pow(2, Math.Ceiling(p));

                Bitmap _tmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                _tmp.SetResolution(ChFilterPb.Image.HorizontalResolution, ChFilterPb.Image.VerticalResolution);

                Graphics g = Graphics.FromImage(_tmp);
                g.DrawImageUnscaled(ChFilterPb.Image, 0, 0);

                ChFLoadAsync(new Bitmap(_tmp));
            }
        }

        private void ChFilter_Click(object sender, EventArgs e)
        {
            ChFilter.Enabled = false;
            ChFilterAsync(new Bitmap(FourierTransform.Image));
        }

        private void VisualK_ValueChanged(object sender, EventArgs e)
        {
            float val = (float)(sender as NumericUpDown).Value;
            if ((panel2.Controls[0] as MyFCanvas).FourierTransform != null)
                (panel2.Controls[0] as MyFCanvas).FourierTransform.Dispose();
            (panel2.Controls[0] as MyFCanvas).FourierTransform = new Bitmap(MyImage.SetImgChannelValue(filter.Item1, 1, val, val, val), (panel2.Controls[0] as MyFCanvas).Size);
        }
        private void ValueK_ValueChanged(object sender, EventArgs e)
        {
            float val = (float)(sender as NumericUpDown).Value;
            if (pictureBox1.Image != null && tmpImage != null)
                pictureBox1.Image.Dispose();
            if (tmpImage != null)
                pictureBox1.Image = MyImage.SetImgChannelValue(tmpImage, 1, val, val, val);
        }
        private void SymmetryChB_CheckedChanged(object sender, EventArgs e)
        {
            (panel2.Controls[0] as MyFCanvas).symmetry_mode = (sender as CheckBox).Checked;
            (panel2.Controls[0] as MyFCanvas).RefreshFurierObr();
        }
        private async void ChFLoadAsync(Image ChFImage)
        {
            progressBar1.Value = 0;
            ChFLoadBut.Enabled = false;
            (Image, double[], double[]) tmpidd = await Task.Run(() => MyImage.CPP_GetDFT(ChFImage));

            if (tmpidd.Item1 != null)
            {
                progressBar1.Value = 100;

                if ((panel2.Controls[0] as MyFCanvas).FourierTransform != null)
                    (panel2.Controls[0] as MyFCanvas).FourierTransform.Dispose();
                (panel2.Controls[0] as MyFCanvas).FourierTransform = new Bitmap(tmpidd.Item1, (panel2.Controls[0] as MyFCanvas).Size);
                (panel2.Controls[0] as MyFCanvas).NewFilterSize(tmpidd.Item1.Size);

                if(filter.Item1 != null)
                    filter.Item1.Dispose();
                filter = tmpidd;
            }
            ChFImage.Dispose();
            FilterType.Enabled = true;
            FilterName.Enabled = true;
            VisualK.Enabled = true;
            ChFilter.Enabled = true;
            ChFLoadBut.Enabled = true;
        }
        private async void ChFilterAsync(Image FTImage)
        {
            progressBar1.Value = 0;
            Image tmpi = await Task.Run(() => MyImage.CPP_ImageFromDFT(FTImage, filter.Item2, filter.Item3));
            (tmpi as Bitmap).SetResolution(pictureBox1.Image.HorizontalResolution, pictureBox1.Image.VerticalResolution);

            Bitmap new_bitamp_ret = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height, PixelFormat.Format24bppRgb);
            new_bitamp_ret.SetResolution(pictureBox1.Image.HorizontalResolution, pictureBox1.Image.VerticalResolution);
            using (Graphics g1 = Graphics.FromImage(new_bitamp_ret))
            {
                g1.DrawImageUnscaled(tmpi, 0, 0);
            }

            if (new_bitamp_ret != null)
            {
                progressBar1.Value = 100;

                pictureBox1.Image.Dispose();
                pictureBox1.Image = new_bitamp_ret;

                if ((panel1.Controls[0] as MyCanvas).img != null)
                    (panel1.Controls[0] as MyCanvas).img.Dispose();
                (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);

                tmpImage = new Bitmap(pictureBox1.Image);
            }
            tmpi.Dispose();
            FTImage.Dispose();
            ValueK.Enabled = true;
            ChFilter.Enabled = true;
            button3.Enabled = true;
        }

        private void FilterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 1)
                nUDN.Enabled = true;
            else
                nUDN.Enabled = false;

            (panel2.Controls[0] as MyFCanvas).RefreshFurierObr();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (FourierTransform.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        FourierTransform.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            (panel2.Controls[0] as MyFCanvas).RefreshFurierObr();
        }

        private async void FilterAsync(int vers, int a, int b, List<double> Core = null)
        {
            progressBar1.Value = 0;
            Image tmpi;
            if (vers == 1)
                tmpi = await Task.Run(() => MyImage.CPP_LineFilter(FilterPb.Image, Core, a, b));
            else
                tmpi = await Task.Run(() => MyImage.CPP_MedianFilter(FilterPb.Image, a, b));

            if (tmpi != null)
            {
                progressBar1.Value = 100;

                pictureBox1.Image.Dispose();
                pictureBox1.Image = tmpi;

                if ((panel1.Controls[0] as MyCanvas).img != null)
                    (panel1.Controls[0] as MyCanvas).img.Dispose();
                (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);
            }

            FMediaBut.Enabled = true;
            Filter.Enabled = true;
            button3.Enabled = true;
        }
    }
}
