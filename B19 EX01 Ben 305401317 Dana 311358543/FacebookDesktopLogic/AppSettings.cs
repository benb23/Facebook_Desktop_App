using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FacebookAppLogic
{
    public class AppSettings
    {
        private static readonly string sr_FileLocation = Application.StartupPath + @"\appSettings.xml";

        public string LastAccessToken { get; set; }

        public Point LastWindowLocation { get; set; }

        public bool RememberUser { get; set; }

        public AppSettings()
        {
            this.LastWindowLocation = new Point(600, 300);
            this.RememberUser = false;
            this.LastAccessToken = null;
        }

        public static AppSettings LoadFromFile()
        {
            AppSettings appSettings = new AppSettings();
            if (File.Exists(sr_FileLocation) && File.ReadAllLines(sr_FileLocation).Length != 0)
            {
                using (Stream stream = new FileStream(sr_FileLocation, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    appSettings = serializer.Deserialize(stream) as AppSettings;
                }
            }

            return appSettings;
        }

        public void SaveToFile()
        {
            using (Stream stream = new FileStream(sr_FileLocation, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stream, this);
            }
        }

        public void DeleteFile()
        {
            try
            {
                if (File.Exists(sr_FileLocation))
                {
                    File.Delete(sr_FileLocation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show($@"Couldn't delete {0}.", sr_FileLocation);
            }
        }
    }
}
