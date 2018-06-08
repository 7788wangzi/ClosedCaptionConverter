using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedCaptionConverter.Model;

namespace ClosedCaptionConverterTest
{
    [TestClass]
    public class TimeFormatterTest
    {
        [TestMethod]
        public void ToDoubleTestMethod1()
        {
            string inputString = "00:00:05.530";
            double expectedValue = 5530;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToDoubleTestMethod2()
        {
            string inputString = "00:00:05,530";
            double expectedValue = 5530;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToDoubleTestMethod3()
        {
            string inputString = "00:05.530";
            double expectedValue = 5530;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToDoubleTestMethod4()
        {
            string inputString = "00:05,530";
            double expectedValue = 5530;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToDoubleTestMethod5()
        {
            string inputString = "00:00:05";
            double expectedValue = 5000;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        [TestMethod]
        public void ToDoubleTestMethod6()
        {
            string inputString = "00:00:00:05";
            double expectedValue = 5;

            double actualValue = TimeFormatter.ToDouble(inputString);

            Assert.AreEqual<double>(expectedValue, actualValue);
        }

        // Test the second method
        [TestMethod]
        public void ToHHMMSSTestMethod1()
        {
            double inputValue = 5530;
            string expectedString = "00:00:05.530";            

            string actualString = TimeFormatter.ToHHMMSS(inputValue,"t1");

            Assert.AreEqual<string>(expectedString, actualString);
        }

        [TestMethod]
        public void ToHHMMSSTestMethod2()
        {
            double inputValue = 1205530;
            string expectedString = "00:20:05.530";

            string actualString = TimeFormatter.ToHHMMSS(inputValue, "t1");

            Assert.AreEqual<string>(expectedString, actualString);
        }

        [TestMethod]
        public void ToHHMMSSTestMethod3()
        {
            double inputValue = 37205530;
            string expectedString = "10:20:05.530";

            string actualString = TimeFormatter.ToHHMMSS(inputValue, "t1");

            Assert.AreEqual<string>(expectedString, actualString);
        }

        [TestMethod]
        public void ToHHMMSSTestMethod4()
        {
            double inputValue = 1205530;
            string expectedString = "00:20:05,530";

            string actualString = TimeFormatter.ToHHMMSS(inputValue, "t2");

            Assert.AreEqual<string>(expectedString, actualString);
        }

        [TestMethod]
        public void ToHHMMSSTestMethod5()
        {
            double inputValue = 37205530;
            string expectedString = "10:20:05,530";

            string actualString = TimeFormatter.ToHHMMSS(inputValue, "t2");

            Assert.AreEqual<string>(expectedString, actualString);
        }
    }
}
