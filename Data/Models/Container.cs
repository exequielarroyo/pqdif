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

    //RELATIONSHIPS
    public List<Observation> Observations
    {
        get;
        set;
    }
}
