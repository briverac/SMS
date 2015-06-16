using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using GsmComm.GsmCommunication;
using GsmComm.PduConverter;


using System.Threading;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Reflection;

namespace SMS
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{

        private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.Label lbl_phone_status;
		private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label1;
	
		private delegate void SetTextCallback(string text);
        private CommSetting comm_settings = new CommSetting();
        private IContainer components;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.lbl_phone_status = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4});
            this.menuItem1.Text = "Otras funciones";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Borrar";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Enviar mensaje";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Recibir mensajes";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // lbl_phone_status
            // 
            this.lbl_phone_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_phone_status.BackColor = System.Drawing.Color.White;
            this.lbl_phone_status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_phone_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_phone_status.ForeColor = System.Drawing.Color.DarkBlue;
            this.lbl_phone_status.Location = new System.Drawing.Point(338, 368);
            this.lbl_phone_status.Name = "lbl_phone_status";
            this.lbl_phone_status.Size = new System.Drawing.Size(218, 23);
            this.lbl_phone_status.TabIndex = 54;
            this.lbl_phone_status.Text = "FALLA EN CONEXION CON MODEM";
            this.lbl_phone_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_phone_status.UseMnemonic = false;
            // 
            // txtOutput
            // 
            this.txtOutput.Enabled = false;
            this.txtOutput.Location = new System.Drawing.Point(88, 72);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(472, 272);
            this.txtOutput.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 24);
            this.label1.TabIndex = 56;
            this.label1.Text = "Mensajes recibidos:";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(656, 417);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.lbl_phone_status);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

	
		private void Form1_Load(object sender, System.EventArgs e)
		{
            try
            {
                string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                string ConfigFile = Path.Combine(AppPath, "App.config");
                XmlDocument oXml = new XmlDocument();
                oXml.Load(ConfigFile);
                XmlNode settings = oXml["configuration"]["appSettings"];
                Constants.setCommPort(Convert.ToInt32(settings.ChildNodes[0].Attributes["value"].Value));
                Constants.setBaudRate(Convert.ToInt32(settings.ChildNodes[1].Attributes["value"].Value));
                Constants.setTimeOut(Convert.ToInt32(settings.ChildNodes[2].Attributes["value"].Value));
                Constants.setServer(settings.ChildNodes[3].Attributes["value"].Value);
                Constants.setPhoneToNotify(settings.ChildNodes[4].Attributes["value"].Value);
                Constants.setKey(settings.ChildNodes[5].Attributes["value"].Value);
                
            }
            catch
            {
            }
            
            int port = Constants.getCommPort();
			int baudRate = Constants.getBaudRate(); 
			int timeout = GsmCommMain.DefaultTimeout;

			CommSetting.Comm_Port=port;
			CommSetting.Comm_BaudRate=baudRate;
			CommSetting.Comm_TimeOut=timeout;

			Cursor.Current = Cursors.WaitCursor;
			CommSetting.comm = new GsmCommMain(port, baudRate, timeout);
			Cursor.Current = Cursors.Default;
			CommSetting.comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
			CommSetting.comm.MessageReceived+=new MessageReceivedEventHandler(comm_MessageReceived);

			bool retry;
			do
			{
				retry = false;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					CommSetting.comm.Open();
					Cursor.Current = Cursors.Default;
				}
				catch(Exception)
				{
					Cursor.Current = Cursors.Default;
					if (MessageBox.Show(this, "Unable to open the port.", "Error",
						MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
						retry = true;
					else
					{
						Close();
						return;
					}
				}
			}
			while(retry);

		}


		private delegate void ConnectedHandler(bool connected);
				
		private void OnPhoneConnectionChange(bool connected)
		{
			lbl_phone_status.Text="CONEXION CON MODEM EXITOSA";
            CommSetting.comm.DeleteMessages(DeleteScope.All, PhoneStorageType.Sim);
            SmsSubmitPdu pdu;
            try
            {
                pdu = new SmsSubmitPdu("Se inicio el servidor de SMS", Constants.getPhoneToNotify(), "");
                CommSetting.comm.SendMessage(pdu);
            }
            catch
            {
                MessageBox.Show("No hay saldo recargue la línea");
            }
		}

		
		private void comm_MessageReceived(object sender, GsmComm.GsmCommunication.MessageReceivedEventArgs e)
		{
			MessageReceived();
		}

		private void comm_PhoneConnected(object sender, EventArgs e)
		{
			this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { true });
		}


		private string GetMessageStorage()
		{
			string storage = string.Empty;
			storage = PhoneStorageType.Sim;
			
			if (storage.Length == 0)
				throw new ApplicationException("Unknown message storage.");
			else
				return storage;
		}


		private void MessageReceived()
		{
			Cursor.Current = Cursors.WaitCursor;
			string storage = GetMessageStorage();

			DecodedShortMessage[] messages = CommSetting.comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, storage);
			foreach(DecodedShortMessage message in messages)
			{
				Output(string.Format("Message status = {0}, Location = {1}/{2}",
					StatusToString(message.Status),	message.Storage, message.Index));
				ShowMessage(message.Data);
				Output("");
			}
			
			Output(string.Format("{0,9} messages read.", messages.Length.ToString()));
			Output("");
		}


		private string StatusToString(PhoneMessageStatus status)
		{
			// Map a message status to a string
			string ret;
			switch(status)
			{
				case PhoneMessageStatus.All:
					ret = "All";
					break;
				case PhoneMessageStatus.ReceivedRead:
					ret = "Read";
					break;
				case PhoneMessageStatus.ReceivedUnread:
					ret = "Unread";
					break;
				case PhoneMessageStatus.StoredSent:
					ret = "Sent";
					break;
				case PhoneMessageStatus.StoredUnsent:
					ret = "Unsent";
					break;
				default:
					ret = "Unknown (" + status.ToString() + ")";
					break;
			}
			return ret;
		}


        private void Output(string text)
		{
			if (this.txtOutput.InvokeRequired)
			{
				SetTextCallback stc = new SetTextCallback(Output);
				this.Invoke(stc, new object[] { text });
			}
			else
			{
				txtOutput.AppendText(text);
				txtOutput.AppendText("\r\n");
			}
		}


		private void ShowMessage(SmsPdu pdu)
		{
			if (pdu is SmsSubmitPdu)
			{
				// Stored (sent/unsent) message
				SmsSubmitPdu data = (SmsSubmitPdu)pdu;
				Output("SENT/UNSENT MESSAGE");
				Output("Recipient: " + data.DestinationAddress);
				Output("Message text: " + data.UserDataText);
				Output("-------------------------------------------------------------------");
				return;
			}
			if (pdu is SmsDeliverPdu)
			{
				// Received message
				SmsDeliverPdu data = (SmsDeliverPdu)pdu;
				Output("Mensaje Recibido");
				Output("Emisor: " + data.OriginatingAddress);
				Output("Hora de envio: " + data.SCTimestamp.ToString());
				Output("Texto del mensaje: " + data.UserDataText);
				Output("-------------------------------------------------------------------");
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(Constants.getServer() + "/api/v1/sms/create");
                    var postData = MessageToJson(data.SCTimestamp.ToString(),data.OriginatingAddress, data.UserDataText,DateTime.Now.Ticks.ToString());
                    var data2 = Encoding.UTF8.GetBytes(postData);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.ContentLength = data2.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data2, 0, data2.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    response.Close();
                }
                catch (System.Net.WebException wx)
                {
                    MessageBox.Show(wx.Message);
                }
                CommSetting.comm.DeleteMessages(DeleteScope.All, PhoneStorageType.Sim);
				return;
			}
			if (pdu is SmsStatusReportPdu)
			{
				// Status report
				SmsStatusReportPdu data = (SmsStatusReportPdu)pdu;
				Output("STATUS REPORT");
				Output("Recipient: " + data.RecipientAddress);
				Output("Status: " + data.Status.ToString());
				Output("Timestamp: " + data.DischargeTime.ToString());
				Output("Message ref: " + data.MessageReference.ToString());
				Output("-------------------------------------------------------------------");
				return;
			}
			Output("Unknown message type: " + pdu.GetType().ToString());
		}

		private void mnu_send_click(object sender, System.EventArgs e)
		{
			Send send_sms=new Send();
			send_sms.Show();
		}


		private void mnu_read_click(object sender, System.EventArgs e)
		{
			Receive receice_sms=new Receive();
			receice_sms.Show();
		}


		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Clean up comm object
			if (CommSetting.comm != null)
			{
				// Unregister events
				CommSetting.comm.PhoneConnected -= new EventHandler(comm_PhoneConnected);
				CommSetting.comm.MessageReceived -= new MessageReceivedEventHandler(comm_MessageReceived);
				
				// Close connection to phone
				if (CommSetting.comm != null && CommSetting.comm.IsOpen())
					CommSetting.comm.Close();

				CommSetting.comm = null;
			}
		}

		private void mnudelete_Click(object sender, System.EventArgs e)
		{
			Delete delete=new Delete();
			delete.Show();
		}
        private string MessageToJson(string pSmscTimestamp, string pMo, string pBody, string pRequestTimestamp)
        {
            JObject user =
                new JObject(
                    new JProperty("smscTimestamp", pSmscTimestamp),
                    new JProperty("mo", pMo),
                    new JProperty("body", pBody),
                    new JProperty("requestTimestamp", pRequestTimestamp),
                    new JProperty("key", Constants.getKey()));
            string result = user.ToString();
            return result;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
           
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Delete d = new Delete();
            d.Visible = true;
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            Send s = new Send();
            s.Visible = true;
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            Receive r = new Receive();
            r.Visible = true;
        }
	
	}
}
