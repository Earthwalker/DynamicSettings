using Ini;
using System.Collections.Generic;

namespace DynamicSettings
{
    /// <summary>
    /// Configuration settings that are loaded from a file and can be accessed by others.
    /// </summary>
    public class SettingsManager
    {
        private readonly List<Setting> settingsList = new List<Setting>();

        /// <summary>
        /// Loads from an ini
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool LoadFromIni(string path)
        {
            var iniFile = new IniFile();

            if (iniFile.LoadFromFile(path))
            {
                foreach (var section in iniFile.Sections)
                {
                    foreach (var key in section.Keys)
                    {
                        var list = new List<string>(key.ToArray)
                        {
                            section.Name
                        };

                        settingsList.Add(new Setting(list.ToArray()));
                    }
                }

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Saves to an ini
        /// </summary>
        /// <param name="path"></param>
        public void SaveToIni(string path)
        {
            var iniFile = new IniFile();

            foreach (var setting in settingsList)
            {
                iniFile.AddSection(setting.Section);
                iniFile[setting.Section].AddKey(new IniKey(setting.Name, setting.Value, setting.Comment));
            }

            iniFile.SaveToFile(path);
        }

        /// <summary>
        /// Gets the data of a setting
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The setting if it exists, otherwise, an empty setting</returns>
        public Setting GetSetting(string name)
        {
            return settingsList.Find(setting => string.CompareOrdinal(setting.Name, name) == 0);
        }

        /// <summary>
        /// Modifies an existing setting or adds a new one
        /// </summary>
        /// <param name="newSetting"></param>
        public void ModifySetting(Setting newSetting)
        {
            var setting = settingsList.Find(item => string.CompareOrdinal(item.Name, newSetting.Name) == 0);

            if (setting == null)
                settingsList.Add(newSetting);
            else
            {
                if (!string.IsNullOrEmpty(newSetting.Value))
                    setting.Value = newSetting.Value;
                if (!string.IsNullOrEmpty(newSetting.Comment))
                    setting.Comment = newSetting.Comment;
                if (!string.IsNullOrEmpty(newSetting.Section))
                    setting.Section = newSetting.Section;
            }
        }

        public List<Setting> GetSettings()
        {
            return settingsList;
        }

        public List<Setting> GetSettings(string section)
        {
            return settingsList.FindAll(s => string.CompareOrdinal(s.Section, section) == 0);
        }
    }
}