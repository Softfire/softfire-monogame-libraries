using System.Net;

namespace Softfire.MonoGame.NTWK.V2
{
    public class NetClient : NetPeer
    {
        public NetClient(string identifier, IPAddress ipAddress, int port) : base(identifier, ipAddress, port)
        {
        }
    }
}