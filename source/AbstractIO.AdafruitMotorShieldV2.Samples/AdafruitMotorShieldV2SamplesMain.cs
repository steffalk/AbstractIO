//#define Sample01LetMotorRun
#define Sample02LetManyMotorsRun

namespace AbstractIO.AdafruitMotorShieldV2.Samples
{
    public static class AdafruitMotorShieldV2SamplesMain
    {
        public static void Main()
        {

#if Sample01LetMotorRun

            // Connect to the Adafruit V2 shield at its default address:
            var shield = new AdafruitMotorShieldV2();

            // Use the sample controlling a lamp just control a motor, as both implement IDoubleOutput:

            AbstractIO.Samples.Sample02SmoothBlinker.Run(
                lamp: shield.GetDcMotor(1));

#elif Sample02LetManyMotorsRun

            // Control 8 motors on 2 motor shields simultaneously:

            var shield1 = new AdafruitMotorShieldV2(96);
            var shield2 = new AdafruitMotorShieldV2(97);

            AbstractIO.Samples.Sample08SmoothManyAnalogOutputs.Run(
                 shield1.GetDcMotor(1), shield1.GetDcMotor(2), shield1.GetDcMotor(3), shield1.GetDcMotor(4),
                 shield2.GetDcMotor(1), shield2.GetDcMotor(2), shield2.GetDcMotor(3), shield2.GetDcMotor(4));

#else
#error Please uncomment exactly one sample.
#endif
        }
    }
}
