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
    public partial class LayerSettings : Form
    {
        bool IsMouseDown = false;
        Point InitPos = new Point();
        Point CurPos = new Point();
        int prenn = 0;
        #region Activation function
        UnipolarSignFunction USF = new UnipolarSignFunction();
        BipolarSignFunction BSF = new BipolarSignFunction();
        UnipolarLinearFunction ULF = new UnipolarLinearFunction();
        BipolarLinearFunction BLF = new BipolarLinearFunction();
        SigmoidalUnipolarFunction SUF = new SigmoidalUnipolarFunction();
        SigmoidalBipolarFunction SBF = new SigmoidalBipolarFunction();
        TangentHyperbolicFunction THF = new TangentHyperbolicFunction();
        GaussianActivationFunction GAF = new GaussianActivationFunction(); 
        #endregion

        #region Graphs
        Image GraphUSF { get { return graphUSF.Image; } set { graphUSF.Image = value; } }
        Image GraphBSF { get { return graphBSF.Image; } set { graphBSF.Image = value; } }
        Image GraphULF { get { return graphULF.Image; } set { graphULF.Image = value; } }
        Image GraphBLF { get { return graphBLF.Image; } set { graphBLF.Image = value; } }
        Image GraphSUF { get { return graphSUF.Image; } set { graphSUF.Image = value; } }
        Image GraphSBF { get { return graphSBF.Image; } set { graphSBF.Image = value; } }
        Image GraphTHF { get { return graphTHF.Image; } set { graphTHF.Image = value; } }
        Image GraphGAF { get { return graphGAF.Image; } set { graphGAF.Image = value; } } 
        #endregion

        Layer _Layer;
        public Layer Layer
        {
            get
            {
                return _Layer;
            }
            set
            {
                _Layer = value;
                SetUI();
            }
        }
        Neuron SelectedNeuron;

        void SetUI()
        {
            if (_Layer != null)
            {
                prenn = _Layer.NumberOfNeurons;
                this.labLayerType.Text = "Layer type: " + _Layer.LayerType.ToString() + " layer";
                numNeurons.Value = _Layer.NumberOfNeurons;
                Point p = new Point(50, 0);
                for (int i = 0; i < _Layer.Neurons.Count; i++)
                {
                    NeuronControl nc = new NeuronControl();
                    nc.Neuron = _Layer.Neurons[i];
                    nc.NeuronControlClicked = new NeuronControl.NeuronControlClickedDelegate(SetCurrentNeuron);
                    panelNeurons.Controls.Add(nc);
                    nc.Location = p;
                    p.Y += 55;
                }               
                switch (_Layer.InputCombinationFunction)
                {
                    case InputCombinationFunctionType.LinearProduct:
                        radioCFLP.Checked = true;
                        labCFHelp.Text = "The net input is the sum of the linear products of outputs of the neurons in the previous layer and weights of the pre synapses.";
                        break;
                    case InputCombinationFunctionType.EucledianDistance:
                        radCFED.Checked = true;
                        labCFHelp.Text = "The net input is the sum of eucledian distance between the synapses weights and outputs of the previous layers neuron.";
                        break;
                    case InputCombinationFunctionType.ManhattanDistance:
                        radCFMD.Checked = true;
                        labCFHelp.Text = "The net input is the sum of manhattan distance between the synapses weights and outputs of the previous layers neuron.";
                        break;
                }
                if (_Layer.ActivationFunction != null)
                {
                    switch (_Layer.ActivationFunction.FunctionName)
                    {
                        case "Unipolar Sign Function":
                            USF = (UnipolarSignFunction)_Layer.ActivationFunction;
                            radAFUSF.Checked = true;
                            break;
                        case "Bipolar Sign Function":
                            BSF = (BipolarSignFunction)_Layer.ActivationFunction;
                            radAFBSF.Checked = true;
                            break;
                        case "Unipolar Linear Function":
                            ULF = (UnipolarLinearFunction)_Layer.ActivationFunction;
                            radAFULF.Checked = true;
                            break;
                        case "Bipolar Linear Function":
                            BLF = (BipolarLinearFunction)_Layer.ActivationFunction;
                            radAFBLF.Checked = true;
                            break;
                        case "Sigmoidal Unipolar Function":
                            SUF = (SigmoidalUnipolarFunction)_Layer.ActivationFunction;
                            radAFSUF.Checked = true;
                            break;
                        case "Sigmoidal Bipolar Function":
                            SBF = (SigmoidalBipolarFunction)_Layer.ActivationFunction;
                            radAFSBF.Checked = true;
                            break;
                        case "Tangent Hyperbolic Function":
                            THF = (TangentHyperbolicFunction)_Layer.ActivationFunction;
                            radAFTHF.Checked = true;
                            break;
                        case "Gaussian Activation Function":
                            GAF = (GaussianActivationFunction)_Layer.ActivationFunction;
                            radAFGAF.Checked = true;
                            break;
                    }
                    SetAFParams();
                }
                else
                {
                    radAFNone.Checked = true;
                }
               
            }
        }

        public LayerSettings()
        {
            InitializeComponent();
            SetAFParams();
            DrawGraphs();
        }

        void SetAFParams()
        {
            numULFScalingFactor.Value = (decimal)ULF.ScalingFactor;
            numBLFScalingFactor.Value = (decimal)BLF.ScalingFactor;
            numSUFSigmoidalGain.Value = (decimal)SUF.SigmoidalGain;
            numSBFSigmoidalGain.Value = (decimal)SBF.SigmoidalGain;
            numTHFSigmoidalGain.Value = (decimal)THF.SigmoidalGain;
            numGAFMean.Value = (decimal)GAF.Mean;
            numGAFDeviation.Value = (decimal)GAF.Deviation;
        }

        void DrawGraphs()
        {
            DrawGraphUSF();
            DrawGraphBSF();
            DrawGraphULF();
            DrawGraphBLF();
            DrawGraphSUF();
            DrawGraphSBF();
            DrawGraphTHF();
            DrawGraphGAF();
        }

        void DrawGraphUSF()
        {
            int w = graphUSF.Width;
            int h = graphUSF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)USF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphUSF = b;
        }

        void DrawGraphBSF()
        {
            int w = graphBSF.Width;
            int h = graphBSF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)BSF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphBSF = b;
        }

        void DrawGraphULF()
        {
            int w = graphULF.Width;
            int h = graphULF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x+=0.1f)
            {
                y = (float)ULF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphULF = b;
        }

        void DrawGraphBLF()
        {
            int w = graphBLF.Width;
            int h = graphBLF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)BLF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphBLF = b;
        }

        void DrawGraphSUF()
        {
            int w = graphSUF.Width;
            int h = graphSUF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)SUF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphSUF = b;
        }

        void DrawGraphSBF()
        {
            int w = graphSBF.Width;
            int h = graphSBF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)SBF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphSBF = b;
        }

        void DrawGraphTHF()
        {
            int w = graphTHF.Width;
            int h = graphTHF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)THF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphTHF = b;
        }

        void DrawGraphGAF()
        {
            int w = graphGAF.Width;
            int h = graphGAF.Height;
            Bitmap b = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(b);
            //Draw axes
            Pen axes_pen = new Pen(new SolidBrush(Color.Green), 1);
            Pen graph_pen = new Pen(new SolidBrush(Color.Red));
            Brush label_brush = new SolidBrush(Color.Blue);
            Font Font = new System.Drawing.Font("MS Sans serif", 7);
            List<PointF> graphpoints = new List<PointF>();
            int min_x = -10;
            int max_x = 10;
            int min_y = -2;
            int max_y = 2;
            int hw = w / 2;
            int hh = h / 2;
            double dx = (float)w / ((float)max_x - (float)min_x);
            double dy = (float)h / ((float)min_y - (float)max_y);
            int hs = 5; //Horizontal separation
            int vs = 1; //Vertical separation
            int p1x, p2x, p1y, p2y;
            // Draw axes
            g.DrawLine(axes_pen, new Point(0, h / 2), new Point(w, h / 2));
            g.DrawLine(axes_pen, new Point(w / 2, 0), new Point(w / 2, h));
            for (int i = min_x; i <= max_x; i += hs)
            {
                if (i == 0) continue;
                p1x = hw + (int)(dx * i);
                p1y = hh - 5;
                p2x = p1x;
                p2y = hh + 5;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 5, p2y + 5);
            }
            for (int i = max_y; i >= min_y; i -= vs)
            {
                if (i == 0) continue;
                p1x = hw + -5;
                p1y = hh + (int)(dy * i);
                p2x = hw + 5;
                p2y = p1y;
                g.DrawLine(axes_pen, p1x, p1y, p2x, p2y);
                g.DrawString(i.ToString(), Font, label_brush, p1x - 15, p1y - 5);
            }

            float x, y;
            for (x = -10; x <= 10; x += 0.1f)
            {
                y = (float)GAF.GetOutput(x);
                graphpoints.Add(new PointF(x, y));
            }

            for (int i = 0; i < graphpoints.Count - 1; i++)
            {
                p1x = hw + (int)(dx * graphpoints[i].X);
                p1y = hh + (int)(dy * graphpoints[i].Y);
                p2x = hw + (int)(dx * graphpoints[i + 1].X);
                p2y = hh + (int)(dy * graphpoints[i + 1].Y);
                g.DrawLine(graph_pen, p1x, p1y, p2x, p2y);
            }
            GraphGAF = b;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_Layer.NumberOfNeurons != prenn)
            {
                _Layer.Network.CreateSynapses();
            }
            this.Close();
        }

        private void LayerSettings_Load(object sender, EventArgs e)
        {

        }

        private void LayerSettings_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            InitPos = this.Location;
            CurPos = Cursor.Position;
        }

        private void LayerSettings_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                this.Location = new Point(InitPos.X + Cursor.Position.X - CurPos.X, InitPos.Y + Cursor.Position.Y - CurPos.Y);
            }
        }

        private void LayerSettings_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }

        private void numNeurons_ValueChanged(object sender, EventArgs e)
        {
            int n = (int)numNeurons.Value;
            if (_Layer.NumberOfNeurons != n)
            {
                _Layer.NumberOfNeurons = n;
                panelNeurons.Controls.Clear();
                Point p = new Point(50, 0);
                for (int i = 0; i < _Layer.Neurons.Count; i++)
                {
                    NeuronControl nc = new NeuronControl();
                    nc.Neuron = _Layer.Neurons[i];
                    nc.NeuronControlClicked = new NeuronControl.NeuronControlClickedDelegate(SetCurrentNeuron);
                    panelNeurons.Controls.Add(nc);
                    nc.Location = p;
                    p.Y += 55;
                }
            }
        }

        void SetCurrentNeuron(NeuronControl neuron_control)
        {
            if (!txtNeuronThreshold.Enabled) txtNeuronThreshold.Enabled = true;
            if (!btnRandThreshold.Enabled) btnRandThreshold.Enabled = true;
            foreach (NeuronControl nc in panelNeurons.Controls)
            {
                if (!nc.Equals(neuron_control))
                {
                    nc.Selected = false;
                }
            }
            neuron_control.Selected = true;
            SelectedNeuron = neuron_control.Neuron;
            txtNeuronThreshold.Value = (decimal)SelectedNeuron.Threshold;
        }

        private void panelNeurons_MouseHover(object sender, EventArgs e)
        {
            if (panelNeurons.CanFocus)
            {
                panelNeurons.Focus();
            }
        }

        private void txtNeuronThreshold_ValueChanged(object sender, EventArgs e)
        {
            double t = (double)txtNeuronThreshold.Value;
            if (SelectedNeuron != null)
            {
                SelectedNeuron.Threshold = t;
            }

        }

        private void txtMinThreshold_ValueChanged(object sender, EventArgs e)
        {
            if ((double)txtMinThreshold.Value > (double)txtMaxThreshold.Value)
            {
                txtMinThreshold.Value = txtMaxThreshold.Value;
            }
        }

        private void txtMaxThreshold_ValueChanged(object sender, EventArgs e)
        {
            if ((double)txtMinThreshold.Value > (double)txtMaxThreshold.Value)
            {
                txtMaxThreshold.Value = txtMinThreshold.Value;
            }
        }

        private void btnRandThreshold_Click(object sender, EventArgs e)
        {
            if (SelectedNeuron != null)
            {
                double Min = (double)txtMinThreshold.Value;
                double Max = (double)txtMaxThreshold.Value;
                SelectedNeuron.SetRandomThreshold(Min, Max);
                txtNeuronThreshold.Value = (decimal)SelectedNeuron.Threshold;
            }
        }

        private void btnRandAllThreshold_Click(object sender, EventArgs e)
        {
            if (_Layer != null)
            {
                double Min = (double)txtMinThreshold.Value;
                double Max = (double)txtMaxThreshold.Value;
                foreach (Neuron n in _Layer.Neurons)
                {
                    n.SetRandomThreshold(Min, Max);
                }
                if (SelectedNeuron != null)
                {
                    txtNeuronThreshold.Value = (decimal)SelectedNeuron.Threshold;
                }
            }
        }

        #region Combination function
        private void radioCFLP_CheckedChanged(object sender, EventArgs e)
        {
            _Layer.InputCombinationFunction = InputCombinationFunctionType.LinearProduct;
            labCFHelp.Text = "The net input is the sum of the linear products of outputs of the neurons in the previous layer and weights of the pre synapses.";
        }

        private void radCFED_CheckedChanged(object sender, EventArgs e)
        {
            _Layer.InputCombinationFunction = InputCombinationFunctionType.EucledianDistance;
            labCFHelp.Text = "The net input is the sum of eucledian distances between the synapses weights and outputs of the previous layers neurons.";
        }

        private void radCFMD_CheckedChanged(object sender, EventArgs e)
        {
            _Layer.InputCombinationFunction = InputCombinationFunctionType.ManhattanDistance;
            labCFHelp.Text = "The net input is the sum of manhattan distances between the synapses weights and outputs of the previous layers neurons.";
        } 
        #endregion

        #region Activation functions

        private void radAFNone_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupAF.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = c as RadioButton;
                    rb.BackColor = Color.DimGray;
                    rb.ForeColor = Color.White;
                }
            }   
            radAFNone.BackColor = Color.Silver;
            radAFNone.ForeColor = Color.FromArgb(75, 75, 75);
            tabAF.Visible = false;
            Layer.ActivationFunction = null;
        }

        private void radAFUSF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFBSF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFULF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFBLF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFSUF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFTHF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFSBF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        private void radAFGAF_CheckedChanged(object sender, EventArgs e)
        {
            SelectAF(sender as RadioButton);
        }

        void SelectAF(RadioButton srb)
        {
            if (!tabAF.Visible) tabAF.Visible = true;
            foreach (Control c in groupAF.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = c as RadioButton;
                    rb.BackColor = Color.DimGray;
                    rb.ForeColor = Color.White;
                }
            }            
            srb.BackColor = Color.Silver;
            srb.ForeColor = Color.FromArgb(75, 75, 75);
            tabAF.SelectedIndex = int.Parse(srb.Tag.ToString());
            switch (srb.Tag.ToString())
            {
                case "0":
                    _Layer.ActivationFunction = USF;
                    break;
                case "1":
                    _Layer.ActivationFunction = BSF;
                    break;
                case "2":
                    _Layer.ActivationFunction = ULF;
                    break;
                case "3":
                    _Layer.ActivationFunction = BLF;
                    break;
                case "4":
                    _Layer.ActivationFunction = SUF;
                    break;
                case "5":
                    _Layer.ActivationFunction = SBF;
                    break;
                case "6":
                    _Layer.ActivationFunction = THF;
                    break;
                case "7":
                    _Layer.ActivationFunction = GAF;
                    break;
            }
        } 
        #endregion           

        private void numULFScalingFactor_ValueChanged(object sender, EventArgs e)
        {
            ULF.ScalingFactor = (double)numULFScalingFactor.Value;
            DrawGraphULF();
        }

        private void numBLFScalingFactor_ValueChanged(object sender, EventArgs e)
        {
            BLF.ScalingFactor = (double)numBLFScalingFactor.Value;
            DrawGraphBLF();
        }

        private void numSUFSigmoidalGain_ValueChanged(object sender, EventArgs e)
        {
            SUF.SigmoidalGain = (double)numSUFSigmoidalGain.Value;
            DrawGraphSUF();
        }

        private void numSBFSigmoidalGain_ValueChanged(object sender, EventArgs e)
        {
            SBF.SigmoidalGain = (double)numSBFSigmoidalGain.Value;
            DrawGraphSBF();
        }

        private void numTHFSigmoidalGain_ValueChanged(object sender, EventArgs e)
        {
            THF.SigmoidalGain = (double)numTHFSigmoidalGain.Value;
            DrawGraphTHF();
        }

        private void numGAFMean_ValueChanged(object sender, EventArgs e)
        {
            GAF.Mean = (double)numGAFMean.Value;
            DrawGraphGAF();
        }

        private void numGAFDeviation_ValueChanged(object sender, EventArgs e)
        {
            GAF.Deviation = (double)numGAFDeviation.Value;
            DrawGraphGAF();
        }       
    }
}
