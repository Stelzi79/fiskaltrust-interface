using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Newtonsoft.Json;
using fiskaltrust.ifPOS.v0;

namespace csConsoleApplicationREST
{
    class Program
    {
        static void Main(string[] args)
        {

            //signaturcloud 1 ef
            string url = "https://signaturcloud-sandbox.fiskaltrust.at/";
            Guid cashboxid = Guid.Parse("654fd35d-75eb-49e2-90cd-2980c67350cf");
            string accesstoken = "BIlyGYZPJXOnwx+MyVekpAFXOjpPFim5+U8EHxuKbOcF/CAA6kSNg1Im+mZstNiIuT0F946RbRPRTO+H6Ngq9q8=";

            //signaturcloud 2 ef
            //string url = "https://signaturcloud-sandbox.fiskaltrust.at/";
            //Guid cashboxid = Guid.Parse("555881f6-4c89-4265-ba39-01f109456d57");
            //string accesstoken = "BGYpiOMteelPfdOIjBzPd1Ny6lUvz4tSUmBgOypYPtKc8f5mDo7tNuTlp34xhddoxGX3XFWWMQH9Br+5i165rd0=";



            echoJson(url, cashboxid, accesstoken);

            echoXml(url, cashboxid, accesstoken);

            signJson(url, cashboxid, accesstoken);

            signXml(url, cashboxid, accesstoken);

            //signJsonStartRequest(url, cashboxid, accesstoken); //Send Start receipt to activate Queue for the first time

            signJsonZeroReceipt(url, cashboxid, accesstoken); //Send Zero receipt 

            signJson(url, cashboxid, accesstoken);

            journalJson(url, cashboxid, accesstoken);

            Console.ReadKey();
        }




        static void echoJson(string url, Guid cashboxid = default(Guid), string accesstoken = null)
        {

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/echo");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);

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
                    var json = respReader.ReadToEnd();
                    var respecho = JsonConvert.DeserializeObject<string>(json);
                    Console.WriteLine("{0:G} Echo {1}", DateTime.Now, respecho);
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }
        }

        static void echoXml(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            string reqdata = "Hello World!";

            var ms = new System.IO.MemoryStream();
            var serializer = new DataContractSerializer(typeof(string));

            serializer.WriteObject(ms, reqdata);
            Console.WriteLine("{0:G} Request {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/xml/echo");
            webreq.Method = "POST";
            webreq.ContentType = "application/xml;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);


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

        static void signXml(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            var reqdata = UseCase17Request(1, 5, 5, cashboxid.ToString());

            var ms = new System.IO.MemoryStream();
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(fiskaltrust.ifPOS.v0.ReceiptRequest));

            serializer.WriteObject(ms, reqdata);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, Encoding.UTF8.GetString(ms.ToArray()));

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/xml/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/xml;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);

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


        static void signJson(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            var reqdata = UseCase17Request(1, 5, 5, cashboxid.ToString());

            var jsonSettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            var reqjson = JsonConvert.SerializeObject(reqdata, jsonSettings);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, reqjson);

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);


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


        static void signJsonStartRequest(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            var reqdata = StartRequest(2, cashboxid.ToString());

            var jsonSettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            var reqjson = JsonConvert.SerializeObject(reqdata, jsonSettings);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, reqjson);

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);


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

        static void signJsonZeroReceipt(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            var reqdata = ZeroReceiptRequest(2, cashboxid.ToString());

            var jsonSettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            var reqjson = JsonConvert.SerializeObject(reqdata, jsonSettings);
            Console.WriteLine("{0:G} Sign request zero receipt {1}", DateTime.Now, reqjson);

            var webreq = (HttpWebRequest)HttpWebRequest.Create(url + "/json/sign");
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);


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

                    Console.WriteLine("{0:G} Sign response zero receipt {1}", DateTime.Now, respjson);
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }

        }


        static void journalJson(string url, Guid cashboxid = default(Guid), string accesstoken = "00000000")
        {
            Console.WriteLine("{0:G} Journal request", DateTime.Now);

            var webreq = (HttpWebRequest)HttpWebRequest.Create(String.Format("{0}/json/journal?type={1}&from=0&to=0", url,Convert.ToInt64("0x4154000000000001", 16)));
            webreq.Method = "POST";
            webreq.ContentType = "application/json;charset=utf-8";
            webreq.ContentLength = 0;

            webreq.Headers.Add("cashboxid", cashboxid.ToString());
            webreq.Headers.Add("accesstoken", accesstoken);

            var webresp = (HttpWebResponse)webreq.GetResponse();
            if (webresp.StatusCode == HttpStatusCode.OK)
            {
                using (var respStream = webresp.GetResponseStream())
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    Console.WriteLine("{0:G} journal response len {1}", DateTime.Now, text.Length); // to show journal text use text instead of text.length
                }
            }
            else
            {
                Console.WriteLine("{0:G} {1} {2}", DateTime.Now, webresp.StatusCode, webresp.StatusDescription);
            }
        }


        internal static ReceiptRequest UseCase17Request(int n, decimal amount1 = 4.8m, decimal amount2 = 3.3m, string cashBoxId = "")
        {

            var reqdata = new ReceiptRequest()
            {
                //                ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
                ftCashBoxID = cashBoxId,
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000000,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,

                cbChargeItems = new ChargeItem[]  {
                    new ChargeItem()
                    {
                        ftChargeItemCase=0x4154000000000000,
                         ProductNumber="1",
                         Description="Artikel 1",
                         Quantity=1.0m,
                         VATRate=20.0m,
                         Amount=amount1
                    },
                    new ChargeItem()
                    {
                        ftChargeItemCase=0x4154000000000000,
                        ProductNumber="2",
                        Description="Artikel 2",
                        Quantity=1.0m,
                        VATRate=20.0m,
                        Amount=amount2
                    }
                },
                cbPayItems = new PayItem[]                {
                    new PayItem()
                    {
                        ftPayItemCase=0x4154000000000000,
                        Amount=amount1+amount2,
                        Quantity=1.0m,
                        Description="Bar"
                    }
                }
            };

            return reqdata;
        }

        internal static ReceiptRequest StartRequest(int n, string cashBoxId)
        {
            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = cashBoxId,
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000003,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,
                cbChargeItems = new ChargeItem[] { },
                cbPayItems = new PayItem[] { }
            };

            return reqdata;
        }

        internal static ReceiptRequest ZeroReceiptRequest(int n, string cashBoxId)
        {
            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = cashBoxId,
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000002,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,
                cbChargeItems = new ChargeItem[] { },
                cbPayItems = new PayItem[] { }
            };

            return reqdata;
        }
    }
}
