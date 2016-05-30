using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using fiskaltrust.ifPOS.v0;


namespace csConsoleApplicationSOAP
{
    class Program
    {
        static void Main(string[] args)
        {

            //..\tools\fiskaltrust-net40\test.cmd runs the service 
            // with id={f9bb4d9f-db98-4c24-a614-87f9d874f0cc} 
            // listening on http://localhost:8524/438BE08C-1D87-440D-A4F0-A21A337C5202
            // it has to be run from a console started with admin permission
            // to reset data delete folder %ProgrammData%\fiskaltrust

            string url = "http://localhost:8524/438BE08C-1D87-440D-A4F0-A21A337C5202";
            //string url = "http://192.168.0.18:8524/438BE08C-1D87-440D-A4F0-A21A337C5202";



            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = 16 * 1024 * 1024;

            var endpoint = new EndpointAddress(url);

            var factory = new ChannelFactory<IPOS>(binding, endpoint);

            var proxy = factory.CreateChannel();


            // use echo for communication test
            var message = proxy.Echo("message");
            if (message != "message") throw new Exception("echo failed");


            int n = 0;
            Random r = new Random();

            // receipts on deactivated queue
            while(n<5)
            {
                var req = UseCase17Request(n, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2));
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }

            // start receipt
            {
                var req = StartRequest(n);
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }

            // receipts on actevated queue

            while(n<35)
            {
                var req = UseCase17Request(n, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2));
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }


            var stream = proxy.Journal(0x4154000000000001, 0, DateTime.UtcNow.Ticks);
            var sr = new System.IO.StreamReader(stream);

            Console.WriteLine("========== RKSV-DEP ==========");
            Console.WriteLine(sr.ReadToEnd());
            



            factory.Close();

            Console.ReadKey();
        }




        internal static ReceiptRequest UseCase17Request(int n, decimal amount1 = 4.8m, decimal amount2 = 3.3m)
        {

            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
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

        internal static void Response(ReceiptResponse data)
        {
            if (data != null)
            {
                Console.WriteLine($"========== n: {data.cbReceiptReference} CashBoxIdentificateion:{data.ftCashBoxIdentification} ReceiptIdentification:{data.ftReceiptIdentification} ==========");
                foreach (var item in data.ftSignatures)
                {
                    Console.WriteLine($"{item.Caption}:{item.Data}");
                }
            }
            else
            {
                Console.WriteLine("null-result!!!");
            }
        }



        internal static ReceiptRequest StartRequest(int n)
        {
            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000003,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,
                cbChargeItems = new ChargeItem[] { },
                cbPayItems = new PayItem[] { }
            };

            return reqdata;
        }




        internal static ReceiptRequest StopRequest(int n)
        {
            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
                cbTerminalID = "1",
                ftReceiptCase = 0x4154000000000004,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,
                cbChargeItems = new ChargeItem[] { },
                cbPayItems = new PayItem[] { }
            };

            return reqdata;
        }

    }
}
