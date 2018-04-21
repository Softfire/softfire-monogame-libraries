using System.Threading.Tasks;

namespace Softfire.MonoGame.NTWK
{
    interface INetPeer
    {
        NetPeerConfiguration Configuration { get; }

        NetPeerStatus Status { get; }

        Task Start(string message);

        Task Shutdown(string message);
    }
}