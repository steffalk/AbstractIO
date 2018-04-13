using System;
using Windows.Devices.Gpio;

namespace AbstractIO.Netduino3
{
    /// <summary>
    /// A base class for digital input/output objects of a Netduino 3 board.
    /// </summary>
    public abstract class DigitalInputOutputBase : DisposableResourceBase
    {
        /// <summary>
        /// The underlying GPIO pin used by this object.
        /// </summary>
        private GpioPin _pin;

        /// <summary>
        /// Checks whether a GpioPinDriveMode is suitable for input.
        /// </summary>
        /// <param name="mode">The mode to check.</param>
        /// <returns>The mode if it is valid for input. Otherwise, an exception gets thrown.</returns>
        public static GpioPinDriveMode CheckInputMode(GpioPinDriveMode mode)
        {
            if (mode < GpioPinDriveMode.Output)
            {
                return mode;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        /// Checks whether a GpioPinDriveMode is suitable for output.
        /// </summary>
        /// <param name="mode">The mode to check.</param>
        /// <returns>The mode if it is valid for output. Otherwise, an exception gets thrown.</returns>
        public static GpioPinDriveMode CheckOutputMode(GpioPinDriveMode mode)
        {
            if (mode >= GpioPinDriveMode.Output)
            {
                return mode;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        /// <summary>
        /// Creates an instance using the default GpioController.
        /// </summary>
        /// <param name="pin">The internal pin number to use.</param>
        /// <param name="driveMode">The mode of the pin. This must be valid for the intended operation (input or
        /// output).</param>
        protected DigitalInputOutputBase(
            int pin,
            GpioPinDriveMode driveMode)
        {
            _pin = GpioController.GetDefault().OpenPin(pin);
            _pin.SetDriveMode(driveMode);
        }

        /// <summary>
        /// Gets the pin used.
        /// </summary>
        protected GpioPin Pin
        {
            get
            {
                return _pin;
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
