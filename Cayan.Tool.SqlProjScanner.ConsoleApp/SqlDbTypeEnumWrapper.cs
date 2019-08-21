namespace Cayan.Tool.SqlProjScanner.ConsoleApp
{
    using System.Data;

    public static class SqlDbTypeEnumWrapper
    {
        public static string ToUpperString(this SqlDbType sqlDbType)
        {
            return sqlDbType.ToString().ToUpper();
        }
    }
}
