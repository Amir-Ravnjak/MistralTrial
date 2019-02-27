using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public class ImgFile
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public string Tip { get; set; }
        public byte[] Podaci { get; set; }
    }
}
