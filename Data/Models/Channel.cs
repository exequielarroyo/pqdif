using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Channel
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get;
            set;
        }
        public int PhaseId
        {
            get;
            set;
        }
        public int MeasuredId
        {
            get;
            set;
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

        //RELATIONSHIP
        public List<Series> Series
        {
            get; set;
        }
        public int ObservationId
        {
            get;
            set;
        }
        public Observation Observation
        {
            get;
            set;
        }
    }
}
