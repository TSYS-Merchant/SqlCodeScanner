namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;
    using System.Linq;
    using ReportObjects;
    using Wrappers;

    public class IgnoreList
    {
        private readonly IFileWrapper _fileWrapper;

        private List<IgnoreSchemaEntry> IgnoreSchemas { get; set; }

        public IgnoreList(IFileWrapper fileWrapper,
            string ignoreListFileName)
        {
            _fileWrapper = fileWrapper;
            IgnoreSchemas = new List<IgnoreSchemaEntry>();

            LoadIgnoreList(ignoreListFileName);
        }

        public bool IsSchemaIgnored(string db, string schema)
        {
            var matchingSchema =
                IgnoreSchemas.FirstOrDefault(entry => entry.Db == db.ToLower()
                                                       && entry.Schema == schema.ToLower());

            return matchingSchema != null;
        }

        private void LoadIgnoreList(string ignoreListFileName)
        {

            if (!_fileWrapper.Exists(ignoreListFileName))
            {
                return;
            }

            var lines = _fileWrapper.ReadLines(ignoreListFileName);

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)
                    || line.Trim().StartsWith("--"))
                {
                    continue;
                }

                var entry = line.Split('.');

                if (entry.Length < 2)
                {
                    continue;
                }

                var db = entry[0].Trim();
                var schema = entry[1].Trim();

                var schemaToIgnore = new IgnoreSchemaEntry(db.ToLower(), schema.ToLower());
                IgnoreSchemas.Add(schemaToIgnore);
            }
        }
    }
}
