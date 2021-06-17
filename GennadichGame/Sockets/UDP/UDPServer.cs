using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GennadichGame.Sockets.UDP
{
    public class UdpServer
    {
        private int _listenPort;
        private System.Net.Sockets.UdpClient _udpServer;
        private IPEndPoint _groupAddresses;
        private Thread _mainLoop;
        private int _tcpPort;
        private string _broadcastMessage;

        public UdpServer(int tcpPort, int updPort, string message)
        {
            _listenPort = updPort;
            _groupAddresses = new IPEndPoint(IPAddress.Any, _listenPort);
            _udpServer = new System.Net.Sockets.UdpClient(_listenPort);
            _tcpPort = tcpPort;
            _broadcastMessage = message;
            _mainLoop = new Thread(UdpListen);
        }

        private void UdpListen()
        {
            while (true)
            {
                try
                {
                    var bytes = _udpServer.Receive(ref _groupAddresses);
                    if (!Equals(Encoding.UTF8.GetString(bytes), _broadcastMessage)) continue;
                    var sendBuff = Encoding.UTF8.GetBytes(_tcpPort.ToString());
                    _udpServer.Send(sendBuff, sendBuff.Length, _groupAddresses.Address.ToString(),
                        _groupAddresses.Port);

                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (SocketException)
                {
                    return;
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }
        
        public void StartListen()
        {
            _mainLoop.Start();
        }

        public void StopListen()
        {
            _udpServer.Close();
            _mainLoop.Interrupt();
        }
    }
}