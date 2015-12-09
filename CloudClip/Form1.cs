using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

namespace CloudClip
{
    public partial class Form1 : Form
    {
        //true or false for if session is connected
        Boolean isConnected = false;
        LinkedList<string> clip = new LinkedList<string>(); //list of items in clipboard(text only)
        // String url = "http://jbosswildfly-rwjames64.rhcloud.com/CloudClipServer/Service?method=";
        String url = "http://159.203.86.104:8080/CloudClipServer/Service?method="; // production
        // String url = "http://192.168.0.105:8080/CloudClipServer/Service?method="; // qa
        // String url = "http://localhost:8080/CloudClipServer/Service?method="; // dev
        String sessionKey;
        String uuid;
        Thread fetchThread;

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback();

        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Removes the given window from the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Sent when the contents of the clipboard have changed.
        /// </summary>
        private const int WM_CLIPBOARDUPDATE = 0x031D;


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                IDataObject iData = Clipboard.GetDataObject();      // Clipboard's data.

                /* Depending on the clipboard's current data format we can process the data differently. 
                 * Feel free to add more checks if you want to process more formats. */
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    string text = (string)iData.GetData(DataFormats.Text);
                    // do something with it
                    if (clip.Contains(text) == false)
                    {
                        clip.AddFirst(text);
                        updateLB();
                        MessageServerAdd(text);
                    }
                         

                }
                else if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);

                    // do something with it
                }
            }
        }


        //initalize form
        public Form1()
        {
            InitializeComponent();
            AddClipboardFormatListener(this.Handle);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveClipboardFormatListener(this.Handle);     // Remove our window from the clipboard's format listener list.
            Disconnect();
        }



        // Methods





        //remove Item at head of list clip & load clipboard with new value
        void removeClip()
        {
            if (clip.Count > 0)
            {
                String text = clip.First();
                clip.RemoveFirst();
                loadClipBoard();
                MessageServerRemove(text);
            }
            
            

        }
       
    //set head of clip list as item in system clipboard
        void loadClipBoard()
        {
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(loadClipBoard);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (clip.Count > 0)
                {
                    Clipboard.SetText(clip.First());
                }
            }            
        }


        //update list box
        void updateLB()
        {
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateLB);
                this.Invoke(d, new object[] { });
            }
            else
            {
                listBox1.Items.Clear();
                foreach (string element in clip) {
                    listBox1.Items.Add(element);
                }
            }
        }

        void setConnectButtonText()
        {
            if (this.connectButton.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setConnectButtonText);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (isConnected)
                {
                    connectButton.Text = "Disconnect";
                }
                else
                {
                    connectButton.Text = "Start/Connect";
                }
            }
        }

        // shiftup currently not used
        void shiftUp()
        {
            LinkedListNode<string> temp = clip.First;
            clip.RemoveFirst();
            clip.AddLast(temp);
            updateLB();
            loadClipBoard();
            
           

        }

        //shift down currently not used
        void shiftDown()
        {
            LinkedListNode<string> temp = clip.Last;
            clip.RemoveLast();
            clip.AddFirst(temp);
            updateLB();
            loadClipBoard();
        }

        //buttton presses
       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selection = listBox1.GetItemText(listBox1.SelectedItem); //set selected index
            LinkedListNode<string> temp = clip.Find(selection); // set index to temp
            clip.Remove(selection); // remove original
            clip.AddFirst(temp); // add back to top
            updateLB(); //
            loadClipBoard(); // load to clipboard
            
        }

        //removes first item from clip array
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            removeClip();
            updateLB();
        }

        //connects to server
        private void Connect()
        {

            sessionKey = sessionKeyTextBox.Text;

            if (sessionKey.Length > 0 && !isConnected)
            {

                HttpWebRequest request = WebRequest.CreateHttp(url + "connect");
                request.Headers.Add("session", sessionKey);

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    JsonReader reader = new JsonTextReader(new StreamReader(response.GetResponseStream()));
                    reader.SupportMultipleContent = true;
                    reader.Read();
                    reader.Read();
                    reader.Read();
                    uuid = reader.Value.ToString();

                    Console.WriteLine(uuid);

                    clip.Clear();

                    while (reader.Read())
                    {
                        if (reader.Value != null && reader.Value.ToString() == "clip")
                        {
                            reader.Read();
                            clip.AddLast(reader.Value.ToString());
                        }
                    }

                    updateLB();
                    loadClipBoard();
                    isConnected = true;
                    setConnectButtonText();

                    fetchThread = new Thread(new ThreadStart(MonitorConnection));
                    fetchThread.Start();

                    response.Close();
                }
                catch (WebException)
                {
                    MessageBox.Show("Unable to establish a connection");
                }
            }
        }
        //disconnects from server
        private void Disconnect()
        {
            if (isConnected)
            {
                HttpWebRequest request = WebRequest.CreateHttp(url + "disconnect");
                request.Headers.Add("session", sessionKey);
                request.Headers.Add("uuid", uuid);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    isConnected = false;
                    setConnectButtonText();
                    fetchThread.Abort();
                }

                response.Close();
            }
        }

        //sends message to server to tell it a new item as been added to the array
        private void MessageServerAdd(String text)
        {
            if (isConnected)
            {
                HttpWebRequest request = WebRequest.CreateHttp(url + "add");
                request.Headers.Add("session", sessionKey);
                request.Headers.Add("uuid", uuid);
                request.Method = "POST";
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.WriteLine(text);
                writer.Flush();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                response.Close();

            }
        }

        //sends message to server to tell it a new item as been removed from the array
        private void MessageServerRemove(String text)
        {
            if (isConnected)
            {
                HttpWebRequest request = WebRequest.CreateHttp(url + "remove");
                request.Headers.Add("session", sessionKey);
                request.Headers.Add("uuid", uuid);
                request.Method = "POST";
                request.Method = "POST";
                StreamWriter writer = new StreamWriter(request.GetRequestStream());
                writer.WriteLine(text);
                writer.Flush();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                response.Close();

            }
        }

        //watches connection 
        private void MonitorConnection()
        {
            while (isConnected)
            {
                HttpWebRequest request = WebRequest.CreateHttp(url + "fetch");
                request.Headers.Add("session", sessionKey);
                request.Headers.Add("uuid", uuid);
                request.KeepAlive = true;
                request.Timeout = Timeout.Infinite;
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    JsonReader reader = new JsonTextReader(new StreamReader(response.GetResponseStream()));

                    while (reader.Read())
                    {
                        if (reader.Value != null && reader.Value.ToString() == "clip")
                        {
                            reader.Read();
                            String text = reader.Value.ToString();
                            reader.Read();
                            reader.Read();
                            if (reader.Value.ToString() == "add")
                            {
                                clip.AddLast(text);
                                updateLB();
                                if (clip.Count == 1)
                                {
                                    loadClipBoard();
                                }
                            }
                            else
                            {
                                LinkedListNode<String> node = clip.Find(text);
                                if (node != null)
                                {
                                    clip.Remove(node);
                                }
                                updateLB();
                                loadClipBoard();
                            }
                        }
                    }
                    response.Close(); 
                }
                catch(WebException) // server returned an error
                {
                    isConnected = false;
                    uuid = null;
                    setConnectButtonText();

                    MessageBox.Show("Connection to server lost");
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }
    }
}
