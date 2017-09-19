using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccelerationAndGyro;

namespace AccelerationAndGyroTests
{
    [TestClass]
    public class RotationAnglesTests
    {
        [TestMethod]
        public void UpdateGravity_Should_ResetEulerAngles()
        {
            var angles = new RotationAngles(-1, -1, -1);

            var sensors = new AccelerationAndGyroModel()
            {
                AccelerationX = 0.75f,
                AccelerationY = 0f,
                AccelerationZ = 0.75f,
                GyroX = 0f,
                GyroY = 0f,
                GyroZ = 0f
            };

            angles.UpdateFromGravity(sensors);

            Assert.IsTrue(angles.Pitch == 359);
            Assert.IsTrue(angles.Roll == 45);
            Assert.IsTrue(angles.Yaw == 359);
        }
    }
}
