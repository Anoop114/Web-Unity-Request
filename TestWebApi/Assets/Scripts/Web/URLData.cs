[System.Serializable]
public class URLData 
{
    //Data of URL (link,height,weight)
    public string url;
    public int width;
    public int height;
}

[System.Serializable]
public class UrlList 
{
    public URLData[] Items;
}
