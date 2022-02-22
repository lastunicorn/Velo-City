using System;
using System.IO;
using Newtonsoft.Json;

namespace VeloCity.DataAccess.Jsonfiles
{
    public class DatabaseFile
    {
        private const string FileName = "database.json";
        private readonly string rootDirectoryPath;

        public DatabaseDocument Document { get; set; }

        public DatabaseFile(string rootDirectoryPath)
        {
            this.rootDirectoryPath = rootDirectoryPath ?? throw new ArgumentNullException(nameof(rootDirectoryPath));
        }

        public void Open()
        {
            string json = File.ReadAllText(FileName);
            Document = JsonConvert.DeserializeObject<DatabaseDocument>(json);
        }

        public void Save()
        {
            JsonSerializerSettings settings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(Document, settings);
            File.WriteAllText(FileName, json);
        }
    }
}