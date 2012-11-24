using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLABS.ANN
{
    /// <summary>
    /// For debugging and tracing in console
    /// </summary>
    public class Debugger
    {
        public static bool Debug1 = false;
        public static bool Debug2 = false;
        public static bool Debug3 = false;
        public static bool Debug4 = false;
        public static bool Debug5 = false;

        public delegate void WriteDelegate(string s);
        public static WriteDelegate Write;
        public static void WriteLine(string s, params object[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        s = s.Replace("{" + i + "}", args[i].ToString());
                    }
                }
                if (Write != null)
                {
                    Write(s);
                }
            }
            catch
            {
            }
        }

        public static void WriteLine(string s)
        {
            try
            {              
                if (Write != null)
                {
                    Write(s);
                }
            }
            catch
            {
            }
        }

        public static void WriteLine(object obj)
        {
            try
            {
                if (Write != null)
                {
                    Write(obj.ToString());
                }
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// Modes of a neural network
    /// </summary>
    public enum NetworkMode
    {
        Normal,
        Training,       
    }
    /// <summary>
    /// Type of a layer
    /// </summary>
    public enum LayerType
    {
        Input,
        Hidden,
        Output
    }

    public enum InputCombinationFunctionType
    {
        /// <summary>
        /// The net input is the sum of products of the previous layers neurons outputs and the synapses weights
        /// </summary>
        LinearProduct,
        /// <summary>
        /// The net input is the sum of eucledian distance between the synapses weights and outputs of the previous layers neuron
        /// </summary>
        EucledianDistance,
        /// <summary>
        /// The net input is the sum of manhattan distance between the synapses weights and outputs of the previous layers neuron
        /// </summary>
        ManhattanDistance
    }

    #region Delegates

    public delegate void NumberOfNeuronsChangedDelegate(Layer l);
    
    #endregion
    /// <summary>
    /// Represents a neural network
    /// </summary>
    [Serializable()]
    public class NeuralNetwork:ISerializable
    {
        #region Properties
        /// <summary>
        /// The mode in which the network is currently operating
        /// </summary>
        public NetworkMode Mode = NetworkMode.Normal;
        Layer _InputLayer;
        Layer _OutputLayer;
        /// <summary>
        /// Gets the input layer of the neural network
        /// </summary>
        public Layer InputLayer
        {
            get
            {
                return this._InputLayer;
            }
            set
            {
                this._InputLayer = value;
            }
        }
        /// <summary>
        /// Gets the output layer of the neural network
        /// </summary>
        public Layer OutputLayer
        {
            get
            {
                return this._OutputLayer;
            }
            set
            {
                this._OutputLayer = value;
            }
        }
        /// <summary>
        /// Gets or sets the list of hidden layers 
        /// </summary>
        public List<Layer> HiddenLayers = new List<Layer>();
        /// <summary>
        /// Gets or sets the list of synapses among the neurons
        /// </summary>
        public List<Synapsis> Synapses = new List<Synapsis>();
        /// <summary>
        /// Gets the number of layers in the network
        /// </summary>
        public int NumberOfLayers
        {
            get
            {
                return 2 + this.HiddenLayers.Count;
            }
        }
        /// <summary>
        /// Random number generator object
        /// </summary>
        public static Random RNG = new Random();
        #endregion
       
        #region Constructors
        public NeuralNetwork()
        {
            this._InputLayer = new Layer(this, LayerType.Input);
            this._OutputLayer = new Layer(this, LayerType.Output);
            this._InputLayer.NextLayer = this._OutputLayer;
        }

        /// <summary>
        /// A serializable synapsis class for serialization
        /// </summary>
        [Serializable()]
        internal class SerializableSynapsis : ISerializable
        {
            public string FromLayerName;
            public string ToLayerName;
            public int FromNeuronIndex;
            public int ToNeuronIndex;
            public double Weight;

            public SerializableSynapsis()
            {
            }

            public SerializableSynapsis(Synapsis s)
            {
                this.FromLayerName = s.From.Layer.Name;
                this.FromNeuronIndex = s.From.Index;

                this.ToLayerName = s.To.Layer.Name;
                this.ToNeuronIndex = s.To.Index;

                this.Weight = s.Weight;
            }

            public SerializableSynapsis(SerializationInfo info, StreamingContext sc)
            {
                FromLayerName = info.GetString("FromLayerName");
                ToLayerName = info.GetString("ToLayerName");
                FromNeuronIndex = info.GetInt32("FromNeuronIndex");
                ToNeuronIndex = info.GetInt32("ToNeuronIndex");
                Weight = info.GetDouble("Weight");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext sc)
            {
                info.AddValue("FromLayerName", FromLayerName);
                info.AddValue("ToLayerName", ToLayerName);
                info.AddValue("FromNeuronIndex", FromNeuronIndex);
                info.AddValue("ToNeuronIndex", ToNeuronIndex);
                info.AddValue("Weight", Weight);
            }

            public void FromSynapsis(Synapsis s)
            {
                this.FromLayerName = s.From.Layer.Name;
                this.FromNeuronIndex = s.From.Index;

                this.ToLayerName = s.To.Layer.Name;
                this.ToNeuronIndex = s.To.Index;

                this.Weight = s.Weight;
            }
        }
        
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sc"></param>
        public NeuralNetwork(SerializationInfo info, StreamingContext sc)
        {
            Layer inputlayer = (Layer)info.GetValue("InputLayer", typeof(Layer));
            Layer outputlayer = (Layer)info.GetValue("OutputLayer", typeof(Layer));
            List<Layer> hiddenlayers = (List<Layer>)info.GetValue("HiddenLayers", typeof(List<Layer>));
            List<string> layerhierarchy = (List<string>)info.GetValue("Layer hierarchy", typeof(List<string>));
            this.InputLayer = new Layer(this, LayerType.Input);
            this.InputLayer.NumberOfNeurons = inputlayer.NumberOfNeurons;
            for (int i = 0; i < inputlayer.NumberOfNeurons; i++)
            {
                this.InputLayer.Neurons[i].Threshold = inputlayer.Neurons[i].Threshold;                
            }
            this.InputLayer.ActivationFunction = inputlayer.ActivationFunction;
            this.InputLayer.InputCombinationFunction = inputlayer.InputCombinationFunction;
            this.OutputLayer = new Layer(this, LayerType.Output);
            this.OutputLayer.NumberOfNeurons = outputlayer.NumberOfNeurons;
            for (int i = 0; i < outputlayer.NumberOfNeurons; i++)
            {
                this.OutputLayer.Neurons[i].Threshold = outputlayer.Neurons[i].Threshold;
            }
            this.OutputLayer.ActivationFunction = outputlayer.ActivationFunction;
            this.OutputLayer.InputCombinationFunction = outputlayer.InputCombinationFunction;
            for (int j = 0; j < hiddenlayers.Count; j++)
            {
                Layer hiddenlayer = new Layer(this, LayerType.Hidden);
                hiddenlayer.NumberOfNeurons = hiddenlayers[j].NumberOfNeurons;
                for (int i = 0; i < hiddenlayers[j].NumberOfNeurons; i++)
                {
                    hiddenlayer.Neurons[i].Threshold = hiddenlayers[j].Neurons[i].Threshold;
                }
                hiddenlayer.ActivationFunction = hiddenlayers[j].ActivationFunction;
                hiddenlayer.InputCombinationFunction = hiddenlayers[j].InputCombinationFunction;
                this.HiddenLayers.Add(hiddenlayer);
            }

            //Restore the layer hierarchy
            Layer current = this.GetLayer(layerhierarchy[0]); ;
            for (int i = 1; i < layerhierarchy.Count; i++)
            {
                if (current != null)
                {
                    Layer next = this.GetLayer(layerhierarchy[i]);
                    if (next != null)
                    {
                        current.NextLayer = next;
                        next.PreviousLayer = current;
                        current = next;
                    }
                }
                else
                {
                    current = this.GetLayer(layerhierarchy[i]);
                }
            }

            //Load synapses
            List<SerializableSynapsis> ss = (List<SerializableSynapsis>)info.GetValue("Synapses", typeof(List<SerializableSynapsis>));
            this.Synapses.Clear();
            for (int i = 0; i < ss.Count; i++)
            {
                try
                {
                    Synapsis s = new Synapsis();
                    s.From = this.GetLayer(ss[i].FromLayerName).Neurons[ss[i].FromNeuronIndex];
                    s.To = this.GetLayer(ss[i].ToLayerName).Neurons[ss[i].ToNeuronIndex];
                    s.Weight = ss[i].Weight;
                    if (s.From != null && s.To != null)
                        Synapses.Add(s);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Serialization function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sc"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("InputLayer", this.InputLayer);
            info.AddValue("OutputLayer", this.OutputLayer);
            info.AddValue("HiddenLayers", this.HiddenLayers);         
            //Save the hierarchy of layers;
            List<string> LayerNames = new List<string>();
            Layer current = this.InputLayer;
            while (current != null)
            {
                LayerNames.Add(current.Name);
                current = current.NextLayer;
            }
            info.AddValue("Layer hierarchy", LayerNames);
            //Save the synapses
            List<SerializableSynapsis> ss = new List<SerializableSynapsis>();
            for (int i = 0; i < this.Synapses.Count; i++)
            {
                ss.Add(new SerializableSynapsis(Synapses[i]));
            }
            info.AddValue("Synapses", ss);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates Synapses between neurons of neignboring layers
        /// </summary>        
        public void CreateSynapses()
        {

            this.Synapses.Clear();
            Layer current = this._InputLayer;
            while (current != this._OutputLayer)
            {
                foreach (Neuron from in current.Neurons)
                {
                    foreach (Neuron to in current.NextLayer.Neurons)
                    {
                        Synapsis synapsis = new Synapsis();
                        synapsis.From = from;
                        synapsis.To = to;
                        from.PostSynapses.Add(synapsis);
                        to.PreSynapses.Add(synapsis);
                        this.Synapses.Add(synapsis);
                    }
                }
                current = current.NextLayer;
            }
        }

        public Synapsis GetSynapsis(Neuron from, Neuron to)
        {
            return this.Synapses.Find(delegate(Synapsis synapsis) { return (synapsis.From.Equals(from) && synapsis.To.Equals(to)); });
        }
        
        /// <summary>
        /// Adds a synapsis between neurons, allows to create feedback and self feedback loop
        /// </summary>
        /// <param name="synapsis">The synapsis to add</param>
        public void AddSynapsis(Synapsis synapsis)
        {
            this.Synapses.Add(synapsis);
            if (!synapsis.From.PostSynapses.Contains(synapsis))
            {
                synapsis.From.PostSynapses.Add(synapsis);
            }
            if (!synapsis.To.PreSynapses.Contains(synapsis))
            {
                synapsis.To.PreSynapses.Add(synapsis);
            }
        }

        public bool RemoveSynapsis(Synapsis synapsis)
        {
            return this.Synapses.Remove(synapsis);
        }

        /// <summary>
        /// Removes the synapsis between neurons 'from' and 'to'
        /// </summary>
        /// <param name="from">The neuron from which the synapsis starts</param>
        /// <param name="to">The neuron at which the synapsis ends</param>
        /// <returns></returns>
        public bool RemoveSynapsis(Neuron from, Neuron to)
        {
            return this.Synapses.Remove(this.Synapses.Find(delegate (Synapsis synapsis){return (synapsis.From.Equals(from)&&synapsis.To.Equals(to));}));
        }
        /// <summary>
        /// Sets random threshold value of the neurons between -1 and 1
        /// </summary>
        public void RandomizeNeuronThresholds()
        {
            Layer current = this.InputLayer;
            while (current != null)
            {
                foreach (Neuron neuron in current.Neurons)
                {
                    neuron.SetRandomThreshold();
                }
                current = current.NextLayer;
            }
        }
        /// <summary>
        /// Sets random threshold value of the neurons between range_start and range_end
        /// </summary>
        /// <param name="range_start"></param>
        /// <param name="range_end"></param>
        public void RandomizeNeuronThresholds(double range_start, double range_end)
        {
            Layer current = this.InputLayer;
            while (current != null)
            {                foreach (Neuron neuron in current.Neurons)
                {
                    neuron.SetRandomThreshold(range_start, range_end);
                }
                current = current.NextLayer;
            }
        }        
        /// <summary>
        /// Set random weights of the synapses between -1 and +1;
        /// </summary>
        public void RandomizeSynapsesWeights()
        {
            foreach (Synapsis synapsis in this.Synapses)
            {
                synapsis.SetRandomWeight();
            }
        }
        /// <summary>
        /// Set random weights of the synapses between the range_start and range_end
        /// </summary>
        /// <param name="range_start"></param>
        /// <param name="range_end"></param>
        public void RandomizeSynapsesWeights(double range_start, double range_end)
        {
            foreach (Synapsis synapsis in this.Synapses)
            {
                synapsis.SetRandomWeight(range_start, range_end);
            }
        }        
        /// <summary>
        /// Resets the weights of the synapses
        /// </summary>
        public void ResetSynapses()
        {
            foreach (Synapsis synapsis in this.Synapses)
            {
                synapsis.Reset();
            }
        }

        /// <summary>
        /// Gets the layer by name
        /// </summary>
        /// <param name="name"></param>
        public Layer GetLayer(string name)
        {
            if (this.InputLayer.Name == name) return this.InputLayer;
            else if (this.OutputLayer.Name == name) return this.OutputLayer;
            else
            {
                for (int i = 0; i < this.HiddenLayers.Count; i++)
                {
                    if (this.HiddenLayers[i].Name == name) return this.HiddenLayers[i]; 
                }
            }
            return null;
        }

        /// <summary>
        /// Propagates from input to output layer
        /// </summary>
        public void Propagate()
        {
            Layer current = this.InputLayer;
            while (current != null)
            {
                current.FeedForward();
                current = current.NextLayer;
            }
        }      

        /// <summary>
        /// Evaluate the output set from the given input set        
        /// </summary>
        /// <param name="input_set">The input set to evaluate</param>
        /// <returns></returns>
        public Set Evaluate(Set input_set)
        {
            if(input_set.Count != this.InputLayer.NumberOfNeurons)
            {
                throw new Exception("Input set size doesnot match number of input layer neurons");
            }
            Set output_set = new Set();            
            // Set inputs
            for (int i = 0; i < this.InputLayer.NumberOfNeurons; i++)
            {
                this.InputLayer.Neurons[i].Input = input_set[i]; 
            }
            // Run network
            this.Propagate();
            // Collect outputs
            for (int i = 0; i < this.OutputLayer.NumberOfNeurons; i++)
            {
                output_set.Add(this.OutputLayer.Neurons[i].Output);
            }
            return output_set;
        }

        /// <summary>
        /// Reset the neural network        
        /// </summary>
        public void Reset()
        {
            this._InputLayer = null;
            this._OutputLayer = null;
            this.HiddenLayers.Clear(); 
            this.Synapses.Clear();
        }

        public bool SaveToFile(string filename)
        {
            try
            {
                using (Stream stream = File.Open(filename, FileMode.Create))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(stream, this);                    
                    stream.Close();
                }
                return true;
            }
            catch
            {               
                return false;
            }
        }
        
        /// <summary>
        /// Loads the network from the file
        /// </summary>
        /// <param name="filename">The file name with .ann extension</param>
        /// <returns></returns>
        public static NeuralNetwork FromFile(string filename)
        {
            try
            {
                NeuralNetwork nn = new NeuralNetwork();
                using (Stream stream = File.Open(filename, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();                    
                    bformatter = new BinaryFormatter();
                    nn = (NeuralNetwork)bformatter.Deserialize(stream);
                    stream.Close();
                }
                return nn;
            }
            catch
            {               
                return null;
            }

        }
        #endregion        
    }        
    /// <summary>
    /// Represents a layer in the neural netwrok
    /// </summary>
    [Serializable()]
    public class Layer:ISerializable
    {
        #region Properties
        LayerType _LayerType;
        /// <summary>
        /// Gets the type of the layer [input/hidden/output]
        /// </summary>
        public LayerType LayerType
        {
            get
            {
                return this._LayerType;
            }
        }
        NeuralNetwork _Network;
        /// <summary>
        /// Gets the parent network the layer belongs to;
        /// </summary>
        public NeuralNetwork Network
        {
            get
            {
                return this._Network;
            }
        }
        List<Neuron> _Neurons = new List<Neuron>();
        /// <summary>
        /// Gets the list of neurons in the layer
        /// </summary>
        public List<Neuron> Neurons
        {
            get
            {
                return this._Neurons;
            }
        }

        public NumberOfNeuronsChangedDelegate NumberOfNeuronsChanged;
        /// <summary>
        /// Gets or sets the number of neurons in the layer
        /// </summary>
        public int NumberOfNeurons
        {
            get
            {
                return this.Neurons.Count;
            }
            set
            {
                if (this.Neurons.Count > 0)
                {
                    this._Neurons.Clear();
                }                
                for (int i = 0; i < value; i++)
                {
                    this._Neurons.Add(new Neuron(this));
                }
                if (this.NumberOfNeuronsChanged != null)
                {
                    NumberOfNeuronsChanged(this);
                }
            }
        }

        /// <summary>
        /// Gets the name of the layer
        /// </summary>
        public string Name
        {
            get
            {
                string name = this.LayerType.ToString() + " layer";
                if (this.Network != null && this.LayerType == ANN.LayerType.Hidden && this.Network.HiddenLayers.Count > 1)
                {
                    name += " " + (this.Network.HiddenLayers.IndexOf(this) + 1);                                
                }
                return name;
            }
        }
        Layer _PreviousLayer;
        /// <summary>
        /// Gets or sets the previous layer of the layer
        /// </summary>
        public Layer PreviousLayer
        {
            get
            {
                return this._PreviousLayer;
            }
            set
            {
                this._PreviousLayer = value;
                value.SetNextLayer(this);
            }
        }
        Layer _NextLayer;
        /// <summary>
        /// Gets or sets the next layer of the neuron
        /// </summary>
        public Layer NextLayer
        {
            get
            {
                return this._NextLayer;
            }
            set
            {
                this._NextLayer = value;
                value.SetPreviousLayer(this);
            }
        }
        ActivationFunction _ActivationFunction;
        /// <summary>
        /// Gets or sets the ActivationFunction to be used by the neurons of this layer
        /// </summary>
        public ActivationFunction ActivationFunction
        {
            get
            {
                return this._ActivationFunction;
            }
            set
            {
                this._ActivationFunction = value;
            }
        }       
        InputCombinationFunctionType _InputCombinationFunction = InputCombinationFunctionType.LinearProduct;
        /// <summary>
        /// Gets or sets the combination function used to combine the inputs of the neuron to produce net input
        /// </summary>
        public InputCombinationFunctionType InputCombinationFunction
        {
            get
            {
                return this._InputCombinationFunction;
            }
            set
            {
                this._InputCombinationFunction = value;
            }
        }

        /// <summary>
        /// Gets the neuron at specified index
        /// </summary>
        /// <param name="index">index of the neuron</param>
        /// <returns></returns>
        public Neuron this[int index]
        {
            get
            {
                if (index >= 0 && index < this.NumberOfNeurons)
                {
                    return this._Neurons[index];
                }
                else
                {
                    return null;
                }
            }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor of layer class
        /// </summary>
        /// <param name="nn">Parent network to whick the layer belongs</param>
        /// <param name="layer_type">Type of the layer</param>
        public Layer(NeuralNetwork nn, LayerType layer_type)
        {
            this._Network = nn;
            this._LayerType = layer_type;
        }

        public Layer(SerializationInfo info, StreamingContext sc)
        {
            this._Neurons.Clear();
            this._LayerType = (LayerType)info.GetValue("Layer type", typeof(LayerType));
            List<double> neurons  = (List<double>)info.GetValue("Neurons", typeof(List<double>));
            for (int i = 0; i < neurons.Count; i++)
            {
                Neuron n = new Neuron(this);
                n.Threshold = neurons[i];
                this._Neurons.Add(n);                
            }

            try
            {
                string afn = info.GetString("Activation function name");                
                switch (afn)
                {
                    case "Unipolar Sign Function":
                        System.Windows.Forms.MessageBox.Show((info.GetValue("Activation function", typeof(UnipolarSignFunction)) as ActivationFunction).FunctionName);
                        this.ActivationFunction = (UnipolarSignFunction)info.GetValue("Activation function", typeof(UnipolarSignFunction));
                        break;
                    case "Bipolar Sign Function":
                        this.ActivationFunction = (BipolarSignFunction)info.GetValue("Activation function", typeof(BipolarSignFunction));
                        break;
                    case "Unipolar Linear Function":
                        this.ActivationFunction = (UnipolarLinearFunction)info.GetValue("Activation function", typeof(UnipolarLinearFunction));
                        break;
                    case "Bipolar Linear Function":
                        this.ActivationFunction = (BipolarLinearFunction)info.GetValue("Activation function", typeof(BipolarLinearFunction));
                        break;
                    case "Sigmoidal Unipolar Function":
                        this.ActivationFunction = (SigmoidalUnipolarFunction)info.GetValue("Activation function", typeof(SigmoidalUnipolarFunction));
                        break;
                    case "Sigmoidal Bipolar Function":
                        this.ActivationFunction = (SigmoidalBipolarFunction)info.GetValue("Activation function", typeof(SigmoidalBipolarFunction));
                        break;
                    case "Tangent Hyperbolic Function":
                        this.ActivationFunction = (TangentHyperbolicFunction)info.GetValue("Activation function", typeof(TangentHyperbolicFunction));
                        break;
                    case "Gaussian Activation Function":
                        this.ActivationFunction = (GaussianActivationFunction)info.GetValue("Activation function", typeof(GaussianActivationFunction));
                        break;
                    default:   
                        this.ActivationFunction = null;
                        break;
                }                
            }
            catch
            {               
                this.ActivationFunction = null;
            }           
            
            this.InputCombinationFunction = (InputCombinationFunctionType)info.GetValue("Combination function", typeof(InputCombinationFunctionType));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("Layer type", this._LayerType);
            //Saving the neurons in a different way
            List<double> neurons = new List<double>();
            for (int i = 0; i < this._Neurons.Count; i++)
            {
                neurons.Add(this._Neurons[i].Threshold);
            }
            info.AddValue("Neurons", neurons);
            if (this.ActivationFunction != null)
            {
                info.AddValue("Activation function name", this.ActivationFunction.FunctionName);
                info.AddValue("Activation function", this.ActivationFunction);
            }
            info.AddValue("Combination function", this.InputCombinationFunction);
        }

        public override string ToString()
        {
            return this.Name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Feed forward to the next layer
        /// </summary>
        public void FeedForward()
        {
            foreach (Neuron neuron in this._Neurons)
            {
                neuron.EvaluateOutput();
            }
        }
        /// <summary>
        /// Evaluate the local error of the neurons with respect to global error
        /// </summary>
        public void EvaluateError()
        {
            foreach (Neuron neuron in this._Neurons)
            {
                neuron.EvaluateError();
            }
        }
        /// <summary>
        /// Sets the previous layer of this layer
        /// </summary>
        /// <param name="previous_layer"></param>
        public void SetPreviousLayer(Layer previous_layer)
        {
            this._PreviousLayer = previous_layer;
        }
        /// <summary>
        /// Sets the next layer of this layer
        /// </summary>
        /// <param name="next_layer"></param>
        public void SetNextLayer(Layer next_layer)
        {
            this._NextLayer = next_layer;
        } 
        #endregion
    }

    /// <summary>
    /// Represents a neuron in the neural network
    /// </summary>
    [Serializable()]
    public class Neuron:ISerializable
    {
        #region Properties
        Layer _Layer;
        /// <summary>
        /// Gets the layer the neuron belongs to
        /// </summary>
        public Layer Layer
        {
            get
            {
                return this._Layer;
            }
        }

        /// <summary>
        /// Gets the network the eneuron belongs to
        /// </summary>
        public NeuralNetwork Network
        {
            get
            {
                return this._Layer.Network;
            }
        }
        /// <summary>
        /// Gets the index in the list of neurons of the parent layer
        /// </summary>
        public int Index
        {
            get
            {
                if (this.Layer != null)
                {
                    return this.Layer.Neurons.IndexOf(this);
                }
                else
                {
                    return -1;
                }
            }
        }
        double _Input = 0.0;
        double _Output = 0.0;
        /// <summary>
        /// Gets or sets the input of the neuron [in training mode]
        /// </summary>
        public double Input
        {
            get
            {
                return this._Input;
            }
            set
            {
                this._Input = value;
            }
        }
        /// <summary>
        /// Get the output of last evaluation
        /// </summary>
        public double Output
        {
            get
            {
                return this._Output;
            }
        }

        double _Threshold = 0.0;
        /// <summary>
        /// Gets or sets the threshold or bias value associated with the neuron
        /// </summary>
        public double Threshold
        {
            get
            {
                return this._Threshold;
            }
            set
            {
                this._Threshold = (-1.0 <= value && value <= 1.0) ? value : (value > 1.0 ? 1.0 : -1.0);
            }
        }

        /// <summary>
        /// Gets the net input depending upon the InputCombinationFunctionType used by the parent layer
        /// </summary>
        public double NetInput
        {
            get
            {
                double ni = this.Input;
                switch (this.Layer.InputCombinationFunction)
                {
                    case InputCombinationFunctionType.LinearProduct:
                        foreach (Synapsis synapsis in this.PreSynapses)
                        {
                            if (synapsis.To == this)
                            {                                
                                ni += synapsis.From.Output * synapsis.Weight;
                            }
                        }                        
                        break;
                    case InputCombinationFunctionType.EucledianDistance:
                        foreach (Synapsis synapsis in this.PreSynapses)
                        {
                            if (synapsis.To == this)
                            {
                                ni += Math.Pow((synapsis.Weight - synapsis.From.Output), 2);
                            }
                        }
                        ni = Math.Sqrt(ni);
                        break;
                    case InputCombinationFunctionType.ManhattanDistance:
                        foreach (Synapsis synapsis in this.PreSynapses)
                        {
                            if (synapsis.To == this)
                            {
                                ni += Math.Abs(synapsis.Weight - synapsis.From.Output);
                            }
                        }                        
                        break;
                }
                return ni;
            }
        }

        /// <summary>
        /// Gets or sets the list of synapses connected backward of the neuron
        /// </summary>
        public List<Synapsis> PreSynapses = new List<Synapsis>();
        /// <summary>
        /// Gets or sets the list of synapses connected forward of the neuron
        /// </summary>
        public List<Synapsis> PostSynapses = new List<Synapsis>();        
        #endregion
        
        #region Training mode properties
        double _DeltaThreshold = 0.0;
        double _SumDeltaThreshold = 0.0;
        /// <summary>
        /// In training mode, gets or sets the change in threshold of the neuron from the last iteration
        /// </summary>
        public double DeltaThreshold
        {
            get
            {
                return this._DeltaThreshold;
            }
            set
            {
                this._DeltaThreshold = value;
            }
        }

        /// <summary>
        /// In batch training mode, gets or sets the total change in threshold of the neuron from the initial value
        /// </summary>
        public double SumDeltaThreshold
        {
            get
            {
                return _SumDeltaThreshold;
            }
            set
            {
                this._SumDeltaThreshold = value;
            }
        }
        double _Error = 0.0;
        /// <summary>
        /// In training mode, gets the error of last evaluation
        /// </summary>
        public double Error
        {
            get
            {
                return _Error;
            }
        }
        double _Target = double.NaN;
        /// <summary>
        /// In training mode, gets or sets the target value of the neuron if it is in output layer.
        /// </summary>
        public double Target
        {
            get
            {
                if (this._Layer.LayerType == LayerType.Output && this.Network.Mode != NetworkMode.Normal)
                {
                    return _Target;
                }
                else
                {
                    return double.NaN;
                }
            }
            set
            {
                this._Target = value;
            }
        }      
        #endregion  

        #region Constructors       

        public Neuron(Layer parent_layer)
        {
            this._Layer = parent_layer;
        }

        public Neuron(SerializationInfo info, StreamingContext sc)
        {
            this._Threshold = info.GetDouble("Threshold");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("Threshold", this._Threshold);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set random threshold value of the neuron between -1 and 1
        /// </summary>
        public void SetRandomThreshold()
        {
            this._Threshold = (NeuralNetwork.RNG.NextDouble() * 2) - 1.0;
        }

        /// <summary>
        /// Set random threshold value of the neuron between range_start and range_end
        /// </summary>
        /// <param name="range_start"></param>
        /// <param name="range_end"></param>
        public void SetRandomThreshold(double range_start, double range_end)
        {
            if (range_end - range_start == 1)
            {
                this._Threshold = range_start + NeuralNetwork.RNG.NextDouble();
            }
            else
            {
                this._Threshold = (NeuralNetwork.RNG.NextDouble() * (range_end - range_start)) - ((range_end - range_start) / 2.0);
            }
        }

        /// <summary>
        /// Evaluate the neurons input and produce output
        /// </summary>
        public void EvaluateOutput()
        {
            if (this.Layer != null)
            {
                if (this.Layer.ActivationFunction == null)
                {
                    //No activation function set; forward input to output without change
                    this._Output = this.NetInput - this.Threshold;
                }
                else
                {
                    this._Output = this.Layer.ActivationFunction.GetOutput(this.NetInput - this.Threshold);
                }
            }
            if (Debugger.Debug3) Debugger.WriteLine("{0} Layer: Neuron {1} => input: {2}, net input: {3}, threshold: {4}, output: {5}", this.Layer.LayerType, this.Index, this.Input, this.NetInput, this.Threshold, this._Output);
        }

        /// <summary>
        /// Evaluates the error of the neuron with respect to total error in gradient descent learning method
        /// </summary>
        public void EvaluateError()
        {
            if (this._Layer.LayerType == LayerType.Output)
            {
                double error = this._Target - this._Output;
                this._Error = this.Layer.ActivationFunction.GetOutputDerivative(this._Output) * error;
            }
            else
            {
                double error = 0.0;
                foreach (Synapsis synapsis in this.PostSynapses)
                {
                    if (synapsis.From == this)
                    {
                        error += synapsis.Weight * synapsis.To.Error;
                    }
                }
                if (this.Layer.ActivationFunction == null)
                {
                    this._Error = error;
                }
                else
                {
                    this._Error = this.Layer.ActivationFunction.GetOutputDerivative(this._Output) * error;
                }
            }
            if (Debugger.Debug3) Debugger.WriteLine("{0} Layer : Neuron {1} => target: {2}, output: {3}, error: {4}", this.Layer.LayerType, this.Index, this.Target, this._Output, this._Error);
        } 
        #endregion
    }
    /// <summary>
    /// Represents a synapsis between two neurons
    /// </summary>
    [Serializable()]
    public class Synapsis:ISerializable
    {
        #region Properties
        /// <summary>
        /// Gets or sets the neuron the synapsis originates from
        /// </summary>
        public Neuron From;
        /// <summary>
        /// Gets or sets the neuron the synapsis ends at
        /// </summary>
        public Neuron To;

        double _Weight = 0.0;
        double _Delta = 0.0;
        double _SumDelta = 0.0;
        /// <summary>
        /// Gets or sets the weight of the synapsis.
        /// </summary>
        public double Weight
        {
            get
            {
                return this._Weight;
            }
            set
            {
                this._Weight = value;
            }
        } 
        #endregion

        #region Training mode properties
        /// <summary>
        /// In training mode, Gets or sets the amount of change in weight with respect to previous iteration
        /// </summary>
        public double Delta
        {
            get
            {
                return this._Delta;
            }
            set
            {
                this._Delta = value;
            }
        }
        /// <summary>
        /// In batch training mode, Gets or sets the total amount of weight change with respect to initial weight
        /// </summary>
        public double SumDelta
        {
            get
            {
                return this._SumDelta;
            }
            set
            {
                this._SumDelta = value;
            }
        } 
        #endregion

        #region Constructors
        public Synapsis()
        {
        }
        
        public Synapsis(Neuron from, Neuron to)
        {
            this.From = from;
            this.To = to;
            from.PostSynapses.Add(this);
            to.PreSynapses.Add(this);
        }

        public Synapsis(Neuron from, Neuron to, bool random_weight)
        {
            this.From = from;
            this.To = to;
            from.PostSynapses.Add(this);
            to.PreSynapses.Add(this);
            if (random_weight)
            {
                this.SetRandomWeight();
            }
        }

        public Synapsis(Neuron from, Neuron to, double weight)
        {
            this.From = from;
            this.To = to;
            from.PostSynapses.Add(this);
            to.PreSynapses.Add(this);
            this.Weight = weight;
        }

        public Synapsis(SerializationInfo info, StreamingContext sc)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext sc)
        {            
        }
        #endregion

        #region Methods
        public void Reset()
        {
            this._Weight = 0.0;
            this._Delta = 0.0;
            this._SumDelta = 0.0;
        }

        /// <summary>
        /// Set a random weight for the synapsis with range between -1 and +1
        /// </summary>
        public void SetRandomWeight()
        {
            this._Weight = (NeuralNetwork.RNG.NextDouble() * 2) - 1.0;
        }

        /// <summary>
        /// Set a random weight for the synapsis with range between range_start and range_end
        /// </summary>
        /// <param name="range_start">The value from which range starts</param>
        /// <param name="range_end">The value at which range ends</param>
        public void SetRandomWeight(double range_start, double range_end)
        {
            if (range_end - range_start == 1)
            {
                this._Weight = range_start + NeuralNetwork.RNG.NextDouble();
            }
            else
            {
                this._Weight = (NeuralNetwork.RNG.NextDouble() * (range_end - range_start)) - ((range_end - range_start) / 2.0);
            }
        } 
        #endregion
    }

    /// <summary>
    /// Represents a set of double type values.
    /// </summary>    
    public class Set:List<double>
    {
        public Set()
        {
        }

        public Set(params double[] values)
        {
            this.AddRange(values);
        }
    }

    /// <summary>
    /// Represents a pattern wirh input and output set.
    /// </summary>
    public class IOSet
    {
        public Set InputSet = new Set(); 
        public Set OutputSet = new Set();

        public IOSet()
        {
        }

        public IOSet(Set input_set, Set output_set)
        {
            this.InputSet = input_set;
            this.OutputSet = output_set;
        }
    }    
}
