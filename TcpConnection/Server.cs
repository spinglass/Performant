﻿using Common;
using DirectConnection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpConnection.Protocol;

namespace TcpConnection
{
    public enum PM3State
    {
        Connected,
        Disconnected,
    }

    public class Server
    {
        public delegate void ConnectionHandler(object sender, PM3State state);
        public event ConnectionHandler ConnectionChanged;

        public Server()
        {
            m_Connection = new DirectConnection.Connection();
            m_CmdData = new uint[64];
            m_RspData = new uint[64];
            m_Responder = new Responder();

            m_Listener = new Listener(7474);
            m_Client = null;

            m_TcpTimeout = new Stopwatch();
            m_PM3State = PM3State.Disconnected;
            m_FrameCount = 0;

            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Server";
            m_Quit = false;
        }

        public void Start()
        {
            m_Thread.Start();
            m_Listener.Start();
        }

        public void Stop()
        {
            m_Quit = true;
            m_Listener.Stop();
            m_Thread.Join();
        }

        private void SetConnectionState(PM3State state)
        {
            if (m_PM3State != state)
            {
                if (ConnectionChanged != null)
                {
                    ConnectionChanged(this, state);
                }
            }
            m_PM3State = state;
        }

        private bool SendKeepAlive()
        {
            // Send empty command to test connection
            int rspLength = m_RspData.Length;
            return m_Connection.SendCSAFECommand(m_CmdData, 0, m_RspData, ref rspLength);
        }

        private void CloseConnection()
        {
            m_Responder.Stream = null;
            if (m_Client != null)
            {
                m_Client.Close();
                m_Client = null;
            }
            m_Listener.AcceptNext();
        }

        private void UpdatePM3Connection()
        {
            switch (m_PM3State)
            {
                case PM3State.Disconnected:
                    if (m_FrameCount % 100 == 0)
                    {
                        if (m_Connection.Open())
                        {
                            SetConnectionState(PM3State.Connected);
                            Debug.WriteLine("Server: PM3 connected");
                        }
                    }
                    break;

                case PM3State.Connected:
                    if (m_FrameCount % 100 == 0)
                    {
                        SendKeepAlive();
                        if (!m_Connection.IsOpen)
                        {
                            SetConnectionState(PM3State.Disconnected);
                            Debug.WriteLine("Server: PM3 lost");
                        }
                    }
                    break;
            }
        }

        private void UpdateTcpConnection()
        {
            if (m_Client == null)
            {
                // No current connection
                m_Client = m_Listener.Client;
                if (m_Client != null)
                {
                    NetworkStream stream = m_Client.GetStream();
                    stream.ReadTimeout = 10;
                    stream.WriteTimeout = 10;
                    m_Responder.Stream = stream;

                    if (m_Responder.Handshake())
                    {
                        Debug.WriteLine("Server: Connected");
                        m_TcpTimeout.Restart();
                    }
                    else
                    {
                        Debug.WriteLine("[Server.UpdateTcpConnection] Handshake failed");
                        CloseConnection();
                    }
                }
            }
            else
            {
                if (m_Responder.Stream.DataAvailable)
                {
                    m_TcpTimeout.Restart();

                    MessageType type = m_Responder.ReadMessage();
                    switch (type)
                    {
                        case MessageType.Connected:
                            // Send connection state
                            if (m_PM3State == PM3State.Connected)
                            {
                                m_Responder.SendConnected();
                            }
                            else
                            {
                                m_Responder.SendDisconnected();
                            }
                            break;

                        case MessageType.Command:
                            int cmdBufferCount = 0;
                            if (m_Responder.ReadCommand(m_CmdData, ref cmdBufferCount))
                            {
                                if (m_PM3State == PM3State.Connected)
                                {
                                    int rspDataCount = m_RspData.Length;
                                    if (m_Connection.SendCSAFECommand(m_CmdData, cmdBufferCount, m_RspData, ref rspDataCount))
                                    {
                                        m_Responder.SendResponse(m_RspData, rspDataCount);
                                    }
                                    else
                                    {
                                        m_Responder.SendSendError();
                                    }
                                }
                                else
                                {
                                    m_Responder.SendDisconnected();
                                }
                            }      
                            break;

                        case MessageType.None:
                            // Nothing to do
                            break;

                        default:
                            Debug.WriteLine("[Responder.ReadCommand] Unexpected message (" + type.ToString() + ")");
                            break;
                    }
                }
                else
                {
                    // Ensure the client is still talking to us
                    if (m_TcpTimeout.ElapsedMilliseconds > 1000)
                    {
                        CloseConnection();
                        Debug.WriteLine("Server: Lost");
                    }
                }
            }
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                UpdatePM3Connection();
                UpdateTcpConnection();

                ++m_FrameCount;
            }
        }

        private IConnection m_Connection;
        private uint[] m_CmdData;
        private uint[] m_RspData;
        private Responder m_Responder;

        private Listener m_Listener;
        private TcpClient m_Client;

        private Stopwatch m_TcpTimeout;
        private Thread m_Thread;
        private bool m_Quit;
        private PM3State m_PM3State;
        private int m_FrameCount;
    }
}
