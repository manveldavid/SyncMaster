namespace SyncMaster.Common;
public static class DirectoryActions
{
    public static void SyncDirectoriesRecursively(Models.Directory main, Models.Directory sub)
    {
        var exsistDirectories = main.Children.Where(m => sub.Children.Select(s => s.Name).Contains(m.Name));
        var newDirectories = main.Children.Where(m => !sub.Children.Select(s => s.Name).Contains(m.Name));
        var deleteDirectories = sub.Children.Where(m => !main.Children.Select(s => s.Name).Contains(m.Name));

        var compareFiles = main.Files.Where(m => sub.Files.Select(s => s.Name).Contains(m.Name));
        var newFiles = main.Files.Where(m => !sub.Files.Select(s => s.Name).Contains(m.Name));
        var deletedFiles = sub.Files.Where(m => !sub.Files.Select(s => s.Name).Contains(m.Name));


        foreach (var directory in deleteDirectories) { AddDeletedDirectory(directory); }
        foreach (var directory in newDirectories) { AddNewDirectory(directory, sub.FullName); }

        foreach (var file in deletedFiles) { FileActions.AddDeletedFile(file); }
        foreach (var file in newFiles) { FileActions.AddNewFile(file, sub.FullName); }


        foreach (var file in compareFiles)
        {
            var mainFile = main.Files.First(m => m.Name == file.Name);
            var subFile = sub.Files.First(s => s.Name == file.Name);
            FileActions.CompareFiles(mainFile, subFile);
        }

        foreach (var directory in exsistDirectories)
        {
            var mainChild = main.Children.First(m => m.Name == directory.Name);
            var subChild = sub.Children.First(s => s.Name == directory.Name);
            SyncDirectoriesRecursively(mainChild, subChild);
        }
    }


    public static void AddNewDirectory(Models.Directory directory, string destination)
    {
        Marks.NewDirectories.Add(directory);
        Marks.NewDirectoriesDestination.Add(directory, destination);
    }
    public static void AddDeletedDirectory(Models.Directory directory) 
    {
        Marks.DeletedDirectories.Add(directory);
    }

    public static void DeleteDirectory(Models.Directory directory)
    {
        if (Directory.Exists(directory.FullName))
        {
            try
            {
                Directory.Delete(directory.FullName, true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public static void CreateDirectoryRecursively(Models.Directory directory, string destination)
    {
        try
        {
            var newDirectory = destination + Models.Directory.Separator + directory.Name;
            Directory.CreateDirectory(newDirectory);
            directory.Files.ForEach(f => FileActions.CopyFile(f, newDirectory));
            directory.Children.ForEach(d => DirectoryActions.CreateDirectoryRecursively(d, newDirectory));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void CopyDirectories()
    {
        Marks.NewDirectories.ForEach(directory => CreateDirectoryRecursively(directory, Marks.NewDirectoriesDestination[directory]));
    }
    public static void DeleteDirectories()
    {
        Marks.DeletedDirectories.ForEach(directory => DeleteDirectory(directory));
    }

    public static void Sync(string from, string to)
    {
        Marks.Clear();

        Console.WriteLine($"From:{from}");
        Console.WriteLine($"To:{to}");
        Console.WriteLine();

        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
        {
            Console.WriteLine("Path Error!");
            Console.ReadLine();
        }

        var mainDirectory = new SyncMaster.Models.Directory(from);
        var subDirectory = new SyncMaster.Models.Directory(to);

        Console.WriteLine("Scaning...");
        DirectoryActions.SyncDirectoriesRecursively(mainDirectory, subDirectory);

        if (!Marks.Check())
        {
            Console.WriteLine("Conflicts!");
            Console.ReadLine();
        }

        Console.WriteLine("Syncing...");
        FileActions.DeleteFiles();
        DirectoryActions.DeleteDirectories();
        DirectoryActions.CopyDirectories();
        FileActions.CopyFiles();
        Console.WriteLine();
        Console.WriteLine("Sync end");
        Console.WriteLine();
    }
}
