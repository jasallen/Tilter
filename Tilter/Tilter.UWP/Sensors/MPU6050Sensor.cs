using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccelerationAndGyro;

namespace Tilter.UWP
{
    class MPU6050Sensor : IAccelerationAndGyroSensor, IDisposable
    {
        //uncomment to record sample data for fakeSensor
        //static List<AccelerationAndGyroViewModel> sampleDataRecorder = new List<AccelerationAndGyroViewModel>();
        //static DateTime sampleDataRecordStart = DateTime.Now;

        public event EventHandler<AccelerationAndGyroViewModel> NewSensorReading;

        private MPU6050 MPU6050 { get; } = new MPU6050();
        
        public MPU6050Sensor()
        {
            MPU6050.Init();
            //Init is fire and forget (not awaited), use code like below to handle logging if it fails.
            //.ContinueWith((t) => { }, TaskContinuationOptions.NotOnRanToCompletion)

            MPU6050.SensorInterruptEvent += MPU6050_SensorInterruptEvent;
        }

        private void MPU6050_SensorInterruptEvent(object sender, MpuSensorEventArgs e)
        {
            var mpuV = e.Values.First();

            var vm = new AccelerationAndGyroViewModel()
            {
                AccelerationX = mpuV.AccelerationX,
                AccelerationY = mpuV.AccelerationY,
                AccelerationZ = mpuV.AccelerationZ,
                GyroX = mpuV.GyroX,
                GyroY = mpuV.GyroY,
                GyroZ = mpuV.GyroZ,
                SamplePeriod = e.SamplePeriod
            };

            NewSensorReading?.Invoke(this, vm);

            //uncomment to record sample data for fakeSensor
            //sampleDataRecorder?.Add(vm);
            //if (sampleDataRecorder != null && TimeSpan.FromSeconds(10) < DateTime.Now - sampleDataRecordStart)
            //{
            //    var path = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sampleData2.txt");
            //    var data = Newtonsoft.Json.JsonConvert.SerializeObject(sampleDataRecorder);

            //    System.IO.File.WriteAllText(path, data);
                    
            //    sampleDataRecorder = null;
            //}
        }

        bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (MPU6050 != null)
                {
                    MPU6050.Dispose();
                }
                disposed = true;
            }
        }

        ~MPU6050Sensor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
