using System.Net;

namespace Softfire.MonoGame.NTWK.V2
{
    public class NetServer : NetPeer
    {
        public NetServer(string identifier, IPAddress ipAddress, int port) : base(identifier, ipAddress, port)
        {
        }
    }
}