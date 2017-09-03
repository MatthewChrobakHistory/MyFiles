using System.Xml.Serialization;
using System.IO;

namespace MyFiles.Data
{
    public class Settings
    {
        [XmlIgnore]
        public static readonly string SettingsPath = Program.StartupPath + "settings.xml";

        public string MyFilesPath;
        public string Username;
        public string Password;

        public static Settings Load() {
            var settings = new Settings();

            if (File.Exists(Settings.SettingsPath)) {
                var xml = new XmlSerializer(settings.GetType());
                var stream = new FileStream(Settings.SettingsPath, FileMode.Open);
                settings = (Settings)xml.Deserialize(stream);
                stream.Close();
            }

            return settings;
        }

        public static void Save(Settings settings) {
            var xml = new XmlSerializer(settings.GetType());
            var stream = new StreamWriter(Settings.SettingsPath);
            xml.Serialize(stream, settings);
            stream.Close();
        }
    }
}
