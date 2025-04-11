using Autodesk.DataExchange;
using Autodesk.DataExchange.Core.Interface;
using Autodesk.DataExchange.Interface;
using Autodesk.DataExchange.OpenAPI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExchangeSDKRevitConnector
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DXSDKRevitCommand : IExternalCommand
    {
        IClient client;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string appName = "DXSDKRevitReadConnector";
            SDKOptionsDefaultSetup sdkOptionsDefaultSetup = new SDKOptionsDefaultSetup()
            {
                ClientId = "",
                CallBack = "",
                ApplicationName = appName
            };
            client = new Autodesk.DataExchange.Client(sdkOptionsDefaultSetup);
            DXSDKConnectorBaseModel dxsdkConnectorBaseModel = new DXSDKConnectorBaseModel(client);
            //baseExchange = dummyModel;
            DXSDKRevitAddin.Logger =  sdkOptionsDefaultSetup.Logger;
            Autodesk.DataExchange.UI.Configuration uiConfiguration = new Autodesk.DataExchange.UI.Configuration();
            uiConfiguration.ConnectorVersion = "1.0.0";
            uiConfiguration.HostingProductID = "Revit";
            uiConfiguration.HostingProductVersion = "2023.2";
            uiConfiguration.LogLevel = Autodesk.DataExchange.Core.Enums.LogLevel.Debug;
            if (uiConfiguration.LogLevel == Autodesk.DataExchange.Core.Enums.LogLevel.Debug)
                SetDebugLogLevel(sdkOptionsDefaultSetup.Logger);
            var application = new Autodesk.DataExchange.UI.Application(dxsdkConnectorBaseModel, uiConfiguration);
            application.Show();
            return Result.Succeeded;
        }

        private void SetDebugLogLevel(ILogger logger)
        {
            logger?.SetDebugLogLevel();
            (client as Autodesk.DataExchange.Client)?.EnableHttpDebugLogging();
        }
    }
   
}
