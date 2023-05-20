using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;
public class Container
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
    public bool IsSync
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
