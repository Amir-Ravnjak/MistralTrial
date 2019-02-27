using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public enum TitleTypes
    {
        Movie,
        TVShow
    }
    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public int? ImgFileId { get; set; }
        public ImgFile ImgFile { get; set; }
        public TitleTypes Type { get; set; }
    }
}
