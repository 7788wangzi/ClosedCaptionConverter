using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedCaptionConverter.Library;


namespace ClosedCaptionConverter.Model
{
    interface IFileHandler
    {
        List<ClosedCaption> ReadFile(string file);

        void WriteFile(string file, List<ClosedCaption> ClosedCaptions);        
    }
}
