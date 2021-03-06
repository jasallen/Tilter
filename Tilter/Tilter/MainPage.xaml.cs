﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tilter
{
    public partial class MainPage : ContentPage
    {
        RotationAngles angles;

        IAccelerationAndGyroSensor sensor { get; } = new MPU6050Sensor();

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

        DispatcherTimer dispatcherTimer;


        async Task StartAccelAndGyroSensors()
        {
            //todo deal with RotationAngles instance being used on multiple threads
            sensor.NewSensorReading += Sensor_NewSensorReading;

            //optional update screen at fixed interval, updates from sensor can be too frequent
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += UpdateRotationAnglesToScreen; ;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            dispatcherTimer.Start();
        }

        private void Sensor_NewSensorReading(object sender, AccelerationAndGyroViewModel e)
        {
            angles.UpdateFromGravity(e);
        }

        private void UpdateRotationAnglesToScreen(object sender, object e)
        {
            PlaneTransform.TranslateX = (((angles.Yaw * 2000 / 360) + 1000) % 2000) - 1000;
            PlaneProjection.RotationX = angles.Pitch;
            PlaneProjection.RotationZ = angles.Roll;

            //Debug.WriteLine($"roll:{angles.Roll};");
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
