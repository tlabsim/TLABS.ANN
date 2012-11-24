using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TLABS.ANN
{
    /// <summary>
    /// Abstract class to define an activation function
    /// </summary>
    [Serializable()]
    public abstract class ActivationFunction:ISerializable
    {
        protected string _FunctionName = string.Empty;
        /// <summary>
        /// Gets the function name
        /// </summary>
        public string FunctionName
        {
            get
            {
                return this._FunctionName;
            }
        }
        /// <summary>
        /// The default learning rate for this activation function, 
        /// these values may come from empirical results, 
        /// while using a specific activation function
        /// using the default learning rate of that function for the network may give the best result.
        /// </summary>
        public const double DefaultLearningRate = 0.9;

        public ActivationFunction()
        {
        }
        
        public ActivationFunction(SerializationInfo info, StreamingContext sc)
        {
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext sc)
        {            
        }
        

        /// <summary>
        /// Evaluates the output for a given input and threshold
        /// </summary>
        /// <param name="input">The input value of the function</param>
        /// <param name="threshold">The threshold value</param>
        /// <returns></returns>
        public virtual double GetOutput(double input)
        {
            return 0.0;
        }

        /// <summary>
        /// Evaluates the error derivative for a given output and nominal error
        /// </summary>
        /// <param name="output">The output value</param>
        /// <param name="error">The nominal error value</param>
        /// <returns></returns>
        public virtual double GetOutputDerivative(double output)
        {
            return 0.0;
        }
    }

    /// <summary>
    /// Represents a sign function which outputs between 0 and 1
    /// </summary>
    [Serializable()]
    public class UnipolarSignFunction : ActivationFunction, ISerializable
    {
        public UnipolarSignFunction()
        {
            this._FunctionName = "Unipolar Sign Function";
        }

        public UnipolarSignFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Unipolar Sign Function";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {            
        }

        public override double GetOutput(double input)
        {
            return input >= 0 ? 1.0 : 0.0;
        }

        public override double GetOutputDerivative(double output)
        {
            return Math.Abs(output) <= 0 ? double.MaxValue : 0;
        }
    }

    /// <summary>
    /// Represents a sign function which outputs between -1 and 1
    /// </summary>
    [Serializable()]
    public class BipolarSignFunction : ActivationFunction, ISerializable
    {
        public BipolarSignFunction()
        {
            this._FunctionName = "Bipolar Sign Function";
        }

        public BipolarSignFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Bipolar Sign Function";
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
        }
        public override double GetOutput(double input)
        {
            return input >= 0 ? 1.0 : -1.0;
        }

        public override double GetOutputDerivative(double output)
        {
            return Math.Abs(output) <= 0 ? double.MaxValue : 0;
        }
    }

    /// <summary>
    /// Represents a unipolar linear activation function  which outputs between 0 and 1
    /// </summary>
    [Serializable()]
    public class UnipolarLinearFunction : ActivationFunction, ISerializable
    {
        double _ScalingFactor = 1.0;
        public double ScalingFactor
        {
            get
            {
                return this._ScalingFactor;
            }
            set
            {
                this._ScalingFactor = (value >= 0 && value <= 1.0) ? value : 1.0;
            }
        }

        public UnipolarLinearFunction()
        {
            this._FunctionName = "Unipolar Linear Function";
        }

        public UnipolarLinearFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Unipolar Linear Function";
            this.ScalingFactor = info.GetDouble("ScalingFactor");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("ScalingFactor", this._ScalingFactor);
        }
        
        public override double  GetOutput(double input)
        {
            if (input > (0.5/this._ScalingFactor)) return 1;
            else if (input < -(0.5/this._ScalingFactor)) return 0;
            else return 0.5 + this._ScalingFactor * input;
        }
        /// <summary>
        /// Get the diff function value
        /// </summary>
        /// <param name="output">x</param>
        /// <returns>f'(x)</returns>
        public override double GetOutputDerivative(double output) 
        {
            if (output > (0.5 / this._ScalingFactor)) return 0;
            else if (output < -(0.5 / this._ScalingFactor)) return 0;
            else return this._ScalingFactor;
        }
        
    }

    /// <summary>
    /// Represents a bipolar linear activation function  which outputs between -1 and 1
    /// </summary>
    [Serializable()]
    public class BipolarLinearFunction : ActivationFunction, ISerializable
    {
        double _ScalingFactor = 1.0;
        public double ScalingFactor
        {
            get
            {
                return this._ScalingFactor;
            }
            set
            {
                this._ScalingFactor = (value >= 0 && value <= 1.0) ? value : 1.0;
            }
        }

        public BipolarLinearFunction()
        {
            this._FunctionName = "Bipolar Linear Function";
        }

        public BipolarLinearFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Bipolar Linear Function";
            this.ScalingFactor = info.GetDouble("ScalingFactor");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("ScalingFactor", this._ScalingFactor);
        }
        
        public override double GetOutput(double input)
        {
            if (input > (1 / this._ScalingFactor)) return 1;
            else if (input < -(1 / this._ScalingFactor)) return -1;
            else return this._ScalingFactor * input;
        }
        /// <summary>
        /// Get the diff function value
        /// </summary>
        /// <param name="output">x</param>
        /// <returns>f'(x)</returns>
        public override double GetOutputDerivative(double output)
        {
            if (output > (0.5 / this._ScalingFactor)) return 0;
            else if (output < -(0.5 / this._ScalingFactor)) return 0;
            else return this._ScalingFactor;
        }

    }

    /// <summary>
    /// Represewnts a sigmoidal logistic/transfer unipolar function
    /// </summary>
    [Serializable()]
    public class SigmoidalUnipolarFunction : ActivationFunction, ISerializable
    {
        /// <summary>
        /// Gets or sets the sigmoidal gain or scaling factor of the function
        /// </summary>
        public double SigmoidalGain = 1.0;
        public SigmoidalUnipolarFunction()
        {
            this._FunctionName = "Sigmoidal Unipolar Function";            
        }

        public SigmoidalUnipolarFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Sigmoidal Unipolar Function";
            this.SigmoidalGain = info.GetDouble("SigmoidalGain");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("SigmoidalGain", this.SigmoidalGain);
        }

        /// <summary>
        /// Evaluates the ouput value for a given input and threshold value using sigmoidal unipolar function [y = 1/(1 + e(-lambda*(input-threshold)))] 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public override double GetOutput(double input)
        {
            return 1.0 / (1.0 + Math.Exp(-1.0 * this.SigmoidalGain * input));
        }

        /// <summary>
        /// Evaluates the error derivative for a given output and nominal error
        /// </summary>
        /// <param name="output">The output value</param>
        /// <param name="error">The nominal error value</param>
        /// <returns></returns>
        public override double GetOutputDerivative(double output)
        {
            return this.SigmoidalGain * output * (1.0 - output);
        }
    }

    /// <summary>
    /// Represewnts a sigmoidal logistic/transfer bipolar function
    /// </summary>
    [Serializable()]
    public class SigmoidalBipolarFunction : ActivationFunction, ISerializable
    {
        /// <summary>
        /// Gets or sets the sigmoidal gain or scaling factor of the function
        /// </summary>
        public double SigmoidalGain = 1.0; 
        public SigmoidalBipolarFunction()
        {
            this._FunctionName = "Sigmoidal Bipolar Function";            
        }

        public SigmoidalBipolarFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Sigmoidal Bipolar Function";
            this.SigmoidalGain = info.GetDouble("SigmoidalGain");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("SigmoidalGain", this.SigmoidalGain);
        }

        /// <summary>
        /// Evaluates the output value for a given input and threshold value using sigmoidal logistic bipolar function [y = 1/(1 + e(-lambda*(input-threshold)))] 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public override double GetOutput(double input)
        {
            return (1.0 - Math.Exp(-1.0 * this.SigmoidalGain * input)) / (1.0 + Math.Exp(-1.0 * this.SigmoidalGain * input));
        }

        /// <summary>
        /// Evaluates the error derivative for a given output and nominal error
        /// </summary>
        /// <param name="output">The output value</param>
        /// <param name="error">The nominal error value</param>
        /// <returns></returns>
        public override double GetOutputDerivative(double output)
        {
            return this.SigmoidalGain * .5 * (1.0 - output) * (1.0 + output);
        }
    }

    /// <summary>
    /// Represents a sigmoidal tanh function
    /// </summary>
    [Serializable()]
    public class TangentHyperbolicFunction : ActivationFunction, ISerializable
    {
        /// <summary>
        /// Gets or sets the sigmoidal gain or scaling factor of the function
        /// </summary>
        public double SigmoidalGain = 1.0;
        public TangentHyperbolicFunction()
        {
            this._FunctionName = "Tangent Hyperbolic Function";
        }

        public TangentHyperbolicFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Tangent Hyperbolic Function";
            this.SigmoidalGain = info.GetDouble("SigmoidalGain");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("SigmoidalGain", this.SigmoidalGain);
        }

        /// <summary>
        /// Evaluates the output for a given input and threshold
        /// </summary>
        /// <param name="input">The input value of the function</param>
        /// <param name="threshold">The threshold value</param>
        /// <returns></returns>
        public override double GetOutput(double input)
        {
            return Math.Tanh(input * this.SigmoidalGain);
        }

        /// <summary>
        /// Evaluates the error derivative for a given output and nominal error
        /// </summary>
        /// <param name="output">The output value</param>
        /// <param name="error">The nominal error value</param>
        /// <returns></returns>
        public override double GetOutputDerivative(double output)
        {
            return this.SigmoidalGain * (1.00 - output * output);
        }
    }

    /// <summary>
    /// Represents a gaussian activation function
    /// </summary>
    [Serializable()]
    public class GaussianActivationFunction : ActivationFunction, ISerializable
    {
        /// <summary>
        /// The standard deviation parameter of the gaussian function
        /// </summary>
        double _Deviation = 0.159155;
        /// <summary>
        /// The mean value  of the gaussian function
        /// </summary>
        double _Mean = 0.0;

        double C, IC, K, IK;

        /// <summary>
        /// Get or set the sigma parameter of the function
        /// (sigma must be positive)
        /// </summary>
        public double Deviation
        {
            get { return _Deviation; }
            set
            {
                _Deviation = (value > 0) ? value : _Deviation;
                ComputeCICKIK();
            }
        }
        /// <summary>
        /// Get or set the mu parameter of the function
        /// </summary>
        public double Mean
        {
            get { return _Mean; }
            set { _Mean = value; }
        }
        /// <summary>
        /// Compute C and K parameters from sigma
        /// </summary>
        void ComputeCICKIK()
        {
            C = (double)Math.Sqrt(2 * Math.PI * _Deviation);
            IC = 1.0 / C;
            K = 2 * _Deviation * _Deviation;
            IK = 1.0 / K;
        }
        /// <summary>
        /// GaussianActivationFunction constructor
        /// </summary>
        public GaussianActivationFunction()
        {
            this._FunctionName = "Gaussian Activation Function";
            ComputeCICKIK();
        }

        public GaussianActivationFunction(SerializationInfo info, StreamingContext sc)
        {
            this._FunctionName = "Gaussian Activation Function";
            this._Mean = info.GetDouble("Mean"); 
            this._Deviation = info.GetDouble("Deviation");
            ComputeCICKIK();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext sc)
        {
            info.AddValue("Mean", this._Mean);
            info.AddValue("Deviation", this._Deviation);
        }
      
        /// <summary>
        /// Compute the value of the gaussian function
        /// <param name="input">x</param>
        /// <returns>f(x)</returns>
        /// </summary>
        public override double GetOutput(double input)
        {
            return  IC * (double)Math.Exp(-(input - _Mean) * (input - _Mean) * IK);
        }
        /// <summary>
        /// compute the derivative value of function
        /// </summary>
        /// <param name="output">x</param>
        /// <returns>f'(x)</returns>
        public override double GetOutputDerivative(double output)
        {
            double x = -Math.Sqrt(Math.Log(output * C) * K);
            return -2 * output * K * x;
        }
    }
}
