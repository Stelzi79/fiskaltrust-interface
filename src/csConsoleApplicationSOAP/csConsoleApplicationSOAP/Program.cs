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

            //tools\fiskaltrust-net40
            string url = "http://localhost:1200/fiskaltrust/";
            string cashBoxId = Guid.Parse("0d1269dc-e2ae-42e3-9c57-b686d7832683").ToString();
            string accesstoken = "BHanhRLW0WK1jyS00C+tTcJGtBHhziGWHqynd52pExpfi99QFRue+S4D/w8p5jugQr6hwJu31Parqx5256Qv9pw=";


            //tools\fiskaltrust-mono
            //string url = "http://localhost:1201/9e37335f-b036-4ee8-a2aa-83916ab6749e";
            //string cashBoxId = Guid.Parse("5f4b1438-8aca-4eda-954f-ec450ed17bde").ToString();
            //string accesstoken = "BEchmMiRzFCG4FLAxv2vQK+otzcnY6iJXTjGf/Ow/muQROkITCht3ctnUF6pDEmR9XCAzn0LQpQmO6qPyQd37OY=";

            System.ServiceModel.Channels.Binding binding = null;

            if (url.StartsWith("http://"))
            {
                var b = new BasicHttpBinding(BasicHttpSecurityMode.None);
                b.MaxReceivedMessageSize = 16 * 1024 * 1024;

                binding = b;
            }
            else if (url.StartsWith("https://"))
            {
                var b = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                b.MaxReceivedMessageSize = 16 * 1024 * 1024;

                binding = b;
            }
            else if (url.StartsWith("net.pipe://"))
            {
                var b = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
                b.MaxReceivedMessageSize = 16 * 1024 * 1024;

                binding = b;
            }
            else if (url.StartsWith("net.tcp://"))
            {
                var b = new NetTcpBinding(SecurityMode.None);
                b.MaxReceivedMessageSize = 16 * 1024 * 1024;

                binding = b;

            }

            var endpoint = new EndpointAddress(url);

            var factory = new ChannelFactory<IPOS>(binding, endpoint);

            var proxy = factory.CreateChannel();


            // use echo for communication test
            var message = proxy.Echo("message");
            if (message != "message") throw new Exception("echo failed");


            int n = 0;
            Random r = new Random();

            // receipts on deactivated queue
            while (n < 5)
            {
                var req = UseCase17Request(n, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2), cashBoxId);
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }

            // start receipt
            {
                var req = StartRequest(n, cashBoxId);
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }

            // receipts on actevated queue

            long max = long.MinValue;
            long min = long.MaxValue;
            long sum = 0;
            while (n < 100)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var req = UseCase17Request(n, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2), cashBoxId);
                var resp = proxy.Sign(req);
                sw.Stop();
                sum += sw.ElapsedMilliseconds;
                if (sw.ElapsedMilliseconds > max) max = sw.ElapsedMilliseconds;
                if (sw.ElapsedMilliseconds < min) min = sw.ElapsedMilliseconds;
                Response(resp);
                n++;
            }

            Console.WriteLine(String.Format("max: {0}, min: {1}, avg: {2}", max, min, sum / 100));

            // zeroreceipt
            {
                var req = ZeroReceiptRequest(n, cashBoxId);
                var resp = proxy.Sign(req);

                Response(resp);
                n++;
            }

            while (n < 25)
            {
                var req = UseCase17Request(n, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2), cashBoxId);
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

        internal static void Response(ReceiptResponse data)
        {
            if (data != null)
            {
                Console.WriteLine("========== n: {0} CashBoxIdentificateion:{1} ReceiptIdentification:{2} ==========", data.cbReceiptReference, data.ftCashBoxIdentification, data.ftReceiptIdentification);
                foreach (var item in data.ftSignatures)
                {
                    if (item.ftSignatureFormat == 0x03)
                    {
                        fiskaltrust.ifPOS.Utilities.QR_TextChars(item.Data, 64, true);
                        Console.WriteLine(fiskaltrust.ifPOS.Utilities.AT_RKSV_Signature_ToBase32(item.Data));
                    }

                    Console.WriteLine("{0}:{1}", item.Caption, item.Data);

                }
            }
            else
            {
                Console.WriteLine("null-result!!!");
            }
        }



        internal static ReceiptRequest StartRequest(int n, string cashBoxId)
        {
            var reqdata = new ReceiptRequest()
            {
                //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
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
                //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
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



        internal static ReceiptRequest StopRequest(int n, string cashBoxId)
        {
            var reqdata = new ReceiptRequest()
            {
                //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
                ftCashBoxID = cashBoxId,
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
