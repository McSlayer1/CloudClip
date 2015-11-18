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
using Newtonsoft.Json;
using System.Net;

namespace CloudClip
{
    
    public partial class Form1 : Form
    {
        LinkedList<string> clip = new LinkedList<string>(); //list of items in clipboard(text only)
        string url = "http://159.203.86.104:8080/CloudClipServer/Service?method=";


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
                    }
                         

                }
                else if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);
                    // do something with it
                }
            }
        }



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

        }



        // Methods





        //remove Item at head of list clip & load clipboard with new value
        void removeClip()
        {
            if (clip.Count > 0)
            {
                clip.RemoveFirst();
                loadClipBoard();
            }
            
            

        }
       
    //set head of clip list as item in system clipboard
        void loadClipBoard()
        {
            if (clip.Count > 0)
            {
                Clipboard.SetText(clip.First());
            }
            
        }


        //update list box
        void updateLB()
        {
            listBox1.Items.Clear();
            foreach (string element in clip) {
                listBox1.Items.Add(element);
            }
           // string json = JsonConvert.SerializeObject(clip.ToArray());
           // System.IO.File.WriteAllText(@"test.txt", json);

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

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            removeClip();
            updateLB();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://159.203.86.104:8080/CloudClipServer/Service?method=connect");
            string session = textBox1.Text;
            request.Headers.Add("session", session );
            WebResponse response = request.GetResponse();

            JsonReader jReader = new JsonTextReader(new StreamReader(response.GetResponseStream()));
            
            while (jReader.Read())
            {
                Console.WriteLine(jReader.Value);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
