using System;
using System.Collections.Generic;
using System.Linq;
using FlexLicensing.Calculator.Models;
using FlexLicensing.Calculator.Extensions;

namespace FlexLicensing.Calculator
{
    /// <summary>
    ///     Provides functionalities to calculate number of license required.
    /// </summary>
    public class LicenseCalculator
    {
        /// <summary>
        ///     List of installation log summary for each user.
        ///     Each summary contains ComputerTypes and count for each type.
        /// </summary>
        public List<InstallLogSummary> InstallSummaries { get; private set; }

        /// <summary>
        ///     Licensing rule to apply for calculation.
        /// </summary>
        public LicenseRule Rule { get; private set; }

        /// <summary>
        ///     Create a new instance of LicenseCalculator.
        /// </summary>
        /// <param name="installLogSummaries">
        ///     List of installation log summary for each user.
        ///     Each summary contains ComputerTypes and count for each type.
        /// </param>
        /// <param name="rule">
        ///     Licensing rule to apply for calculation.
        /// </param>
        public LicenseCalculator(IEnumerable<InstallLogSummary> installLogSummaries, LicenseRule rule)
        {
            Rule = rule;
            InstallSummaries = installLogSummaries.ToList();

            Validate();
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
            Rule = rule;

            var logsByUser = logs.GroupBy(x => x.UserID);

            InstallSummaries = new List<InstallLogSummary>();
            foreach (var userLogs in logsByUser)
            {
                uint userID = userLogs.Key;
                var userSummary = new InstallLogSummary(userLogs, userID);
                InstallSummaries.Add(userSummary);
            }

            Validate();
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

            uint license = 0;
            foreach (var userSummary in InstallSummaries)
            {
                while (!userSummary.IsEmpty())
                {
                    watcher.ResetWatch();

                    // Apply a new license.
                    license++;

                    // Using the new license, keep applying to Computer Types consecutively,
                    // until MaxInstall is reached or no more ComputerTypes can be applied with this license.
                    uint install = 0;
                    while (install < Rule.TotalMaxInstall)
                    {
                        // Get next ComputerType to apply license to.
                        // Has to be the ComputerTypes with highest numbers
                        // and doesn't violate the MaxInstallPerComputerType.
                        var matchingLogs = userSummary.Summary.Where(x => watcher.Watch[x.Key] < Rule.MaxInstallPerComputerType[x.Key]);
                        var next = matchingLogs.MaxByValue();

                        // Check if there's no next machine which can be applied to.
                        if (next == null || next.Value.Value == 0)
                        {
                            if (install == 0)
                            {
                                // If install is 0, this means that there is at least one machine which cannot be covered by a fresh license.
                                string error = "Specified rule cannot cover installation of all machines.";
                                throw new InvalidOperationException(error);
                            }
                            else
                            {
                                // As install is greater than 0, this means that current license cannot be applied to any other computer.
                                // Break from inner loop to apply for a new license.
                                break;
                            }
                        }

                        // Apply license to next and track this installation in watcher.
                        userSummary.Summary[next.Value.Key]--;
                        watcher.Watch[next.Value.Key]++;

                        install++;
                    }
                }
            }

            return license;
        }

        /// <summary>
        ///     Validate input data of this class.
        /// </summary>
        private void Validate()
        {
            // Check for null properties.
            if(Rule == null)
                throw new ArgumentNullException("Rule");
            
            if (InstallSummaries == null)
                throw new ArgumentNullException("InstallSummary");

            // Ensure licensing rule is valid.
            if (!Rule.IsValid())
            {
                string error = @"Specified rule is not valid.
                    Ensure that total MaxInstall is not zero and MaxInstall for any device does not exceed total MaxInstall.";
                throw new Exception(error);
            }
        }
    }
}