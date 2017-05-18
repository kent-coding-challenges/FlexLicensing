using FlexLicensing.Calculator.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// <remarks>
        ///     Defining a unique primary key will yield a better performance
        ///      compared to defining 4 composite primary keys.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Index("IX_UniqueInstallLog", IsUnique = true, Order = 1)]
        public int ComputerID { get; set; }

        [Index("IX_UniqueInstallLog", IsUnique = true, Order = 2)]
        public int UserID { get; set; }

        [Index("IX_UniqueInstallLog", IsUnique = true, Order = 3)]
        public int ApplicationID { get; set; }

        [Index("IX_UniqueInstallLog", IsUnique = true, Order = 4)]
        public ComputerType ComputerType { get; set; }
    }
}