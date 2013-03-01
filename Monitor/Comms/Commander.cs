using Monitor.Commands;
using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class Commander
    {
        public Commander(Connection connection)
        {
            m_Connection = connection;

            m_PM3Commands = new List<Command>();
            m_CSAFECommands = new List<Command>();

            m_CmdWriter = new CommandWriter(64);
            m_RspReader = new ResponseReader(64);
        }

        public void Reset()
        {
            m_PM3Commands.Clear();
            m_CSAFECommands.Clear();
            m_CmdWriter.Reset();
            m_RspReader.Reset(0);
        }

        public void Add(Command cmd)
        {
            if (m_CmdWriter.Size == 0)
            {
                if (cmd is PM3Command)
                {
                    m_PM3Commands.Add(cmd);
                }
                else
                {
                    m_CSAFECommands.Add(cmd);
                }
            }
            else
            {
                Debug.WriteLine("[Commander.Send] Command list already prepared - cannot add more commands");
            }
        }

        public void Prepare()
        {
            // Prepare the command list for sending
            if (m_CmdWriter.Size == 0)
            {
                if (m_PM3Commands.Count > 0)
                {
                    // Add PM3 commands
                    m_CmdWriter.WriteByte((uint)CSAFE.SETUSERCFG1_CMD);
                    m_CmdWriter.WriteByte((uint)m_PM3Commands.Count);

                    foreach (Command cmd in m_PM3Commands)
                    {
                        cmd.Write(m_CmdWriter);
                    }
                }

                // Add CSAFE commands
                foreach (Command cmd in m_CSAFECommands)
                {
                    cmd.Write(m_CmdWriter);
                }
            }
        }

        public bool Send()
        {
            bool success = false;

            if (m_CmdWriter.Size > 0)
            {
                int rspDataCount = m_RspReader.Capacity;
                if (m_Connection.SendCSAFECommand(m_CmdWriter.Buffer, m_CmdWriter.Size, m_RspReader.Buffer, ref rspDataCount))
                {
                    // Reset the reader to the correct length
                    m_RspReader.Reset(rspDataCount);

                    // Read the response
                    success = Read();
                }
            }
            else
            {
                Debug.WriteLine("[Commander.Send] Nothing to send");
            }

            return success;
        }

        private bool Read()
        {
            bool success = false;

            try
            {
                if (m_PM3Commands.Count > 0)
                {
                    // Read the PM3 custom command marker and size
                    if (m_RspReader.ReadByte() == (uint)CSAFE.SETUSERCFG1_CMD)
                    {
                        // Read the size
                        m_RspReader.ReadByte();
                    }

                    // Read PM3 commands
                    foreach (Command cmd in m_PM3Commands)
                    {
                        cmd.Read(m_RspReader);
                    }

                    // Read CSAFE commands
                    foreach (Command cmd in m_CSAFECommands)
                    {
                        cmd.Read(m_RspReader);
                    }

                    // Ensure whole response has been read
                    success = (m_RspReader.Position == m_RspReader.Size);

                }
            }
            catch (EndOfStreamException e)
            {
                Debug.WriteLine(string.Format("[Commander.Read] {0}", e.Message));
            }

            return success;
        }

        Connection m_Connection;
        List<Command> m_PM3Commands;
        List<Command> m_CSAFECommands;
        CommandWriter m_CmdWriter;
        ResponseReader m_RspReader;
    }
}
