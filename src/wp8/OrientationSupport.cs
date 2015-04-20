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
using Windows.Graphics.Display;


namespace Cordova.Extension.Commands
{
    /// <summary>
    /// This plugin fixes missing support of the device orientation settings.
    /// </summary>
    public class OrientationSupport : BaseCommand
    {
        private string prefOrientations = "";


        public OrientationSupport()
        {
           loadConfigPrefs();
        }

        /// <summary>
        /// Apply orientation when plugin is auto-loaded.
        /// </summary>
        public override void OnInit()
        {
            applyOrientationSettings(this.prefOrientations);
        }

        /// <summary>
        /// Reads "orientation" value from the config.xml file.
        /// </summary>
        private void loadConfigPrefs()
        {
            StreamResourceInfo streamInfo = Application.GetResourceStream(new Uri("config.xml", UriKind.Relative));
            if (streamInfo != null)
            {
                using (StreamReader sr = new StreamReader(streamInfo.Stream))
                {
                    // Read Keys Collection from the xml file.
                    XDocument configFile = XDocument.Parse(sr.ReadToEnd());

                    this.prefOrientations = configFile.Descendants()
                                      .Where(x => (string)x.Attribute("name") == "orientation")
                                      .Select(x => (string)x.Attribute("value"))
                                      .FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Applies supported device orientations as set in the config.xml.
        /// </summary>
        public void applyOrientationSettings(string settings)
        {
            if (String.IsNullOrEmpty(settings))
            {
                DisplayOrientations supportedOrientations = DisplayOrientations.None;

                switch (settings.ToLower())
                {
                    case "portrait":
                        supportedOrientations = DisplayOrientations.Portrait;
                        break;
                    case "landscape":
                        supportedOrientations = DisplayOrientations.Landscape;
                        break;
                    case "default":
                        supportedOrientations = DisplayOrientations.Portrait | DisplayOrientations.Landscape;
                        break;
                    default:
                        supportedOrientations = DisplayOrientations.None;
                        System.Diagnostics.Debug.WriteLine("Error: \"orientation\" value in config.xml is not supported!");
                        break;
                }

                DisplayProperties.AutoRotationPreferences = supportedOrientations;
            }

            DispatchCommandResult();
        }
    }
}