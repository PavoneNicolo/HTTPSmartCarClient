using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    class VirtualTemperatureSensor : TemperatureSensorInterface
    {
        RandomGenerator random;

        public VirtualTemperatureSensor(RandomGenerator random)
        {
                this.random = random;
        }
        public void setTemperature(decimal temperature)
        { }

        public decimal getTemperature()
        {
            return this.random.getRan(1, 50);
        }

        public string toJson()
        {
            return "\"temperature\":\"" + getTemperature() + "\"";
        }
    }
}
