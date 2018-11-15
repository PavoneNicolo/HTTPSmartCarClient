using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Sensors
{
    interface SpeedSensorInterface
    {
        void setSpeed(decimal speed);
        decimal getSpeed();
        string toJson();
    }
}
