namespace MiniBookstoreCatalog.Mvc.ViewModels;

public class DataHealthViewModel
{
    public bool DatabaseConnected
    {
        get;
        set;
    }

    public string Message
    {
        get;
        set;
    } = string.Empty;
}

