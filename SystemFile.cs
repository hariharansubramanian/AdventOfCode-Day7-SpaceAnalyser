namespace SpaceAnalyser_AdventOfCodeDay7;

public class SystemFile
{
    public SystemFile(string name, long size)
    {
        Name = name;
        Size = size;
    }

    private string Name { get; set; }
    private long Size { get; set; }
}