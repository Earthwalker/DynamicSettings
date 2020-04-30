namespace Ini
{
    internal class IniKey
    {
        private readonly string _value;
        private readonly string _comment;

        public IniKey(string name)
        {
            Name = name;
            _value = string.Empty;
            _comment = string.Empty;
        }

        public IniKey(string name, string value, string comment = "")
        {
            Name = name;
            _value = value;
            _comment = comment;
        }

        public IniKey(string[] data)
        {
            Name = data[0].Trim();

            if (data.Length >= 2)
                _value = data[1].Trim();
            else
                _value = string.Empty;

            if (data.Length >= 3)
                _comment = data[2].Trim();
            else
                _comment = string.Empty;
        }

        public string Name { get; }

        public string ToFormattedString => Name + "=" + _value + " ;" + _comment;

        public string[] ToArray => new string[] { Name, _value, _comment };
    }
}