using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Info;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// </summary>
    public class OrientationSupport: BaseCommand
    {
        /// <summary>
        /// </summary>
        public void applyOrientationSettings(string options)
        {
            System.Diagnostics.Debug.WriteLine("options for plugin: " + options);

            var resolution = (Size)DeviceExtendedProperties.GetValue("PhysicalScreenResolution");
            var width = resolution.Width.ToString();
            var height = resolution.Height.ToString();
            var result = "{\"width\":\"" + width + "\",\"height\":\"" + height + "\"}";

            System.Diagnostics.Debug.WriteLine("result from plugin: " + result);
            //DispatchCommandResult(new PluginResult(PluginResult.Status.OK, result));
        }
    }
}


