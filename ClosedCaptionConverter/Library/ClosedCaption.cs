using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosedCaptionConverter.Library
{
    public class ClosedCaption
    {
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string Transcript { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} --> {1}", StartPoint, EndPoint));
            sb.AppendLine(Transcript);
            return sb.ToString();
        }
    }
}
