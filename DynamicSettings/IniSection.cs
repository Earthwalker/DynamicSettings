using System.Collections.Generic;

namespace Ini
{
    internal class IniSection
    {
        private readonly List<IniKey> _keyList = new List<IniKey>();

        public string Name { get; }

        public IniKey[] Keys => _keyList.ToArray();

        public string[] ToArray
        {
            get
            {
                string[] array = new string[_keyList.Count];

                for (int i = 0; i < array.Length; i++)
                    array[i] = _keyList[i].ToFormattedString;

                return array;
            }
        }

        public IniSection(string name)
        {
            Name = name;
        }

        public void AddKey(string name)
        {
            _keyList.Add(new IniKey(name));
        }

        public bool AddKey(IniKey key)
        {
            if (!KeyExists(key.Name))
            {
                _keyList.Add(key);
                return true;
            }
            else
                return false;
        }

        public bool RemoveKey(string name)
        {
            return _keyList.Remove(_keyList.Find(key => string.CompareOrdinal(key.Name, name) == 0));
        }

        public bool RemoveKey(IniKey key)
        {
            return _keyList.Remove(key);
        }

        public bool KeyExists(string name)
        {
            return _keyList.Find(key => string.CompareOrdinal(key.Name, name) == 0) != null;
        }

        public IniKey this[int ind]
        {
            get => _keyList[ind];
            set => _keyList[ind] = value;
        }

        public IniKey this[string name]
        {
            get => _keyList.Find(key => string.CompareOrdinal(key.Name, name) == 0);
            set => _keyList[_keyList.FindIndex(key => string.CompareOrdinal(key.Name, name) == 0)] = value;
        }
    }
}