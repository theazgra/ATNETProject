using System;
using ExtensibleApp.PluginEngine.Interface;
using System.Drawing;
using System.Windows.Forms;

namespace SaveExtension
{
    [Serializable]
    public class SavePlugin : IGraphicPlugin
    {
        public string Name => "SaveExtension";

        public string MenuItemName => "Save image";

        /// <summary>
        /// Saves bitmap to file.
        /// </summary>
        /// <param name="bitmap"></param>
        public void DoWork(Bitmap bitmap)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                CheckPathExists = true,
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = ".png",
                FileName = "obrazek.png"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
