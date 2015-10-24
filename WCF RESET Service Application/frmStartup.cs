using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using System.ServiceModel;
using System.ServiceModel.Web;
using System.Xml;

namespace HHTD.Service
{
    public partial class frmStartup : Form
    {
        public frmStartup()
        {
            InitializeComponent();
            btnEnd.Enabled = false;
            btnStart.Enabled = true;
        }

        private WebServiceHost _host;

        private void button1_Click(object sender, EventArgs e)
        {
            _host = new WebServiceHost(typeof(Service));
            WebHttpBinding baseHttpBinding = new WebHttpBinding();
            
            baseHttpBinding.TransferMode= TransferMode.Streamed;
            baseHttpBinding.MaxBufferPoolSize = 2147483647;
            baseHttpBinding.MaxReceivedMessageSize = 2147483647;
            baseHttpBinding.MaxBufferSize = 2147483647;

            XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas();
            xmlDictionaryReaderQuotas.MaxArrayLength = 2147483647;
            xmlDictionaryReaderQuotas.MaxBytesPerRead = 2147483647;
            xmlDictionaryReaderQuotas.MaxDepth = 32;
            xmlDictionaryReaderQuotas.MaxStringContentLength = 2147483647;

            baseHttpBinding.ReaderQuotas = xmlDictionaryReaderQuotas;
                
            _host.AddServiceEndpoint(typeof(IService),baseHttpBinding,new Uri("http://127.0.0.1:81"));
            _host.Opened += _host_Opened;
            _host.Closed += _host_Closed;
            _host.Open();
        }

        void _host_Closed(object sender, EventArgs e)
        {
            btnEnd.Enabled = false;
            btnStart.Enabled = true;
        }

        void _host_Opened(object sender, EventArgs e)
        {
            btnEnd.Enabled = true;
            btnStart.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_host != null)
            {
                _host.Close();
                ((IDisposable)_host).Dispose();
            }  
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
