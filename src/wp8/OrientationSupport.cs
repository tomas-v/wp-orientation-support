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
        private const string PORTRAIT_OR_LANDSCAPE = "default";
        private const string PORTRAIT = "portrait";
        private const string LANDSCAPE = "landscape";


        private string prefOrientations = "none";

        /// <summary>
        /// Loads configuration and sets supported orientations.
        /// </summary>
        public OrientationSupport()
        {
            loadConfigPrefs();

            applyOrientationSettings(this.prefOrientations);
        }

        /// <summary>
        /// Apply orientation when plugin is auto-loaded.
        /// </summary>
        /*public override void OnInit()
        {
            applyOrientationSettings(this.prefOrientations);
        
        }*/

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
            try
            {
                PhoneApplicationPage currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content as PhoneApplicationPage;
                if (currentPage != null)
                {
                    SupportedPageOrientation supportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

                    switch (settings.ToLower())
                    {
                        case PORTRAIT:
                            supportedOrientations = SupportedPageOrientation.Portrait;
                            break;
                        case LANDSCAPE:
                            supportedOrientations = SupportedPageOrientation.Landscape;
                            break;
                        case PORTRAIT_OR_LANDSCAPE:
                            supportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Error: \"orientation\" value in config.xml is not supported!");
                            break;
                    }

                    currentPage.SupportedOrientations = supportedOrientations;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Current applicationPage not found. Error: " + e.Message);
                return;
            }
        }
    }
}