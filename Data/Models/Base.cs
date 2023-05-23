using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;
public class Base
{
    // UNCOMMENT ATTRIBUTES FOR MYSQL
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt
    {
        get; set;
    }

    //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt
    {
        get; set;
    }
}
