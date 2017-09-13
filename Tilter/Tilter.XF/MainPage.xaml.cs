using AccelerationAndGyro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tilter
{
    public partial class MainPage : ContentPage
    {
        RotationAngles angles;

        IAccelerationAndGyroSensor sensor { get; } = DependencyService.Get<IAccelerationAndGyroSensor>();

        ObservableCollection<string> output = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();

            angles = new RotationAngles(-1, -1, -1);
            angles.gyroXNoiseCorrect = -4.84 * .02;
            angles.gyroYNoiseCorrect = 2.35 * .02;
            angles.gyroZNoiseCorrect = -1.03 * .02;

            listView.ItemsSource = output;

            StartAccelAndGyroSensors();
        }
        
        async Task StartAccelAndGyroSensors()
        {
            //todo deal with RotationAngles instance being used on multiple threads
            sensor.NewSensorReading += Sensor_NewSensorReading;

            //update screen at fixed interval, updates from sensor can be too frequent
            //for cool future way to handle these situations check out async streams coming in c#8
            Device.StartTimer(
                new TimeSpan(0, 0, 0, 0, 30),
                UpdateRotationAnglesToScreen);
            
        }

        private void Sensor_NewSensorReading(object sender, AccelerationAndGyroViewModel e)
        {
            angles.UpdateFromGravity(e);
        }

        private bool UpdateRotationAnglesToScreen()
        {
            Plane.TranslationX = (((angles.Yaw * 2000 / 360) + 1000) % 2000) - 1000;
            Plane.RotationX = angles.Pitch;
            Plane.Rotation = angles.Roll;

            return true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            output.Add($"pitch: {angles.Pitch} yaw: {angles.Yaw} roll: {angles.Roll} ");

            var sensor = angles?.AccelAndGyro;

            if (sensor != null)
            {
                output.Add($"gravX:{sensor.AccelerationX} gravY:{sensor.AccelerationY} gravZ:{sensor.AccelerationZ}");
                output.Add($"gyroX:{sensor.GyroX} gyroY:{sensor.GyroY} gyroZ:{sensor.GyroZ}");
            }
        }
    }
}
