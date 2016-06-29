using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace csConsoleApplicationREST
{
    class Program
    {
        static void Main(string[] args)
        {

            string url = "http://localhost:1201/fiskaltrust/POS";


            echoJson(url);

            echoXml(url);

            signJson(url);

            signXml(url);
           

            Console.ReadLine();
        }

        static void signXml( string url)
        {
            var reqdata = getReceiptRequest();

            var ms = new System.IO.MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(fiskaltrust.ifPOS.v0.ReceiptRequest));

            serializer.WriteObject(ms, reqdata);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/xml/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/xml;charset=utf-8";

            webreq.ContentLength = ms.Length;
            using (var reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(ms.ToArray(), 0, (int)ms.Length);
            }

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(fiskaltrust.ifPOS.v0.ReceiptResponse));
                ms = new System.IO.MemoryStream();
                webresp.GetResponseStream().CopyTo(ms);

                Console.WriteLine("{0:G} Sign response {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

                ms.Position = 0;
                fiskaltrust.ifPOS.v0.ReceiptResponse resp = (fiskaltrust.ifPOS.v0.ReceiptResponse)serializer.ReadObject(ms);
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }





        }


        static void signJson(string url)
        {
            var reqdata = getReceiptRequest();
            var jsonSettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            var reqjson = JsonConvert.SerializeObject(reqdata, jsonSettings);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, reqjson);

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";
            byte[] reqecho = Encoding.UTF8.GetBytes(reqjson);
            webreq.ContentLength = reqecho.Length;
            using (var reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(reqecho, 0, reqecho.Length);
            }

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                using (var respReader = new System.IO.StreamReader(webresp.GetResponseStream(), Encoding.UTF8))
                {
                    var respdata = JsonConvert.DeserializeObject<fiskaltrust.ifPOS.v0.ReceiptResponse>(respReader.ReadToEnd(), jsonSettings);
                    var respjson = JsonConvert.SerializeObject(respdata, jsonSettings);

                    Console.WriteLine("{0:G} Sign response {1}", DateTime.Now, respjson);
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }

        }

        static void echoJson(string url)
        {

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/echo");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";
            byte[] reqecho = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("Hello World"));
            webreq.ContentLength = reqecho.Length;
            using (var reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(reqecho, 0, reqecho.Length);
            }

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                using (var respReader = new System.IO.StreamReader(webresp.GetResponseStream(), Encoding.UTF8))
                {
                    var respecho = JsonConvert.DeserializeObject<string>(respReader.ReadToEnd());
                    Console.WriteLine("{0:G} Echo {1}", DateTime.Now, respecho);
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }

        }

        static void echoXml(string url)
        {
            string reqdata = "Hello World!";

            var ms = new System.IO.MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(string));

            serializer.WriteObject(ms, reqdata);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/xml/echo");
            webreq.Method = "POST";
            webreq.ContentType = "application/xml;charset=utf-8";



            webreq.ContentLength = ms.Length;
            using (var reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(ms.ToArray(), 0, (int)ms.Length);
            }

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                ms = new System.IO.MemoryStream();
                webresp.GetResponseStream().CopyTo(ms);

                Console.WriteLine("{0:G} Echo {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

                ms.Position = 0;
                string resp = (string)serializer.ReadObject(ms);
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }


            /*
            var ms = new System.IO.MemoryStream();

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(string));
            serializer.Serialize(ms, reqdata);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/xml/echo");
            webreq.Method = "POST";
            webreq.ContentType = "application/xml;charset=utf-8";



            webreq.ContentLength = ms.Length;
            using (var reqStream = webreq.GetRequestStream())
            {
                reqStream.Write(ms.ToArray(), 0, (int)ms.Length);
            }

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                using (var respReader = new System.IO.StreamReader(webresp.GetResponseStream(), Encoding.UTF8))
                {
                    string respxml = respReader.ReadToEnd();

                    //var respecho = JsonConvert.DeserializeObject<string>(respReader.ReadToEnd());
                    Console.WriteLine("{0:G} Echo {1}", DateTime.Now, respxml);
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }

            */
        }

        static fiskaltrust.ifPOS.v0.ReceiptRequest getReceiptRequest()
        {
            var reqdata = new fiskaltrust.ifPOS.v0.ReceiptRequest()
            {
                ftCashBoxID = "fiskaltrust-TEST",
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000000,
                cbReceiptReference = Guid.NewGuid().ToString(),
                cbReceiptMoment = DateTime.Now,
                cbChargeItems = new fiskaltrust.ifPOS.v0.ChargeItem[]  {
                    new fiskaltrust.ifPOS.v0.ChargeItem()
                    {
                        ftChargeItemCase=0x4154000000000000,
                         ProductNumber="1",
                         Description="Artikel 1",
                         Quantity=1.0m,
                         VATRate=20.0m,
                         Amount=4.8m
                    },
                    new fiskaltrust.ifPOS.v0.ChargeItem()
                    {
                        ftChargeItemCase=0x4154000000000000,
                        ProductNumber="2",
                        Description="Artikel 2",
                        Quantity=1.0m,
                        VATRate=20.0m,
                        Amount=3.6m
                    }
                },
                cbPayItems = new fiskaltrust.ifPOS.v0.PayItem[]                {
                    new fiskaltrust.ifPOS.v0.PayItem()
                    {
                        ftPayItemCase=0x4154000000000000,
                        Amount=8.4m,
                        Quantity=1.0m,
                        Description="Bar"
                    }
                }
            };

            return reqdata;
        }

    }
}
