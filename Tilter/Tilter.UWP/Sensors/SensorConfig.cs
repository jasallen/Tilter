using AccelerationAndGyro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tilter.XF;

namespace Tilter.UWP.Sensors
{
   class SensorConfig : CommonSensorConfig
    {
        public static IAccelerationAndGyroSensor GetMPU6050Sensor()
        {
            IAccelerationAndGyroSensor sensor = new MPU6050Sensor();

            var config = new SensorConfig(); //the lifetime of the instance is tied to the sensor

            sensor.NewSensorReading += config.PublishToHub;
            sensor.NewSensorReading += config.PublishToSignalR;

            return sensor;
        }

    }
}
