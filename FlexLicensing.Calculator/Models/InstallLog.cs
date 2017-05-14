using FlexLicensing.Calculator.Enums;

namespace FlexLicensing.Calculator.Models
{
    /// <summary>
    ///     Holds information about an installation log / requirement.
    /// </summary>
    /// <remarks>
    ///     For simplicity, this model does not assume any foreign key impositions.
    /// </remarks>
    public class InstallLog
    {
        public uint ComputerID { get; set; }
        public uint UserID { get; set; }
        public uint ApplicationID { get; set; }
        public ComputerType ComputerType { get; set; }
    }
}
