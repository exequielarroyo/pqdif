using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;
public class Source : Base
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
    public string Type
    {
        get;
        set;
    }
    public string VendorId
    {
        get;
        set;
    }
    public string EquipmentId
    {
        get;
        set;
    }

    //RELATIONSHIPS
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
