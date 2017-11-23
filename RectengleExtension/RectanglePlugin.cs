using System;
using System.Drawing;
using ExtensibleApp.PluginEngine.Interface;

namespace RectangleExtension
{
    [Serializable]
    public class RectanglePlugin : IGraphicPlugin
    {
        public string Name => "RectangleExtension";

        public string MenuItemName => "Create a rectangle";

        /// <summary>
        /// Draw rectangle in bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        public void DoWork(Bitmap bitmap)
        {
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.FillRectangle(Brushes.Red, new Rectangle(0, 0, 100, 100));
            }
        }
    }
}
