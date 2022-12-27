using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Assets2.Code.Net
{
    public class NetworkUtils
    {
        public static string GetLocalIPv4()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            string result = null;

            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string found = ip.ToString();
                    if (found.StartsWith("192.168.1."))
                    {
                        return found;
                    }
                    result = found;
                }
            }
            return result;
        }

    }
}
