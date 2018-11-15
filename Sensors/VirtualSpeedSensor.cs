using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    class VirtualSpeedSensor : SpeedSensorInterface
    {
        RandomGenerator random;

        public VirtualSpeedSensor(RandomGenerator random)
        {
            this.random = random;
        }
        public void setSpeed(decimal Speed)
        { }

        public decimal getSpeed()
        {

            return random.getRan(1, 50);
        }

        public string toJson()
        {
            return "{\"Speed\":\"" + getSpeed() + "\"}";
        }
    }
}
