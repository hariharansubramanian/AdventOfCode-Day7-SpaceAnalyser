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
                    var fileSize = int.Parse(itemParts[0]);
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

// Puzzle 2
const int totalCapacity = 70000000;
const int spaceRequired = 30000000;
var spaceUsed = CalculateDirectorySize(rootDirectory);
var spaceRemaining = totalCapacity - spaceUsed;

var (smallestDir, size) = directories
    .Select(d => (Directory: d, Size: CalculateDirectorySize(d)))
    .OrderByDescending(x => spaceRemaining + x.Size >= spaceRequired)
    .ThenBy(x => x.Size)
    .FirstOrDefault();

Console.WriteLine($"Smallest directory to delete is {smallestDir.Name} with size {size}, freeing the system with a total unused space of {totalCapacity - size}");

/*
 *  Recursively calculates total size of a directory and all its subDirectories
 */
int CalculateDirectorySize(Directory directory)
{
    if (!directory.SubDirectories.Any()) return directory.Size;
    return directory.Size + directory.SubDirectories.Sum(CalculateDirectorySize);
}