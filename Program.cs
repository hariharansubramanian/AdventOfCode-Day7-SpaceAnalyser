using SpaceAnalyser_AdventOfCodeDay7;
using Directory = SpaceAnalyser_AdventOfCodeDay7.Directory;

Console.WriteLine("Analysing commands...");
var commands = File.ReadLines("system_commands.txt").ToList();

var rootDirectory = new Directory("root", null);
var currentDirectory = rootDirectory;
var directories = new List<Directory> {rootDirectory}; // list of all directories

// Puzzle 1
for (var i = 0; i < commands.Count; i++)
{
    var commandParts = commands[i].Split(' ');
    switch (commandParts[1])
    {
        case "cd": // handle navigation
            currentDirectory = commandParts[2] switch
            {
                "/" => rootDirectory,
                ".." => currentDirectory.Parent ?? currentDirectory,
                _ => currentDirectory.SubDirectories.First(d => d.Name == commandParts[2])
            };
            break;
        case "ls": // handle listing
            var listOutput = commands.Skip(i + 1).TakeWhile(s => !s.StartsWith("$"));
            foreach (var item in listOutput)
            {
                var itemParts = item.Split(' ');
                if (itemParts[0] == "dir") // add directory
                {
                    // register directory
                    var subDirectory = new Directory(itemParts[1], currentDirectory);
                    currentDirectory.SubDirectories.Add(subDirectory);
                    directories.Add(subDirectory);
                }
                else // add file and add to directory size
                {
                    var fileSize = long.Parse(itemParts[0]);
                    currentDirectory.Files.Add(new SystemFile(itemParts[1], fileSize));
                    currentDirectory.Size += fileSize;
                }
            }

            // Done listing, skip to next command
            i += listOutput.Count();
            break;
    }
}

var totalSumLessThan100000 = directories.Select(CalculateDirectorySize).Where(dirSize => dirSize <= 100000).Sum();

Console.WriteLine($"Total size of all directories with at most 10000 size: {totalSumLessThan100000}");

/*
 *  Recursively calculates total size of a directory and all its subDirectories
 */
long CalculateDirectorySize(Directory directory)
{
    if (!directory.SubDirectories.Any()) return directory.Size;
    return directory.Size + directory.SubDirectories.Sum(CalculateDirectorySize);
}