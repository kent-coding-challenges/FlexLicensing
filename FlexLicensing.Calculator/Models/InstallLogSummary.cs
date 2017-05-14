using System.Collections.Generic;
using System.Linq;
using FlexLicensing.Calculator.Enums;

namespace FlexLicensing.Calculator.Models
{
    /// <summary>
    ///     Holds summarized information of InstallLogs,
    ///     storing the number of installations on each ComputerType.
    /// </summary>
    public class InstallLogSummary
    {
        /// <summary>
        ///     Dictionary which holds summarized information of each ComputerType with their number of installs.
        /// </summary>
        public Dictionary<ComputerType, uint> Summary { get; private set; }

        /// <summary>
        ///     Create a new instance of InstallLogSummary from logs.
        /// </summary>
        /// <param name="logs">
        ///     Enumerable of InstallLog to be summarized.
        /// </param>
        public InstallLogSummary(IEnumerable<InstallLog> logs)
        {
            Summary = new Dictionary<ComputerType, uint>();

            var logsGroupedByComputerType = logs.GroupBy(x => x.ComputerType);
            foreach(var groupedLogs in logsGroupedByComputerType)
            {
                Summary.Add(groupedLogs.Key, (uint)groupedLogs.Count());
            }
        }

        /// <summary>
        ///     Create a new instance of InstallLogSummary from a summarized dictionary count.
        /// </summary>
        /// <param name="summary">
        ///     Summarized dictionary containing ComputerType, each containing its number of installs.
        /// </param>
        public InstallLogSummary(Dictionary<ComputerType, uint> summary)
        {
            Summary = summary;
        }
        
        /// <summary>
        ///     Copy constructor.
        /// </summary>
        /// <param name="summary">
        ///     The source object to be copied from.
        /// </param>
        public InstallLogSummary(InstallLogSummary installLogSummary)
        {
            this.Summary = installLogSummary?.Summary;
        }

        /// <summary>
        ///     Checks whether the summary contains any install.
        ///     Note that summary with all ComputerTypes' counts as zero are considered empty.
        /// </summary>
        /// <returns>
        ///     True if summary has no InstallLog, false otherwise.
        /// </returns>
        public bool IsEmpty()
        {
            return Summary.Count == 0 || !Summary.Any(x => x.Value > 0);
        }
    }
}
