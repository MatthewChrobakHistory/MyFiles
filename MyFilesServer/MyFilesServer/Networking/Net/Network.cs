using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace MyFilesServer.Networking.Net
{
    public class Network : INetwork
    {
        private Socket _server;
        public List<Client> _client { private set; get; }

        public Network() {
            _client = new List<Client>();

            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, 7001));

            _server.Listen(5);

            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult ar) {
            AddConnection(_server.EndAccept(ar));

            _server.Listen(5);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void Destroy() {
            // Disconnect every client in the collection.
            foreach (var client in this._client) {
                client.Disconnect();
            }
            // Clear the list.
            this._client.Clear();
        }

        private void AddConnection(Socket connection) {
            // Look through our collection for a free slot.
            for (int i = 0; i < this._client.Count; i++) {
                if (!this._client[i].Connected) {
                    // If there is a free slot, create a new client at that position.
                    this._client[i] = new Client(connection, i);
                    return;
                }
            }

            // If an unused spot could not be found, add a new entry in the collection.
            this._client.Add(new Client(connection, this._client.Count));
        }
    }
}
