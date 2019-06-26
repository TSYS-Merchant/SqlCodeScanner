﻿namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReturnReportComparer : IReturnReportComparer
    {
        private List<string> _existingSps;
        private List<string> _newSps;

        public void CompareReports(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            Init(masterReport, newReport);

            CheckForMissingReturnValues(masterReport, newReport, errors);

            CheckReturnValueOrder(masterReport, newReport, errors);
        }

        private void Init(SqlReport masterReport, SqlReport newReport)
        {
            _existingSps = new List<string>();
            _newSps = new List<string>();

            foreach (var sp in masterReport.ReturnValues)
            {
                if (!_existingSps.Contains(sp.SpUniqueName))
                {
                    _existingSps.Add(sp.SpUniqueName);
                }
            }

            foreach (var sp in newReport.ReturnValues)
            {
                if (!_newSps.Contains(sp.SpUniqueName))
                {
                    _newSps.Add(sp.SpUniqueName);
                }
            }
        }

        private void CheckForMissingReturnValues(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            Parallel.ForEach(masterReport.ReturnValues, (returnValue) =>
            {
                var newReturnValue =
                    newReport.ReturnValues.Where(
                        x => x.ReturnValueNameId == returnValue.ReturnValueNameId).ToList();

                if (!newReturnValue.Any() && _existingSps.Contains(returnValue.SpUniqueName)
                                          && _newSps.Contains(returnValue.SpUniqueName))
                {
                    errors.Add($"{returnValue.ReturnValueNameId}|existing return value is missing from new code");
                }
            });
        }

        private void CheckReturnValueOrder(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            foreach (var sp in _existingSps)
            {
                var oldReturnValueList =
                    masterReport.ReturnValues.Where(x => x.SpUniqueName == sp).ToList();

                var newReturnValueList =
                    newReport.ReturnValues.Where(x => x.SpUniqueName == sp).ToList();

                if (newReturnValueList.Count < oldReturnValueList.Count)
                {
                    continue;
                }

                for (var i = 0; i < oldReturnValueList.Count; i++)
                {
                    if (oldReturnValueList[i].ReturnValueNameId != newReturnValueList[i].ReturnValueNameId)
                    {
                        errors.Add($"{oldReturnValueList[i].ReturnValueNameId}|existing return value is out of order");
                    }
                }
            }
        }
    }
}
