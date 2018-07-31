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

namespace Revai_Jak_OPP2AnotherLang_AS3
{
    public partial class Client1 : Form
    {
        NamedPipeServerStream serverPipe = new NamedPipeServerStream("serverPipe"); //Pipe 1 - Client 1 sends to client 2
        NamedPipeClientStream clientPipe = new NamedPipeClientStream("serverPipe2"); //Pipe 2 - Client 2 sends to client 1

        delegate void setlstBoxCallBack(string text); //Creates a callback delegate to detect when the thread needs to invoke the main thread control
        Thread readThread = null; //Creates a thread

        //Creates the StreamWriters to write the text files
        private StreamWriter date;
        private StreamWriter previous;
        private StreamWriter error;

        string currentDate = DateTime.Today.ToShortDateString(); //String to retrieve the current date/time
        string currentTime = DateTime.Now.ToShortTimeString();

        public Client1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); //Closes the form
        }

        private void btnStartChat_Click(object sender, EventArgs e)
        {
            this.Text = "Waiting for Connection....";
            serverPipe.WaitForConnection();

            lblxChat.Items.Clear(); //This is here to clear any text before the session has started incase the user clicks previous conversation before starting the session

            this.Text = "Client 1 - Online!";
            btnStartChat.Enabled = false;
            btnSend.Enabled = true;
            txtSend.Focus();
            txtSend.Text = "Send a message..."; 
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string msg = "Client 1: " + txtSend.Text; //Sends a message from the text box the user entered the information in
            string strDateMsg = "(" + currentDate + " " + currentTime + ")" + " " + msg; //Builds a string based on the current date/time and message
            
            msg.Trim(); //Trims any extra spaces off the ends of the messages
            strDateMsg.Trim();

            lblxChat.Items.Add(strDateMsg); //Add the string to the list box
            txtSend.Clear(); //Clears the text box once a message has been sent

            Byte[] msgByte = System.Text.Encoding.GetEncoding("windows-1256").GetBytes(msg); //Converts the string to an array of bytes and encodes it to windows format
                      
            try
            {
                serverPipe.Write(msgByte, 0, msg.Length); //Writes the message to the server buffer 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Error writing to serverPipe.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";

                error.WriteLine(strErrorMsg); //Writes the message to the textfile
                error.WriteLine(" ");
            }
        }

        private void Client1_Load(object sender, EventArgs e)
        {
            try
            {
                error = new StreamWriter("../../../errorClient1.log"); //Creates a new StreamWriter to write any errors to a textfile
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Something went wrong while trying to open the file.");
                
                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";

                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                clientPipe.Connect(); //Connects to client 2's serverPipe
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Connection Error.");
                
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
                MessageBox.Show(ex.ToString(), "Client 1: Thread could not start.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";

                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            try
            {
                date = new StreamWriter("../../../dateTimeClient1.txt"); //Creates a new StreamWriter to write the current chat session to a textfile
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Something went wrong while trying to open the file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }
        }

        public void read() //Method to read from the serverpipe and write to the textbox
        {
            while (clientPipe.IsConnected)
            {
                Byte[] ClientByte;
                ClientByte = new Byte[1000]; //It can retrieve 1000 bytes 

                //Bugs the output to textfiles as it fills any empty Bytes in the array to become spaces
                //for (int i = 0; i < ClientByte.Length; i++)
                //{
                //    ClientByte[i] = 0x20; //Initialises the byte array with the hexadecimal value of space
                //}

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
                    MessageBox.Show(ex.ToString(), "Error: Client 1 List box.");

                    int linenum = 0;
                    linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                    string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                       + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                    
                    error.WriteLine(strErrorMsg);
                    error.WriteLine(" ");
                }
            }
            //clientPipe.Flush();
            this.lblxAdd("Client 2 has disconnected.");
            clientPipe.Close(); //Close the connection
        }

        //Method to add text to the listbox, used in the Thread
        private void lblxAdd(string text)
        {
            if (this.lblxChat.InvokeRequired) //Compares the thread ID of the calling thread to the thread ID of the creating thread, returns true if different
            {
                setlstBoxCallBack d = new setlstBoxCallBack(lblxAdd); //Creates a new delegate loopback
                this.Invoke(d, new object[] { text }); //Executes the specified delegate on the thread that owns the object
            }
            else
            {
                this.lblxChat.Items.Add(text); //If the calling thread is the same as the thread that created the object then it is set directly
            }
        }

        private void Client1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            
        }

        private void Client1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                WriteToFile(lblxChat, date); //Write to file method includes the list box and StreamWriter you are writing to

                previous = new StreamWriter("../../../previousConvoClient1.txt");
                WriteToFile(lblxChat, previous);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Could'nt write to file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }

            //CLOSE ALL CONNECTIONS
            try
            {
                serverPipe.Close(); //Closes the serverPipe
                readThread.Abort(); //Closes the thread  
                date.Close();
                previous.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Unable to close the connections.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }
            finally
            {
                MessageBox.Show("All connections have been closed.", "Client 1: Connection Closed");
                error.Close(); //Closes the Error log stream writer
            }            
        }

        private void previousConversationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] previousArray = File.ReadAllLines("../../../previousConvoClient1.txt"); //Reads all lines from the text file into an array
                lblxChat.Items.Clear();
                lblxChat.Items.AddRange(previousArray); //Adds the array to the list box
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Client 1: Something went wrong while trying to read file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }            
        }

        private void viewErrorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                error.Close(); //Closes the error writer to read from it
                string[] errorArray = File.ReadAllLines("../../../errorClient1.log");

                if (errorArray.Length == 0) //If there is nothing in the array, which means no errors have been written to the error log
                {
                    lblxChat.Items.Clear();
                    lblxChat.Items.Add("No errors to display");
                    error = new StreamWriter("../../../errorClient1.log"); //Reopens the error writer if there is nothing to read from
                }
                else
                {
                    lblxChat.Items.Clear();
                    lblxChat.Items.AddRange(errorArray); //Adds the array to the list box
                    error = new StreamWriter("../../../errorClient1.log"); //Recconnects the error log writer
                }
            }
            catch (Exception ex)
            {
                error = new StreamWriter("../../../errorClient1.log");
                MessageBox.Show(ex.ToString(), "Client 1: Something went wrong while trying to read file.");

                int linenum = 0;
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                string strErrorMsg = "(" + currentDate + " " + currentTime + ")" + "+" + ex.ToString()
                                   + "\n\n" + "Line: " + linenum + "\n\n" + "----- End Of Error -----";
                
                error.WriteLine(strErrorMsg);
                error.WriteLine(" ");
            }            
        }

        //Method to write out each line from the list box
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
