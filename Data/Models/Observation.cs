using Gemstone.PQDIF.Logical;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Observation
    {
        public string Name { get; set; }
        public DbSet<ChannelInstance> ChannelInstances { get; set; }
        public TriggerMethod TriggerMethod { get; set; }
        public DateTime TimeTriggered { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
