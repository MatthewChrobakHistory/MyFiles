using MyFilesServer.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyFilesServer.Networking
{
    public class PacketManager
    {
        private delegate void HandleData(int index, byte[] array);
        private List<HandleData> _handleData;

        public PacketManager() {
            // Create the array of data handlers.
            this._handleData = new List<HandleData>((int)Packet.Length);

            // Add all packet handlers to the array here.
        }

        private byte[] RemovePacketHead(byte[] array) {
            // If the size of the entire buffer is 8, all the packet contains is the head and size.
            // Packets like that are just initiation packets, and don't actually contin
            // other data. So, what we return won't be manipulated anyways. Return null.
            if (array.Length == 8) {
                return null;
            }

            // Create a new array of the size desired.
            byte[] clippedArray = new byte[array.Length - 8];

            // Copy the offset bytes into the clipped array.
            Array.Copy(array, 8, clippedArray, 0, array.Length - 8);

            // Return the clipped array.
            return clippedArray;
        }

        private Net.Client GetClient(int index) {
            var network = (Net.Network)NetworkManager.Network;
            if (network._client.Count >= index || index < 0) {
                return null;
            } else {
                return network._client[index];
            }
        }

        public byte[] HandlePacket(int index, byte[] array) {
            var client = GetClient(index);
            if (client.IncomingFile) {
                HandleFileData(index, client.FileName, client.FileSize, array);
                return new byte[0];
            }


            // Push the bytes into a new databuffer object.
            var packet = new DataBuffer(array);
            bool process = true;

            // Continue to loop while there's still data to process.
            while (process) {
                // Get the size of the next packet.
                int size = packet.ReadInt();

                // Do we have more than all the data need for the packet?
                if (array.Length > size) {

                    // Resize the array containing the bytes needed for this packet, and
                    // create a new array containing the excess.
                    byte[] excessbuffer = new byte[array.Length - size];
                    Array.ConstrainedCopy(array, size, excessbuffer, 0, array.Length - size);
                    Array.Resize(ref array, size);

                    // Read the packet head, validate its contents, and invoke its data handler.
                    int head = packet.ReadInt();
                    if (head >= 0 && head < _handleData.Count) {
                        _handleData[head].Invoke(index, RemovePacketHead(array));
                    }

                    // Re-create the databuffer object with just the excess bytes, and 
                    // continue to loop.
                    packet = new DataBuffer(excessbuffer);

                    // Do we have all the data needed for the packet?
                } else if (array.Length == size) {

                    // Read the packet head, validate its contents, and invoke its data handler.
                    int head = packet.ReadInt();
                    if (head >= 0 && head < _handleData.Count) {
                        _handleData[head].Invoke(index, RemovePacketHead(array));
                    }

                    // Return an empty array.
                    return new byte[0];
                } else {
                    // We have less data than we need. There's nothing to process yet.
                    process = false;
                }
            }

            // Return the unprocessed bytes.
            return packet.ToArray();
        }

        #region Handling incoming packets
        private void HandleIncomingFile(int index, byte[] array) {
            var packet = new DataBuffer(array);
            var client = GetClient(index);

            client.FileName = packet.ReadString();
            client.FileSize = packet.ReadLong();
            client.IncomingFile = true;
        }
        private void HandleFileData(int index, string filename, long size, byte[] data) {
            if (!Directory.Exists(Server.StartupPath + filename)) {
                Directory.CreateDirectory(Server.StartupPath + filename);
            }
            using (var output = File.OpenWrite(Server.StartupPath + filename)) {
                output.Write(data, 0, data.Length);
            }
        }
        #endregion

        #region Sending outgoing packets

        #endregion
    }
}
