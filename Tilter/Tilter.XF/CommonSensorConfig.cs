using AccelerationAndGyro;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tilter.XF
{
    public class CommonSensorConfig
    {
        public static IAccelerationAndGyroSensor GetVirtualSensor()
        {
            IAccelerationAndGyroSensor sensor = new VirtualAccelerationAndGyroSensorFromAzure(AzureSignalRConfig.EndPoint, AzureSignalRConfig.HubName, AzureIoTHubConfig.DeviceId);

            var config = new CommonSensorConfig(); //the lifetime of the instance is tied to the sensor

            sensor.NewSensorReading += config.PublishToHub;
            sensor.NewSensorReading += config.PublishToSignalR;

            return sensor;
        }

        DateTime LastAzurePush = DateTime.Now - TimeSpan.FromSeconds(10);

        Lazy<HandleAccelAndGyroPushToAzure> azurePusher
            = new Lazy<HandleAccelAndGyroPushToAzure>(() => new HandleAccelAndGyroPushToAzure(AzureIoTHubConfig.DeviceId, AzureIoTHubConfig.DeviceKey, AzureIoTHubConfig.IotHubUri));

        Lazy<IHubProxy> hubProxy
            = new Lazy<IHubProxy>(createHubProxy);

        private static IHubProxy createHubProxy()
        {
            var hub = new HubConnection(AzureSignalRConfig.EndPoint);
            var ret = hub.CreateHubProxy(AzureSignalRConfig.HubName);
            hub.Start().Wait();
            return ret;
        }

        public void PublishToSignalR(object sender, AccelerationAndGyroModel e)
        {
            hubProxy.Value.Invoke("DeviceDataReceived", JsonConvert.SerializeObject(e), AzureIoTHubConfig.DeviceId);
        }

        public void PublishToHub(object sender, AccelerationAndGyroModel e)
        {
            lock (azurePusher)
            {
                if (DateTime.Now - LastAzurePush > TimeSpan.FromSeconds(1))
                {
                    azurePusher.Value.SendDeviceToCloudSensorDataAsync(e);
                    LastAzurePush = DateTime.Now;
                }
            }
        }


    }
}
