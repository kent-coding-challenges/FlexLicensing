using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FlexLicensing.Calculator.Models;
using FlexLicensing.Calculator.Enums;
using FlexLicensing.Calculator.Tests.Factory;

namespace FlexLicensing.Calculator.Tests
{
    [TestClass]
    public class LicenseCalculatorTest
    {
        [TestMethod]
        public void OnDuplicate()
        {
            // Create new instance with default rule.
            var classUnderTest = LicenseCalculatorFactory.CreateFromCase("duplicate.csv");

            // Ensure result is equal to expected output.
            uint output = classUnderTest.GetMinLicenseRequired();
            uint expectedOutput = 1;
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OnNoCompletionPossible()
        {
            // Create new instance with default rule.
            // Note that rule doen't allow mobile installation.
            var classUnderTest = LicenseCalculatorFactory.CreateFromCase("default-with-mobile.csv");
            classUnderTest.GetMinLicenseRequired();
        }


        [TestMethod]
        public void OnUserGrouping()
        {
            // Create mockup for user summaries with differing user id.
            var mockupSummaries = new List<InstallLogUserSummary>();

            // Laptop is chosen here as default rule allows 1 license to install 2 laptops.
            var computerType = ComputerType.Laptop;
            uint numberOfLaptops = 1;
            uint reptition = 100;

            for (uint i = 1; i <= reptition; i++)
            {
                // Create mockup summary with static computer type, static count, and userID = i.
                var mockup = new Dictionary<ComputerType, uint>();
                mockup.Add(computerType, numberOfLaptops);
                var mockupSummary = new InstallLogUserSummary(mockup, i);

                mockupSummaries.Add(mockupSummary);
            }

            var classUnderTest = LicenseCalculatorFactory.CreateFromUserSummaries(mockupSummaries);

            // Ensure result is equal to expected output.
            // Expected output is equal to repetition, as each user can't share license with other users.
            uint output = classUnderTest.GetMinLicenseRequired();
            uint expectedOutput = reptition;
            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod]
        public void OnLinearCase()
        {
            uint userID = 1;
            uint repetition = 100;
            var computerType = ComputerType.Desktop;

            for (uint i = 0; i <= repetition; i++)
            {
                // Create mockup summary with static computer type, static userID, and count = i.
                var mockup = new Dictionary<ComputerType, uint>();
                mockup.Add(computerType, i);
                var mockupSummary = new InstallLogUserSummary(mockup, userID);

                var classUnderTest = LicenseCalculatorFactory.CreateFromSingleSummary(mockupSummary);

                // Ensure result is equal to expected output.
                uint output = classUnderTest.GetMinLicenseRequired();
                uint expectedOutput = i;
                Assert.AreEqual(output, expectedOutput);
            }
        }
        
    }
}
