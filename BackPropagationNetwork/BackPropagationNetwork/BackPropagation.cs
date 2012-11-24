using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLABS.ANN;

namespace TLABS.BPN
{
    public class BackPropagationNetwork : LearningAlgorithm
    {
        #region Constructors
        public BackPropagationNetwork()
        {
        }

        public BackPropagationNetwork(NeuralNetwork network)
        {
            this.NeuralNetwork = network;
        } 
        #endregion
        #region Methods
        /// <summary>
        /// The learning method
        /// </summary>
        public override void Learn()
        {            
            if (this.NetworkLearningMode == LearningMode.Online)
            {
                OnlineLearning();
            }
            else if (this.NetworkLearningMode == LearningMode.Offline)
            {
                BatchLearning();
            }
        }
        /// <summary>
        /// Batch or offline learning method
        /// </summary>
        void BatchLearning()
        {
            double Error = 0.0;
            this._Iteration = 0;
            this.LearningStopCause = StopCause.UnknownCause;
            do
            {
                try
                {
                    Error = 0.0;
                    for (int i = 0; i < this.TrainingSets.Count; i++)
                    {
                        this.SetInputs(this.TrainingSets[i].InputSet);
                        this.SetTargets(this.TrainingSets[i].OutputSet);
                        this.Propagate();
                        Error += this.OutputError();
                        this.BackPropagate();
                        this.CalculateParameterChanges();
                    }
                    this.AdjustWeightsOffline();
                    if (!this.FixedNeuronThreshold)
                    {
                        this.AdjustThresholdOffline();
                    }
                    this._Iteration++;
                    Error = Error / this.TrainingSets.Count;
                    if (Debugger.Debug1) Debugger.WriteLine("Iteration {0}=> Error: {1}", this._Iteration, Error);
                    if (this._Iteration >= MaxIteration && (Error > this.ErrorThreshold / 10.0))
                    {
                        this.LearningStopCause = StopCause.MaxIterationReached;
                        break;
                    }
                    if (Error <= ErrorThreshold)
                    {
                        this.LearningStopCause = StopCause.DesiredErrorRateReached;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    if (Debugger.Debug1) Debugger.WriteLine(ex.Message + "\r\n");
                    this.LearningStopCause = StopCause.InvalidOperation;
                    break;
                }
            }
            while (true);
        }
        /// <summary>
        /// Online learning method
        /// </summary>
        void OnlineLearning()
        {            
            double Error = 0.0;           
            this._Iteration = 0;
            this.LearningStopCause = StopCause.UnknownCause;
            do
            {
                try
                {
                    Error = 0.0;
                    for (int i = 0; i < this.TrainingSets.Count; i++)
                    {
                        //Console.WriteLine("Called me!");
                        this.SetInputs(this.TrainingSets[i].InputSet);
                        this.SetTargets(this.TrainingSets[i].OutputSet);
                        this.Propagate();                        
                        Error += this.OutputError();
                        this.BackPropagate();                        
                        this.CalculateParameterChanges();
                        this.AdjustWeightsOnline();
                        if (!this.FixedNeuronThreshold)
                        {
                            this.AdjustThresholdOnline();
                        }
                    }
                    this._Iteration++;    
                    Error = Error / this.TrainingSets.Count;                 
                    if(Debugger.Debug1) Debugger.WriteLine("Iteration {0}=> Error: {1}", this._Iteration, Error);                    
                    if (this._Iteration >= MaxIteration && (Error > this.ErrorThreshold/10.0))
                    {
                        this.LearningStopCause = StopCause.MaxIterationReached;
                        break;
                    }
                    if (Error <= ErrorThreshold)
                    {
                        this.LearningStopCause = StopCause.DesiredErrorRateReached;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    if (Debugger.Debug1) Debugger.WriteLine(ex.Message + "\r\n");
                    this.LearningStopCause = StopCause.InvalidOperation;
                    break;
                }
            }
            while (true);
        }
        /// <summary>
        /// Propagate from input layer to output layer
        /// </summary>
        void Propagate()
        {
            this.NeuralNetwork.Propagate();
        }
        /// <summary>
        /// Backpropagate from output layer to input layer
        /// </summary>
        void BackPropagate()
        {
            Layer current = this.NeuralNetwork.OutputLayer;
            while (current != null)
            {
                current.EvaluateError();
                current = current.PreviousLayer;
            }
        }
        /// <summary>
        /// Calculates the total output error for the given training set
        /// </summary>
        /// <returns></returns>
        double OutputError()
        {
            double error = 0.0;
            for (int i = 0; i < this.NeuralNetwork.OutputLayer.NumberOfNeurons; i++)
            {
                Neuron n = this.NeuralNetwork.OutputLayer.Neurons[i];
                error += 0.5 * (n.Target - n.Output) * (n.Target - n.Output);
            }
            //Console.WriteLine("Error for the current training set: {0}", error);
            return error;
        }
        /// <summary>
        /// Adjusts the weights of the synapses of the network in offline mode
        /// </summary>
        void AdjustWeightsOffline()
        {
            foreach (Synapsis synapsis in this.NeuralNetwork.Synapses)
            {
                synapsis.Weight += synapsis.SumDelta;
                synapsis.SumDelta = 0.0;
            }
        }
        /// <summary>
        /// Adjusts the threshold of the neurons in offline mode
        /// </summary>
        void AdjustThresholdOffline()
        {
            Layer current = this.NeuralNetwork.InputLayer.NextLayer;
            while (current != null)
            {
                foreach (Neuron neuron in current.Neurons)
                {
                    neuron.Threshold += neuron.SumDeltaThreshold;
                    neuron.SumDeltaThreshold = 0.0;
                }
                current = current.NextLayer;
            }
        }
        /// <summary>
        /// Adjust the weights of the synapses of the network in online mode
        /// </summary>
        void AdjustWeightsOnline()
        {
            foreach (Synapsis synapsis in this.NeuralNetwork.Synapses)
            {
                synapsis.Weight += synapsis.Delta;
            }
        }
        /// <summary>
        /// Adjust the thresholds of the neurons in online mode
        /// </summary>
        void AdjustThresholdOnline()
        {       
            Layer current = this.NeuralNetwork.InputLayer.NextLayer;
            while (current != null)
            {
                foreach (Neuron neuron in current.Neurons)
                {
                    neuron.Threshold += neuron.DeltaThreshold;

                }
                current = current.NextLayer;
            }
        }
        /// <summary>
        /// Calculate the changes in weight and threshold
        /// </summary>
        void CalculateParameterChanges()
        {
            foreach (Synapsis synapsis in this.NeuralNetwork.Synapses)
            {
                synapsis.Delta = this.LearningRate * synapsis.From.Output * synapsis.To.Error + this.Momentum * synapsis.Delta;
                //Console.WriteLine("Weight change between neuron {0} in {1} layer and neuron {2} in {3} layer is {4}", synapsis.From.Index, synapsis.From.Layer.LayerType, synapsis.To.Index, synapsis.To.Layer.LayerType, synapsis.Delta);
                synapsis.SumDelta += synapsis.Delta;
            }

            Layer current = this.NeuralNetwork.InputLayer.NextLayer;
            while (current != null)
            {
                foreach (Neuron neuron in current.Neurons)
                {
                    neuron.DeltaThreshold = this.LearningRate * neuron.Error + this.Momentum * neuron.DeltaThreshold;
                    neuron.SumDeltaThreshold += neuron.DeltaThreshold;
                }
                current = current.NextLayer;
            }
        }
        public override ValidationResult Validate()
        {
            ValidationResult vr = new ValidationResult();
            vr.ValidationSet = this.ValidationSets.Count;
            vr.Passed=0;
            for (int i = 0; i < this.ValidationSets.Count; i++)
            {
                this.SetInputs(this.ValidationSets[i].InputSet);
                this.SetTargets(this.ValidationSets[i].OutputSet);
                this.Propagate();
                double error = this.OutputError();
                if(Debugger.Debug2) Debugger.WriteLine("validation set {0}, Error: {1}", i, error);
                if (error < this.ErrorThreshold)
                {
                    if (Debugger.Debug2) Debugger.WriteLine("Passed validation set {0}", i);
                    vr.Passed++;
                }
            }
            return vr;
        }
        #endregion
    }    
}
