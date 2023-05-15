using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "tinyint")]
        public int TriggerMethod { get; set; }
        public DateTime TriggeredAt { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }

        //RELATIONSHIPS
        public int SourceId
        {
            get;
            set;
        }
        public List<Channel> Channels
        {
            get;
        }
        public Container Container
        {
            get;
            set;
        }
    }
}
