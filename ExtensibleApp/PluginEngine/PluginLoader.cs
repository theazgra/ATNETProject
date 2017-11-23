using ExtensibleApp.PluginEngine.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace ExtensibleApp.PluginEngine
{
    internal class PluginLoader
    {

        /// <summary>
        /// Dictionary of plugin - app domain. 
        /// It is used so when plugin is deleted app domain of that plugin can be unloaded.
        /// </summary>
        private Dictionary<IGraphicPlugin, AppDomain> plugins;
        private Type pluginType = typeof(IGraphicPlugin);

        public PluginLoader()
        {
            plugins = new Dictionary<IGraphicPlugin, AppDomain>();
        }

        /// <summary>
        /// Get all loaded plugins.
        /// </summary>
        /// <returns>Collection of loaded plugins.</returns>
        public ICollection<IGraphicPlugin> GetPlugins()
        {
            return plugins.Keys;
        }


        /// <summary>
        /// Try to load plugins from the base directory of the application.
        /// </summary>
        public void LoadPlugins()
        {
            string[] dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (string dllFile in dllFiles)
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(assemblyName);

                CheckAndAddAssembly(assembly);
            }
        }

        /// <summary>
        /// Check if found assembly contains class which implemenst IGraphicPlugin interface.
        /// </summary>
        /// <param name="assembly">Assemble to be checked and eventually loaded.</param>
        private void CheckAndAddAssembly(Assembly assembly)
        {
            if (assembly == null)
                return;

            foreach (Type type in assembly.GetTypes())
            {
                ///if is class and implements interface
                if (type.IsClass && (type.GetInterface(pluginType.FullName) != null))
                {
                    string path = Path.GetDirectoryName(assembly.Location);

                    ///Create new domain for the plugin.
                    Evidence evidence = AppDomain.CurrentDomain.Evidence;
                    AppDomain domain = AppDomain.CreateDomain(nameof(type), evidence);

                    if (domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName) is IGraphicPlugin obj)
                    {
                        ///Duplicate check.
                        if (GetPlugins().Count(p => p.Name == obj.Name) == 0)
                            plugins.Add(obj, domain);

                    }
                }
            }
        }

        /// <summary>
        /// Unload plugin app domain and remove it from collection.
        /// </summary>
        /// <param name="plugin">dll file of the plugin.</param>
        public void RemovePlugin(string plugin)
        {
            IGraphicPlugin pluginToRemove =
                plugins.Keys.Where(p => p.Name == plugin.Replace(".dll", string.Empty)).FirstOrDefault();

            if (pluginToRemove != null)
            {
                AppDomain.Unload(plugins[pluginToRemove]);
                plugins.Remove(pluginToRemove);
            }
        }

        /// <summary>
        /// Add a plugin to application.
        /// </summary>
        /// <param name="plugin">dll file of the plugin.</param>
        public void AddPlugin(string plugin)
        {
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(plugin);
            Assembly assembly = Assembly.Load(assemblyName);
            CheckAndAddAssembly(assembly);
        }
    }
}
