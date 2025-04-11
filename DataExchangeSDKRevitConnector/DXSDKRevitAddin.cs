using Autodesk.DataExchange.Core.Interface;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataExchangeSDKRevitConnector
{
    public class DXSDKRevitAddin:IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        internal static string _installationPath;
        private AppDomain _appDomain;
        public static ILogger Logger;
        public Result OnStartup(UIControlledApplication application)
        {
            var connectorInstallationDir = new System.Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            _installationPath = Path.GetDirectoryName(connectorInstallationDir);
            _appDomain = AppDomain.CurrentDomain;
            _appDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("DXSDKRevitConnector");

            // Create a push button to trigger a command add it to the ribbon panel.
            PushButton pushButton = ribbonPanel.AddItem(new PushButtonData("cmdDXSDKRevitConnector", "DXSDK Revit Connector", @"D:\repo\DataExchangeSDKRevitConnector\DataExchangeSDKRevitConnector\bin\Debug\DataExchangeSDKRevitConnector.exe", "DataExchangeSDKRevitConnector.DXSDKRevitCommand")) as PushButton;

            // Optionally, other properties may be assigned to the button
            // a) tool-tip
            pushButton.ToolTip = "DataExchangeSDKRevitConnector";

            // b) lar
            return Result.Succeeded;
        }
        public static void Main(string[] args)
        {
            //Main method so that application compiles
        }

        /// <summary>
        /// Assembly resolve event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            try
            {
                var name = GetAssemblyName(args);
                var dllPath = Path.Combine(_installationPath, name + ".dll");
                if (File.Exists(dllPath))
                {
                    Logger?.Debug("Loading assembly " + args.Name);
                    assembly = Assembly.LoadFile(dllPath);
                }

            }
            catch (Exception e)
            {
                Logger?.Debug("Failed to load assembly " + args.Name);
                Logger?.Error(e);
            }
            return assembly;
        }

        /// <summary>
        /// Get assembly name
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string GetAssemblyName(ResolveEventArgs args)
        {
            string name;
            if (args.Name.IndexOf(",") > -1)
            {
                name = args.Name.Substring(0, args.Name.IndexOf(","));
            }
            else
            {
                name = args.Name;
            }
            return name;
        }
    }
}
