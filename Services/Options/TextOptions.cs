namespace Services.Options;

public class TextOptions
{
    public const string Options = "TextOptions";
    
    public int MinHeadingLength { get; set; }
    
    public int MaxHeadingLength { get; set; }
    
    public int MinTextLength { get; set; }
    
    public int MaxTextLength { get; set; }
}