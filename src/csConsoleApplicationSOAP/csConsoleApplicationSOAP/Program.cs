﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using fiskaltrust.ifPOS.v0;
using Newtonsoft.Json;

namespace csConsoleApplicationSOAP
{
    class Program
    {
        static int i = 0;
        static string url;
        static string cashBoxId;

        static IPOS proxy = null;

        static void Main(string[] args)
        {

            Console.Write("fiskaltrust-service-url:");
            url = Console.ReadLine();

            Console.Write("cashboxid:");
            cashBoxId = Console.ReadLine();

            Guid _tempCashBoxID;
            if(!Guid.TryParse(cashBoxId, out _tempCashBoxID))
            {
                throw new ArgumentException("cashboxid is not a guid!");
            }
                
            //Console.Write("accesstoken:");
            //accesstoken = Console.ReadLine();


            //tools\fiskaltrust-net40
            //string url = "http://localhost:1200/fiskaltrust/";
            //string cashBoxId = Guid.Parse("0d1269dc-e2ae-42e3-9c57-b686d7832683").ToString();
            //string accesstoken = "BHanhRLW0WK1jyS00C+tTcJGtBHhziGWHqynd52pExpfi99QFRue+S4D/w8p5jugQr6hwJu31Parqx5256Qv9pw=";


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

            proxy = factory.CreateChannel();


            // use echo for communication test
            var message = proxy.Echo("message");
            if (message != "message") throw new Exception("echo failed");


            while(true)
            {
                Menu();
            }
        }



        static void Menu()
        {
            Console.WriteLine("1: Barumsatz (0x4154000000000001)");
            Console.WriteLine("2: Null-Beleg (0x4154000000000002)");
            Console.WriteLine("3: Inbetriebnahme-Beleg (0x4154000000000003)");
            Console.WriteLine("4: Außerbetriebnahme-Beleg (0x4154000000000004)");
            Console.WriteLine("5: Monats-Beleg (0x4154000000000005)");
            Console.WriteLine("6: Jahres-Beleg (0x4154000000000006)");

            Console.WriteLine("10: Anzahl der zu sendenden Barumsatzbelege (max 999)");

            Console.WriteLine("exit: Program beenden");

            string input = Console.ReadLine();

            Command(input);
        }

        static void Command(string input)
        {
            if (input.ToLower().StartsWith("exit"))
            {
                var stream = proxy.Journal(0x4154000000000001, 0, DateTime.UtcNow.Ticks);
                var sr = new System.IO.StreamReader(stream);

                Console.WriteLine("{0:G} ========== RKSV-DEP ==========",DateTime.Now);

                Console.WriteLine(sr.ReadToEnd());

                Environment.Exit(0);
            }


            Random r = new Random((int)DateTime.Now.Ticks);

            int inputInt;
            if (!int.TryParse(input, out inputInt))
            {
                Console.WriteLine($"\"{input}\" nicht erkannt.");
                return;
            }

            if (inputInt == 1)
            {
                var req = UseCase17Request(++i, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2), cashBoxId);
                Console.WriteLine("{0:G} Barumsatz request: {1}",DateTime.Now, JsonConvert.SerializeObject(req));

                var resp = proxy.Sign(req);

                Response(resp);
            }
            else if (inputInt == 2)
            {
                var req = ZeroReceiptRequest(++i, cashBoxId, 0x4154000000000002);
                Console.WriteLine("{0:G} Null-Beleg request: {1}", DateTime.Now, JsonConvert.SerializeObject(req));
                var resp = proxy.Sign(req);

                Response(resp);
            }
            else if (inputInt == 3)
            {
                var req = ZeroReceiptRequest(++i, cashBoxId, 0x4154000000000003);
                Console.WriteLine("{0:G} Inbetriebnahme-Beleg request: {1}", DateTime.Now, JsonConvert.SerializeObject(req));
                var resp = proxy.Sign(req);

                Response(resp);
            }
            else if (inputInt == 4)
            {
                var req = ZeroReceiptRequest(++i, cashBoxId, 0x4154000000000004);
                Console.WriteLine("{0:G} Außerbetriebnahme-Beleg request: {1}", DateTime.Now, JsonConvert.SerializeObject(req));
                var resp = proxy.Sign(req);

                Response(resp);
            }
            else if (inputInt == 5)
            {
                var req = ZeroReceiptRequest(++i, cashBoxId, 0x4154000000000005);
                Console.WriteLine("{0:G} Monats-Beleg request: {1}", DateTime.Now, JsonConvert.SerializeObject(req));
                var resp = proxy.Sign(req);

                Response(resp);

            }
            else if (inputInt == 6)
            {
                var req = ZeroReceiptRequest(++i, cashBoxId, 0x4154000000000006);
                Console.WriteLine("{0:G} Jahres-Beleg request: {1}", DateTime.Now, JsonConvert.SerializeObject(req));
                var resp = proxy.Sign(req);

                Response(resp);

            }
            else if (inputInt >= 10 && inputInt < 1000)
            {

                // receipts on actevated queue

                long max = long.MinValue;
                long min = long.MaxValue;
                long sum = 0;
                int n = 0;
                while (n < inputInt)
                {
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    var req = UseCase17Request(++i, decimal.Round((decimal)(r.NextDouble() * 100), 2), decimal.Round((decimal)(r.NextDouble() * 100), 2), cashBoxId);
                    var resp = proxy.Sign(req);
                    sw.Stop();
                    sum += sw.ElapsedMilliseconds;
                    if (sw.ElapsedMilliseconds > max) max = sw.ElapsedMilliseconds;
                    if (sw.ElapsedMilliseconds < min) min = sw.ElapsedMilliseconds;
                    Response(resp);
                }

                Console.WriteLine("Performance in ms => max: {0}, min: {1}, avg: {2}", max, min, sum / (decimal)inputInt);

            }
            else
            {
                Console.WriteLine($"\"{input}\" nicht erkannt.");
            }

            Menu();
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
                Console.WriteLine("{0:G} ========== response: {1}", DateTime.Now, JsonConvert.SerializeObject(data));
                Console.WriteLine("========== n: {0} CashBoxIdentificateion:{1} ReceiptIdentification:{2} ==========", data.cbReceiptReference, data.ftCashBoxIdentification, data.ftReceiptIdentification);
                foreach (var item in data.ftSignatures)
                {
                    if (item.ftSignatureFormat == 0x03)
                    {
                        fiskaltrust.ifPOS.Utilities.QR_TextChars(item.Data, 64, true);
                    }
                    else if(item.ftSignatureFormat==0x08)
                    {
                        fiskaltrust.ifPOS.Utilities.AZTEC_TextChars(item.Data, 80, true);
                    }

                    Console.WriteLine("{0}:{1}", item.Caption, item.Data);

                    if(item.ftSignatureType== 0x4154000000000001)
                    {
                        Console.WriteLine(fiskaltrust.ifPOS.Utilities.AT_RKSV_Signature_ToBase32(item.Data));
                        //Console.WriteLine(fiskaltrust.ifPOS.Utilities.AT_RKSV_Signature_ToLink(item.Data));
                    }
                }
            }
            else
            {
                Console.WriteLine("null-result!!!");
            }
        }



        internal static ReceiptRequest ZeroReceiptRequest(int n, string cashBoxId, long ftReceiptCase)
        {
            var reqdata = new ReceiptRequest()
            {
                ftCashBoxID = cashBoxId,
                cbTerminalID = "1",
                ftReceiptCase = ftReceiptCase,
                cbReceiptReference = n.ToString(),
                cbReceiptMoment = DateTime.UtcNow,
                cbChargeItems = new ChargeItem[] { },
                cbPayItems = new PayItem[] { }
            };

            return reqdata;
        }


        //internal static ReceiptRequest StartRequest(int n, string cashBoxId)
        //{
        //    var reqdata = new ReceiptRequest()
        //    {
        //        //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
        //        ftCashBoxID = cashBoxId,
        //        cbTerminalID = "1",
        //        ftReceiptCase = 0x4154000000000003,
        //        cbReceiptReference = n.ToString(),
        //        cbReceiptMoment = DateTime.UtcNow,
        //        cbChargeItems = new ChargeItem[] { },
        //        cbPayItems = new PayItem[] { }
        //    };

        //    return reqdata;
        //}

        //internal static ReceiptRequest ZeroReceiptRequest(int n, string cashBoxId)
        //{
        //    var reqdata = new ReceiptRequest()
        //    {
        //        //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
        //        ftCashBoxID = cashBoxId,
        //        cbTerminalID = "1",
        //        ftReceiptCase = 0x4154000000000002,
        //        cbReceiptReference = n.ToString(),
        //        cbReceiptMoment = DateTime.UtcNow,
        //        cbChargeItems = new ChargeItem[] { },
        //        cbPayItems = new PayItem[] { }
        //    };

        //    return reqdata;
        //}



        //internal static ReceiptRequest StopRequest(int n, string cashBoxId)
        //{
        //    var reqdata = new ReceiptRequest()
        //    {
        //        //ftCashBoxID = "f9bb4d9f-db98-4c24-a614-87f9d874f0cc",
        //        ftCashBoxID = cashBoxId,
        //        cbTerminalID = "1",
        //        ftReceiptCase = 0x4154000000000004,
        //        cbReceiptReference = n.ToString(),
        //        cbReceiptMoment = DateTime.UtcNow,
        //        cbChargeItems = new ChargeItem[] { },
        //        cbPayItems = new PayItem[] { }
        //    };

        //    return reqdata;
        //}

    }
}
