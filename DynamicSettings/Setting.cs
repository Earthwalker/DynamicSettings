namespace DynamicSettings
{
    public class Setting
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Comment { get; set; }

        public string Section { get; set; }

        public Setting(string name, string value, string comment, string section)
        {
            Name = name;
            Value = value;
            Comment = comment;
            Section = section;
        }

        public Setting(string[] data)
        {
            Name = data[0];
            Value = data[1];

            if (data.Length >= 3)
                Comment = data[2];
            else
                Comment = string.Empty;

            if (data.Length >= 4)
                Section = data[3];
            else
                Section = string.Empty;
        }
    }
}