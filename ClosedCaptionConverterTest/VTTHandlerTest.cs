using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedCaptionConverter.Model;

namespace ClosedCaptionConverterTest
{
    [TestClass]
    public class VTTHandlerTest
    {
        [TestMethod]
        public void ReadFileTestMethod1()
        {
            string inputFile = @"TestData\WebVTT_sample1.vtt";
            int expectedClosedCaptionCount = 43;
            string expectedLastStartPoint = "00:03:03.806";
            string expectedLastTranscript = @"A variable can be incremented or";

            VTTHandler  vttHandler = new VTTHandler();
            var CCs = vttHandler.ReadFile(inputFile);

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 43)
            {
                string actualLastStartPoint = CCs[41].StartPoint;

                Assert.AreEqual(expectedLastStartPoint, actualLastStartPoint);

                string actualLastTranscript = CCs[41].Transcript;
                Assert.AreEqual(expectedLastTranscript, actualLastTranscript);
            }
        }

        [TestMethod]
        public void ReadFileTestMethod2()
        {
            string inputFile = @"TestData\WebVTT_sample2.vtt";
            int expectedClosedCaptionCount = 17;
            string expectedLastStartPoint = "00:00:02.633";
            string expectedLastTranscript = @"Hello! I am Anusha Muthiah and welcome to week 2 of the Introduction to HTML5.";

            VTTHandler vttHandler = new VTTHandler();
            var CCs = vttHandler.ReadFile(inputFile);

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 17)
            {
                string actualLastStartPoint = CCs[0].StartPoint;

                Assert.AreEqual(expectedLastStartPoint, actualLastStartPoint);

                string actualLastTranscript = CCs[0].Transcript;
                Assert.AreEqual(expectedLastTranscript, actualLastTranscript);
            }
        }
    }
}
