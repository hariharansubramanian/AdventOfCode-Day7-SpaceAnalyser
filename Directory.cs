namespace SpaceAnalyser_AdventOfCodeDay7;

public class Directory
{
    public Directory(string name, Directory? parent)
    {
        Name = name;
        Parent = parent;
    }

    public string Name { get; set; }
    public Directory? Parent { get; set; }
    public ICollection<Directory> SubDirectories { get; set; } = new List<Directory>();
    public ICollection<SystemFile> Files { get; set; } = new List<SystemFile>();
    public int Size { get; set; }
}