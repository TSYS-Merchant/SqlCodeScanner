namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Wrappers;

    public class HtmlReportGenerator : IHtmlReportGenerator
    {
        private readonly IFileWrapper _fileWrapper;

        private const string NaPlaceholder = "N/A";

        private const string Css =
        @"<style>

        table {
        padding: 10px;
        font-size: 25px;
        }

        td {
	        padding: 5px;
	        border: 2px solid grey;
        }

		span {
			color: red;
		}

        </style>";

        public HtmlReportGenerator()
            :this(new FileWrapper())
        {

        }

        internal HtmlReportGenerator(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public void GenerateComparisonReport(string htmlReportPath, List<string> errors)
        {
            if (string.IsNullOrEmpty(htmlReportPath))
            {
                throw new ArgumentException("Parameter 'reportPath' cannot be null or empty");
            }

            if (errors == null)
            {
                throw new ArgumentException("Parameter 'errors' cannot be null");
            }

            var sb = new StringBuilder();

            sb.Append("<html>\r\n");
            sb.Append($"{Css}\r\n");

            sb.Append($"{DateTime.Now}<br /><br />\r\n");

            if (errors.Count == 0)
            {
                sb.Append("No errors found.<br />\r\n");
            }
            else
            {
                sb.Append("<table>\r\n");
                sb.Append("<th>Database</th>");
                sb.Append("<th>SP Name</th>");
                sb.Append("<th>Parameter Name</th>");
                sb.Append("<th>Error</th>\r\n");
                foreach (var error in errors)
                {
                    var errorParts = error.Split('|');
                    var dbInfo = errorParts[0].Split('\\');
                    var paramName =
                        string.IsNullOrEmpty(dbInfo[3]) ? NaPlaceholder : dbInfo[3];

                    sb.Append($"<tr><td>{dbInfo[0]}</td>");

                    FormatSpName(paramName, dbInfo, sb);

                    sb.Append($"<td>{paramName}</td>");
                    sb.Append($"<td>{errorParts[1]}</td></tr>\r\n");
                }

                sb.Append("</table>\r\n");
            }

            sb.Append("</html>\r\n");

            _fileWrapper.WriteAllText(htmlReportPath, sb.ToString());
        }

        private void FormatSpName(string paramName, string[] dbInfo,
            StringBuilder sb)
        {
            if (paramName == NaPlaceholder)
            {
                var spNames = dbInfo[2].Split(':');

                if (spNames.Length < 2)
                {
                    sb.Append($"<td>{dbInfo[1]}.{dbInfo[2]}</td>");
                    return;
                }

                var oldName = spNames[0];
                var newName = spNames[1];
                var formattedNewName = "";

                for(var i = 0; i < oldName.Length; i++)
                {
                    if (newName[i] != oldName[i])
                    {
                        formattedNewName += $"<span>{newName[i]}</span>";
                    }
                    else
                    {
                        formattedNewName += newName[i];
                    }
                }

                sb.Append($"<td>{dbInfo[1]}.{oldName}<br />{dbInfo[1]}.{formattedNewName}</td>");
            }
            else
            {
                sb.Append($"<td>{dbInfo[1]}.{dbInfo[2]}</td>");
            }
        }
    }
}
