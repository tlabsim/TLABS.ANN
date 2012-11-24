namespace TLABS.BPN
{
    partial class SynapsesSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelSynapses = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.comboShowToLayer = new System.Windows.Forms.ComboBox();
            this.comboShowFromLayer = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRandomizeAll = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnAddSynapsis = new System.Windows.Forms.Button();
            this.numAddWeight = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboAddToLayer = new System.Windows.Forms.ComboBox();
            this.comboAddToNeuron = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboAddFromLayer = new System.Windows.Forms.ComboBox();
            this.comboAddFromNeuron = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAddWeight)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 53);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(66)))), ((int)(((byte)(99)))));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Synapses";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(672, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.Controls.Add(this.panelSynapses);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(776, 476);
            this.panel3.TabIndex = 3;
            // 
            // panelSynapses
            // 
            this.panelSynapses.AutoScroll = true;
            this.panelSynapses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSynapses.Location = new System.Drawing.Point(200, 114);
            this.panelSynapses.Name = "panelSynapses";
            this.panelSynapses.Size = new System.Drawing.Size(576, 278);
            this.panelSynapses.TabIndex = 7;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Silver;
            this.panel7.Controls.Add(this.comboShowToLayer);
            this.panel7.Controls.Add(this.comboShowFromLayer);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(200, 54);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(576, 60);
            this.panel7.TabIndex = 6;
            // 
            // comboShowToLayer
            // 
            this.comboShowToLayer.BackColor = System.Drawing.Color.LightGray;
            this.comboShowToLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboShowToLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboShowToLayer.FormattingEnabled = true;
            this.comboShowToLayer.Location = new System.Drawing.Point(305, 35);
            this.comboShowToLayer.Name = "comboShowToLayer";
            this.comboShowToLayer.Size = new System.Drawing.Size(144, 21);
            this.comboShowToLayer.TabIndex = 5;
            // 
            // comboShowFromLayer
            // 
            this.comboShowFromLayer.BackColor = System.Drawing.Color.LightGray;
            this.comboShowFromLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboShowFromLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboShowFromLayer.FormattingEnabled = true;
            this.comboShowFromLayer.Location = new System.Drawing.Point(128, 35);
            this.comboShowFromLayer.Name = "comboShowFromLayer";
            this.comboShowFromLayer.Size = new System.Drawing.Size(144, 21);
            this.comboShowFromLayer.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label10.Location = new System.Drawing.Point(6, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 20);
            this.label10.TabIndex = 0;
            this.label10.Text = "Synapses";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label12.Location = new System.Drawing.Point(279, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "To:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.ForestGreen;
            this.label13.Location = new System.Drawing.Point(7, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Show synapses:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label11.Location = new System.Drawing.Point(92, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "From:";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DarkGray;
            this.panel6.Controls.Add(this.btnRandomizeAll);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(200, 392);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(576, 43);
            this.panel6.TabIndex = 5;
            // 
            // btnRandomizeAll
            // 
            this.btnRandomizeAll.Location = new System.Drawing.Point(334, 9);
            this.btnRandomizeAll.Name = "btnRandomizeAll";
            this.btnRandomizeAll.Size = new System.Drawing.Size(213, 23);
            this.btnRandomizeAll.TabIndex = 8;
            this.btnRandomizeAll.Text = "Randomize all synapsis weights";
            this.btnRandomizeAll.UseVisualStyleBackColor = true;
            this.btnRandomizeAll.Click += new System.EventHandler(this.btnRandomizeAll_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.btnAddSynapsis);
            this.panel4.Controls.Add(this.numAddWeight);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 54);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 381);
            this.panel4.TabIndex = 4;
            // 
            // btnAddSynapsis
            // 
            this.btnAddSynapsis.Location = new System.Drawing.Point(17, 282);
            this.btnAddSynapsis.Name = "btnAddSynapsis";
            this.btnAddSynapsis.Size = new System.Drawing.Size(174, 23);
            this.btnAddSynapsis.TabIndex = 8;
            this.btnAddSynapsis.Text = "Add >";
            this.btnAddSynapsis.UseVisualStyleBackColor = true;
            this.btnAddSynapsis.Click += new System.EventHandler(this.btnAddSynapsis_Click);
            // 
            // numAddWeight
            // 
            this.numAddWeight.BackColor = System.Drawing.Color.LightGray;
            this.numAddWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.numAddWeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numAddWeight.Location = new System.Drawing.Point(53, 256);
            this.numAddWeight.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numAddWeight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.numAddWeight.Name = "numAddWeight";
            this.numAddWeight.Size = new System.Drawing.Size(138, 20);
            this.numAddWeight.TabIndex = 7;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboAddToLayer);
            this.groupBox3.Controls.Add(this.comboAddToNeuron);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox3.Location = new System.Drawing.Point(3, 160);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(191, 88);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "To";
            // 
            // comboAddToLayer
            // 
            this.comboAddToLayer.BackColor = System.Drawing.Color.LightGray;
            this.comboAddToLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboAddToLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboAddToLayer.FormattingEnabled = true;
            this.comboAddToLayer.Location = new System.Drawing.Point(51, 25);
            this.comboAddToLayer.Name = "comboAddToLayer";
            this.comboAddToLayer.Size = new System.Drawing.Size(134, 21);
            this.comboAddToLayer.TabIndex = 5;
            this.comboAddToLayer.SelectedIndexChanged += new System.EventHandler(this.comboAddToLayer_SelectedIndexChanged);
            // 
            // comboAddToNeuron
            // 
            this.comboAddToNeuron.BackColor = System.Drawing.Color.LightGray;
            this.comboAddToNeuron.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboAddToNeuron.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboAddToNeuron.FormattingEnabled = true;
            this.comboAddToNeuron.Location = new System.Drawing.Point(51, 56);
            this.comboAddToNeuron.Name = "comboAddToNeuron";
            this.comboAddToNeuron.Size = new System.Drawing.Size(134, 21);
            this.comboAddToNeuron.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Layer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label8.Location = new System.Drawing.Point(6, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Neuron";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboAddFromLayer);
            this.groupBox2.Controls.Add(this.comboAddFromNeuron);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox2.Location = new System.Drawing.Point(3, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 88);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "From";
            // 
            // comboAddFromLayer
            // 
            this.comboAddFromLayer.BackColor = System.Drawing.Color.LightGray;
            this.comboAddFromLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboAddFromLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboAddFromLayer.FormattingEnabled = true;
            this.comboAddFromLayer.Location = new System.Drawing.Point(51, 25);
            this.comboAddFromLayer.Name = "comboAddFromLayer";
            this.comboAddFromLayer.Size = new System.Drawing.Size(134, 21);
            this.comboAddFromLayer.TabIndex = 5;
            this.comboAddFromLayer.SelectedIndexChanged += new System.EventHandler(this.comboAddFromLayer_SelectedIndexChanged);
            // 
            // comboAddFromNeuron
            // 
            this.comboAddFromNeuron.BackColor = System.Drawing.Color.LightGray;
            this.comboAddFromNeuron.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboAddFromNeuron.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.comboAddFromNeuron.FormattingEnabled = true;
            this.comboAddFromNeuron.Location = new System.Drawing.Point(51, 56);
            this.comboAddFromNeuron.Name = "comboAddFromNeuron";
            this.comboAddFromNeuron.Size = new System.Drawing.Size(134, 21);
            this.comboAddFromNeuron.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Layer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label6.Location = new System.Drawing.Point(6, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Neuron";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label9.Location = new System.Drawing.Point(10, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Weight";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.DarkGray;
            this.panel9.Controls.Add(this.pictureBox1);
            this.panel9.Controls.Add(this.label2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(200, 60);
            this.panel9.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BackPropagationNetwork.Properties.Resources.Add;
            this.pictureBox1.Location = new System.Drawing.Point(12, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 17);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.label2.Location = new System.Drawing.Point(28, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Add synapsis";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DimGray;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 53);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(776, 1);
            this.panel5.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panel2.Controls.Add(this.panel10);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 435);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(776, 41);
            this.panel2.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(776, 1);
            this.panel10.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label14.Location = new System.Drawing.Point(40, 334);
            this.label14.MaximumSize = new System.Drawing.Size(160, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(160, 36);
            this.label14.TabIndex = 9;
            this.label14.Text = "You can add any feedback synapsis between two neurons, even from and to same neur" +
                "on.";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::BackPropagationNetwork.Properties.Resources.Categories_system_help_icon;
            this.pictureBox2.Location = new System.Drawing.Point(12, 338);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // SynapsesSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SynapsesSettings";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SynapsesSettings";
            this.Load += new System.EventHandler(this.SynapsesSettings_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SynapsesSettings_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SynapsesSettings_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SynapsesSettings_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAddWeight)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboAddToLayer;
        private System.Windows.Forms.ComboBox comboAddToNeuron;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboAddFromLayer;
        private System.Windows.Forms.ComboBox comboAddFromNeuron;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddSynapsis;
        private System.Windows.Forms.NumericUpDown numAddWeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboShowToLayer;
        private System.Windows.Forms.ComboBox comboShowFromLayer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnRandomizeAll;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panelSynapses;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBox2;

    }
}