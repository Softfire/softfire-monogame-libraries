using System.Net;
using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Softfire.MonoGame.UTESTS.TextClasses;

namespace Softfire.MonoGame.UTESTS
{
    /// <summary>
    /// Summary description for NTWKTests
    /// </summary>
    [TestClass]
    public class NTWKTests
    {
        private static LidgrenServerTestClass TestServer { get; set; }
        private static LidgrenClientTestClass TestClient { get; set; }

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            TestClient = new LidgrenClientTestClass("Test", new IPAddress(new byte[] {192, 168, 0, 52}), 16462);
            TestServer = new LidgrenServerTestClass("Test", new IPAddress(new byte[] {192, 168, 0, 52}), 16464);

            Assert.AreNotEqual(null, TestClient, "The Client object was NULL. Check the Client's constructor for issues.");
            Assert.AreNotEqual(null, TestServer, "The Server object was NULL. Check the Server's constructor for issues.");
        }

        [TestMethod]
        public void StartClient()
        {
            TestClient.Start();

            Assert.AreEqual(true, TestClient.Status == NetPeerStatus.Running, "The Client object was not started. Check the Client.Start() method for issues.");
        }

        [TestMethod]
        public void StartServer()
        {
            TestServer.Start();

            Assert.AreEqual(true, TestServer.Status == NetPeerStatus.Running, "The Server object was not started. Check the Server.Start() method for issues.");
        }

        [TestMethod]
        public void SendMessage()
        {
            var result = false;

            StartServer();
            StartClient();
            
            var outMessage = TestClient.CreateMessage();
            outMessage.Write("Test");
            TestClient.Connect(new IPEndPoint(new IPAddress(new byte[] {192, 168, 0, 52}), 16464), outMessage);
            
            while (TestClient.ConnectionStatus != NetConnectionStatus.Connected)
            {
                TestServer.Update(new GameTime());
                TestClient.Update(new GameTime());
            }

            if (TestClient.ConnectionStatus == NetConnectionStatus.Connected)
            {
                result = true;
            }

            Assert.IsTrue(result);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (TestClient != null)
            {
                TestClient.Shutdown("Shutting down.");
                TestClient = null;
            }

            if (TestServer != null)
            {
                TestServer.Shutdown("Shutting down.");
                TestServer = null;
            }

            Assert.AreEqual(null, TestClient, "The Client object was not NULL. Check the Client.Shutdown() method for issues.");
            Assert.AreEqual(null, TestServer, "The Server object was not NULL. Check the Server.Shutdown() method for issues.");
        }
    }
}
