using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedCaptionConverter.Model;

namespace ClosedCaptionConverterTest
{
    [TestClass]
    public class TTMLHandlerTest
    {
        [TestMethod]
        public void ReadFileTestMethod1()
        {
            string inputFile = @"TestData\TTML_sample1.ttml";
            int expectedClosedCaptionCount = 547;
            string expectedLastStartPoint = "00:24:34.656";
            string expectedLastTranscript = "Thank you so much and have a great day.";

            TTMLHandler ttmlHandler = new TTMLHandler();
            var CCs = ttmlHandler.ReadFile(inputFile);

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 547)
            {
                string actualLastStartPoint = CCs[546].StartPoint;

                Assert.AreEqual(expectedLastStartPoint, actualLastStartPoint);

                string actualLastTranscript = CCs[546].Transcript;
                Assert.AreEqual(expectedLastTranscript, actualLastTranscript);
            }
        }

        [TestMethod]
        public void ReadFileTestMethod2()
        {
            string inputFile = @"TestData\TTML_sample1.ttml";
            int expectedClosedCaptionCount = 547;
            string expectedFirstStartPoint = "00:00:00.560";
            string expectedFirstTranscript = "&gt;&gt; Everybody, as the top of the hour,";

            TTMLHandler ttmlHandler = new TTMLHandler();
            var CCs = ttmlHandler.ReadFile(inputFile);

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 547)
            {
                string actualFirstStartPoint = CCs[0].StartPoint;

                Assert.AreEqual(expectedFirstStartPoint, actualFirstStartPoint);

                string actualFirstTranscript = CCs[0].Transcript;
                Assert.AreEqual(expectedFirstTranscript, actualFirstTranscript);
            }
        }
    }
}
