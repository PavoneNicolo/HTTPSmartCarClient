using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    class VirtualDirectionSensor : DirectionSensorInterface
    {
        Dictionary<int,String> dictionary = new Dictionary<int, string>();
        public VirtualDirectionSensor()
        {
            dictionary.Add(0, "N");
            dictionary.Add(1, "S");
            dictionary.Add(2, "E");
            dictionary.Add(3, "W");
            dictionary.Add(4, "NE");
            dictionary.Add(5, "NW");
            dictionary.Add(6, "SE");
            dictionary.Add(7, "SW");
        }
        public void setDirection(string direction)
        { }

        public string getDirection()
        {
            Random ran = new Random();
            return dictionary[ran.Next(0, 8)];
        }

        public string toJson()
        {
            return "{\"Direction\":\"" + getDirection() + "\"}";
        }
    }
}

