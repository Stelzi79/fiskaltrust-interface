using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.ServiceModel;

namespace csClassLibraryCOMInterop
{

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("CB7A5038-A2A0-4462-944D-7B429E43B20B")]
    public class ft
    {

        //public string serviceUrl { get; set; }

        //public ft(string url="http://localhost:1201/fiskaltrust/POS")
        //{
        //    serviceUrl = url;
        //}

        //public ft()
        //{
        //    serviceUrl = "http://localhost:1201/fiskaltrust/POS";
        //}

        //private fiskaltrust.ifPOS.v0.IPOS proxy = null;
      
        //public bool Connected
        //{
        //    get
        //    {
        //        return proxy == null;
        //    }
        //}

        //public bool Connect()
        //{
        //    try
        //    {
        //        var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        //        var endpoint = new EndpointAddress(serviceUrl);
        //        var factory = new ChannelFactory<fiskaltrust.ifPOS.v0.IPOS>(binding, endpoint);
        //        proxy = factory.CreateChannel();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return Connected;
        //}

        //public void Disconnect()
        //{
        //    if(Connected)
        //    {
        //        proxy = null;
        //    }
        //}

        public string test()
        {
            return "test";
        }

        //public string echo(string message)
        //{
        //    if (!Connected) throw new Exception("Not Connected");

        //    return proxy.Echo(message);
        //}




    }
}
