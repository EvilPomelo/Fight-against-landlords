using System.Net.Sockets;

namespace ServerLibrary
{
    /// <summary>
    /// 客户端连接对象
    /// </summary>
    public class ClientPeer
    {
        private Socket clientSocket;

        /// <summary>
        /// 设置连接对象
        /// </summary>
        public void SetSocket(Socket socket)
        {
            this.clientSocket = socket;
        }
    }
}