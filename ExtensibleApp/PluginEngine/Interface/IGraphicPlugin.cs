using System.Drawing;

namespace ExtensibleApp.PluginEngine.Interface
{
    /// <summary>
    /// Interface which all plugins must implement.
    /// </summary>
    public interface IGraphicPlugin
    {
        /// <summary>
        /// Name of the dll file without the .dll extension.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Name used in application menu.
        /// </summary>
        string MenuItemName { get; }

        /// <summary>
        /// Work to be done with the bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        void DoWork(Bitmap bitmap);
    }
}
