using System.Collections.Generic;

namespace ServerLibrary
{
    public class ClientPeerPool
    {
        /// <summary>
        /// 用一个队列来存储连接池
        /// </summary>
        private Queue<ClientPeer> clientPeerQueue;

        /// <summary>
        /// 构造函数中指定连接池大小
        /// </summary>
        /// <param name="capacity"></param>
        public ClientPeerPool(int capacity)
        {
            clientPeerQueue=new Queue<ClientPeer>(capacity);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void Enqueue(ClientPeer client)
        {
            clientPeerQueue.Enqueue(client);
        }

        public ClientPeer Dequeue()
        {
            return clientPeerQueue.Dequeue();
        }

    }
}