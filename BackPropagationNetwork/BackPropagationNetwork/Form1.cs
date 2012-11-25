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
    public partial class BPNFrom : Form
    {
        ToolTip ToolTip = new ToolTip();
        public BackPropagationTrainer BPN = new BackPropagationTrainer();
        NeuralNetwork NN = new NeuralNetwork();
        
        public BPNFrom()
        {
            InitializeComponent();
            Initialize();
            SetUI();
        }

        void Initialize()
        {
            BPN.NeuralNetwork = NN;
            BPN.LearningRate = (double)numLearningRate.Value;
            BPN.Momentum = (double)numMomentum.Value;
            BPN.ErrorThreshold = (double)numErrorThreshold.Value;
            BPN.MaxIteration = (int)numMaxIteration.Value;
            Debugger.Write = WriteOnConsole;
            NN.InputLayer.NumberOfNeurons = 1;
            NN.InputLayer.NumberOfNeuronsChanged = NumberOfNeuronsChanged;
            NN.OutputLayer.NumberOfNeuronsChanged = NumberOfNeuronsChanged;
            NN.OutputLayer.NumberOfNeurons = 1;
        }

        void SetUI()
        {            
            panelNetwork.Location = new Point((panelNetworkBack.Width - panelNetwork.Width) / 2, panelNetwork.Top);
            comboLearningMode.SelectedIndex = 0;
            CreateNetwork();
            ToolTip.SetToolTip(checkDebug1, "Debug level 1");
            ToolTip.SetToolTip(checkDebug2, "Debug level 2");
            ToolTip.SetToolTip(checkDebug3, "Debug level 3");
            ToolTip.SetToolTip(checkDebug4, "Debug level 4");
            ToolTip.SetToolTip(checkDebug5, "Debug level 5");
            checkDebug1.Checked = true;
        }
     

        #region Event Handlers 
        void NumberOfNeuronsChanged(Layer layer)
        {
            dgvTrainingSets.Rows.Clear();
            dgvTrainingSets.Columns.Clear();
            dgvValidationSets.Rows.Clear();
            dgvValidationSets.Columns.Clear();

            int c = 0;
            for (int i = 1; i <= NN.InputLayer.NumberOfNeurons; i++)
            {
                dgvTrainingSets.Columns.Add("col" + c++, "Input " + i);
            }
            for (int i = 1; i <= NN.OutputLayer.NumberOfNeurons; i++)
            {
                dgvTrainingSets.Columns.Add("col" + c++, "Output " + i);
            }
            c = 0;
            for (int i = 1; i <= NN.InputLayer.NumberOfNeurons; i++)
            {
                dgvValidationSets.Columns.Add("col" + c++, "Input " + i);
            }
            for (int i = 1; i <= NN.OutputLayer.NumberOfNeurons; i++)
            {
                dgvValidationSets.Columns.Add("col" + c++, "Output " + i);
            }
        }

        void CreateNetwork()
        {
            int hidden_layers = (int)numHiddenLayers.Value;
            if (hidden_layers > NN.HiddenLayers.Count)
            {
                for (int i = NN.HiddenLayers.Count; i < hidden_layers; i++)
                {
                    Layer hl = new Layer(NN, LayerType.Hidden);
                    NN.HiddenLayers.Add(hl);
                }
            }
            else if (hidden_layers < NN.HiddenLayers.Count)
            {
                NN.HiddenLayers.RemoveRange(hidden_layers, NN.HiddenLayers.Count - hidden_layers);

            }
            if (hidden_layers > 0)
            {
                NN.InputLayer.NextLayer = NN.HiddenLayers[0];
                for (int i = 0; i < BPN.NeuralNetwork.HiddenLayers.Count - 1; i++)
                {
                    NN.HiddenLayers[i].NextLayer = NN.HiddenLayers[i + 1];
                }
                NN.HiddenLayers.Last().NextLayer = NN.OutputLayer;
            }
            else
            {
                NN.InputLayer.NextLayer = NN.OutputLayer;
            }
            DrawNetwork();
            NN.CreateSynapses();
        }

        void DrawNetwork()
        {
            int hidden_layers = NN.HiddenLayers.Count;
            panelNetwork.Size = new Size((hidden_layers + 4) * 100 + (hidden_layers + 1) * 50, 100);

            Point P = new Point(0, 0);
            panelNetwork.Controls.Clear();
            PictureBox picInput = new PictureBox();
            picInput.Size = new System.Drawing.Size(100, 100);
            picInput.Image = global::BackPropagationNetwork.Properties.Resources.Input;
            panelNetwork.Controls.Add(picInput);
            picInput.Location = P;
            P.X += 100;
            Layer current = NN.InputLayer;
            LayerControl lcInput = new LayerControl();
            lcInput.Layer = NN.InputLayer;
            panelNetwork.Controls.Add(lcInput);
            lcInput.Location = P;
            P.X += 100;
            SynapsesControl sc1 = new SynapsesControl(NN, current, current.NextLayer);
            panelNetwork.Controls.Add(sc1);
            sc1.Location = P;
            P.X += 50;
            current = current.NextLayer;
            for (int i = 0; i < hidden_layers; i++)
            {
                LayerControl lc = new LayerControl();
                lc.Layer = NN.HiddenLayers[i];
                panelNetwork.Controls.Add(lc);
                lc.Location = P;
                P.X += 100;

                SynapsesControl sc = new SynapsesControl(NN, current, current.NextLayer);
                panelNetwork.Controls.Add(sc);
                sc.Location = P;
                P.X += 50;
                current = current.NextLayer;
            }

            LayerControl lcOutput = new LayerControl();
            lcOutput.Layer = NN.OutputLayer;
            panelNetwork.Controls.Add(lcOutput);
            lcOutput.Location = P;
            P.X += 100;

            PictureBox picOutput = new PictureBox();
            picOutput.Size = new Size(100, 100);
            picOutput.Image = global::BackPropagationNetwork.Properties.Resources.Output;
            panelNetwork.Controls.Add(picOutput);
            picOutput.Location = P;
        }

        void WriteOnConsole(string s)
        {
            txtConsole.AppendText(Environment.NewLine + s);
        }
        
        private void BPNFrom_Load(object sender, EventArgs e)
        {
        }        

        private void btnLoadTS_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title="Select training sets file";
            OFD.Multiselect = false;            
            OFD.DefaultExt=".ioset";
            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<IOSet> TS = LearningAlgorithm.LoadIOSetFromFile(OFD.FileName);
                if (TS.Count > 0)
                {
                    if (TS[0].InputSet.Count != NN.InputLayer.NumberOfNeurons || TS[0].OutputSet.Count != NN.OutputLayer.NumberOfNeurons)
                    {
                        MessageBox.Show("The training sets does not match neural network parameters");
                    }
                    else
                    {
                        try
                        {
                            txtTSFile.Text = OFD.FileName;
                            dgvTrainingSets.Rows.Clear();
                            dgvTrainingSets.Columns.Clear();
                            int c = 0;
                            for (int i = 1; i <= TS[0].InputSet.Count; i++)
                            {
                                dgvTrainingSets.Columns.Add("col" + c++, "Input " + i);
                            }
                            for (int i = 1; i <= TS[0].OutputSet.Count; i++)
                            {
                                dgvTrainingSets.Columns.Add("col" + c++, "Output " + i);
                            }
                            for (int i = 0; i < TS.Count; i++)
                            {
                                dgvTrainingSets.Rows.Add();
                                c = 0;
                                for (int j = 0; j < TS[i].InputSet.Count; j++)
                                {
                                    dgvTrainingSets[c++, i].Value = TS[i].InputSet[j];
                                }
                                for (int j = 0; j < TS[i].OutputSet.Count; j++)
                                {
                                    dgvTrainingSets[c++, i].Value = TS[i].OutputSet[j];
                                }
                            }
                            BPN.TrainingSets = TS;
                        }
                        catch
                        {                           
                            MessageBox.Show("An error occured while loading the training set file. May be the file is corrupted");
                            txtTSFile.Clear();
                            dgvTrainingSets.Rows.Clear();
                            dgvTrainingSets.Columns.Clear();
                        }
                    }
                }
            }
        }

        private void btnClearTS_Click(object sender, EventArgs e)
        {
            ClearTS();     
        }
       
        void ClearTS()
        {
            txtTSFile.Clear();
            dgvTrainingSets.Rows.Clear();
            dgvTrainingSets.Columns.Clear();

            int c = 0;
            for (int i = 1; i <= NN.InputLayer.NumberOfNeurons; i++)
            {
                dgvTrainingSets.Columns.Add("col" + c++, "Input " + i);
            }
            for (int i = 1; i <= NN.OutputLayer.NumberOfNeurons; i++)
            {
                dgvTrainingSets.Columns.Add("col" + c++, "Output " + i);
            }     
        }

        private void btnLoadVS_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Select validation sets file";
            OFD.Multiselect = false;
            OFD.DefaultExt = ".ioset";
            if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<IOSet> VS = LearningAlgorithm.LoadIOSetFromFile(OFD.FileName);
                if (VS.Count > 0)
                {
                    if (VS[0].InputSet.Count != NN.InputLayer.NumberOfNeurons || VS[0].OutputSet.Count != NN.OutputLayer.NumberOfNeurons)
                    {
                        MessageBox.Show("The validation sets does not match neural network parameters");
                    }
                    else
                    {
                        try
                        {
                            txtVSFile.Text = OFD.FileName;
                            dgvValidationSets.Rows.Clear();
                            dgvValidationSets.Columns.Clear();
                            int c = 0;
                            for (int i = 1; i <= VS[0].InputSet.Count; i++)
                            {
                                dgvValidationSets.Columns.Add("col" + c++, "Input " + i);
                            }
                            for (int i = 1; i <= VS[0].OutputSet.Count; i++)
                            {
                                dgvValidationSets.Columns.Add("col" + c++, "Output " + i);
                            }
                            for (int i = 0; i < VS.Count; i++)
                            {
                                dgvValidationSets.Rows.Add();
                                c = 0;
                                for (int j = 0; j < VS[i].InputSet.Count; j++)
                                {
                                    dgvValidationSets[c++, i].Value = VS[i].InputSet[j];
                                }
                                for (int j = 0; j < VS[i].OutputSet.Count; j++)
                                {
                                    dgvValidationSets[c++, i].Value = VS[i].OutputSet[j];
                                }
                            }
                            BPN.ValidationSets = VS;
                        }
                        catch
                        {
                            MessageBox.Show("An error occured while loading the validation set file. May be the file is corrupted");
                            txtVSFile.Clear();
                            dgvValidationSets.Rows.Clear();
                            dgvValidationSets.Columns.Clear();
                        }
                    }
                }
            }
        }

        private void btnClearVS_Click(object sender, EventArgs e)
        {
            ClearVS();
        }

        void ClearVS()
        {
            txtVSFile.Clear();
            dgvValidationSets.Rows.Clear();
            dgvValidationSets.Columns.Clear();

            int c = 0;
            for (int i = 1; i <= NN.InputLayer.NumberOfNeurons; i++)
            {
                dgvValidationSets.Columns.Add("col" + c++, "Input " + i);
            }
            for (int i = 1; i <= NN.OutputLayer.NumberOfNeurons; i++)
            {
                dgvValidationSets.Columns.Add("col" + c++, "Output " + i);
            }
        }

        private void comboLearningMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLearningMode.SelectedIndex == 0)
            {
                BPN.NetworkLearningMode = LearningAlgorithm.LearningMode.Offline;
            }
            else
            {
                BPN.NetworkLearningMode = LearningAlgorithm.LearningMode.Online;
            }
        }
        

        private void numLearningRate_ValueChanged(object sender, EventArgs e)
        {
            BPN.LearningRate = (double)numLearningRate.Value;
        }

        private void numMomentum_ValueChanged(object sender, EventArgs e)
        {
            BPN.Momentum = (double)numMomentum.Value;
        }

        private void numErrorThreshold_ValueChanged(object sender, EventArgs e)
        {
            BPN.ErrorThreshold = (double)numErrorThreshold.Value;
        }

        private void numMaxIteration_ValueChanged(object sender, EventArgs e)
        {
            BPN.MaxIteration = (int)numMaxIteration.Value;
        }

        private void btnStartLearning_Click(object sender, EventArgs e)
        {
            BPN.NeuralNetwork.Mode = NetworkMode.Training;
            txtConsole.Clear();
            //Load the training sets and validation sets
            if (dgvTrainingSets.Columns.Count != (NN.InputLayer.NumberOfNeurons + NN.OutputLayer.NumberOfNeurons))
            {
                MessageBox.Show("The training set provided does not match network inputs and output size.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }           
            BPN.TrainingSets.Clear();            
            try
            {
                for (int i = 0; i < dgvTrainingSets.Rows.Count; i++)
                {
                    try
                    {
                        int c = 0;
                        Set iset = new Set();
                        for (int j = 0; j < NN.InputLayer.NumberOfNeurons; j++)
                        {
                            iset.Add(double.Parse(dgvTrainingSets[c++, i].Value.ToString()));
                        }
                        Set oset = new Set();
                        for (int j = 0; j < NN.OutputLayer.NumberOfNeurons; j++)
                        {
                            oset.Add(double.Parse(dgvTrainingSets[c++, i].Value.ToString()));
                        }
                        BPN.TrainingSets.Add(new IOSet(iset, oset));
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            if (BPN.TrainingSets.Count == 0)
            {
                MessageBox.Show("No training set provided", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }            

            BPN.TrainingSets.Normalize();            
            WriteOnConsole("Learning started.");
            BPN.StartLearning();
            WriteOnConsole("Learning Ended.");
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            BPN.NeuralNetwork.Mode = NetworkMode.Training;
            txtConsole.Clear();
            if (dgvValidationSets.Columns.Count != (NN.InputLayer.NumberOfNeurons + NN.OutputLayer.NumberOfNeurons))
            {
                MessageBox.Show("The validation set provided does not match network inputs and output size.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            BPN.ValidationSets.Clear();
            try
            {
                for (int i = 0; i < dgvValidationSets.Rows.Count; i++)
                {
                    try
                    {
                        int c = 0;
                        Set iset = new Set();
                        for (int j = 0; j < NN.InputLayer.NumberOfNeurons; j++)
                        {
                            iset.Add(double.Parse(dgvValidationSets[c++, i].Value.ToString()));
                        }
                        Set oset = new Set();
                        for (int j = 0; j < NN.OutputLayer.NumberOfNeurons; j++)
                        {
                            oset.Add(double.Parse(dgvValidationSets[c++, i].Value.ToString()));
                        }
                        BPN.ValidationSets.Add(new IOSet(iset, oset));
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            if (BPN.ValidationSets.Count == 0)
            {
                MessageBox.Show("No validation set provided", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            BPN.ValidationSets.Normalize();
            WriteOnConsole("Validating: " + BPN.ValidationSets.Count + " validation sets given.");
            LearningAlgorithm.ValidationResult vr = BPN.Validate();
            WriteOnConsole("Validation ended");
            WriteOnConsole(vr.Passed + " sets passed validation out of " + vr.ValidationSet);            
        }
        

        private void panelNetwork_SizeChanged(object sender, EventArgs e)
        {
            panelNetwork.Location = new Point((panelNetworkBack.Width - panelNetwork.Width) / 2, panelNetwork.Top);
        }

        private void panelNetworkBack_Resize(object sender, EventArgs e)
        {
            panelNetwork.Location = new Point((panelNetworkBack.Width - panelNetwork.Width) / 2, panelNetwork.Top);
        }
        #endregion

        private void numHiddenLayers_ValueChanged(object sender, EventArgs e)
        {
            CreateNetwork();
        }

        private void checkDebug1_CheckedChanged(object sender, EventArgs e)
        {
            Debugger.Debug1 = checkDebug1.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Debugger.Debug2 = checkDebug2.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Debugger.Debug3 = checkDebug3.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Debugger.Debug4 = checkDebug4.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Debugger.Debug5 = checkDebug5.Checked;
        }

        private void btnSaveTS_Click(object sender, EventArgs e)
        {
            try
            {
                List<IOSet> tsets = new List<IOSet>();
                for (int i = 0; i < dgvTrainingSets.Rows.Count; i++)
                {
                    try
                    {
                        int c = 0;
                        Set iset = new Set();
                        for (int j = 0; j < NN.InputLayer.NumberOfNeurons; j++)
                        {
                            iset.Add(double.Parse(dgvTrainingSets[c++, i].Value.ToString()));
                        }
                        Set oset = new Set();
                        for (int j = 0; j < NN.OutputLayer.NumberOfNeurons; j++)
                        {
                            oset.Add(double.Parse(dgvTrainingSets[c++, i].Value.ToString()));
                        }
                        tsets.Add(new IOSet(iset, oset));
                    }
                    catch
                    {
                    }
                }
                if (tsets.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save training set";                    
                    sfd.DefaultExt = ".ioset";
                    sfd.AddExtension = true;
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string filename = sfd.FileName;
                        if (LearningAlgorithm.SaveIOSetToFile(filename, tsets))
                        {
                            MessageBox.Show("Training sets successfully saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occured, training sets cannot be saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveVS_Click(object sender, EventArgs e)
        {
            try
            {
                List<IOSet> vsets = new List<IOSet>();
                for (int i = 0; i < dgvValidationSets.Rows.Count; i++)
                {
                    try
                    {
                        int c = 0;
                        Set iset = new Set();
                        for (int j = 0; j < NN.InputLayer.NumberOfNeurons; j++)
                        {
                            iset.Add(double.Parse(dgvValidationSets[c++, i].Value.ToString()));
                        }
                        Set oset = new Set();
                        for (int j = 0; j < NN.OutputLayer.NumberOfNeurons; j++)
                        {
                            oset.Add(double.Parse(dgvValidationSets[c++, i].Value.ToString()));
                        }
                        vsets.Add(new IOSet(iset, oset));
                    }
                    catch
                    {
                    }
                }
                if (vsets.Count > 0)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save validation set";
                    sfd.DefaultExt = ".ioset";
                    sfd.AddExtension = true;
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string filename = sfd.FileName;
                        if (LearningAlgorithm.SaveIOSetToFile(filename, vsets))
                        {
                            MessageBox.Show("Validation sets successfully saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error occured, validation sets cannot be saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NN.RandomizeNeuronThresholds();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NN.RandomizeSynapsesWeights();
        }

        private void btnResetNN_Click(object sender, EventArgs e)
        {
            NN.HiddenLayers.Clear();
            NN.InputLayer.NumberOfNeurons = 1;
            NN.OutputLayer.NumberOfNeurons = 1;
            NN.InputLayer.Neurons[0].Threshold = 0;
            NN.OutputLayer.Neurons[0].Threshold = 0;
            NN.InputLayer.NextLayer = NN.OutputLayer;
            NN.CreateSynapses();
            DrawNetwork();
            ClearVS();
            ClearTS();
        }

        private void btnLoadNN_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".ann";
            ofd.Filter = "Artificial neural network files | *.ann";
            ofd.Title = "Select neural network file";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    NeuralNetwork nn = NeuralNetwork.FromFile(ofd.FileName);
                    if (nn != null)
                    {
                        NN = nn;                             
                        BPN.NeuralNetwork = NN;
                        numHiddenLayers.ValueChanged -= numHiddenLayers_ValueChanged;
                        numHiddenLayers.Value = NN.HiddenLayers.Count;
                        numHiddenLayers.ValueChanged += numHiddenLayers_ValueChanged;
                        DrawNetwork();
                        ClearVS();
                        ClearTS();                          
                        MessageBox.Show("Neural netwrok successfully loaded from file: " + ofd.FileName, "Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("An error occured while loading neural netwrok from file: " + ofd.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("An error occured while loading neural netwrok from file: " + ofd.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnSaveNN_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save neural network";
            sfd.DefaultExt = ".ann";
            sfd.Filter = "Artificial neural network files | *.ann";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (NN.SaveToFile(sfd.FileName))
                    {
                        MessageBox.Show("Neural netwrok successfully saved to file: " + sfd.FileName, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("An error occured while saving neural netwrok to file: " + sfd.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("An error occured while saving neural netwrok to file: " + sfd.FileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }       
    }   
}
