using System;
using System.Collections.Generic;
using Client.Sensors;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //init sensors
            MessageQueuing queue = new MessageQueuing();
            RandomGenerator random = new RandomGenerator();
            TemperatureSensorInterface temp = new VirtualTemperatureSensor(random);
            SpeedSensorInterface speed = new VirtualSpeedSensor(random);
            GPSSensorInterface gps = new VirtualGPSSensor(random);
            DirectionSensorInterface dir = new VirtualDirectionSensor();
          
            while (true)
            {
                long epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var x = new
                {
                    Temperature = temp.getTemperature(),
                    Speed = speed.getSpeed(),
                    Gps = new
                    {
                        lat = gps.getLat(),
                        lon = gps.getLon()
                    },
                    Direction = dir.getDirection(),
                    Timestamp = epoch
                    };

                queue.addMessage(JsonConvert.SerializeObject(x));
/*
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.101.53:8080/data");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                
                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    streamWriter.Write(JsonConvert.SerializeObject(x));
                                }
                                

                  var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                  Console.Out.WriteLine(httpResponse.StatusCode);  */

                  Console.WriteLine("Added to queue : \n {0}",JsonConvert.SerializeObject(x));
                 
                  System.Threading.Thread.Sleep(1000);
            }

        }

    }

}
