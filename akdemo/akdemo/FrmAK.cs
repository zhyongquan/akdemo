using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace akdemo
{
    public partial class FrmAK : Form
    {
        private TcpClient m_Client;
        private NetworkStream m_Stream;

        public FrmAK()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            int port = int.Parse(txtPort.Text);
            m_Client = new TcpClient();
            try
            {
                m_Client.Connect(System.Net.IPAddress.Parse(ip), port);
                m_Stream = m_Client.GetStream();
                Log(string.Format("Connect: IP{0}, Port{1}", ip, port));
            }
            catch (Exception ex)
            {
                Log("Exception: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string ak = string.Format("{0} {1} {2}{3}", (char)2, cmbCode.Text, txtChannel.Text, (char)3);
            byte[] buffer = Encoding.ASCII.GetBytes(ak);
            m_Stream.Write(buffer, 0, buffer.Length);
            Log("Send: " + ak);
            //
            buffer = new byte[1024];
            int len = m_Stream.Read(buffer, 0, 1024);
            byte[] bufferRecv = new byte[len];
            Array.Copy(buffer, bufferRecv, len);
            ak = Encoding.ASCII.GetString(bufferRecv);
            Log("Receive: " + ak);
        }
        private void Log(string log)
        {
            txtLog.AppendText(string.Format("{0:G}: {1}\r\n", DateTime.Now, log));
        }

        private void FrmAK_Load(object sender, EventArgs e)
        {
        }
    }
}
