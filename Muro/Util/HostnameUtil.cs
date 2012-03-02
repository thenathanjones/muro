using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Muro.Util
{
    public class HostnameUtil
    {
        public static Uri[] GetUriParams(int port)
        {
            var uriParams = new List<Uri>();
            var hostName = Dns.GetHostName();

            CreateUriForHostname(port, uriParams);

            CreateUrisForIPs(uriParams, hostName);

            uriParams.Add(new Uri(string.Format("http://localhost:{0}", port)));

            return uriParams.ToArray();
        }

        private static void CreateUrisForIPs(List<Uri> uriParams, string hostName)
        {
            var hostEntry = Dns.GetHostEntry(hostName);
            foreach (var ipAddress in hostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    var addressBytes = ipAddress.GetAddressBytes();
                    var hostAddressUri = string.Format("http://{0}.{1}.{2}.{3}:{4}", addressBytes[0], addressBytes[1],
                                                       addressBytes[2], addressBytes[3], 0);
                    uriParams.Add(new Uri(hostAddressUri));
                }
            }
        }

        private static void CreateUriForHostname(int port, IList<Uri> uriParams)
        {
            var hostNameUri = string.Format("http://{0}:{1}", Dns.GetHostName(), port);
            uriParams.Add(new Uri("hostNameUri"));
        }
    }
}
