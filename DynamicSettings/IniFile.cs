using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Ini
{
    /// <summary>
    /// Holds the data of an .ini file.
    /// </summary>
    internal class IniFile
    {
        /// <summary>
        /// List of the sections in the file.
        /// </summary>
        private readonly List<IniSection> _sectionList = new List<IniSection>();

        /// <summary>
        /// Load data from a string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Whether loading was successful.</returns>
        public bool LoadFromString(string str)
        {
            try
            {
                // Create a StringReader to read the data.
                using (var reader = new StringReader(str))
                {
                    string line;
                    string section = string.Empty;

                    // Read until the end.
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line[0] == '[')
                        {
                            section = line.Remove(0, 1).Split(']')[0];
                            AddSection(section);
                        }

                        if (line[0] != ';')
                            this[section].AddKey(new IniKey(line.Split(new char[] { '=', ';' })));
                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                // Log the exception.
                Trace.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load data from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Whether loading was successful.</returns>
        public bool LoadFromFile(string path)
        {
            //check if the file exists
            if (!File.Exists(path))
                return false;

            try
            {
                //create a StreamReader to read the data
                using (var reader = new StreamReader(path))
                {
                    string line;
                    string section = string.Empty;

                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                        {
                            if (line[0] == '[')
                            {
                                section = line.Remove(0, 1).Split(']')[0];
                                AddSection(section);
                            }
                            else
                            {
                                if (line[0] != ';')
                                {
                                    var stringArray = line.Split(new char[] { '=', ';' });

                                    if (stringArray.Length > 0)
                                        this[section].AddKey(new IniKey(stringArray));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Log the exception.
                Trace.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves data to a string.
        /// </summary>
        /// <returns>Data string.</returns>
        public string SaveToString()
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    foreach (var section in _sectionList)
                    {
                        writer.WriteLine("[" + section.Name + "]");

                        for (int i = 0; i < section.ToArray.Length; i++)
                            writer.WriteLine(section.ToArray[i]);

                        writer.WriteLine();
                    }

                    return writer.ToString();
                }
            }
            catch (Exception e)
            {
                // Log the exception.
                Trace.WriteLine(e.ToString());
                return "";
            }
        }

        /// <summary>
        /// Saves data to a file.
        /// </summary>
        /// <param name="path"></param>
        public void SaveToFile(string path)
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    foreach (var section in _sectionList)
                    {
                        writer.WriteLine("[" + section.Name + "]");

                        for (int i = 0; i < section.ToArray.Length; i++)
                            writer.WriteLine(section.ToArray[i]);

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }
            catch (Exception e)
            {
                // Log the exception.
                Trace.WriteLine(e.ToString());
                return;
            }
        }

        /// <summary>
        /// Adds a new section
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Whether a new section was added.</returns>
        public bool AddSection(string name)
        {
            // Check if a section of the same name already exists.
            if (!SectionExists(name))
            {
                _sectionList.Add(new IniSection(name));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes a section of the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Whether the section was removed.</returns>
        public bool RemoveSection(string name)
        {
            return _sectionList.Remove(_sectionList.Find(section => string.CompareOrdinal(section.Name, name) == 0));
        }

        /// <summary>
        /// Checks if a section exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Whether the section exists.</returns>
        public bool SectionExists(string name)
        {
            return _sectionList.Find(section => string.CompareOrdinal(section.Name, name) == 0) != null;
        }

        public IniSection this[int ind]
        {
            get => _sectionList[ind];
            set => _sectionList[ind] = value;
        }

        public IniSection this[string name]
        {
            get => _sectionList.Find(section => string.CompareOrdinal(section.Name, name) == 0);
            set => _sectionList[_sectionList.FindIndex(section => string.CompareOrdinal(section.Name, name) == 0)] = value;
        }

        /// <summary>
        /// Gets a key of the specified name and section.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="section"></param>
        /// <returns>The key found.</returns>
        public IniKey GetKey(string name, string section = "")
        {
            if (!string.IsNullOrEmpty(section))
                return this[section][name];
            else
            {
                foreach (var sect in _sectionList)
                {
                    if (sect[name] != null)
                        return sect[name];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets an array of the sections.
        /// </summary>
        /// <returns>Array of sections.</returns>
        public IniSection[] Sections => _sectionList.ToArray();
    }
}