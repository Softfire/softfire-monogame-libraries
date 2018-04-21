using System.Net;

namespace Softfire.MonoGame.NTWK
{
    public class NetServer : NetPeer
    {
        public NetServer(string identifier, IPAddress ipAddress, int port) : base(identifier, ipAddress, port)
        {
        }
    }
}