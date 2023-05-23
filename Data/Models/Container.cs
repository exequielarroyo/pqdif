using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;
public class Container : Base
{
    public int Id
    {
        get; set;
    }
    public string FileName
    {
        get;
        set;
    }
    public DateTime Creation
    {
        get;
        set;
    }
    public int CompressionStyle
    {
        get;
        set;
    }
    public int CompressionAlgorithm
    {
        get;
        set;
    }
    public bool? IsSync
    {
        get;
        set;
    }

    //RELATIONSHIPS
    public List<Observation> Observations
    {
        get;
        set;
    }
    public Source Source
    {
        get;
        set;
    }
}
