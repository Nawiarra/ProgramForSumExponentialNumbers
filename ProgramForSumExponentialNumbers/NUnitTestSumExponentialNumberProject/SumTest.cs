using NUnit.Framework;
using ProgramForSumExponentialNumbers;
using System;
using ValidationCore;

namespace NUnitTestSumExponentialNumberProject
{
    [TestFixture]
    public class Tests
    {
        private ExponentialNumber sum;
        private ExponentialNumber arg1;
        [SetUp]
        public void Setup()
        {
            sum = ExponentialNumber.ExponentialNumberCreate("0.0");
        }

        [TestCase(new string[] { "12.7e+12", "10.3e+10" },  "1.2803e+13")]
        [TestCase(new string[] { "12.7e-12", "10.34e-10" }, "1.0467e-9")]
        [TestCase(new string[] { "12.7e-12", "10.34e+10" }, "1.034000000000000000000127e+11")]
        [TestCase(new string[] { "12.7e+12", "10.34e+12" }, "2.304e+13")]
        [TestCase(new string[] { "12.87e+12", "19.999e-24" }, "1.28700000000000000000000000000000000200e+13")]
        [TestCase(new string[] { "1.2e0", "2.2e0" }, "3.4e0")]
        [TestCase(new string[] { "1.2e-37", "2.2e0" }, "2.20000000000000000000000000000000000012e0")]
        [TestCase(new string[] { "4.2134e+23", "6.97826e-62" }, "4.2134e+23")]
        public void SumTwoArgs_ExponentialNumbers_ReturnsSameNumber(string[] nums, string waitingResult)
        {
            sum = sum = ExponentialNumber.ExponentialNumberCreate("0.0");

            foreach (string str in nums)
            {
                arg1 = ExponentialNumber.ExponentialNumberCreate(str);
                sum += arg1;
            }

            Assert.AreEqual(waitingResult, sum.ToString(), "Value is not correct");
        }

        [TestCase("0.0e0")]
        public void NullCheck_Nothing_CorrectNumberInExpoentialFormat(string waitingResult)
        {
            Assert.AreEqual(waitingResult, sum.ToString(), "Value is not correct");
        }

        [TestCase(new string[] { "1.1e0", "2.2e0", "3.3e0" }, "6.6e0")]
        [TestCase(new string[] { "10.0e5", "10.0e4", "10.0e-32" }, "1.1000000000000000000000000000000000001e+6")]
        [TestCase(new string[] { "0.0e0", "0.0e0", "0.0e0" }, "0e0")]
        [TestCase(new string[] { "112.5e-3", "12.7e-1", "10.0e-2" }, "1.4825e0")]
        [TestCase(new string[] { "1.5e-3", "2.5e-3", "7.0e-3" }, "1.1e-2")]
        public void SumThreeArgs_ExponentialNumbers_ReturnsSameNumber(string[] nums, string waitingResult)
        {
            sum = sum = ExponentialNumber.ExponentialNumberCreate("0.0");

            foreach (string str in nums)
            {
                arg1 = ExponentialNumber.ExponentialNumberCreate(str);
                sum += arg1;
            }

            Assert.AreEqual(waitingResult, sum.ToString(), "Value is not correct");
        }

       
    }
}