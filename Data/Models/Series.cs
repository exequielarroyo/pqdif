using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Series
    {
        public int Id { get; set; }
        public int Offset { get; set; }
        public int Scale { get; set; }
        public string Values { get; set; }
    }
}
