using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void BinarLoad_Click(object sender, EventArgs e)
        {
            if (BinarPchB.Image != null)
                BinarPchB.Image.Dispose();
            else if(pictureBox1.Image != null)
            {
                BinarCmB.SelectedIndex = 0;
                BinarCmB.Enabled = true;
                BinarizeButton.Enabled = true;
                (sender as Button).Text = "Изменить";
            }
            if (pictureBox1.Image != null)
                if (BinarGrayCHB.Checked)
                    BinarPchB.Image = MyImage.CastingToGray(pictureBox1.Image);
                else
                    BinarPchB.Image = new Bitmap(pictureBox1.Image);

        }

        private void BinarizeButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();
            switch (BinarCmB.SelectedIndex)
            {
                case 0:
                    pictureBox1.Image = MyImage.CPU_GlobalBinarize(BinarPchB.Image, 0);
                    break;
                case 1:
                    pictureBox1.Image = MyImage.CPU_GlobalBinarize(BinarPchB.Image, 1);
                    break;
                case 2:
                    pictureBox1.Image = MyImage.CPU_LocalBinarize(BinarPchB.Image, 0);
                    break;
            }

            (panel1.Controls[0] as MyCanvas).img = new Bitmap(pictureBox1.Image);
        }
    }
}
