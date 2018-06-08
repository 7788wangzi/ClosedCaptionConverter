using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedCaptionConverter.Library;
using System.IO;

namespace ClosedCaptionConverter.Model
{
    public class VTTHandler : IFileHandler
    {
        public List<ClosedCaption> ReadFile(string file)
        {
            // use same logic to the SRT handler
            return new SRTHandler().ReadFile(file);
        }

        public void WriteFile(string file, List<ClosedCaption> ClosedCaptions)
        {
            if(ClosedCaptions.Count>0)
            {
                StringBuilder sbFileContent = new StringBuilder();
                sbFileContent.AppendLine("WEBVTT");
                sbFileContent.AppendLine();
                foreach (var item in ClosedCaptions)
                {
                    sbFileContent.AppendLine(item.ToString());
                }

                using (StreamWriter writer = new StreamWriter(file, false))
                {
                    writer.Write(sbFileContent);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}
