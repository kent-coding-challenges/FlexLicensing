using System;
using System.Collections.Generic;
using System.Linq;
using FlexLicensing.Calculator.Enums;

namespace FlexLicensing.Calculator
{
    /// <summary>
    ///     Provides way to keep track (book-keeping/watch) number of installs in each computer type.
    /// </summary>
    public class InstallWatcher
    {
        /// <summary>
        ///     Dictionary which maintains the number of installs in each computer type.
        /// </summary>
        public Dictionary<ComputerType, uint> Watch { get; private set; }

        /// <summary>
        ///     Initialize a new InstallWatcher instance with all counts in Watch object as zero.
        /// </summary>
        public InstallWatcher()
        {
            ResetWatch();
        }

        /// <summary>
        ///     Reset current Watch object and set all items in Watch to zero.
        /// </summary>
        public void ResetWatch()
        {
            Watch = new Dictionary<ComputerType, uint>();

            // Add each computer type as key, with default count = 0.
            var computerTypes = Enum.GetValues(typeof(ComputerType)).Cast<ComputerType>();
            foreach (var type in computerTypes)
            {
                Watch.Add(type, 0);
            }
        }
    }
}