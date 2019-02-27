using MistralTrialAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialWeb.ViewModels
{
    public class HomeTitleListVM
    {
        public List<Row> rows { get; set; }

        public TitleTypes titleType { get; set; }

        public class Row
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public float AverageGrade { get; set; }
            public int? ImageId { get; set; }
            public float Rating { get; set; }
        }
    }
}
