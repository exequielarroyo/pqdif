using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;
public class Container
{
    public int Id
    {
        get;
        set;
    }
    public string FileName
    {
        get;
        set;
    }
    public string Author
    {
        get;
        set;
    }
    public string Title
    {
        get;
        set;
    }
    public string Comments
    {
        get;
        set;
    }

    [DefaultValue(0)]
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
}
