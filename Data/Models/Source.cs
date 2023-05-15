using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;
public class Source
{
    public int Id
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }

    //RELATIONSHIPS
    public int CotainerId
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
