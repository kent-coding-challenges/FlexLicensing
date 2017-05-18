using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexLicensing.Calculator.Tests.Factory;

namespace FlexLicensing.Calculator.Tests
{
    [TestClass]
    public class InstallWatcherTest
    {
        [TestMethod]
        public void OnResetWatch()
        {
            var classUnderTest = InstallWatcherFactory.NewInstanceWithDummyData;
            classUnderTest.ResetWatch();
            foreach(var item in classUnderTest.Watch)
            {
                if (item.Value != 0)
                    Assert.Fail("Watch contains non-zero elements after reset.");
            }
        }
    }
}
