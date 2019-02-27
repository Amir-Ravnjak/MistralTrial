using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public class Rate
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
