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
    public partial class CloudClip : Form
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
        public CloudClip()
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
                String text = clip.First();
                clip.RemoveFirst();
                loadClipBoard();
            }
            
            

        }
       
    //set head of clip list as item in system clipboard
        void loadClipBoard()
        {
            if (this.tbpMisc.InvokeRequired)
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
            if (this.tbpMisc.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(updateLB);
                this.Invoke(d, new object[] { });
            }
            else
            {
                tbpMisc.Items.Clear();
                foreach (string element in clip) {
                    tbpMisc.Items.Add(element);
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
            if (tbpMisc.SelectedItem == null) {
                return;
            }
            string selection = tbpMisc.GetItemText(tbpMisc.SelectedItem); //set selected index
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

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            int removedNum = removeAllButFirst();
            clip.RemoveFirst();
            updateLB();
            MessageBox.Show(++removedNum + " clips removed");
        }

        private void btnClearAllButSelected_Click(object sender, EventArgs e)
        {
            int removedNum = removeAllButFirst();
            updateLB();
            MessageBox.Show(removedNum + " clips removed");
        }

        private int removeAllButFirst()
        {
            int removedNum = 0;
            if (clip.First == null)
            {
                MessageBox.Show("There are no clips available");
                return 0;
            }
            while (clip.First.Next != null)
            {
                clip.RemoveLast();
                removedNum++;
            }

            return removedNum;
        }
    }
}
