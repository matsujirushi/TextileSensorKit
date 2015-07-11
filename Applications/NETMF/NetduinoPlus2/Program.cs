using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace MjTextileSensor_Hello
{
    public class Program
    {
        public static void Main()
        {
            var i2c = new I2CDevice(new I2CDevice.Configuration(0x42, 400));
            var sensor = new MjTextileSensor.MjTextileSensor(i2c);

            byte majorVersion;
            byte minorVersion;
            sensor.GetVersion(out majorVersion, out minorVersion);
            Debug.Print("Firmware version is " + majorVersion + "." + minorVersion + ".");

            var values = new byte[10];
            for (; ; )
            {
                int valuesCount = sensor.GetSensorValues(values);
                if (valuesCount != 10)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                var message = "";
                for (int i = 0; i < valuesCount; i++)
                {
                    message += values[i].ToString() + " ";
                }
                Debug.Print(message);

                Thread.Sleep(1000);
            }
        }

    }
}
