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
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// </summary>
    public class OrientationSupport: BaseCommand
    {
        private int prefDelay = 12345; 


        public OrientationSupport()
        {
            LoadConfigPrefs();

            System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>inited from plugin...value: "+ this.prefDelay);
        }


        private void LoadConfigPrefs()
        {
            StreamResourceInfo streamInfo = Application.GetResourceStream(new Uri("config.xml", UriKind.Relative));
            if (streamInfo != null)
            {
                using (StreamReader sr = new StreamReader(streamInfo.Stream))
                {
                    //This will Read Keys Collection for the xml file
                    XDocument configFile = XDocument.Parse(sr.ReadToEnd());
                    
                    string configDelay = configFile.Descendants()
                                      .Where(x => (string)x.Attribute("name") == "SplashScreenDelay")
                                      .Select(x => (string)x.Attribute("value"))
                                      .FirstOrDefault();
                    int nVal;
                    prefDelay = int.TryParse(configDelay, out nVal) ? nVal : prefDelay;
                }
            }
        }

        /// <summary>
        /// </summary>
        public void applyOrientationSettings(string options)
        {
            System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>options for plugin: " + options);

            var resolution = (Size)DeviceExtendedProperties.GetValue("PhysicalScreenResolution");
            var width = resolution.Width.ToString();
            var height = resolution.Height.ToString();
            var result = "{\"width\":\"" + width + "\",\"height\":\"" + height + "\"}";

            System.Diagnostics.Debug.WriteLine(">>>>>>>>>>result from plugin: " + result);
            //DispatchCommandResult(new PluginResult(PluginResult.Status.OK, result));
        }
    }
}