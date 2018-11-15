using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    class VirtualGPSSensor : GPSSensorInterface
    {
        RandomGenerator random;
        public VirtualGPSSensor(RandomGenerator random)
        {
            this.random = random;
        }
        public void setLat(decimal Lat)
        { }
        public void setLon(decimal Lon)
        { }

        public decimal getLat()
        {
            return random.getRan(-90, 90);
        }
        public decimal getLon()
        {
            return random.getRan(-180, 180);
        }
        public string toJson()
        {
            return " GPS: {\"Latitude\":\"" + getLat() + "\", \"Longitude\":\"" + getLon() + "\"}";
        }

    
    }
}
