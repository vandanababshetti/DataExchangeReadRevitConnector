# DataExchangeSDKRevitConnector
Sample Revit Read connector using DXSDK
This application is Revit Read Connector using DXSDK. 

## Prerequisites
Register an app, and select the Data Management and the Data Exchange APIs. 
Note down the values of Client ID, Client Secret and Auth callback.
For more information on different types of apps, refer Application Types page.
Verify that you have access to the Autodesk Construction Cloud (ACC).
Visual Studio.
Dot NET Framework 4.8 with basic knowledge of C#.

## How to use it
Clone this repository using git clone.
Follow these instructions for installing the Data Exchange .Net SDK NuGet package in Visual Studio.
Building the solution using Visual Studio IDE
Add values for Client Id, Client Secret and Auth callback in the DXSDKRevitCommand.cs file.
Creat Addin file for it
Once you build and run it by attaching Revit application, When you click on adding it will open the URL for authentication in a web browser. You can enter your credentials in the authentication page and on successful authentication, you will see the Connector UI screen.
