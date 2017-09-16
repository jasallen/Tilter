using AccelerationAndGyro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Tilter
{
    public partial class App : Application
    {
        public static Func<IAccelerationAndGyroSensor> GetGyroSensor { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new Tilter.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
