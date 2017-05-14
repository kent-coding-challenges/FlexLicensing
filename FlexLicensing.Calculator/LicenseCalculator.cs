using System;
using System.Collections.Generic;
using System.Linq;
using FlexLicensing.Calculator.Models;
using FlexLicensing.Calculator.Enums;
using FlexLicensing.Calculator.Extensions;

namespace FlexLicensing.Calculator
{
    /// <summary>
    ///     Provides functionalities to calculate number of license required.
    /// </summary>
    public class LicenseCalculator
    {
        /// <summary>
        ///     The installation log summary.
        /// </summary>
        public InstallLogSummary InstallSummary { get; set; }

        /// <summary>
        ///     Licensing rule to apply for calculation.
        /// </summary>
        public LicenseRule Rule { get; set; }
        
        /// <summary>
        ///     Create a new instance of LicenseCalculator.
        /// </summary>
        /// <param name="installLogSummary">
        ///     InstallLogSummary object which contains the ComputerTypes and count of installation of each type.
        /// </param>
        /// <param name="rule">
        ///     Licensing rule to apply for calculation.
        /// </param>
        public LicenseCalculator(InstallLogSummary installLogSummary, LicenseRule rule)
        {
            InstallSummary = installLogSummary;
            Rule = rule;
        }

        /// <summary>
        ///     Create a new instance of LicenseCalculator with sequence of InstallLog.
        /// </summary>
        /// <param name="logs">
        ///     Sequence/list of Installation Logs.
        /// </param>
        /// <param name="rule">
        ///     Licensing rule to apply for calculation.
        /// </param>
        public LicenseCalculator(IEnumerable<InstallLog> logs, LicenseRule rule)
        {
            InstallSummary = new InstallLogSummary(logs);
            Rule = rule;
        }

        /// <summary>
        ///     Create a new instance of LicenseCalculator.
        /// </summary>
        /// <param name="summarizedInstallCount">
        ///     Summarized dictionary with ComputerType as key
        ///     and number of computers in each type as unsigned int value.
        /// </param>
        /// <param name="rule">
        ///     Licensing rule to apply for calculation.
        /// </param>
        public LicenseCalculator(Dictionary<ComputerType, uint> summarizedInstallCount, LicenseRule rule)
        {
            InstallSummary = new InstallLogSummary(summarizedInstallCount);
            Rule = rule;
        }

        /// <summary>
        ///     Get the minimum number of license required based on the logs summary and rule specified.
        /// </summary>
        /// <returns>
        ///     Number of minimum license required.
        /// </returns>
        public uint GetMinLicenseRequired()
        {
            var watcher = new InstallWatcher();
            var summaryCopy = new InstallLogSummary(InstallSummary);

            uint license = 0;
            while(!summaryCopy.IsEmpty())
            {
                watcher.ResetWatch();
                
                // Apply a new license.
                license++;

                // Using the new license, keep applying to Computer Types consecutively,
                // until MaxInstall is reached or no more ComputerTypes can be applied with this license.
                uint install = 0;
                while(install < Rule.TotalMaxInstall)
                {
                    // Get next ComputerType to apply license to.
                    // Has to be the ComputerTypes with highest numbers
                    // and doesn't violate the MaxInstallPerComputerType.
                    var matchingLogs = summaryCopy.Summary.Where(x => watcher.Watch[x.Key] < Rule.MaxInstallPerComputerType[x.Key]);
                    var next = matchingLogs.MaxByValue();
                    
                    // Check if there's no next machine which can be applied to.
                    if(next == null || next.Value.Value == 0)
                    {
                        if(install == 0)
                        {
                            // If install is 0, this means that there is at least one machine which cannot be covered by a fresh license.
                            string error = "Specified rule cannot cover installation of all machines.";
                            throw new InvalidOperationException(error);
                        } else
                        {
                            // As install is greater than 0, this means that current license cannot be applied to any other computer.
                            // Break from inner loop to apply for a new license.
                            break;
                        }
                    }

                    // Apply license to next and track this installation in watcher.
                    summaryCopy.Summary[next.Value.Key]--;
                    watcher.Watch[next.Value.Key]++;

                    install++;
                }
            }

            return license;
        }
    }
}