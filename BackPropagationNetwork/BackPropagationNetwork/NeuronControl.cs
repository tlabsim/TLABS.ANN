using System;
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
    public class NeuronControl:Panel
    {
        public delegate void NeuronControlClickedDelegate(NeuronControl sender);
        public NeuronControlClickedDelegate NeuronControlClicked;
        bool _Selected = false;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                if (_Selected)
                {
                    this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.NeuronSelected;
                }
                else
                {
                    this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.neuron;
                }
            }
        }
        Neuron _Neuron;
        public Neuron Neuron
        {
            get
            {
                return _Neuron;
            }
            set
            {
                _Neuron = value;
                if (_Neuron != null)
                {
                    labIndex.Text = (_Neuron.Index + 1).ToString();
                    labIndex.Location = new Point((this.Width - labIndex.Width) / 2, (this.Height - labIndex.Height) / 2);
                }
            }
        }
        Label labIndex = new Label();
        public NeuronControl()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(122, 50);
            this.Cursor = Cursors.Hand;
            this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.neuron;
            this.MouseEnter += new EventHandler(NeuronControl_MouseEnter);
            this.MouseLeave += new EventHandler(NeuronControl_MouseLeave);
            this.Click += new EventHandler(NeuronControl_Click);
            this.Controls.Add(labIndex);
            labIndex.AutoSize = true;
            labIndex.BackColor = Color.Transparent;
            labIndex.ForeColor = Color.Black;
            labIndex.Font = new Font("MS Sans serif", 16, FontStyle.Regular);
            labIndex.Click += NeuronControl_Click;
        }

        void NeuronControl_Click(object sender, EventArgs e)
        {
            if (NeuronControlClicked != null)
            {
                NeuronControlClicked(this);
            }            
        }

        void NeuronControl_MouseLeave(object sender, EventArgs e)
        {
            if (!this.Selected)
            {
                this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.neuron;
            }
        }

        void NeuronControl_MouseEnter(object sender, EventArgs e)
        {
            if (!this.Selected)
            {
                this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.NeuronHover;
            }
        }     
    }
}
