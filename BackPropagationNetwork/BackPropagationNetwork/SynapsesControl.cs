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
    public class SynapsesControl:PictureBox
    {
        NeuralNetwork NeuralNetwork;
        Layer From;
        Layer To;
        public SynapsesControl(NeuralNetwork NN, Layer from, Layer to)
        {
            Initialize();
            this.NeuralNetwork = NN;
            From = from;
            To = to;
        }

        void Initialize()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(50, 100);
            this.Image = global::BackPropagationNetwork.Properties.Resources.Synapses;
            this.Cursor = Cursors.Hand;
            this.Click += new EventHandler(SynapsesControl_Click);
        }

        void SynapsesControl_Click(object sender, EventArgs e)
        {
            SynapsesSettings ss = new SynapsesSettings(this.NeuralNetwork, From, To);            
            ss.ShowDialog();
        }       
    }
}
