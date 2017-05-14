using System.Collections.Generic;
using System.Linq;
using FlexLicensing.Calculator.Enums;

namespace FlexLicensing.Calculator.Models
{
    /// <summary>
    ///     Dynamic rule to apply Sotware Licensing.
    /// </summary>
    public class LicenseRule
    {
        /// <summary>
        ///     Defines the maximum number of installs required per license.
        /// </summary>
        public int TotalMaxInstall { get; set; }

        /// <summary>
        ///     Dictionary which holds the maximum number of installs required per ComputerType.
        ///     Note that specifying greater value for a ComputerType then allowed in TotalMaxInstall
        ///     will yield an invalid rule.
        /// </summary>
        public Dictionary<ComputerType, uint> MaxInstallPerComputerType { get; set; }

        /// <summary>
        ///     Checks that specified rule is valid.
        /// </summary>
        /// <returns>
        ///     True if rule is valid, false otherwise.
        /// </returns>
        public bool IsValid()
        {
            return !IsNoInstallAllowed() && !AnyMaxInstallExceedsTotal();
        }

        /// <summary>
        ///     Checks whether license allows at least one machine to install to.
        /// </summary>
        /// <returns>
        ///     True if rule allows at least one installable machine, false otherwise.
        /// </returns>
        public bool IsNoInstallAllowed()
        {
            // If either max install is 0, or all values in dictionary are 0,
            // Then model is not valid.
            return !(TotalMaxInstall == 0 || MaxInstallPerComputerType.All(x => x.Value == 0));
        }

        /// <summary>
        ///     Checks whether any max install for ComputerType exceeds the TotalMaxInstall.
        /// </summary>
        /// <returns>
        ///     True if any max install for ComputerType exceeds TotalMaxInstall, false otherwise.
        /// </returns>
        public bool AnyMaxInstallExceedsTotal()
        {
            return MaxInstallPerComputerType.Any(x => x.Value > TotalMaxInstall);
        }
    }
}