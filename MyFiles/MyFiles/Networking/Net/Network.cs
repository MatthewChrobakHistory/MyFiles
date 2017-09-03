using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyFiles.Networking.Net
{
    public class Network : INetwork
    {
        private int _pos = 0;
        private Socket _client;
        private SocketSendFlag _sendFlag;
        // Wait packet variables.
        private int _ticket;
        private int _servicing;

        public Network() {

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var host = Dns.GetHostAddresses("mycloudfiles.ddns.net");

            if (host.Length > 0) {
                _client.BeginConnect(new IPEndPoint(host[0], 7001), new AsyncCallback(ConnectCallback), null);
            }
            while (_client?.Connected == false) ;
        }

        private void Retry() {
            string IP;

            switch (++_pos) {
                case 1:
                    Program.ShowMessage("Error", "Could not connect via external host.\nAttempting to connect to LAN host.", false);
                    IP = "192.168.2.31";
                    break;
                case 2:
                    Program.ShowMessage("Error", "Could not connect to LAN host. \nAttempting to connect locally.");
                    IP = "127.0.0.1";
                    break;
                default:
                    Program.ShowMessage("Error", "Unable to connect to any servers.", true);
                    return;
            }

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.BeginConnect(new IPEndPoint(IPAddress.Parse(IP), 7001), new AsyncCallback(ConnectCallback), null);
            while (_client?.Connected == false) ;
        }

        private void ConnectCallback(IAsyncResult ar) {
            try {
                _client.EndConnect(ar);
            } catch (SocketException e) {
                Retry();
                return;
            }

            this._sendFlag = SocketSendFlag.CanSend;
        }

        public void SendData(byte[] array) {
            // Make sure we're connected.
            if (this._client?.Connected != true) {
                return;
            }

            // Make sure the data we're sending isn't over the limit.
            if (array.Length > this._client.SendBufferSize) {
                return;
            }


            // Can we send data right now?
            if (this._sendFlag != SocketSendFlag.CanSend) {

                // Create and start a new thread that will wait to send the data.
                var thread = new Thread(new ParameterizedThreadStart(SendDataWait));
                thread.Start(array);
                return;
            }

            // Starting an asynchronous operation might cause an error. Encase it
            // in a try/catch.
            try {
                // Flag the socket as sending data, and begin sending data.
                this._sendFlag = SocketSendFlag.Sending;
                this._client.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            } catch (SocketException e) {

                // Figure out what caused the error.
                switch (e.SocketErrorCode) {

                    // An unknown error occured. Throw an exception.
                    default:
                        throw new Exception("SendData: Unknown SocketException '" + e.SocketErrorCode + "'");
                }
            }
        }

        private void SendDataWait(object obj) {
            // Create a ticket.
            int ticket = this._ticket++;

            // Cast the argument to an array of bytes.
            byte[] array = (byte[])obj;

            // Take note of when this method was called.
            int start = Environment.TickCount;

            // Continue to loop while we're waiting to send data, or we're not first in line.
            while (this._sendFlag != SocketSendFlag.WaitCanSend || this._servicing != ticket) ;

            // Starting an asynchronous operation might cause an error. Encase it
            // in a try/catch.
            try {
                // Flag the socket as sending data, and begin to send.
                this._sendFlag = SocketSendFlag.Sending;
                this._client.BeginSend(array, 0, array.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            } catch (SocketException e) {
                System.Environment.Exit(0);
            }
        }

        private void SendCallback(IAsyncResult ar) {
            // End the asynchronous operation.
            this._client.EndSend(ar);

            // If the ticket is equal to the current ticket being served, then
            // we're either not using the waiting system or we've fully caught up.
            if (this._ticket == this._servicing) {
                // Reset the ticketing system, and flag the socket so 
                // it can begin to send data.
                this._ticket = 0;
                this._servicing = 0;
                this._sendFlag = SocketSendFlag.CanSend;
            } else {
                // Otherwise, increment the service number and
                // flag the socket to service another wait packet.
                this._sendFlag = SocketSendFlag.WaitCanSend;
                this._servicing++;
            }
        }

        private void SendFile(byte[] array) {

        }
    }
}
