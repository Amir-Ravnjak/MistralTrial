using MistralTrialAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialWeb.ViewModels
{
    public class HomeTitlesVM
    {
        public List<Row> rows { get; set; }



        public class Row
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public float AverageGrade { get; set; }
            public int? ImageId { get; set; }

            public List<Actors> actors { get; set; }
        }
    }
}
