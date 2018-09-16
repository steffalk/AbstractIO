using System;
using Windows.Devices.Pwm;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// PWM output on any supported digital output pin or LED on the Netduino 3.
    /// </summary>
    public class AnalogPwmOutput : DisposableResourceBase, ISingleOutput
    {
        /// <summary>
        /// The default <see cref="PwmFrequency"/> used.
        /// </summary>
        public const double DefaultPwmFrequency = 250.0;

        /// <summary>
        /// The PWM controller used by all instances of this class.
        /// </summary>
        private static PwmController _controller;

        /// <summary>
        /// The underlying PWM pin used by this object.
        /// </summary>
        private PwmPin _pin;

        /// <summary>
        /// The current <see cref="Value"/> of this object.
        /// </summary>
        private float _value = 0.0f;

        /// <summary>
        /// Initializes the PwmController instance used by every instance of this class.
        /// </summary>
        /// <param name="pwmFrequency"></param>
        private static void CreateController(double pwmFrequency)
        {
            _controller = PwmController.FromId("TIM1");
            _controller.SetDesiredFrequency(pwmFrequency);
        }

        /// <summary>
        /// Gets the actual PWM frequency used, or sets the desired PWM frequency.
        /// </summary>
        /// <remarks>
        /// You may set this property only once, and only before creating the first object instance of this class. All
        /// objects created from this class will use the frequency set prior to the first object creation. Setting this
        /// property afterwards will throw an <see cref="InvalidOperationException"/>. Reading this property will return
        /// the <see cref="DefaultPwmFrequency"/> before the first time this property gets set or the first instance
        /// gets created. After setting this property or creating the first class instance, this property will return
        /// the actual PWM frequency used by the hardware, which may be different from the
        /// <see cref="DefaultPwmFrequency"/> or the frequency this property was set to.
        /// </remarks>
        public static double PwmFrequency
        {
            get
            {
                if (_controller == null)
                {
                    return DefaultPwmFrequency;
                }
                else
                {
                    return _controller.ActualFrequency;
                }
            }
            set
            {
                if (_controller != null)
                {
                    throw new InvalidOperationException(
                        "Setting PwmFrequency is only allowed once and before creation of the first " +
                        nameof(AnalogPwmOutput) + " instance.");
                }
                else if (value <= 0.0)
                {
                    throw new ArgumentOutOfRangeException(nameof(PwmFrequency));
                }
                else
                {
                    CreateController(value);
                }
            }
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="pin">The PWM-enabled pin to use.</param>
        public AnalogPwmOutput(DigitalPwmOutputPin pin)
        {
            if (_controller == null)
            {
                CreateController(PwmFrequency);
            }
            _pin = _controller.OpenPin((int)pin);
            _pin.SetActiveDutyCyclePercentage(0.0);
            _pin.Start();
        }

        /// <summary>
        /// Gets or sets the value of this pin in the range from 0.0 to 1.0. Setting this property to a value smaller
        /// than 0.0 will silently set it to 0.0, setting it to value greater than 1.0 will silently set it to 1.0, so
        /// that an invalid value will not cause an exception.
        /// </summary>
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value < 0.0f)
                {
                    _value = 0.0f;
                }
                else if (value > 1.0f)
                {
                    _value = 1.0f;
                }
                else
                {
                    _value = value;
                }
                _pin.SetActiveDutyCyclePercentage(_value);
            }
        }

        /// <summary>
        /// Disposes the underlying objects. This method will be called automatically and should not be called by user
        /// code, but inheritors should call the base method.
        /// </summary>
        protected override void DisposeResource()
        {
            if (_pin != null)
            {
                _pin.Dispose();
                _pin = null;
            }
        }
    }
}