using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TLABS.ANN;

namespace TLABS.BPN
{
    public partial class SynapsesSettings : Form
    {
        bool IsMouseDown = false;
        Point InitPos = new Point();
        Point CurPos = new Point();

        List<Layer> AddFromLayer = new List<Layer>();
        List<Layer> AddToLayer = new List<Layer>();
        List<string> LayerList2 = new List<string>();
     
        NeuralNetwork NeuralNetwork;
        Layer From;
        Layer To;

        public SynapsesSettings(NeuralNetwork nn, Layer from, Layer to)
        {
            InitializeComponent();
            NeuralNetwork = nn; 
            From = from;
            To = to;
            SetUI();
        }

        void SetUI()
        {
            int f=0, t=0, c=0;
            Layer current = NeuralNetwork.InputLayer;
            while(current !=null)
            {
                if (From != null && From == current) f = c;
                if (To != null && To == current) t = c;
                AddFromLayer.Add(current);
                AddToLayer.Add(current);
                LayerList2.Add(current.Name);
                current = current.NextLayer;
                c++;
            }
            comboAddFromLayer.DataSource = AddFromLayer;
            comboAddFromLayer.SelectedIndex = f;
            comboAddToLayer.DataSource = AddToLayer;
            comboAddToLayer.SelectedIndex = t;
            LayerList2.Add("All layers");           
            comboShowFromLayer.DataSource = LayerList2.ToList();
            comboShowFromLayer.SelectedIndex = f;
            comboShowToLayer.DataSource = LayerList2.ToList();
            comboShowToLayer.SelectedIndex = t;
            comboShowFromLayer.SelectedIndexChanged += new EventHandler(comboShowFromLayer_SelectedIndexChanged);
            comboShowToLayer.SelectedIndexChanged +=new EventHandler(comboShowToLayer_SelectedIndexChanged);
            ShowSynapses();
        }

        void ShowSynapses()
        {
            try
            {
                List<Synapsis> synapses = NeuralNetwork.Synapses.ToList();
                if (comboShowFromLayer.SelectedItem.ToString() != "All layers")
                {
                    Layer l = this.NeuralNetwork.GetLayer(comboShowFromLayer.SelectedItem.ToString());
                    if (l != null)
                    {
                        synapses = synapses.FindAll(delegate(Synapsis s) { return s.From.Layer == l; });
                    }
                    else MessageBox.Show("Damn");
                }

                if (comboShowToLayer.SelectedItem.ToString() != "All layers")
                {
                    Layer l = this.NeuralNetwork.GetLayer(comboShowToLayer.SelectedItem.ToString());
                    if (l != null)
                    {
                        synapses = synapses.FindAll(delegate(Synapsis s) { return s.To.Layer == l; });
                    }
                }

                panelSynapses.Controls.Clear();
                Point P = new Point(0, 0);
                for (int i = 0; i < synapses.Count; i++)
                {
                    SynapsisControl sc = new SynapsisControl(NeuralNetwork, synapses[i]);
                    panelSynapses.Controls.Add(sc);
                    sc.Deleted = SynapsisDeleted;
                    sc.Location = P;
                    P.Y += 44;
                }
            }
            catch
            {
            }
        }

        void SynapsisDeleted()
        {
            ShowSynapses();
        }


        private void SynapsesSettings_Load(object sender, EventArgs e)
        {

        }

        private void SynapsesSettings_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            InitPos = this.Location;
            CurPos = Cursor.Position;
        }

        private void SynapsesSettings_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }

        private void SynapsesSettings_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                this.Location = new Point(InitPos.X + Cursor.Position.X - CurPos.X, InitPos.Y + Cursor.Position.Y - CurPos.Y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddSynapsis_Click(object sender, EventArgs e)
        {
            Synapsis s = new Synapsis();
            s.From = ((Layer)comboAddFromLayer.SelectedItem).Neurons[comboAddFromNeuron.SelectedIndex];
            s.To = ((Layer)comboAddToLayer.SelectedItem).Neurons[comboAddToNeuron.SelectedIndex];
            s.Weight = (double)numAddWeight.Value;
            this.NeuralNetwork.Synapses.Add(s);
            if (comboAddFromLayer.Text == comboShowFromLayer.Text || comboAddToLayer.Text == comboShowToLayer.Text)
            {
                ShowSynapses();
            }
        }

        private void btnRandomizeAll_Click(object sender, EventArgs e)
        {
            foreach (Synapsis s in NeuralNetwork.Synapses)
            {
                s.SetRandomWeight();
            }
            foreach (Control c in panelSynapses.Controls)
            {
                if (c.GetType() == typeof(SynapsisControl))
                {
                    SynapsisControl sc = c as SynapsisControl;
                    sc.UpdateWeightChange();
                }
            }
        }

        private void comboShowFromLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSynapses();
        }

        private void comboShowToLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSynapses();
        }

        private void comboAddFromLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layer l = (Layer)comboAddFromLayer.SelectedItem;
            List<string> n = new List<string>();
            for (int i = 0; i < l.NumberOfNeurons; i++)
            {
                n.Add("Neuron " + i);
            }
            comboAddFromNeuron.DataSource = n;
        }

        private void comboAddToLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Layer l = (Layer)comboAddToLayer.SelectedItem;
            List<string> n = new List<string>();
            for (int i = 0; i < l.NumberOfNeurons; i++)
            {
                n.Add("Neuron " + i);
            }
            comboAddToNeuron.DataSource = n;
        }
    }
}
