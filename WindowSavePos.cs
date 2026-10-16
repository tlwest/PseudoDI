using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Ticker
{
    //
    // To use, add the following line in the Form's constructor after the line
    //                      InitializeComponent();
    //
    //          new Ticker.WindowSavePos(this, @"Software\CTV\<program name>");
    //
    public class WindowSavePos
    {
        public const bool SET_SIZE = true;

        // event info that allows form to persist extra window state data
        private Form parent;
        private string registryPath;
        private int normalLeft;
        private int normalTop;
        private int normalWidth;
        private int normalHeight;
        private FormWindowState windowState;
        private bool setSize;

        public WindowSavePos(Form parent, String registryPath, bool setSize)
        {
            this.parent = parent;
            this.registryPath = registryPath;
            this.setSize = setSize;

            // subscribe to parent form's events
            parent.Closing += new System.ComponentModel.CancelEventHandler(OnClosing);
            parent.Resize += new System.EventHandler(OnResize);
            parent.Move += new System.EventHandler(OnMove);
            parent.Load += new System.EventHandler(OnLoad);

            // get initial width and height in case form is never resized
            normalWidth = parent.Width;
            normalHeight = parent.Height;
        } // WindowSavePos constructor


        public WindowSavePos(Form parent, String registryPath) :
            this(parent, registryPath, false) { }


        private void OnResize(object sender, System.EventArgs e)
        {
            // save width and height
            if (parent.WindowState == FormWindowState.Normal)
            {
                normalWidth = parent.Width;
                normalHeight = parent.Height;
            }
        } // OnResize


        private void OnMove(object sender, System.EventArgs e)
        {
            // save position
            if (parent.WindowState == FormWindowState.Normal)
            {
                normalLeft = parent.Left;
                normalTop = parent.Top;
            }
            // save state
            windowState = parent.WindowState;
        } // OnMove


        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // save position, size and state
            RegistryKey key = Registry.CurrentUser.CreateSubKey(registryPath);
            key.SetValue("Left", normalLeft);
            key.SetValue("Top", normalTop);
            key.SetValue("Width", normalWidth);
            key.SetValue("Height", normalHeight);
            // check if we are allowed to save the state as minimized (not normally)
            if (windowState == FormWindowState.Minimized)
            {
                windowState = FormWindowState.Normal;
            }
            key.SetValue("WindowState", (int)windowState);
        } // OnClosing


        private void OnLoad(object sender, System.EventArgs e)
        {
            // attempt to read state from registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath);
            if (key != null)
            {
                int left = (int)key.GetValue("Left", parent.Left);
                int top = (int)key.GetValue("Top", parent.Top);
                int width = (int)key.GetValue("Width", parent.Width);
                int height = (int)key.GetValue("Height", parent.Height);
                FormWindowState windowState = 
                    (FormWindowState)key.GetValue("WindowState", (int)parent.WindowState);

                // Ensure that it's on screen.
                Rectangle formRectangle = new Rectangle(left, top, width, height);
                Screen[] screens = Screen.AllScreens;
                bool onScreen = false;
                foreach( Screen screen in screens )
                {
                    if( screen.WorkingArea.Contains( formRectangle ) )
                    {
                        onScreen = true;
                    }
                }

                if (onScreen)
                {
                    parent.Location = new Point(left, top);
                }
                else
                {
                    parent.Location = new Point(screens[0].WorkingArea.X, screens[0].WorkingArea.Y);
                }

                if (setSize)
                {
                    parent.Size = new Size(width, height);
                }
                parent.WindowState = windowState;
            } // if (key != null)
        } // OnLoad
    } // WindowSavePos class
} // Ticker namespace
