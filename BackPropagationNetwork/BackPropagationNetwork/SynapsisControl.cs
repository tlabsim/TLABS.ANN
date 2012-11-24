using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TLABS.ANN;
using TLABS.Controls;
namespace TLABS.BPN
{
    public delegate void SynapsisDeletedDelegate();
    public class SynapsisControl:Control
    {
        NeuralNetwork NN;
        Synapsis Synapsis;
        NumericTextBox numWeight;        
        ToolTip Tooltip = new ToolTip();
        public SynapsisDeletedDelegate Deleted;
        public SynapsisControl(NeuralNetwork nn, Synapsis synapsis)
        {
            NN = nn;
            Synapsis = synapsis;
            SetUI();
        }

        public void SetUI()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(562, 40);             
            this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.scb;
            Label labFrom = new Label();
            labFrom.BackColor = Color.Transparent;
            labFrom.AutoSize = true;
            labFrom.Text = Synapsis.From.Layer.Name + ": Neuron " + Synapsis.From.Index.ToString();
            labFrom.ForeColor = Color.DarkCyan;
            this.Controls.Add(labFrom);
            labFrom.Location = new Point(22, 16);
            Label labTo = new Label();
            labTo.BackColor = Color.Transparent;
            labTo.AutoSize = true;
            labTo.Text = Synapsis.To.Layer.Name + ": Neuron " + Synapsis.To.Index.ToString();
            labTo.ForeColor = Color.DarkCyan;
            this.Controls.Add(labTo);
            labTo.Location = new Point(192, 16);
            numWeight = new NumericTextBox();
            numWeight.BackColor = Color.WhiteSmoke;     
            numWeight.BorderStyle = BorderStyle.None;
            numWeight.Size = new Size(95, 13); 
            numWeight.ForeColor = Color.FromArgb(75, 75, 75);
            numWeight.TextChanged +=new EventHandler(numWeight_TextChanged);
            numWeight.Text = Synapsis.Weight.ToString();
            this.Controls.Add(numWeight);
            numWeight.Location = new Point(400, 15);

            PictureBox picRandomize = new PictureBox();
            picRandomize.Image = global::BackPropagationNetwork.Properties.Resources.Randomize;
            picRandomize.Cursor = Cursors.Hand;
            picRandomize.Size = new Size(21, 21);
            this.Controls.Add(picRandomize);
            picRandomize.Location = new Point(504, 12);
            Tooltip.SetToolTip(picRandomize, "Randomize synapsis weight");
            picRandomize.Click += new EventHandler(picRandomize_Click);

            PictureBox picRemove = new PictureBox();
            picRemove.Image = global::BackPropagationNetwork.Properties.Resources.delete;
            picRemove.Cursor = Cursors.Hand;
            picRemove.Size = new Size(21, 21);
            this.Controls.Add(picRemove);
            picRemove.Location = new Point(525, 12);
            Tooltip.SetToolTip(picRemove, "Remove this synapsis");
            picRemove.Click += new EventHandler(picRemove_Click);

        }

        void picRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to delete this synapsis?", "Delete synapsis?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (NN.Synapses.Remove(this.Synapsis))
                    {
                        if (this.Deleted != null)
                        {
                            Deleted();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        void picRandomize_Click(object sender, EventArgs e)
        {
            Synapsis.SetRandomWeight();
            numWeight.Text = Synapsis.Weight.ToString();
        }
     
        void numWeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Synapsis.Weight = double.Parse(numWeight.Text);
            }
            catch
            {
            }
        }

        public void UpdateWeightChange()
        {
            numWeight.Text = Synapsis.Weight.ToString();
        }

    }
}
