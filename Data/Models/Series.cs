using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;

public class Series : Base
{
    public int Id
    {
        get; set;
    }
    public int UnitsId
    {
        get; set;
    }
    public int CharacteristicId
    {
        get; set;
    }
    public int TypeId
    {
        get;
        set;
    }
    public string Values
    {
        get; set;
    }

    //RELATIONSHIPS
    public int ChannelId
    {
        get;
        set;
    }
    public Channel Channel
    {
        get;
        set;
    }
}
