using System;
using System.Net;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.NTWK.Services.Lidgren;

namespace Softfire.MonoGame.UTESTS.TextClasses
{
    public class LidgrenClientTestClass : LidgrenClient
    {
        public LidgrenClientTestClass(string applicationIdentifier = null,
                                      IPAddress ipAddress = null,
                                      int port = 16462,
                                      string logFilePath = @"Config\Logs\Server") : base(applicationIdentifier, ipAddress, port, logFilePath)
        {
        }

        public override void Update(GameTime gameTime)
        {
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
                        Console.WriteLine(netIncMsg.ReadString());
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
                        Console.WriteLine(netIncMsg.ReadString());
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
                        Console.WriteLine("Unhandled Message Type: " + netIncMsg.MessageType);
                        break;
                }

                Recycle(netIncMsg);
            }
        }
    }
}