namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParamReportComparer : IParamReportComparer
    {

        public void CompareReports(SqlReport masterReport, SqlReport newReport, List<string> errors)
        {

            Parallel.ForEach(masterReport.StoredProcedures, (masterSp) =>
            {
                var newSp =
                    newReport.StoredProcedures.FirstOrDefault(
                        x => x.SpUniqueName == masterSp.SpUniqueName);

                if (newSp == null)
                {
                    return;
                }

                CheckForRemovedParameters(masterSp, newSp, errors);
                CheckForNewNonDefaultedParameters(masterSp, newSp, errors);
                CheckParameterOrder(masterSp, newSp, errors);
                CheckParameterType(masterSp, newSp, errors);
            });

            CheckForRename(masterReport, newReport, errors);
            CheckForDuplicate(newReport, errors);
        }

        private void CheckForRemovedParameters(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            foreach (var masterParam in masterSp.Parameters)
            {
                var newParam = newSp.Parameters.FirstOrDefault(
                    x => x.ParameterName == masterParam.ParameterName);

                if (newParam == null)
                {
                    errors.Add($"{masterSp.SpUniqueName}\\{masterParam.ParameterName}|existing parameter is missing from new code");
                }
                else if (masterParam.IsDefaulted && !newParam.IsDefaulted)
                {
                    errors.Add($"{masterSp.SpUniqueName}\\{masterParam.ParameterName}|parameter is defaulted in master but not in new code");
                }
            }
        }

        private void CheckForNewNonDefaultedParameters(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            var newParams =
                newSp.Parameters.Where(
                    x => masterSp.Parameters.All(y => x.ParameterName != y.ParameterName));

            errors.AddRange(from newParam in newParams where !newParam.IsDefaulted select $"{newSp.SpUniqueName}\\{newParam.ParameterName}|new parameter has no default");
        }

        private void CheckParameterOrder(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            if (newSp.Parameters.Count < masterSp.Parameters.Count)
            {
                return;
            }

            errors.AddRange(masterSp.Parameters.Where((t, i) => t.ParameterName != newSp.Parameters[i].ParameterName)
                .Select(t => $"{masterSp.SpUniqueName}\\{t.ParameterName}|existing parameter is out of order"));
        }

        private void CheckParameterType(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            var masterBigIntParameters = masterSp.Parameters
                .Where(x => x.ParameterType.ToUpper() == SqlDbType.BigInt.ToUpperString());

            var newShrunkParameters =
                from newParam in newSp.Parameters
                join oldParam in masterBigIntParameters
                    on newParam.ParameterName equals oldParam.ParameterName
                where newParam.ParameterType.ToUpper() != SqlDbType.BigInt.ToUpperString()
                select newParam;

            errors.AddRange(newShrunkParameters.Select(shrunkParameter => $"{masterSp.SpUniqueName}\\{shrunkParameter.ParameterName}|existing BIGINT parameter was changed to {shrunkParameter.ParameterType}"));
        }

        private void CheckForRename(SqlReport masterReport,
            SqlReport newReport, List<string> errors)
        {
            var spsNotInMaster =
                newReport.StoredProcedures.Where(
                    x => masterReport.StoredProcedures.All(y => x.SpUniqueName != y.SpUniqueName));

            errors.AddRange(from sp in spsNotInMaster
                let differentCaseSp = masterReport.StoredProcedures.Where(y => sp.SpUniqueName.Equals(y.SpUniqueName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList()
                where differentCaseSp.Any()
                select $"{differentCaseSp[0].SpUniqueName}:{sp.SpName}\\|sp was renamed with different case");
        }

        private void CheckForDuplicate(SqlReport newReport, List<string> errors)
        {
            var duplicates =
                newReport.StoredProcedures.GroupBy(sp => sp.SpUniqueName)
                    .Where(spGroup => spGroup.Count() > 1)
                    .Select(dupeSps => new { UniqueName = dupeSps.Key, Count = dupeSps.Count()})
                    .ToList();

            foreach (var duplicate in duplicates)
            {
                errors.Add($"{duplicate.UniqueName}\\|sp defined {duplicate.Count} times");
            }
        }
    }
}
