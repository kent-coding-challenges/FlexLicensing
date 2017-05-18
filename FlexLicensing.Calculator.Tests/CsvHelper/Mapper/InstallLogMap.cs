using CsvHelper.Configuration;
using FlexLicensing.Calculator.Models;

namespace FlexLicensing.Calculator.Tests.CsvHelper.Mapper
{
    /// <summary>
    ///     Mapper class to map CSV header names to object properties.
    /// </summary>
    public class InstallLogMap : CsvClassMap<InstallLog>
    {
        public InstallLogMap()
        {
            Map(m => m.ApplicationID).Name("ApplicationID");
            Map(m => m.ComputerID).Name("ComputerID");
            Map(m => m.UserID).Name("UserID");
            
            // Default Enum conversion mapper is not case sensitive.
            // Hence, no custom mapper is required for ComputerType.
            Map(m => m.ComputerType).Name("ComputerType");
        }
    }
}
