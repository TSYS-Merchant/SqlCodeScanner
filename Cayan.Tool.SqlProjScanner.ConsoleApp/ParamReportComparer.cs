namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParamReportComparer : IParamReportComparer
    {
        private List<string> _existingSps;
        private List<string> _newSps;

        public void CompareReports(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            Init(masterReport, newReport);

            // Master Comparison
            CheckForRemovedParameters(masterReport, newReport, errors);
  
            // New Param Check
            CheckForNewNonDefaultedParameters(masterReport, newReport, errors);

            // Arity check
            CheckParameterOrder(masterReport, newReport, errors);

            // Check for renamed SP
            CheckForRename(errors);
        }

        private void Init(SqlReport masterReport, SqlReport newReport)
        {
            _existingSps = new List<string>();
            _newSps = new List<string>();

            foreach (var sp in masterReport.Parameters)
            {
                if (!_existingSps.Contains(sp.SpUniqueName))
                {
                    _existingSps.Add(sp.SpUniqueName);
                }
            }

            foreach (var sp in newReport.Parameters)
            {
                if (!_newSps.Contains(sp.SpUniqueName))
                {
                    _newSps.Add(sp.SpUniqueName);
                }
            }
        }

        private void CheckForRemovedParameters(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            Parallel.ForEach(masterReport.Parameters, (param) =>
            {
                var newParam =
                    newReport.Parameters.Where(
                        x => x.ParameterId == param.ParameterId).ToList();

                if (!newParam.Any() && _existingSps.Contains(param.SpUniqueName)
                                    && _newSps.Contains(param.SpUniqueName))
                {
                    errors.Add($"{param.ParameterId}|existing parameter is missing from new code");
                }
                else if (param.IsDefaulted &&
                         newParam.Count > 0 && !newParam[0].IsDefaulted)
                {
                    errors.Add($"{param.ParameterId}|parameter is defaulted in master but not in new code");
                }
            });
        }

        private void CheckForNewNonDefaultedParameters(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            var newParams =
                newReport.Parameters.Where(x => masterReport.Parameters.All(y => y.ParameterId != x.ParameterId)).ToList();

            if (newParams.Count == 0)
            {
                return;
            }

            foreach (var newParam in newParams)
            {
                if (!newParam.IsDefaulted && _existingSps.Contains(newParam.SpUniqueName))
                {
                    errors.Add($"{newParam.ParameterId}|new parameter has no default");
                }
            }
        }

        private void CheckParameterOrder(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {
            foreach (var sp in _existingSps)
            {
                var oldParamList =
                    masterReport.Parameters.Where(x => x.SpUniqueName == sp).ToList();

                var newParamList = 
                    newReport.Parameters.Where(x => x.SpUniqueName == sp).ToList();

                if (newParamList.Count < oldParamList.Count)
                {
                    continue;
                }

                for (var i = 0; i < oldParamList.Count; i++)
                {
                    if (oldParamList[i].ParameterId != newParamList[i].ParameterId)
                    {
                        errors.Add($"{oldParamList[i].ParameterId}|existing parameter is out of order");
                    }
                }
            }
        }

        private void CheckForRename(List<string> errors)
        {
            var spsNotInMaster =
                _existingSps.Where(
                    oldSp => _newSps.All(newSp => oldSp != newSp));

            foreach (var sp in spsNotInMaster)
            {
                var differentCaseSp = _newSps.Where(
                    newSp => newSp.Equals(sp, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();

                if (!differentCaseSp.Any())
                {
                    continue;
                }

                var shortSpName = differentCaseSp[0].Split('\\')[2];

                errors.Add($"{sp}:{shortSpName}\\|sp was renamed with different case");
            }
        }
    }
}
