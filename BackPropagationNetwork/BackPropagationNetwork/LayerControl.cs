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
    public class LayerControl : Panel
    {
        Layer _Layer;
        Label labName = new Label();
        Label labNON = new Label();
        PictureBox picCF = new PictureBox();
        PictureBox picAF = new PictureBox();
        ToolTip ToolTip = new ToolTip();
        int _NON = 0;

        public Layer Layer
        {
            get
            {
                return _Layer;
            }
            set
            {
                this._Layer = value;
                if (value != null)
                {
                    this.Title = _Layer.Name;
                    this.NumberOfNeurons = _Layer.NumberOfNeurons;
                    ToolTip.SetToolTip(picAF, "Input combination function: " + _Layer.InputCombinationFunction.ToString());
                    if (_Layer.ActivationFunction != null)
                    {
                        ToolTip.SetToolTip(picCF, "Activation Function: " + _Layer.ActivationFunction.FunctionName);
                    }
                    else
                    {
                        ToolTip.SetToolTip(picCF, "Activation Function: Not set");
                    }
                }
            }
        }
        string Title
        {
            get
            {
                return this.labName.Text;
            }
            set
            {
                this.labName.Text = value;
                labName.Location = new Point((this.Width - labName.Width) / 2, 9);
            }
        }

        int NumberOfNeurons
        {
            get
            {
                return _NON;
            }
            set
            {
                _NON = value;
                labNON.Text = value.ToString() + " neurons(s)";
                labNON.Location = new Point((this.Width - labNON.Width) / 2, 77);
            }
        }




        public LayerControl()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(100, 100);
            this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.Layer;
            this.MouseEnter += new EventHandler(LayerControl_MouseEnter);
            this.MouseLeave += new EventHandler(LayerControl_MouseLeave);
            this.Click += new EventHandler(LayerControl_Click);
            this.Controls.Add(labName);
            this.Controls.Add(labNON);
            this.Controls.Add(picAF);
            this.Controls.Add(picCF);
            foreach (Control control in this.Controls)
            {
                control.Click +=LayerControl_Click;
            }
            labName.Location = new Point(5, 5);
            labName.AutoSize = true;
            labNON.AutoSize = true;
            labName.BackColor = Color.Transparent;
            labName.ForeColor = Color.DarkCyan;
            labNON.BackColor = Color.Transparent;
            labNON.ForeColor = Color.SteelBlue;
            this.Cursor = Cursors.Hand;
            picAF.BackColor = Color.Transparent;
            picCF.BackColor = Color.Transparent;
            picAF.Size = new Size(28, 28);
            picAF.Location = new Point(15, 36);
            picCF.Size = new Size(28, 28);
            picCF.Location = new Point(55, 36);           
        }

        void LayerControl_Click(object sender, EventArgs e)
        {
            if (this._Layer != null)
            {
                LayerSettings LS = new LayerSettings();
                LS.Layer = this._Layer;
                LS.ShowDialog();
                this.NumberOfNeurons = _Layer.NumberOfNeurons;
                ToolTip.SetToolTip(picAF, "Input combination function: " + _Layer.InputCombinationFunction.ToString());
                if (_Layer.ActivationFunction != null)
                {
                    ToolTip.SetToolTip(picCF, "Activation Function: " + _Layer.ActivationFunction.FunctionName);
                }
                else
                {
                    ToolTip.SetToolTip(picCF, "Activation Function: Not set");
                }                
            }
        }

        void LayerControl_MouseEnter(object sender, EventArgs e)
        {
            this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.LayerHover;
        }
        void LayerControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackgroundImage = global::BackPropagationNetwork.Properties.Resources.Layer;
        }


    }
}
