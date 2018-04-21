using System;
using System.Net;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.NTWK.Services.Lidgren;

namespace Softfire.MonoGame.UTESTS.TextClasses
{
    public class LidgrenServerTestClass : LidgrenServer
    {
        public LidgrenServerTestClass(string applicationIdentifier = null,
                                      IPAddress ipAddress = null,
                                      int port = 16464,
                                      string logFilePath = @"Config\Logs\Server") : base(applicationIdentifier, ipAddress, port, logFilePath)
        {

        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            NetIncomingMessage netIncMsg;

            while ((netIncMsg = ReadMessage()) != null)
            {
                switch (netIncMsg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.Error:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        netIncMsg.SenderConnection.Approve();

                        break;

                    case NetIncomingMessageType.ConnectionLatencyUpdated:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.Data:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:

                        var response = CreateMessage();

                        response.Write(Configuration.AppIdentifier);
                        response.Write(Configuration.LocalAddress.ToString());
                        response.Write(Configuration.Port);

                        SendDiscoveryResponse(response, netIncMsg.SenderEndPoint);

                        break;

                    case NetIncomingMessageType.NatIntroductionSuccess:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.Receipt:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    case NetIncomingMessageType.UnconnectedData:
                        Console.WriteLine(netIncMsg.ReadString());
                        break;

                    default:
                        Console.WriteLine($"Unhandled type: {netIncMsg.MessageType}.");
                        break;
                }

                Recycle(netIncMsg);
            }
        }
    }
}