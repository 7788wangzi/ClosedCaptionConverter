using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedCaptionConverter.Model;

namespace ClosedCaptionConverterTest
{
    [TestClass]
    public class SRTHandlerTest
    {
        [TestMethod]
        public void ReadFileTestMethod1()
        {
            string inputFile = @"TestData\SRT_downloadFromedX.srt";
            int expectedClosedCaptionCount = 106;
            string expectedLastStartPoint = "00:05:12.380";
            string expectedLastTranscript = "I hope it taught you something that you could use later on";

            SRTHandler srtHandler = new SRTHandler();
            var CCs =  srtHandler.ReadFile(inputFile);            

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 106)
            {
                string actualLastStartPoint = CCs[105].StartPoint;

                Assert.AreEqual(expectedLastStartPoint, actualLastStartPoint);

                string actualLastTranscript = CCs[105].Transcript;
                Assert.AreEqual(expectedLastTranscript, actualLastTranscript);
            }
        }

        [TestMethod]
        public void ReadFileTestMethod2()
        {
            string inputFile = @"TestData\SRT_fromVendor.srt";
            int expectedClosedCaptionCount = 34;
            string expectedFirstStartPoint = "00:00:01.107";
            string expectedFirstTranscript = "Before we jump into our demonstrations and we start walking through our specific scenarios,";

            SRTHandler srtHandler = new SRTHandler();
            var CCs = srtHandler.ReadFile(inputFile);

            Assert.AreEqual(expectedClosedCaptionCount, CCs.Count);

            if (CCs.Count == 34)
            {
                string actualFirstStartPoint = CCs[0].StartPoint;

                Assert.AreEqual(expectedFirstStartPoint, actualFirstStartPoint);

                string actualFirstTranscript = CCs[0].Transcript;
                Assert.AreEqual(expectedFirstTranscript, actualFirstTranscript);
            }
        }
    }
}
