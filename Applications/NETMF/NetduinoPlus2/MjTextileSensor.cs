using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace MjTextileSensor
{
    class MjTextileSensor
    {
        public MjTextileSensor(I2CDevice i2c)
        {
            this.i2c = i2c;
        }

        public bool GetVersion(out byte majorVersion, out byte minorVersion)
        {
            var command = new byte[1];
            command[0] = 0x00;
            var writeAction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(command),
            };
            i2c.Execute(writeAction, 1000);

            var response = new byte[3];
            var readAction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateReadTransaction(response),
            };
            i2c.Execute(readAction, 1000);

            if (response[0] != 0x80)
            {
                majorVersion = 0;
                minorVersion = 0;
                return false;
            }

            majorVersion = response[2];
            minorVersion = response[1];
            return true;
        }

        public int GetSensorValues(byte[] sensorValues)
        {
            var response = new byte[11];
            var readAction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateReadTransaction(response),
            };
            i2c.Execute(readAction, 1000);

            if (response[0] != 0xff)
            {
                for (int i = 0; i < sensorValues.Length; i++)
                {
                    sensorValues[i] = 0;
                }
                return 0;
            }

            int sensorValuesSizeActual = sensorValues.Length < 10 ? sensorValues.Length : 10;
            Array.Copy(response, 1, sensorValues, 0, sensorValuesSizeActual);

            return sensorValuesSizeActual;
        }

        private I2CDevice i2c;

    }
}
