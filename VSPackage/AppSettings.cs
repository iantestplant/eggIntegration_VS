using System.Configuration;

namespace TestPlant.EggPlantVSPackage
{
    public class AppSettings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        [DefaultSettingValue("localhost")]
        public string Host
        {
            get { return ((string) this["Host"]); }
            set { this["Host"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("5400")]
        public int Port
        {
            get { return (int) this["Port"]; }
            set { this["Port"] = value; }
        }

        [UserScopedSetting]
        public string Suite
        {
            get { return (string) this["Suite"]; }
            set { this["Suite"] = value; }
        }

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public bool DriveMode
        {
            get { return (bool) this["DriveMode"]; }
            set { this["DriveMode"] = value; }
        }
    }
}