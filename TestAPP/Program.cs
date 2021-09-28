using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Gpio;

namespace TestAPP
{
    public class Program
    {
        private const int _pinNumberShockDetection = 21;
        private static GpioPin _shockDetection;
        private static AP.NanoFrameWork.Vibration.HDX vibrationObject;

        public static void Main()
        {
            Debug.WriteLine("In The Name Of God");

            var s_GpioController = new GpioController();

            _shockDetection = s_GpioController.OpenPin(_pinNumberShockDetection);
            _shockDetection.SetDriveMode(GpioPinDriveMode.Input);
            _shockDetection.ValueChanged += _shockDetection_ValueChanged;

            Thread.Sleep(100);

            vibrationObject = new AP.NanoFrameWork.Vibration.HDX();
            vibrationObject.Threshold = 20;
            vibrationObject.InvokeEvent += VibrationObject_InvokeEvent;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void _shockDetection_ValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            vibrationObject.AnalyseSignal(e.Edge);
        }

        private static void VibrationObject_InvokeEvent(object sender, EventArgs e)
        {
            Debug.WriteLine("Vibrate!!!!!!");
        }
    }
}
