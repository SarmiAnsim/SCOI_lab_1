namespace SCOI_lab_1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.BinarNUD2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.BinarNUD1 = new System.Windows.Forms.NumericUpDown();
            this.BinarizeButton = new System.Windows.Forms.Button();
            this.BinarGrayCHB = new System.Windows.Forms.CheckBox();
            this.BinarLoad = new System.Windows.Forms.Button();
            this.BinarPchB = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.KR1 = new System.Windows.Forms.CheckBox();
            this.KR2 = new System.Windows.Forms.CheckBox();
            this.KR3 = new System.Windows.Forms.CheckBox();
            this.KR4 = new System.Windows.Forms.CheckBox();
            this.KR5 = new System.Windows.Forms.CheckBox();
            this.KR6 = new System.Windows.Forms.CheckBox();
            this.KRtT = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinarNUD2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinarNUD1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinarPchB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1397, 634);
            this.splitContainer1.SplitterDistance = 1036;
            this.splitContainer1.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(918, 600);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 34);
            this.button3.TabIndex = 2;
            this.button3.Text = "Сохранить";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 606);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(909, 24);
            this.progressBar1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1037, 601);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(357, 634);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(349, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Слои";
            this.tabPage1.ToolTipText = "Работа со слоями и получение результирующего изображения";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Panel2.Controls.Add(this.button1);
            this.splitContainer2.Size = new System.Drawing.Size(343, 599);
            this.splitContainer2.SplitterDistance = 525;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoScrollMinSize = new System.Drawing.Size(0, 526);
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.99145F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.00855F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(343, 525);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.Location = new System.Drawing.Point(201, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 52);
            this.button2.TabIndex = 1;
            this.button2.Text = "Результат";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Location = new System.Drawing.Point(3, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(349, 605);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ГиГП";
            this.tabPage2.ToolTipText = "Гистограмма и градационные преобразования";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(248, 347);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(99, 24);
            this.button4.TabIndex = 3;
            this.button4.Text = "Сбросить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Линейная зависимость",
            "Квадратичный сплайн",
            "Кубический сплайн",
            "Многочлен Лагранжа",
            "Многочлен Ньютона",
            "Кривая Безье"});
            this.comboBox1.Location = new System.Drawing.Point(6, 348);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(236, 24);
            this.comboBox1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 347);
            this.panel1.TabIndex = 1;
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chart1.BackColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.BackColor = System.Drawing.Color.White;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Location = new System.Drawing.Point(0, 375);
            this.chart1.Margin = new System.Windows.Forms.Padding(1);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series2.BorderColor = System.Drawing.Color.Black;
            series2.BorderWidth = 0;
            series2.ChartArea = "ChartArea1";
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            series2.CustomProperties = "PointWidth=1";
            series2.LabelBackColor = System.Drawing.Color.White;
            series2.LabelBorderColor = System.Drawing.Color.White;
            series2.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            series2.LabelBorderWidth = 0;
            series2.Name = "Series1";
            series2.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(347, 230);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "Гистограмма";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tabPage3.Controls.Add(this.BinarizeButton);
            this.tabPage3.Controls.Add(this.KR6);
            this.tabPage3.Controls.Add(this.KR5);
            this.tabPage3.Controls.Add(this.KR4);
            this.tabPage3.Controls.Add(this.KR3);
            this.tabPage3.Controls.Add(this.KR2);
            this.tabPage3.Controls.Add(this.KR1);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.BinarNUD2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.BinarNUD1);
            this.tabPage3.Controls.Add(this.BinarGrayCHB);
            this.tabPage3.Controls.Add(this.BinarLoad);
            this.tabPage3.Controls.Add(this.BinarPchB);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(349, 605);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Бинаризация";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 538);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Чувствительность/усиление ,k";
            // 
            // BinarNUD2
            // 
            this.BinarNUD2.DecimalPlaces = 5;
            this.BinarNUD2.Enabled = false;
            this.BinarNUD2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BinarNUD2.Location = new System.Drawing.Point(237, 536);
            this.BinarNUD2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BinarNUD2.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.BinarNUD2.Name = "BinarNUD2";
            this.BinarNUD2.Size = new System.Drawing.Size(90, 22);
            this.BinarNUD2.TabIndex = 7;
            this.BinarNUD2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BinarNUD2.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.BinarNUD2.ValueChanged += new System.EventHandler(this.BinarNUD2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 504);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Сторона квадратной области";
            // 
            // BinarNUD1
            // 
            this.BinarNUD1.Enabled = false;
            this.BinarNUD1.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BinarNUD1.Location = new System.Drawing.Point(237, 502);
            this.BinarNUD1.Maximum = new decimal(new int[] {
            1001,
            0,
            0,
            0});
            this.BinarNUD1.MaximumSize = new System.Drawing.Size(90, 0);
            this.BinarNUD1.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.BinarNUD1.Name = "BinarNUD1";
            this.BinarNUD1.ReadOnly = true;
            this.BinarNUD1.Size = new System.Drawing.Size(90, 22);
            this.BinarNUD1.TabIndex = 5;
            this.BinarNUD1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BinarNUD1.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.BinarNUD1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.BinarNUD1.ValueChanged += new System.EventHandler(this.BinarNUD1_ValueChanged);
            // 
            // BinarizeButton
            // 
            this.BinarizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BinarizeButton.Enabled = false;
            this.BinarizeButton.Location = new System.Drawing.Point(3, 567);
            this.BinarizeButton.Name = "BinarizeButton";
            this.BinarizeButton.Size = new System.Drawing.Size(343, 35);
            this.BinarizeButton.TabIndex = 4;
            this.BinarizeButton.Text = "Бинаризировать";
            this.BinarizeButton.UseVisualStyleBackColor = true;
            this.BinarizeButton.Click += new System.EventHandler(this.BinarizeButton_Click);
            // 
            // BinarGrayCHB
            // 
            this.BinarGrayCHB.Location = new System.Drawing.Point(145, 13);
            this.BinarGrayCHB.Name = "BinarGrayCHB";
            this.BinarGrayCHB.Size = new System.Drawing.Size(201, 38);
            this.BinarGrayCHB.TabIndex = 2;
            this.BinarGrayCHB.Text = "Привести изображение к градациям серого";
            this.BinarGrayCHB.UseVisualStyleBackColor = true;
            // 
            // BinarLoad
            // 
            this.BinarLoad.Location = new System.Drawing.Point(3, 16);
            this.BinarLoad.Name = "BinarLoad";
            this.BinarLoad.Size = new System.Drawing.Size(136, 30);
            this.BinarLoad.TabIndex = 1;
            this.BinarLoad.Text = "Начать";
            this.BinarLoad.UseVisualStyleBackColor = true;
            this.BinarLoad.Click += new System.EventHandler(this.BinarLoad_Click);
            // 
            // BinarPchB
            // 
            this.BinarPchB.Location = new System.Drawing.Point(2, 57);
            this.BinarPchB.Name = "BinarPchB";
            this.BinarPchB.Size = new System.Drawing.Size(347, 201);
            this.BinarPchB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BinarPchB.TabIndex = 0;
            this.BinarPchB.TabStop = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(5, 5);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(1397, 663);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 1;
            // 
            // KR1
            // 
            this.KR1.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR1.Enabled = false;
            this.KR1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KR1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.KR1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR1.Location = new System.Drawing.Point(15, 279);
            this.KR1.Name = "KR1";
            this.KR1.Size = new System.Drawing.Size(105, 98);
            this.KR1.TabIndex = 9;
            this.KR1.Text = "Критерий Гаврилова";
            this.KR1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR1.UseVisualStyleBackColor = true;
            // 
            // KR2
            // 
            this.KR2.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR2.Enabled = false;
            this.KR2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR2.Location = new System.Drawing.Point(126, 279);
            this.KR2.Name = "KR2";
            this.KR2.Size = new System.Drawing.Size(105, 98);
            this.KR2.TabIndex = 10;
            this.KR2.Text = "Критерий Отсу";
            this.KR2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR2.UseVisualStyleBackColor = true;
            // 
            // KR3
            // 
            this.KR3.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR3.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR3.Enabled = false;
            this.KR3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR3.Location = new System.Drawing.Point(237, 279);
            this.KR3.Name = "KR3";
            this.KR3.Size = new System.Drawing.Size(105, 98);
            this.KR3.TabIndex = 11;
            this.KR3.Text = "Критерий Ниблека";
            this.KR3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR3.UseVisualStyleBackColor = true;
            // 
            // KR4
            // 
            this.KR4.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR4.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR4.Enabled = false;
            this.KR4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR4.Location = new System.Drawing.Point(15, 380);
            this.KR4.Name = "KR4";
            this.KR4.Size = new System.Drawing.Size(105, 98);
            this.KR4.TabIndex = 12;
            this.KR4.Text = "Критерий Сауволы";
            this.KR4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR4.UseVisualStyleBackColor = true;
            // 
            // KR5
            // 
            this.KR5.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR5.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR5.Enabled = false;
            this.KR5.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR5.Location = new System.Drawing.Point(126, 380);
            this.KR5.Name = "KR5";
            this.KR5.Size = new System.Drawing.Size(105, 98);
            this.KR5.TabIndex = 13;
            this.KR5.Text = "Критерий Кристиана Вульфа";
            this.KR5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR5.UseVisualStyleBackColor = true;
            // 
            // KR6
            // 
            this.KR6.Appearance = System.Windows.Forms.Appearance.Button;
            this.KR6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.KR6.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.KR6.Enabled = false;
            this.KR6.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.KR6.Location = new System.Drawing.Point(237, 380);
            this.KR6.Name = "KR6";
            this.KR6.Size = new System.Drawing.Size(105, 98);
            this.KR6.TabIndex = 14;
            this.KR6.Text = "Критерий Брэдли-Рота";
            this.KR6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.KR6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1407, 673);
            this.Controls.Add(this.splitContainer3);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "СЦОИЛаба1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BinarNUD2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinarNUD1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BinarPchB)).EndInit();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox BinarPchB;
        private System.Windows.Forms.CheckBox BinarGrayCHB;
        private System.Windows.Forms.Button BinarLoad;
        private System.Windows.Forms.Button BinarizeButton;
        private System.Windows.Forms.NumericUpDown BinarNUD1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BinarNUD2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox KR6;
        private System.Windows.Forms.CheckBox KR5;
        private System.Windows.Forms.CheckBox KR4;
        private System.Windows.Forms.CheckBox KR3;
        private System.Windows.Forms.CheckBox KR2;
        private System.Windows.Forms.CheckBox KR1;
        private System.Windows.Forms.ToolTip KRtT;
    }
}

