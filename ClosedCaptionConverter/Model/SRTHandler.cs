using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedCaptionConverter.Library;
using System.IO;
using System.Text.RegularExpressions;

namespace ClosedCaptionConverter.Model
{
    public class SRTHandler : IFileHandler
    {
        public List<ClosedCaption> ReadFile(string file)
        {
            if(!File.Exists(file))
            {
                throw new FileNotFoundException($"input file not found-{file}");
            }
            //0:0:4.480 --> 0:0:7.430
            string timePattern = @"([0-9]+:)?([0-9]+):([0-9]+)([\.|,][0-9]+)? --> ([0-9]+:)?([0-9]+):([0-9]+)([\.|,][0-9]+)?";

            List<ClosedCaption> ccResults = new List<ClosedCaption>();
            using (StreamReader reader = new StreamReader(file))
            {
                string fileContent = reader.ReadToEnd();                

                var cues = Regex.Matches(fileContent, timePattern, RegexOptions.IgnoreCase);                

                // Handle Time Stamp
                foreach (Match cue in cues)
                {
                    string textOftheCue = cue.Value.ToString();
                    string[] timePoints = textOftheCue.Split(new string[] { "-->" }, StringSplitOptions.RemoveEmptyEntries);
                    if(timePoints.Length ==2)
                    {
                        string startPoint = timePoints[0].Trim();
                        string endPoint = timePoints[1].Trim();

                        // Format to standard Time Stamp
                        startPoint = TimeFormatter.ToHHMMSS(TimeFormatter.ToDouble(startPoint), "t1");
                        endPoint = TimeFormatter.ToHHMMSS(TimeFormatter.ToDouble(endPoint), "t1");
                        ccResults.Add(new ClosedCaption
                        {
                            StartPoint = startPoint,
                            EndPoint = endPoint
                        });
                    }
                }

                // Handle Transcript
                //string contentWithTimeRemoved = Regex.Replace(fileContent, timePattern, "-->");
                // Use regex pattern (time stamp) to seperate the transcripts
                string[] transcripts = Regex.Split(fileContent, timePattern, RegexOptions.ExplicitCapture);
      
                // both for VTT and SRT, there is an extra line, for VTT is WebVTT, for SRT is the first number 1.
                if(transcripts.Length == ccResults.Count+1&&transcripts.Length>1)
                {
                    for(int i=1; i<transcripts.Length; i++)
                    {
                        string transcript = transcripts[i].Trim(new char[] { '\r', '\n' });
                        string transcriptWithNoLineNumber = Regex.Replace(transcript, "[0-9]*$", string.Empty); //remove the line numbers at the vey end of each line
                        // NOTE: there is an issue if there is a real digital number at the end of transcript line, it will be removed intentionally
                        string transcriptTrimRNagain = transcriptWithNoLineNumber.Trim(new char[] { '\r', '\n' });
                        ccResults[i - 1].Transcript = transcriptTrimRNagain.Trim();
                    }
                }
            }
            return ccResults;
        }

        public void WriteFile(string file, List<ClosedCaption> ClosedCaptions)
        {
            if(ClosedCaptions.Count>0)
            {
                StringBuilder sbFileContent = new StringBuilder();
                for(int i=0; i<ClosedCaptions.Count; i++)
                {
                    sbFileContent.AppendLine((i + 1).ToString());
                    sbFileContent.AppendLine(ClosedCaptions[i].ToString());
                }

                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Write(sbFileContent.ToString());
                    writer.Flush();
                    writer.Close();
                }
            }            
        }
    }
}
