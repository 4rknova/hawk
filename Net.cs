using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Hawk
{
    public enum MSG_CODE
    {
          MSG_CODE_NICKNAME
        , MSG_CODE_TEXT
        , MSG_CODE_REQ_USER_LIST
    }

    public class Message
    {
        public MSG_CODE Code;
        public string   Data;

        public Byte[] Serialize()
        {
            string msg = Serializer.Serialize(this);
            return Encoding.ASCII.GetBytes(msg);
        }

        public void Deserialize(Byte[] pkg)
        {
            string  str = System.Text.Encoding.Default.GetString(pkg);
            Object  msg = Serializer.Deserialize<Message>(str);
            Message obj = msg as Message;
            Code = obj.Code;
            Data = obj.Data;
        }
    }

    class Bumper
    {
        const int PORT_NUMBER = 15000;

        static private Dictionary<IPAddress, string> m_nicknames = null;
        static public EventHandler evhnkm = null;
       
        static public IEnumerable<string> NickNames
        {
            get
            {
                foreach (KeyValuePair<IPAddress, string> entry in m_nicknames)
                {
                    bool is_me = false;

                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress my_ip in host.AddressList)
                    {
                        if (my_ip.AddressFamily == AddressFamily.InterNetwork && my_ip.Equals(entry.Key))
                        {
                            is_me = true;
                        }
                    }

                    string postfix = (is_me ? " (me)" : "");
                    yield return entry.Value + postfix;
                }
            }
        }

        static public void Start()
        {
            try
            {
                m_nicknames = new Dictionary<IPAddress, string>();
                Logger.Post("Started listening..");
                StartListening();

                NickName = "Anonymous";
            }
            catch 
            {
                Logger.Post("Failed to initialize. Another instance running?");
            }
        }

        static public void Stop()
        {
            try
            {
                udp.Close();
                Logger.Post("Stopped listening..\r\n");
            }
            catch { /* don't care */ }
        }

        static private readonly UdpClient udp = new UdpClient(PORT_NUMBER);
        static IAsyncResult ar_ = null;

        static private void StartListening()
        {
            ar_ = udp.BeginReceive(Receive, new object());
        }

        static private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            byte[] bytes = udp.EndReceive(ar, ref ip);

            Message msg = new Message();
            msg.Deserialize(bytes);

            StartListening();

            switch (msg.Code)
            {
                case MSG_CODE.MSG_CODE_TEXT:
                {
                    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress my_ip in host.AddressList)
                    {
                        if (my_ip.AddressFamily == AddressFamily.InterNetwork && my_ip.Equals(ip.Address))
                        {
                            Logger.Post(ip.Address.ToString() + " Me: " + msg.Data);
                            return;
                        }
                    }

                    string name = m_nicknames.ContainsKey(ip.Address) ? m_nicknames[ip.Address] : "Anonymous";
                    Logger.Post(ip.Address.ToString() + " : " + msg.Data);
                }
                break;

                case MSG_CODE.MSG_CODE_NICKNAME:
                {
                    Logger.Post(ip.Address.ToString() + " identified as " + msg.Data);
                    m_nicknames[ip.Address] = msg.Data;
                    evhnkm(null, null);
                }
                break;

                case MSG_CODE.MSG_CODE_REQ_USER_LIST:
                {
                    Logger.Post("User listing requested by: " + ip.Address.ToString());
                    SendNickName();
                }
                break;

            }
        }

        static private string _NickName;
        static public string  NickName
        {
            get
            {
                return _NickName;
            }

            set
            {
                _NickName = value;
                SendNickName();
            }
        }
                
        static public void SendText(string message)
        {
            Message msg = new Message();
            msg.Code = MSG_CODE.MSG_CODE_TEXT;
            msg.Data = message;            

            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
            byte[] bytes = msg.Serialize();
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }

        static public void SendNickName()
        {
            Message msg = new Message();
            msg.Code = MSG_CODE.MSG_CODE_NICKNAME;
            msg.Data = NickName;

            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
            byte[] bytes = msg.Serialize();
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
}
