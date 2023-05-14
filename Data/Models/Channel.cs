using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public List<Series> Series { get; set; }
    }
}
