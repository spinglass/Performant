using Monitor.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class CommandSet
    {
        public CommandSet()
        {
            m_PM3Commands = new List<Command>();
            m_CSAFECommands = new List<Command>();
            m_CmdWriter = new CommandWriter(64);

            m_Open = true;
        }

        public bool CanAdd { get { return m_Open; } }
        public bool CanSend { get { return !m_Open; } }
        public uint[] Buffer { get { return m_CmdWriter.Buffer; } }
        public int Size { get { return m_CmdWriter.Size; } }

        public void Reset()
        {
            m_PM3Commands.Clear();
            m_CSAFECommands.Clear();
            m_CmdWriter.Reset();

            m_Open = true;
        }

        public void Add(Command cmd)
        {
            if (m_Open)
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
                throw new CommandSetException("Cannot add new commands once set is prepared.");
            }
        }

        public void Prepare()
        {
            if (m_Open)
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

                // Ensure no more commands are added
                m_Open = false;
            }
            else
            {
                throw new CommandSetException("Set is already prepared.");
            }
        }

        public bool Read(ResponseReader reader)
        {
            if (m_Open)
            {
                throw new CommandSetException("Attempting to read set before it has been prepared.");
            }

            bool success = false;
            try
            {
                if (m_PM3Commands.Count > 0)
                {
                    // Read the PM3 custom command marker and size
                    if (reader.ReadByte() == (uint)CSAFE.SETUSERCFG1_CMD)
                    {
                        // Read the size
                        reader.ReadByte();
                    }

                    // Read PM3 commands
                    foreach (Command cmd in m_PM3Commands)
                    {
                        cmd.Read(reader);
                    }

                    // Read CSAFE commands
                    foreach (Command cmd in m_CSAFECommands)
                    {
                        cmd.Read(reader);
                    }

                    // Ensure whole response has been read
                    success = (reader.Position == reader.Size);

                }
            }
            catch (BufferExceededException e)
            {
                Debug.WriteLine(string.Format("[CommandSet.Read] {0}", e.Message));
            }

            return success;
        }

        private List<Command> m_PM3Commands;
        private List<Command> m_CSAFECommands;
        private CommandWriter m_CmdWriter;
        private bool m_Open;
    }
}
