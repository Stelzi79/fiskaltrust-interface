using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace csConsoleApplicationSOAP
{
    class Program
    {
        static void Main(string[] args)
        {

            string url = "http://localhost:1201/fiskaltrust/POS";
            //string url = "http://192.168.0.11:1201/fiskaltrust/POS";

            var reqdata = getReceiptRequest();

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

            var endpoint = new EndpointAddress(url);

            var factory = new ChannelFactory<fiskaltrust.ifPOS.v0.IPOS>(binding, endpoint);

            var proxy = factory.CreateChannel();


            Console.WriteLine("{0:G} Echo start", DateTime.Now);
            var echo = proxy.Echo("Hello World!");
            Console.WriteLine("{0:G} Echo {1}", DateTime.Now, echo);


            var jsonSettings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
            string reqjson = JsonConvert.SerializeObject(reqdata, jsonSettings);
            Console.WriteLine("{0:G} Sign request {1}", DateTime.Now, reqjson);
            var respdata = proxy.Sign(reqdata);
            string respjson = JsonConvert.SerializeObject(respdata, jsonSettings);
            Console.WriteLine("{0:G} Sign response {1}", DateTime.Now, respjson);

            Console.ReadLine();
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
