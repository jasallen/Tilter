using AccelerationAndGyro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Tilter.GTK
{
    class Program
    {
        static void Main(string[] args)
        {
            Xamarin.Forms.DependencyService.Register<IAccelerationAndGyroSensor, FakeAccelerometerAndGyro>();

            Gtk.Application.Init();
            Forms.Init();

            var app = new Tilter.App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Tilter");
            window.Show();
            Gtk.Application.Run();
        }
    }
}
