using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ExtensibleApp.PluginEngine;
using ExtensibleApp.PluginEngine.Interface;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace ExtensibleApp
{
    internal partial class MainForm : Form
    {
        private FileSystemWatcher pluginWatcher;
        private PluginLoader pluginLoader;

        private bool canvasMouseDown = false;
        private Bitmap bitmap;


        /// <summary>
        /// Delegates are used because file system watcher will raise events from another thread.
        /// These events try to modify controls on this form but controls can be changed only be thread which created them.
        /// </summary>
        public delegate void AddListItem();
        private delegate void removePluginButtons();
        private delegate void addPluginButtons();

        public AddListItem addListItemDelegate;
        private removePluginButtons removePluginsDelegate;
        private addPluginButtons addPluginButtonsDelegate;

        /// <summary>
        /// Queues are used because of delegates. They will store messages are buttons to be created by main thread.
        /// </summary>
        private Queue msgQueue = new Queue();
        private Queue btnQueue = new Queue();

        public MainForm()
        {
            InitializeComponent();
            
            ///Register Debug or Trace listener.
            Debug.Listeners.Add(new MyTraceListener(Mode.Debug));
            //Trace.Listeners.Add(new MyTraceListener(Mode.Trace));

            pluginLoader = new PluginLoader();

            ///Watch over base directory for dll files.
            pluginWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            pluginWatcher.Created += PlugginAdded;
            pluginWatcher.Deleted += PlugginDeleted;
            pluginWatcher.EnableRaisingEvents = true;

            ///Bitmap used to draw stuff.
            bitmap = new Bitmap(canvas.Width, canvas.Height);

            ///Load plugins.
            pluginLoader.LoadPlugins();

            ///Register delegates.
            addListItemDelegate = AddListItemMethod;
            removePluginsDelegate = RemoveAllPluginButtons;
            addPluginButtonsDelegate = AddPlugingButtons;

            ///Set different culture info. (Default and cs are avaible.)
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB");
        }

        /// <summary>
        /// Add buttons in queue into submenu.
        /// </summary>
        private void AddPlugingButtons()
        {
            while (btnQueue.Count > 0)
            {
                miPlugins.DropDownItems.Add(btnQueue.Dequeue() as ToolStripMenuItem);
            }
        }

        /// <summary>
        /// Removes all plugin buttons.
        /// </summary>
        private void RemoveAllPluginButtons()
        {
            miPlugins.DropDownItems.Clear();
        }

        /// <summary>
        /// Write message to list box.
        /// </summary>
        public void AddListItemMethod()
        {
            while (msgQueue.Count > 0)
            {
                statusBox.Items.Add(msgQueue.Dequeue());
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogStatusMsg(strings.AppStarted + DateTime.Now.ToString());
            LogStatusMsg(string.Format(strings.PluginLoadInfo, pluginLoader.GetPlugins().Count));

            SetPlugins();
        }

        /// <summary>
        /// Creates menu items for plugins.
        /// </summary>
        private void SetPlugins()
        {
            this.Invoke(removePluginsDelegate);

            foreach (IGraphicPlugin plugin in pluginLoader.GetPlugins())
            {
                ToolStripMenuItem miPlugin = new ToolStripMenuItem(plugin.MenuItemName);
                miPlugin.Click += MiPlugin_Click;

                btnQueue.Enqueue(miPlugin);
            }

            this.Invoke(addPluginButtonsDelegate);
        }

        /// <summary>
        /// Event raised when plugin button is clicked.
        /// </summary>
        /// <param name="sender">Sender of the event. ToolStripMenuItem</param>
        /// <param name="e">EventArgs..</param>
        private void MiPlugin_Click(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Text;

            if (pluginLoader.GetPlugins().Where(p => p.MenuItemName == name).FirstOrDefault() is IGraphicPlugin plugin)
            {
                plugin.DoWork(bitmap);
                canvas.Invalidate();
            }
        }

        /// <summary>
        /// Log the message into lix box and debug or trace listeners.
        /// </summary>
        /// <param name="message"></param>
        public void LogStatusMsg(string message)
        {
            msgQueue.Enqueue(message);

            this.Invoke(addListItemDelegate);

            Debug.WriteLine(message);
        }

        /// <summary>
        /// Event raised when new dll file is added to base directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlugginAdded(object sender, FileSystemEventArgs e)
        {
            pluginLoader.AddPlugin(e.FullPath);
            SetPlugins();

            LogStatusMsg(string.Format(strings.PluginAdded, e.Name));
        }

        /// <summary>
        /// Event raised when dll file is moved out of the base directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlugginDeleted(object sender, FileSystemEventArgs e)
        {
            pluginLoader.RemovePlugin(e.Name);
            SetPlugins();

            LogStatusMsg(string.Format(strings.PluginDeleted, e.Name));
        }

      
        /// <summary>
        /// Changes mouse down status to true to allow drawing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                canvasMouseDown = true;
        }

        /// <summary>
        /// Changes mouse down status to false to forbid drawing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            canvasMouseDown = false;
        }

        /// <summary>
        /// Used to draw lines in canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (canvasMouseDown && btnDrawLine.Checked)
            {
                using (Graphics gfx = Graphics.FromImage(bitmap))
                {
                    Rectangle rect = new Rectangle(
                    e.Location.X,
                    e.Location.Y,
                    2,
                    2);

                    gfx.FillRectangle(Brushes.Black, rect);
                }
                canvas.Invalidate();
            }
            
        }


        /// <summary>
        /// Clear the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miClear_Click(object sender, EventArgs e)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.Clear(Color.White);
            }
            canvas.Invalidate();
        }
        
        /// <summary>
        /// Event raised when canvas is invalidated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, Point.Empty);
        }
    }
}
