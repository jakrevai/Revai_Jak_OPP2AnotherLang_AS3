using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Threading;
using System.IO;

//Author Block
//Author: Jak Revai
//Project: Revai_Jak_OPP2AnotherLang_AS3
//Description: C# Simple Chat GUI Program
//Version: 1.0

namespace Client
{
    public partial class Client2 : Form
    {
        NamedPipeClientStream clientPipe = new NamedPipeClientStream("serverPipe"); //Pipe 1 - client 1 sends to client 2
        NamedPipeServerStream serverPipe = new NamedPipeServerStream("serverPipe2"); //Pipe 2 - Client 2 sends to client 1

        delegate void setlstBoxCallBack(string text); //Creates a callback delegate to detect when the thread needs to invoke the main thread control
        Thread readThread = null; //Creates a thread

        //Creates the StreamWriters to write to the text files
        private StreamWriter date;
        private StreamWriter error;
        private StreamWriter previous;

        string currentDate = DateTime.Today.ToShortDateString();
        string currentTime = DateTime.Now.ToShortTimeString();

        public Client2()
        {
            InitializeComponent();
        }

        private void Client2_Load(object sender, EventArgs e)
        {
            try
            {
                error = new StreamWriter("../../../errorClient2.log");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Something went wrong while trying to open the file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                clientPipe.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Connection Error.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                readThread = new Thread(new ThreadStart(read)); //Creates a thread with the read method
                readThread.Start(); //Starts the thread
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Thread could not start.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                date = new StreamWriter("../../../dateTimeClient2.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Something went wrong while trying to open the file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            this.SetDesktopLocation(this.Location.X + this.Size.Width, this.Location.Y); //Sets the location of the form on startup
        }

        private void read() //Method that will be called in a seperate thread to continuously update the list box
        {
            while (clientPipe.IsConnected)
            {
                Byte[] ClientByte;
                ClientByte = new Byte[1000]; //It can retrieve 1000 bytes

                for (int i = 0; i <= ClientByte.Length - 1; i++)
                {
                    ClientByte[i] = 0x20; //Initialises the byte array
                }
                
                clientPipe.Read(ClientByte, 0, ClientByte.Length); //Pulls the information from the pipe
                
                string msgStr = System.Text.Encoding.GetEncoding("windows-1256").GetString(ClientByte); //Converts the bytes to a string
                string strDateMsg = "(" + currentDate + " " + currentTime + ")" + " " + msgStr; 

                try
                {
                    strDateMsg.Trim();
                    this.lblxAdd(strDateMsg); //Calls the method to add the string to the list box
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error: Client 2 List box.");

                    int linenum = 0;
                    linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                    string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                       + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                    
                    error.WriteLine(strErrorMsg);
                    error.WriteLine(" ");
                }                    
            }          
            this.lblxAdd("Client 1 has disconnected.");
            clientPipe.Close(); //Close the connection
        }

        private void lblxAdd(string text)
        {
            if (this.lblxChat.InvokeRequired) //Compares the thread ID of the calling thread to the thread ID of the creating thread, returns true if different
            {
                setlstBoxCallBack d = new setlstBoxCallBack(lblxAdd); //Creates a new delegate
                this.Invoke(d, new object[] { text }); //Executes the specified delegate on the thread that owns the object
            }
            else
            {//Doubles up here?
                this.lblxChat.Items.Add(text); //If the calling thread is the same as the thread that created the object then it is set directly
            }
        }

        private void btnStartChat_Click(object sender, EventArgs e)
        {
            this.Text = "Waiting for Connection....";
            serverPipe.WaitForConnection(); //Sets the server pipe to wait for a client connection

            lblxChat.Items.Clear();

            this.Text = "Client 2 - Online!";
            btnStartChat.Enabled = false;
            btnSend.Enabled = true;
            txtSend.Focus();
            txtSend.Text = "Send a message...";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string msg = "Client 2: " + txtSend.Text;
            string strDateMsg = "(" + currentDate + " " + currentTime + ")" + " " + msg;

            msg.Trim();
            strDateMsg.Trim();

            lblxChat.Items.Add(strDateMsg); //Add the string to the list box
            txtSend.Clear();

            Byte[] msgByte = System.Text.Encoding.GetEncoding("windows-1256").GetBytes(msg); //Converts the string to an array of bytes and encodes it to windows format
            
            try
            {
                serverPipe.Write(msgByte, 0, msg.Length); //Writes the message to the server buffer 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Error writing to serverPipe.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }
        }

        private void Client2_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Client2_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                WriteToFile(lblxChat, date); //Write to file method includes the list box and StreamWriter you are writing to

                previous = new StreamWriter("../../../previousConvoClient2.txt");
                WriteToFile(lblxChat, previous);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Could'nt write to file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                serverPipe.Close(); //Closes the serverPipe
                readThread.Abort(); //Closes the thread  
                date.Close();
                previous.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Unable to close the connections.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }
            finally
            {
                MessageBox.Show("All connections have been closed.", "Client 2: Connection Closed");
                error.Close(); //Closes the Error log stream writer
            }            
        }

        private void viewErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                error.Close();
                string[] errorArray = File.ReadAllLines("../../../errorClient2.log");

                if (errorArray.Length == 0) //If there is nothing in the array, which means no errors have been written to the error log
                {
                    lblxChat.Items.Clear();
                    lblxChat.Items.Add("No errors to display");
                    error = new StreamWriter("../../../errorClient2.log");
                }
                else
                {
                    lblxChat.Items.Clear();
                    lblxChat.Items.AddRange(errorArray); //Adds the array to the list box
                    error = new StreamWriter("../../../errorClient2.log"); //Recconnects the error log writer
                }
            }
            catch (Exception ex)
            {
                error = new StreamWriter("../../../errorClient2.log");
                MessageBox.Show(ex.ToString(), "Client 2: Something went wrong while trying to read file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }
        }

        private void previousConversationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] previousArray = File.ReadAllLines("../../../previousConvoClient2.txt"); //Reads all lines from the text file into an array
                lblxChat.Items.Clear();
                lblxChat.Items.AddRange(previousArray); //Adds the array to the list box
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 2: Something went wrong while trying to read file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }    
        }

        public void WriteToFile(ListBox listBox, StreamWriter writer)
        {
            string line;

            for (int i = 0; i < lblxChat.Items.Count; i++)
            {
                line = (string)lblxChat.Items[i];
                writer.WriteLine(line); //Writes out the line to the text file
                writer.Flush(); //Clears any buffer that is stored in the writer
            }
        }
    }
}
