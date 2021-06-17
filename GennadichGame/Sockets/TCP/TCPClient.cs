using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GennadichGame.Sockets.Common;
using GennadichGame.Sockets.Data;

namespace GennadichGame.Sockets.TCP
{
    public class TcpAsyncClient
    {
        private string ClientName { get; set; } = System.Net.Dns.GetHostName();

        private string LobbyName { get; set; } = null;

        public ConnectedObject Client { get; set; }

        public GameData ReceiverGameData { get; set; }
        
        public delegate void PlayerConnect(List<string> players);
        public delegate void ViewGameData(GameData gameData);
        public event PlayerConnect handleConnection;
        public event ViewGameData handleViewData;
        
        public delegate void JoinGame();
        public event JoinGame handleGame;

        public TcpAsyncClient(ConnectionManager connection)
        {
            Client = new ConnectedObject
            {
                Name = ClientName, Socket = ConnectionManager.CreateSocket()
            };
            // Create a new socket
            Client.Socket.Connect(connection.EndPoint);
            // Receive message from server async
            Task.Run(() =>
            {
                while (Receive())
                {
                }
            });
        }

        public void SendFindLobbyMessage()
        {
            var clientData = new LobbyWaitData {ClientName = ClientName, Status = ReceiverStatus.Ok};
            var data = new DataReceiver(DataType.LobbyWait, clientData.ToString());
            SendMessage(data.ToString());
        }

        public void SendGameMessage()
        {
            var data = new DataReceiver(DataType.Game, ReceiverGameData.ToString());
            SendMessage(data.ToString());
        }

        public bool SendMessage(string sendingData)
        {
            var sendingDataBytes = Encoding.UTF8.GetBytes(sendingData);
            try
            {
                Client.Socket.BeginSend(sendingDataBytes, 0, sendingDataBytes.Length, SocketFlags.None, SendCallback, Client);
            }
            catch (SocketException)
            {
                Client.Close();
                return false;
            }
            catch (ObjectDisposedException)
            {
                // ignored
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static void SendCallback(IAsyncResult ar)
        {
            //Console.WriteLine("Message Sent");
        }

        private bool Receive()
        {
            int bytesRead;
            try
            {
                bytesRead = Client.Socket.Receive(Client.Buffer, SocketFlags.None);
            }
            catch (SocketException)
            {
                Client.Close();
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }

            if (bytesRead == 0) return true;
            var receiverData = DataReceiver.Create(Client.Buffer,bytesRead);
            switch (receiverData.DataType)
            {
                case DataType.LobbyWait:
                {
                    var lobbyData = LobbyWaitData.Create(receiverData.MessageData);
                    switch (lobbyData.Status)
                    {
                        case ReceiverStatus.Ok:
                        {
                            if (LobbyName == null)
                            {
                                LobbyName = lobbyData.LobbyName;
                            }

                            handleConnection(lobbyData.CurrentPlayers);
                            break;
                        }
                        case ReceiverStatus.Rejected:
                        {
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case DataType.Game:
                    ReceiverGameData = GameData.Create(receiverData.MessageData);
                    switch (ReceiverGameData.Status)
                    {
                        case ReceiverStatus.Ok:
                        {
                            if (ReceiverGameData.NumStep == 0 && ReceiverGameData.TurnPlayerNumShoot == 0)
                                handleGame();
                            handleViewData(ReceiverGameData);
                            break;
                        }
                        case ReceiverStatus.Rejected:
                        {
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}