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
            Send.msmq = queue;
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

                Console.WriteLine("Added to queue : \n {0}", JsonConvert.SerializeObject(x));

                Thread.Sleep(1000);
            }
        }
    }
    class Send{
        public static MessageQueuing msmq;
       
        public static void sendData()
        {
            while (true)
            {
                Message m = msmq.messageQueue.Receive();
                m.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                Console.WriteLine("Message popped from queue: {0} ", m.Body);
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.16:8080/data");
                    httpWebRequest.ContentType = "text/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                            streamWriter.Write(m.Body);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Console.Out.WriteLine(httpResponse.StatusCode);  
                    }catch(Exception e) {
                        msmq.addMessage(m.Body.ToString());
                    }
                }
            } 
    }
}



