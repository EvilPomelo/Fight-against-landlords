using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerLibrary
{
    public class ServerPeer
    {
        /// <summary>
        /// 服务端对象
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore acceptSemaphore;

        /// <summary>
        /// 客户端连接池
        /// </summary>
        private ClientPeerPool clientPeerPool;
        
        /// <summary>
        /// 用来开启服务器
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="maxCount">最大连接数量</param>
        public void Start(int port, int maxCount)
        {
            try
            {
                //构建一个服务端Socket
                serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                //指定信号量初始连接数量和最大连接数量
                acceptSemaphore=new Semaphore(maxCount,maxCount);
                clientPeerPool=new  ClientPeerPool(maxCount);
                ClientPeer tmpClientPeer = null;
                for (int i = 0; i < maxCount; i++)
                {
                    tmpClientPeer=new ClientPeer();
                    clientPeerPool.Enqueue(tmpClientPeer);
                }
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                Console.WriteLine("服务器启动...");
                startAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);              
            }
        }
//
        #region 接收客户端的连接

        /// <summary>
        /// 开始等待客户端连接
        /// </summary>
        /// <param name="e"></param>
        private void startAccept(SocketAsyncEventArgs e)
        {
            if (e==null)
            {
                e=new SocketAsyncEventArgs();
                e.Completed += accept_Completed;
            }
            //限制线程访问
            acceptSemaphore.WaitOne();
            //返回值判断线程是否执行完成，返回值为true则表示正在执行，等待执行异步回调函数，为false则同步执行完成直接处理
            bool result = serverSocket.AcceptAsync(e);
            
            if (result==false)
            {
                processAccept(e);
            }
        }

        /// <summary>
        /// 接受连接请求异步事件完成时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Completed(object sender,SocketAsyncEventArgs e)
        {
            processAccept(e);

        }

        /// <summary>
        /// 处理连接请求
        /// </summary>
        /// <param name="e"></param>
        private void processAccept(SocketAsyncEventArgs e)
        {
            //得到客户端的对象
            ClientPeer client = clientPeerPool.Dequeue();
            client.SetSocket(e.AcceptSocket);
            //进行再保存处理
            //todo 一直接受客户端发来的数据
            e.AcceptSocket = null;
            startAccept(e);
        }

        #endregion

    }
}