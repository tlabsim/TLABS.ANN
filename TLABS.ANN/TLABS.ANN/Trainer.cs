using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TLABS.ANN
{
    public class LearningEventArgs : EventArgs
    {
        public DateTime Time;
        public NeuralNetwork Network;
        public LearningEventArgs(NeuralNetwork network, DateTime time)
        {            
            Network = network;
            Time = time;
        }
    }
    public delegate void LearningStartedDelegate(object sender, LearningEventArgs le);
    public delegate void LearningAbortedDelegate(object sender, LearningEventArgs le);
    public delegate void LearningEndedDelegate(object sender, LearningEventArgs le);
    /// <summary>
    /// Represents a trainer of neural network. This abstract class can be used to model various neural network trainer.
    /// </summary>
    public abstract class LearningAlgorithm
    {
        /// <summary>
        /// The cause of stopping of learning
        /// </summary>
        public enum StopCause
        {
            MaxIterationReached,
            DesiredErrorRateReached,
            InvalidOperation,
            UnknownCause
        }

        /// <summary>
        /// Mode of the learning
        /// </summary>
        public enum LearningMode
        {
            /// <summary>
            /// Offline or batch training mode
            /// </summary>
            Offline,
            /// <summary>
            /// Online training mode
            /// </summary>
            Online
        }

        /// <summary>
        /// Represents the result of validation test
        /// </summary>
        public struct ValidationResult
        {
            /// <summary>
            /// Gets or sets the number of sets in validation sets
            /// </summary>
            public int ValidationSet;
            /// <summary>
            /// Gets ot sets the number of sets passed in validation test
            /// </summary>
            public int Passed;
            /// <summary>
            /// Gets the percentage of the number of sets passed in validation test
            /// </summary>
            public int PassRate
            {
                get
                {
                    return (int)((double)Passed / (double)ValidationSet * 100);
                }
            }
        }

        public LearningStartedDelegate LearningStarted;
        public LearningAbortedDelegate LearningAborted;
        public LearningEndedDelegate LearningEnded;
        protected NeuralNetwork _NeuralNetwork;
        /// <summary>
        /// Gets or sets the neural network with which the trainer is associated
        /// </summary>
        public NeuralNetwork NeuralNetwork
        {
            get
            {
                return this._NeuralNetwork;
            }
            set
            {
                this._NeuralNetwork = value;
                this._NeuralNetwork.Mode = NetworkMode.Training;
            }
        }        
        protected double _LearningRate = 0.9;
        protected double _Momentum = 0.0;
        protected double _ErrorRate = 0.0;
        protected double _ErrorThreshold = 0.0;
        protected int _Iteration = 0;
        protected int _MaxIteration = 1000;

        /// <summary>
        /// Gets or sets the learning mode of the network
        /// </summary>
        public LearningMode NetworkLearningMode = LearningMode.Online;        

        public double LearningRate
        {
            get
            {
                return this._LearningRate;
            }
            set
            {
                this._LearningRate = value > 0 ? value : 0.01;
            }
        }

        public double Momentum
        {
            get
            {
                return this._Momentum;
            }
            set
            {
                this._Momentum = value >= 0 ? value : 0;
            }
        }

        public double ErrorRate
        {
            get
            {
                return this._ErrorRate;
            }
            set
            {
                this._ErrorRate = value;
            }
        }

        public double ErrorThreshold
        {
            get
            {
                return this._ErrorThreshold;
            }
            set
            {
                this._ErrorThreshold = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the neurons threshold value is fixed or adjustable
        /// </summary>
        public bool FixedNeuronThreshold = false;

        public int Iteration
        {
            get
            {
                return this._Iteration;
            }
        }

        public int MaxIteration
        {
            get
            {
                return this._MaxIteration;
            }
            set
            {
                this._MaxIteration = value > 0 ? value : 1;
            }
        }

        public StopCause LearningStopCause = StopCause.UnknownCause;

        /// <summary>
        /// Gets or sets the training sets by which the netwrok will be trained
        /// </summary>
        public List<IOSet> TrainingSets = new List<IOSet>();
        /// <summary>
        /// Gets ot sets the validation sets to determine whether the network is properly trained or not.
        /// </summary>
        public List<IOSet> ValidationSets = new List<IOSet>();

        public LearningAlgorithm()
        {
        }
        public LearningAlgorithm(NeuralNetwork neural_network)
        {
            this.NeuralNetwork = neural_network;            
        }
        /// <summary>
        /// Normalize the training sets
        /// </summary>
        public void NormalizeTrainigSets()
        {
            double max = 0.0;
            for (int i = 0; i < this.TrainingSets.Count; i++)
            {
                for (int j = 0; j < this.TrainingSets[i].InputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(this.TrainingSets[i].InputSet[j]), max);
                }

                for (int j = 0; j < this.TrainingSets[i].OutputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(this.TrainingSets[i].OutputSet[j]), max);
                }
            }
            if (max > 1)
            {
                double scaling_factor = 1.0 / max;
                for (int i = 0; i < this.TrainingSets.Count; i++)
                {
                    this.TrainingSets[i].InputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                    this.TrainingSets[i].OutputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                }
            }
        }

        public void NormalizeValidationSets()
        {
            double max = 0.0;
            for (int i = 0; i < this.ValidationSets.Count; i++)
            {
                for (int j = 0; j < this.ValidationSets[i].InputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(this.ValidationSets[i].InputSet[j]), max);
                }

                for (int j = 0; j < this.ValidationSets[i].OutputSet.Count; j++)
                {
                    max = Math.Max(Math.Abs(this.ValidationSets[i].OutputSet[j]), max);
                }
            }
            if (max > 1)
            {
                double scaling_factor = 1.0 / max;
                for (int i = 0; i < this.ValidationSets.Count; i++)
                {
                    this.ValidationSets[i].InputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                    this.ValidationSets[i].OutputSet.ForEach(delegate(double d) { d *= scaling_factor; });
                }
            }
        }

        /// <summary>
        /// Sets the current input set for learning
        /// </summary>
        /// <param name="input_set"></param>
        public void SetInputs(Set input_set)
        {
            if (input_set.Count != this.NeuralNetwork.InputLayer.NumberOfNeurons)
            {
                throw new Exception("Size of input set must be equal to number of neurons in the input layer of the network");
            }
            for (int i = 0; i < this.NeuralNetwork.InputLayer.NumberOfNeurons; i++)
            {
                this.NeuralNetwork.InputLayer.Neurons[i].Input = input_set[i];
            }
        }

        /// <summary>
        /// Sets the current target set for learning
        /// </summary>
        /// <param name="target_set"></param>
        public void SetTargets(Set target_set) 
        {
            if (target_set.Count != this.NeuralNetwork.OutputLayer.NumberOfNeurons)
            {
                throw new Exception("Size of target set must be equal to number of neurons in the output layer of the network");
            }
            for (int i = 0; i < this.NeuralNetwork.OutputLayer.NumberOfNeurons; i++)
            {
                this.NeuralNetwork.OutputLayer.Neurons[i].Target = target_set[i];
            }
        }
        
        /// <summary>
        /// Here the training workflow will be defined
        /// </summary>
        public virtual void Learn()
        {
        }

        /// <summary>
        /// Start the learning
        /// </summary>
        public void StartLearning()
        {
            if (this.LearningStarted != null)
            {
                LearningEventArgs le = new LearningEventArgs(this.NeuralNetwork, DateTime.Now);
                this.LearningStarted(this, le);
            }
            this.Learn();
            if (this.LearningStopCause == StopCause.InvalidOperation || this.LearningStopCause== StopCause.UnknownCause)
            {
                if (this.LearningAborted != null)
                {
                    LearningEventArgs le = new LearningEventArgs(this.NeuralNetwork, DateTime.Now);
                    this.LearningAborted(this, le);
                }
            }
            else if (this.LearningStopCause == StopCause.MaxIterationReached || this.LearningStopCause == StopCause.MaxIterationReached)
            {
                if (this.LearningEnded != null)
                {
                    LearningEventArgs le = new LearningEventArgs(this.NeuralNetwork, DateTime.Now);
                    this.LearningEnded(this, le);
                }
            }
        }    

         /// <summary>
        /// Runs a validation test to see if the learning is properly done
        /// </summary>
        public virtual ValidationResult Validate()
        {
            ValidationResult vr = new ValidationResult();
            return vr;
        }

        public static List<IOSet> LoadIOSetFromFile(string filename)
        {
            List<IOSet> iosets = new List<IOSet>();
            try
            {
                if (filename.IndexOf(".ioset") == (filename.Length - 6))               
                {
                    using (StreamReader file = new StreamReader(filename, Encoding.ASCII))
                    {
                        if (file != null)
                        {                            
                            string s = string.Empty;
                            while(!file.EndOfStream)
                            {
                                s = file.ReadLine();                                
                                s = s.Trim();
                                if (!s.IndexOf("!").Equals(0) && !s.Equals(string.Empty))
                                {                                    
                                    if (s.StartsWith("[") && s.EndsWith("]"))
                                    {                                       
                                        //Well this seems to be a valid ioset                                        
                                        s = s.Replace("[", "");
                                        s = s.Replace("]", "");
                                        s = s.Trim();
                                        if (s.StartsWith("<") && s.EndsWith(">"))
                                        {                                            
                                            int si = s.IndexOf("<");
                                            int ei = s.IndexOf(">", si);
                                            int so = s.IndexOf("<", ei + 1);
                                            int eo = s.IndexOf(">", so);
                                            if (-1 < si && si < ei && ei < so && so < eo)
                                            {                                            
                                                string[] i_set = s.Substring(si +1, ei - si -1).Split(',');
                                                string[] o_set = s.Substring(so +1, eo - so -1).Split(',');
                                                if (i_set.Length > 0 && o_set.Length > 0)
                                                {                                                    
                                                    try
                                                    {
                                                        // Infer inputs
                                                        Set _i = new Set();
                                                        for (int i = 0; i < i_set.Length; i++)
                                                        {
                                                            _i.Add(double.Parse(i_set[i].Trim()));
                                                        }
                                                        Set _o = new Set();
                                                        for (int i = 0; i < o_set.Length; i++)
                                                        {
                                                            _o.Add(double.Parse(o_set[i].Trim()));
                                                        }
                                                        iosets.Add(new IOSet(_i, _o));
                                                    }
                                                    catch
                                                    {
                                                    }

                                                }
                                            }                                            
                                        }    
                                    }

                                }
                            }
                            file.Close();                                                   
                        }
                    }
                }
            }
            catch
            {                
            }
            return iosets;
        }

        public static bool SaveIOSetToFile(string filename, List<IOSet> iosets)
        {
            bool Success = true;
            try
            {
                if (filename.ToLower().IndexOf(".ioset") != filename.Length - 6)
                {
                    filename += ".ioset";
                }
                using (StreamWriter file = new StreamWriter(filename, false, Encoding.ASCII))
                {
                    if (file != null)
                    {
                        //Write some info
                        file.WriteLine("! IO Sets for training and validation of neural networks");
                        file.WriteLine("! File format defined by Iftekhar Mahmud Towhid");
                        file.WriteLine("! TLABS @ 2012, email: im_tlabs@yahoo.com");
                        file.WriteLine("! Date created: " + DateTime.Now.ToString("F"));
                        file.WriteLine("");
                        string s = string.Empty;
                        for (int i = 0; i < iosets.Count; i++)
                        {
                            s = "[<";
                            for (int j = 0; j < iosets[i].InputSet.Count; j++)
                            {
                                s += iosets[i].InputSet[j].ToString().PadLeft(8);
                                if (j < iosets[i].InputSet.Count - 1)
                                {
                                    s += ",";
                                }
                            }
                            s += "><";
                            for (int j = 0; j < iosets[i].OutputSet.Count; j++)
                            {
                                s += iosets[i].OutputSet[j].ToString().PadLeft(8);
                                if (j < iosets[i].OutputSet.Count - 1)
                                {
                                    s += ",";
                                }
                            }
                            s += ">]";
                            file.WriteLine(s);
                        }
                        file.Close();
                    }
                }
            }
            catch
            {
                Success = false;
            }
            return Success;
        }
    }   
}
