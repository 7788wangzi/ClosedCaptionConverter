using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedCaptionConverter.Library;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace ClosedCaptionConverter.Model
{
    public class TTMLHandler : IFileHandler
    {
        public List<ClosedCaption> ReadFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"input file not found-{file}");
            }

            XmlDocument _xDocument = new XmlDocument();
            _xDocument.Load(file);
            XmlNamespaceManager xmlNSMgr = new XmlNamespaceManager(_xDocument.NameTable);
            xmlNSMgr.AddNamespace("ttm", "http://www.w3.org/ns/ttml#metadata");
            xmlNSMgr.AddNamespace("ttp", "http://www.w3.org/ns/ttml#parameter");
            xmlNSMgr.AddNamespace("tts", "http://www.w3.org/ns/ttml#styling");

            XmlElement rootElement = _xDocument.DocumentElement;

            List<ClosedCaption> ccResults = new List<ClosedCaption>();

            /*
             * <body>
             * <div region="subtitleArea">
             * <p xml:id="subtitle1" ttm:role="caption" begin="00:00:01.180" end="00:00:03.050">Now you know more<br />about the Internet.</p>
             * */
            foreach (XmlElement node in rootElement.ChildNodes)
            {
                var tagName = node.Name;
                if(tagName.ToLower() =="body")
                {
                    foreach (XmlElement pTagNode in node.ChildNodes[0].ChildNodes)
                    {
                        XmlElement element = pTagNode;
                        var nodeContent = element.InnerXml;
                        string htmlTagPattern = "<[a-zA-Z]+(\\s+[a-zA-Z]+\\s*=\\s*(\"([^\"]*)\"|'([^']*)'))*\\s*/>";
                        nodeContent = Regex.Replace(nodeContent, htmlTagPattern, " "); // replace <br/> with space
                        var beginTime = pTagNode.GetAttribute("begin");
                        var endTime = pTagNode.GetAttribute("end");

                        if(!string.IsNullOrEmpty(nodeContent))
                        {
                            string startPoint = beginTime;
                            string endPoint = endTime;

                            startPoint = TimeFormatter.ToHHMMSS(TimeFormatter.ToDouble(startPoint), "t1");
                            endPoint = TimeFormatter.ToHHMMSS(TimeFormatter.ToDouble(endPoint), "t1");

                            ccResults.Add(new ClosedCaption
                            {
                                StartPoint = startPoint,
                                EndPoint = endPoint,
                                Transcript = nodeContent
                            });
                        }
                    }
                }
            }

            return ccResults;
        }

        public void WriteFile(string file, List<ClosedCaption> ClosedCaptions)
        {
            StringBuilder sbCCContent = new StringBuilder();
            string fileContent = string.Empty;
            using (StreamReader reader = new StreamReader("ttSample1.txt"))
            {
                fileContent = reader.ReadToEnd();
            }

            if(ClosedCaptions.Count >0)
            {
                sbCCContent.AppendLine("<div region=\"subtitleArea\">");
                for(int i=0; i<ClosedCaptions.Count; i++)
                {
                    double beginTime = TimeFormatter.ToDouble(ClosedCaptions[i].StartPoint);
                    double endTime = TimeFormatter.ToDouble(ClosedCaptions[i].EndPoint);

                    string begin = TimeFormatter.ToHHMMSS(beginTime, "t1");
                    string end = TimeFormatter.ToHHMMSS(endTime, "t1");
                    string content = ClosedCaptions[i].Transcript;//HttpUtility.HtmlEncode(Captions[i].Transcript);
                    sbCCContent.AppendLine(string.Format("<p begin=\"{1}\" xml:id=\"{0}\" end=\"{2}\">{3}</p>", "p" + i, begin, end, content));
                }
                sbCCContent.AppendLine(@"</div>");

                fileContent = string.Format(fileContent, sbCCContent.ToString());

                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.Write(fileContent);
                    writer.Flush();
                    writer.Close();
                }
            }                
        }
    }
}
