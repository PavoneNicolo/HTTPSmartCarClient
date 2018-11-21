using System;
using Client.Sensors;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Messaging;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //init sensors
            MessageQueuing queue = new MessageQueuing();
            ThreadStart threadDelegate = new ThreadStart(Send.sendData);
            
            Thread t = new Thread(threadDelegate);
            t.Start();
            RandomGenerator random = new RandomGenerator();
            TemperatureSensorInterface temp = new VirtualTemperatureSensor(random);
            SpeedSensorInterface speed = new VirtualSpeedSensor(random);
            GPSSensorInterface gps = new VirtualGPSSensor(random);
            DirectionSensorInterface dir = new VirtualDirectionSensor();
            while (true)
            {
                long epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                var Speed = speed.getSpeed();
                string s = "temperatura:" + temp.getTemperature();
                var x = new
                {
                    fields = new[] {
                        new { fldName = "temperatura", value = temp.getTemperature() },
                        new { fldName = "velocità", value = speed.getSpeed() },
                        new { fldName = "lat", value = gps.getLat() },
                        new { fldName = "lon", value = gps.getLon() }
                    },
                    tags = new[] {
                        new { fldName = "temperatura", value = temp.getTemperature() },
                        new { fldName = "velocità", value = speed.getSpeed() },
                        new { fldName = "lat", value = gps.getLat() },
                        new { fldName = "lon", value = gps.getLon() }
                    },
                    
                    direction = dir.getDirection(),
                    timestamp = epoch
                };

                queue.addMessage(JsonConvert.SerializeObject(x));

                Console.WriteLine("Added to queue : \n {0}", JsonConvert.SerializeObject(x));

                Thread.Sleep(1000);
            }
        }
    }
    class Send{

        public static MessageQueuing messageQueuing;
        public static void sendData()
        {
            messageQueuing = new MessageQueuing();
            while (true)
            {
                Message m = messageQueuing.messageQueue.Receive();
                m.Formatter = new XmlMessageFormatter(new string[] { "System.String,mscorlib" });
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.59:8080/v1/sensors/write");
                    //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.6:8080/data");
                    httpWebRequest.ContentType = "text/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                            streamWriter.Write(m.Body);
             
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Console.Out.WriteLine("Sent : {0}", m.Body.ToString()); 
                    }catch(Exception e) {
                        Console.WriteLine("Waiting for an internet connection to send data...");
                        messageQueuing.messageQueue.Send(m.Body.ToString());
                    }
                }
            } 
    }
}



