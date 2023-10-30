namespace SyncMaster.Common;
public static class Marks
{
    public static List<Models.Directory> DeletedDirectories { get; set; }
    public static List<Models.Directory> NewDirectories { get; set; }
    public static Dictionary<Models.Directory, string> NewDirectoriesDestination { get; set; }

    public static List<FileInfo> DeletedFiles { get; set; }
    public static List<FileInfo> NewFiles { get; set; }
    public static Dictionary<FileInfo, string> NewFilesDestination { get; set; }

    public static void Clear()
    {
        DeletedDirectories = new();
        NewDirectories = new();
        NewDirectoriesDestination = new();
        DeletedFiles = new();
        NewFiles = new();
        NewFilesDestination = new();
    }

    public static bool Check()
    {
        return CheckDirs() && CheckFiles() && CheckDirsDestination() && CheckFilesDestination();
    }

    public static bool CheckDirsDestination() 
    {
        return NewDirectoriesDestination.Keys.Count == NewDirectories.Count && NewDirectoriesDestination.Values.All(p => !string.IsNullOrEmpty(p));
    }
    public static bool CheckFilesDestination()
    {
        return NewFilesDestination.Keys.Count == NewFiles.Count && NewFilesDestination.Values.All(p => !string.IsNullOrEmpty(p));
    }
    public static bool CheckDirs()
    {
        return DeletedDirectories.Where(d => NewDirectories.Select(n => n.FullName).Contains(d.FullName)).Count() == 0;
    }
    public static bool CheckFiles()
    {
        return DeletedFiles.Where(d => NewFiles.Select(n => n.FullName).Contains(d.FullName)).Count() == 0;
    }
}
