namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using ReportObjects;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReturnReportComparer : IReturnReportComparer
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

                CheckForMissingReturnValues(masterSp, newSp, errors);
                CheckReturnValueOrder(masterSp, newSp, errors);
            });
        }

        private void CheckForMissingReturnValues(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            foreach (var masterReturnValue in masterSp.ReturnValues)
            {
                var newReturnValue = newSp.ReturnValues.FirstOrDefault(
                    x => x.ReturnValueName == masterReturnValue.ReturnValueName);

                if (newReturnValue == null)
                {
                    errors.Add($"{masterSp.SpUniqueName}\\{masterReturnValue.ReturnValueName}|existing return value is missing from new code");
                }
            }
        }

        private void CheckReturnValueOrder(StoredProcedureReport masterSp,
            StoredProcedureReport newSp, List<string> errors)
        {
            if (newSp.ReturnValues.Count < masterSp.ReturnValues.Count)
            {
                return;
            }

            errors.AddRange(masterSp.ReturnValues.Where((t, i) => t.ReturnValueName != newSp.ReturnValues[i].ReturnValueName)
                .Select(t => $"{masterSp.SpUniqueName}\\{t.ReturnValueName}|existing return value is out of order"));
        }
    }
}
