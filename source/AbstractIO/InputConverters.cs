using System;
using System.Threading;

namespace AbstractIO
{
    #region Inverter

    /// <summary>
    /// A class inverting an <see cref="IBooleanInput"/>.
    /// </summary>
    public class BooleanInputInverter : IBooleanInput
    {
        private readonly IBooleanInput _source;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input to be converted.</param>
        public BooleanInputInverter(IBooleanInput source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets the inverted value of the source input passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_source.Value;
            }
        }
    }

    /// <summary>
    /// A class inverting an <see cref="IObservableBooleanInput"/>.
    /// </summary>
    public class ObserverableBooleanInputInverter : IObservableBooleanInput
    {
        private readonly IObservableBooleanInput _source;

        /// <summary>
        /// This event gets fired when the Value property of the abstract input/output interfaces has changed.
        /// </summary>
        /// <remarks>
        /// In addition to the Value property of the observed object, the new value to which that property changed will
        /// be readily passed to the newValue parameter of the event handler. Thus you have the guarantee to see the
        /// original value causing the event, not a possibly meanwhile again changed Value property. So, handlers of
        /// this event should usually inspect their newValue parameter and not query the object's Value property.
        /// </remarks>
        public event BooleanValueChangedHandler ValueChanged;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="newValue">The new value to which the input has changed.</param>
        protected void OnValueChanged(bool newValue)
        {
            ValueChanged(this, newValue);
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="source">The input to be converted.</param>
        public ObserverableBooleanInputInverter(IObservableBooleanInput source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _source.ValueChanged += SourceValueChangedHandler;
        }

        /// <summary>
        /// Gets the inverted value of the source input passed to the constructor.
        /// </summary>
        public bool Value
        {
            get
            {
                return !_source.Value;
            }
        }

        /// <summary>
        /// Handles the _source <see cref="IObservableBooleanInput.ValueChanged"/> event.
        /// </summary>
        /// <param name="sender">The object raising the event (that is, <see cref="_source"/>).</param>
        /// <param name="newValue">The new value to which <see cref="_source"/> has changed.</param>
        private void SourceValueChangedHandler(object sender, bool newValue)
        {
            // Raise the event with the inverted value.
            OnValueChanged(!newValue);
        }
    }

    #endregion

    #region Boolean Operators

    /// <summary>
    /// The base class for <see cref="IBooleanInput"/> objects combining other <see cref="IBooleanInput"/> objecs with
    /// binary operators.
    /// </summary>
    public abstract class BooleanOperatorInputBase : IBooleanInput
    {
        /// <summary>
        /// The source inputs to be operated on.
        /// </summary>
        private IBooleanInput[] _sourceInputs;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        protected BooleanOperatorInputBase(params IBooleanInput[] sourceInputs)
        {
            if (sourceInputs == null || sourceInputs.Length == 0)
            {
                throw new ArgumentNullException(nameof(sourceInputs));
            }
            foreach (var input in sourceInputs)
            {
                if (input == null)
                {
                    throw new ArgumentException("sourceInputs must not contain empty elements.");
                }
            }
            _sourceInputs = new IBooleanInput[sourceInputs.Length];
            sourceInputs.CopyTo(_sourceInputs, 0);
        }

        /// <summary>
        /// Gets the source inputs to be operated on.
        /// </summary>
        protected IBooleanInput[] SourceInputs
        {
            get
            {
                return _sourceInputs;
            }
        }

        /// <summary>
        /// Gets (reads) the value.
        /// </summary>
        public abstract bool Value { get; }
    }

    /// <summary>
    /// An <see cref="IBooleanInput"/> combinding several other <see cref="IBooleanInput"/> objects using AND.
    /// </summary>
    public class BooleanAndInput : BooleanOperatorInputBase
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        public BooleanAndInput(params IBooleanInput[] sourceInputs) : base(sourceInputs)
        {
        }

        /// <summary>
        /// Return true if all <see cref="SourceInputs"/> are true, otherwise false.
        /// </summary>
        public override bool Value
        {
            get
            {
                foreach (IBooleanInput input in SourceInputs)
                {
                    if (!input.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }

    /// <summary>
    /// An <see cref="IBooleanInput"/> combinding several other <see cref="IBooleanInput"/> objects using OR.
    /// </summary>
    public class BooleanOrInput : BooleanOperatorInputBase
    {
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="sourceInputs">The inputs to be operated on.</param>
        public BooleanOrInput(params IBooleanInput[] sourceInputs) : base(sourceInputs)
        {
        }

        /// <summary>
        /// Return true if at least one of the <see cref="SourceInputs"/> is true, otherwise false.
        /// </summary>
        public override bool Value
        {
            get
            {
                foreach (IBooleanInput input in SourceInputs)
                {
                    if (input.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    #endregion

    #region Double-valued Transformations

    public class ScaleToRangeInput : IDoubleInput
    {
        private IDoubleInput _source;
        private double _minimum, _maximum, _sourceMinimum, _sourceMaximum;

        public ScaleToRangeInput(IDoubleInput source, double minimum, double maximum)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (minimum >= maximum) { throw new ArgumentException(); }
            _source = source;
            _minimum = minimum;
            _maximum = maximum;
            _sourceMinimum = double.MaxValue;
            _sourceMaximum = double.MinValue;
        }

        public double Value
        {
            get
            {
                double sourceValue = _source.Value;

                if (sourceValue < _sourceMinimum) { _sourceMinimum = sourceValue; }
                if (sourceValue > _sourceMaximum) { _sourceMaximum = sourceValue; }

                double result;
                double interval = _sourceMaximum - _sourceMinimum;

                if (interval > 0.0)
                {
                    result = (sourceValue - _sourceMinimum) / interval * (_maximum - _minimum) + _minimum;
                }
                else
                {
                    result = (_maximum + _minimum) / 2.0;
                }
                if (result < _minimum)
                {
                    result = _minimum;
                }
                else if (result > _maximum)
                {
                    result = _maximum;
                }
                return result;
            }
        }
    }

    public class SchmittTriggerInput : IBooleanInput
    {
        private IDoubleInput _sourceInput;
        private double _lowLimit, _highLimit;
        bool _currentValue;

        public SchmittTriggerInput(IDoubleInput sourceInput, double triggerValue, double hysteresis)
        {
            if (sourceInput == null) { throw new ArgumentNullException(nameof(sourceInput)); }
            if (hysteresis < 0.0) { throw new ArgumentOutOfRangeException(nameof(hysteresis)); }
            _sourceInput = sourceInput;
            hysteresis = hysteresis / 2.0;
            _lowLimit = triggerValue - hysteresis;
            _highLimit = triggerValue + hysteresis;
        }

        public bool Value
        {
            get
            {
                double value = _sourceInput.Value;

                if (value < _lowLimit)
                {
                    _currentValue = false;
                }
                else if (value > _highLimit)
                {
                    _currentValue = true;
                }
                return _currentValue;
            }
        }
    }

    #endregion

    #region Extension methods

    /// <summary>
    /// This class contains convenient extension methods for abstract I/O objects that make it possibly to easily chain
    /// converters using a fluent language.
    /// </summary>
    public static class InputConverterExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="BooleanInputInverter"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IBooleanInput Invert(this IBooleanInput source)
        {
            return new BooleanInputInverter(source);
        }

        /// <summary>
        /// Creates a <see cref="IObservableBooleanInput"/> using the specified source input.
        /// </summary>
        /// <param name="source">The input which shall be inverted.</param>
        /// <returns>The inverted input.</returns>
        /// <remarks>For instance, if you have an <see cref="IBooleanInput"/> object named "input", you can just code
        /// input.Invert() to get an inverted version of input.</remarks>
        public static IObservableBooleanInput Invert(this IObservableBooleanInput source)
        {
            return new ObserverableBooleanInputInverter(source);
        }

        public static void WaitFor(this IBooleanInput port, bool value)
        {
            while (port.Value != value)
            {
                Thread.Sleep(1);
            }
        }

        public static ScaleToRangeInput ScaleToRange(this IDoubleInput source, double minimum, double maximum)
        {
            return new ScaleToRangeInput(source, minimum, maximum);
        }

        public static SchmittTriggerInput SchmittTrigger(this IDoubleInput source, double triggerValue, double hysteresis)
        {
            return new SchmittTriggerInput(source, triggerValue, hysteresis);
        }
    }

    #endregion
}
