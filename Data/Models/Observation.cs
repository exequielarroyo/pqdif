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
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        
        public DateTime CreateAt
        {
            get;
            set;
        }
        public DateTime StartAt
        {
            get; set;
        }
        [Column(TypeName = "tinyint")]
        public int TriggerMethod
        {
            get; set;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt
        {
            get;
            set;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt
        {
            get;
            set;
        }

        //RELATIONSHIPS
        public List<Channel> Channels
        {
            get;
        }
        public int ContainerId
        {
            get;
            set;
        }
        public Container Container
        {
            get;
            set;
        }
    }
}
