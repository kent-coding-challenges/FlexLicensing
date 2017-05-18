using FlexLicensing.Calculator.Enums;
using FlexLicensing.Calculator.Models;
using System.Collections.Generic;

namespace FlexLicensing.Calculator.Tests.Factory
{
    public static class LicenseRuleFactory
    {
        public static LicenseRule NewInstance => new LicenseRule();

        /// <remarks>
        ///     This rule is used as a base rule to many test cases for LicenseCalculator.
        ///     This default rule represents the licensing rule specified in the coding challenge.
        /// </remarks>
        public static LicenseRule CreateDefault()
        {
            var instance = NewInstance;
            instance.TotalMaxInstall = 2;
            instance.MaxInstallPerComputerType[ComputerType.Desktop] = 1;
            instance.MaxInstallPerComputerType[ComputerType.Laptop] = 1;

            return instance;
        }
    }
}
