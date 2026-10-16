using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace PseudoDI
{
    // 
    public partial class PseudoDI : Form
    {
        //
        // Class constants
        //
        private const String PSEUDO_DI_INIT_FILE_NAME = "PseudoDI.ini";
        private const int RETRY_TIME = 5;
        private const int BUFFER_SIZE = 1024;

        //
        // Delegates
        //
        private delegate void FeedbackDelegate(String message, Color? textColour);
        private delegate void EnableSendButtonDelegate(bool enabled);

        //
        // Object variables
        //
        private int port;
        private List<String> commandList = new List<string>();
        private Font boldFont;
        private bool applicationClosing = false;
        private StreamReader inStream;
        private StreamWriter outStream;
        private bool connected = false;
        private System.Windows.Forms.Timer clock;
        private DateTime startTime;

        public PseudoDI()
        {
            InitializeComponent();
            new Ticker.WindowSavePos(this, @"Software\CTV\PseudoDI");
        } // PseudoDI constructor

        //
        // Event Handlers
        //

        private void PseudoDI_Load(object sender, EventArgs e)
        {
            sendButton.Enabled = false;

            // Create the bold font
            Font currentFont = feedbackArea.SelectionFont;
            boldFont = new Font(currentFont.FontFamily, currentFont.Size, FontStyle.Bold);

            // Read the INI file. It consists of the port number followed by the lines to 
            // populate the ListBox.
            if (!ReadIni())
            {
                return;
            } // if (!readIni())

            commandListBox.DataSource = commandList;

            Thread t = new Thread(new ThreadStart(CreateConnector));
            t.Name = " Port Opener";
            t.Start();

            startTime = DateTime.Now;

            clock = new System.Windows.Forms.Timer();
            clock.Interval=1000;
            clock.Start();
            clock.Tick+=new EventHandler(Timer_Tick);
        } // PseudoDI_Load


        private void commandListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            commandTextBox.Text = commandList[commandListBox.SelectedIndex];
        } // commandListBox_SelectedIndexChanged


        private void sendButton_Click(object sender, EventArgs e)
        {
            String msg = commandTextBox.Text;
            Feedback(msg, Color.Green);

            // Remove the square brackets.
            int pos = msg.IndexOf(']');
            if (pos >= 0)
            {
                msg = msg.Substring(pos + 1).Trim();
            } // if (pos >= 0)

            Send(msg);

            // Advance to the next item in the list, if the original wasn't edited
            if (commandTextBox.Text == commandList[commandListBox.SelectedIndex])
            {
                int index = commandListBox.SelectedIndex;
                index = (index + 1) % commandList.Count;
                commandListBox.SelectedIndex = index;
            } //if (msg == commandList[commandListBox.SelectedIndex])

            lock(this)
            {
                startTime = DateTime.Now;
            } // lock(this)
        } // sendButton_Click


        private void PseudoDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            applicationClosing = true;
            Disconnect();
        } // PseudoDI_FormClosing


        public void Timer_Tick(object sender,EventArgs eArgs)
        {
            if (sender == clock)
            {
                lock(this)
                {
                    TimeSpan span = DateTime.Now - startTime;
                    timeTextBox.Text = span.ToString().Substring(3).Substring(0, 5);
                } // lock(this)
            } // if (sender == clock)
        } // Timer_Tick


        //
        // Private methods
        //

        private bool ReadIni()
        {
            try
            {
                String line;
                StreamReader ini = new StreamReader(PSEUDO_DI_INIT_FILE_NAME);

                do
                {
                    line = ini.ReadLine();
                    if (line == null)
                    {
                        Feedback("No port number found in INI file '" +
                                 PSEUDO_DI_INIT_FILE_NAME + "'", Color.Red);
                        return false;
                    } // if (line == null)
                    line = line.Trim();
                } while ((line.Length == 0) || line.StartsWith(";") || line.StartsWith("!"));
                try
                {
                    port = Int32.Parse(line);
                } // try
                catch (Exception)
                {
                    Feedback("Port number read from INI file '" +
                             PSEUDO_DI_INIT_FILE_NAME + 
                             "' (" + line + ") is non-numeric", Color.Red);
                    return false;
                } // catch

                while(true)
                {
                    do
                    {
                        line = ini.ReadLine();
                        if (line == null)
                        {
                            // We've reached the end, let's return.
                            if (commandList.Count == 0)
                            {
                                Feedback("No command items in the INI file '" +
                                         PSEUDO_DI_INIT_FILE_NAME + "'", Color.Red);
                            }

                            ini.Close();

                            return true;
                        } // if (line == null)

                        line = line.Trim();
                    } while ((line.Length == 0) || line.StartsWith(";") || line.StartsWith("!"));

                    commandList.Add(line);
                } // while(true)
            } // try
            catch (Exception e)
            {
                Feedback("Trouble reading INI file '" + PSEUDO_DI_INIT_FILE_NAME +
                         ": " + e.Message, Color.Red);
                return false;
            } // catch
        } // readIni method


        private void EnableSendButton(bool enabled)
        {
            if (sendButton.InvokeRequired)
            {
                sendButton.Invoke(new EnableSendButtonDelegate(EnableSendButton),
                    new Object[] { enabled });
                return;
            }
            sendButton.Enabled = enabled;
        } // EnableSendButton


        private void Feedback(String message, Color? textColour)
        {
            if (feedbackArea.InvokeRequired)
            {
                feedbackArea.Invoke(new FeedbackDelegate(Feedback),
                    new object[] { message, textColour });
                return;
            } // if (feedbackArea.InvokeRequired)

            int start = 0, end = 0;

            if (textColour != null)
            {
                start = feedbackArea.TextLength;
            }
            feedbackArea.AppendText(message + Environment.NewLine);
            end = feedbackArea.TextLength;
            if (textColour != null)
            {
                // Textbox may transform chars, so (end-start) != text.Length
                feedbackArea.Select(start, end - start);
                feedbackArea.SelectionColor = (Color)textColour;
                feedbackArea.SelectionFont = boldFont;
                feedbackArea.SelectionLength = 0; // clear
            }
            feedbackArea.Select(end, end);
            feedbackArea.ScrollToCaret();
        } // Feedback


        private void CreateConnector()
        {
            TcpClient client;

            while (true)
            {
                int attemptNumber = 0;

                client = null;

                do
                {
                    // We haven't created a connection yet, so if we've been ordered
                    // to close, simply exit now.
                    if (applicationClosing)
                    {
                        return;
                    }

                    attemptNumber++;

                    //
                    // Output Diagnostic/Log message
                    //
                    if (attemptNumber == 1)
                    {
                        Feedback("Attempting to connect to port " + port, Color.Black);
                    }
                    else
                    {
                        Feedback("Attempt #" + attemptNumber + " to connect to port " + port,
                                 Color.Black);
                    }

                    //
                    // Attempt to connect
                    //
                    try
                    {
                        client = new TcpClient("localhost", port);
                    }
                    catch (SocketException)
                    {
                        if (applicationClosing)
                        {
                            return;
                        }

                        //
                        // The attempt to connect failed.  This may be because there's simply nobody
                        // listening, or there might be a more serious problem.  For a more serious
                        // problem, turn the diagnostic red.
                        //
                        int timeToGo = RETRY_TIME;
                        do
                        {
                            Thread.Sleep(1000);
                            timeToGo -= 1;
                        } while ((timeToGo > 0) && !applicationClosing);
                    } // catch (SocketException e)
                } while (client == null);

                // If we've fallen out of the loop, then we've connected

                Encoding encoding = Encoding.GetEncoding(1252);
                try
                {
                    outStream = new StreamWriter(client.GetStream(), encoding);
                    inStream = new StreamReader(client.GetStream(), encoding, false);
                    EnableSendButton(true);
                    Feedback("Connected!", Color.Red);
                    HandleFeedbackFromTickerManager(inStream);
                }
                catch (Exception e)
                {
                    Feedback("Exception occurred trying to connect: " + e.Message, Color.Red);
                }
            } // while(true)
        } // CreateConnector


        public void HandleFeedbackFromTickerManager(StreamReader inStream)
        {
            // Create the stream used to receive inbound messages.  Use either Unicode
            // or ASCII, depending on the constructor parameter.
            String previousPartsOfMessage = "";

            connected = true;

            try
            {
                while (true)
                {
                    char[] buffer = new char[BUFFER_SIZE];
                    int len = inStream.Read(buffer, 0, BUFFER_SIZE);

                    if (len == 0)
                    {
                        Disconnect();
                        return;
                    } // if (len == 0)

                    String s = new String(buffer, 0, len);

                    // Append to the previous part of the message
                    s = previousPartsOfMessage + s;

                    // Go through the string 's', calling processMessage with each 
                    // line read from the socket.
                    while (true)
                    {
                        String message;

                        int pos;
                        int posNL = s.IndexOf('\n');
                        int posCR = s.IndexOf('\r');
                        if ((posNL == -1) && (posCR == -1))
                        {
                            break;
                        }
                        else if (posNL == -1)
                        {
                            pos = posCR;
                        }
                        else if (posCR == -1)
                        {
                            pos = posNL;
                        }
                        else
                        {
                            pos = Math.Min(posCR, posNL);
                        }

                        message = s.Substring(0, pos);
                        s = s.Substring(pos + 1);

                        if (message.Length > 0)
                        {
                            Feedback(message, Color.Blue);
                        }
                    } // while (true)

                    previousPartsOfMessage = s;
                } // while(true)
            } // try
            catch (Exception)
            {
                Disconnect();
            } // catch
        } // HandleSocket


        public void Disconnect()
        {
            lock (this)
            {
                if (connected)
                {
                    Feedback ("Disconnected", Color.Red);
                    connected = false;
                    EnableSendButton(false);
                    inStream.Close();
                    outStream.Close();
                    return;
                } // if (connected)
            } // lock (this)
        } // Disconnect


        public void Send(String message)
        {
            try
            {
                outStream.WriteLine(message);
                outStream.Flush();
            } // try
            catch (Exception)
            {
                Disconnect();
            } // catch
        } // Send

        private void openButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("PseudoDi.ini");
        } // openButton_Click
    } // PseudoDI
} // PseudoDI
