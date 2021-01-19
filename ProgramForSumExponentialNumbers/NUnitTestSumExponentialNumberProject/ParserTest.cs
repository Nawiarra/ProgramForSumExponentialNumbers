using NUnit.Framework;
using ProgramForSumExponentialNumbers;
using System;
using ValidationCore;

namespace NUnitTestSumExponentialNumberProject
{
    [TestFixture]
    public class ParserTest
    {
        private const int MantissaSize = 39;
        [TestCase("1.3e+2")]
        [TestCase("1.2e-3")]
        [TestCase("1.8e5")]
        public void CheckExponentFormOnCorrectness_CorrectExponentialNumber_True(string number)
        {
            Validation.ParseString(number, out string mantissa, out short order);
            Assert.Pass();
        }
        [TestCase("1.8c5")]
        [TestCase("1,6e5")]
        public void CheckExponentFormOnCorrectness_IncorrectExponentialNumber_False(string number)
        {
            Assert.Throws<FormatException>(() => Validation.ParseString(number, out string mantissa, out short order));
        }

        [TestCase("1.0e5")]
        [TestCase("1.2e8")]

        public void CheckMantissaSize_CorrectExponentialNumber_True(string number)
        {
           Validation.CheckMantissaSize(number, MantissaSize);
           Assert.Pass();
        }

        [TestCase("1.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000008e15")]
        public void CheckMantissaSize_IncorrectExponentialNumber_False(string number)
        {
            Assert.Throws<FormatException>(() => Validation.CheckMantissaSize(number, MantissaSize));
        }
    }
}
