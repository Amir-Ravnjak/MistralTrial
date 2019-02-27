using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public class TitleActors
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public int ActorsId { get; set; }
        public Actors Actors { get; set; }
    }
}
