using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    interface DirectionSensorInterface
    {
        void setDirection(string Direction);
        string getDirection();
        string toJson();
    }
}
