using FlexLicensing.Calculator.Models;
using FlexLicensing.Calculator.Tests.CsvHelper.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexLicensing.Calculator.Tests.Factory
{
    public static class LicenseCalculatorFactory
    {
        public static LicenseCalculator CreateFromCase(string csvFileName, LicenseRule rule = null)
        {
            // If no rule is passed, use default rule.
            if(rule == null)
                rule = LicenseRuleFactory.CreateDefault();

            var logs = CaseFactory<InstallLog, InstallLogMap>.ReadCase(csvFileName);
            return new LicenseCalculator(logs, rule);
        }

        public static LicenseCalculator CreateFromSingleSummary(InstallLogUserSummary userSummary, LicenseRule rule = null)
        {
            // If no rule is passed, use default rule.
            if (rule == null)
                rule = LicenseRuleFactory.CreateDefault();

            var summaries = new List<InstallLogUserSummary>();
            summaries.Add(userSummary);

            return new LicenseCalculator(summaries, rule);
        }

        public static LicenseCalculator CreateFromUserSummaries(List<InstallLogUserSummary> userSummaries, LicenseRule rule = null)
        {
            // If no rule is passed, use default rule.
            if (rule == null)
                rule = LicenseRuleFactory.CreateDefault();
            
            return new LicenseCalculator(userSummaries, rule);
        }
    }
}
