namespace SyncMaster.Models;
public class Directory
{
    public const char Separator = '\\';
    public Directory(string path, Directory parent = null)
    {
        if(!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        Parent = parent;
        FullName = path;
        Name = path.Substring(path.LastIndexOf(Separator) + 1);
        Files = 
            System.IO.Directory.GetFiles(FullName)
            .Select(p => new FileInfo(p)).ToList();
        Children = 
            System.IO.Directory.GetDirectories(path)
            .Select(p => new Directory(p, this)).ToList();
    }

    public List<FileInfo> Files { get; set; }
    public Directory Parent { get; set; }
    public List<Directory> Children { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }

    public override string ToString()
    {
        return Parent != null ? Parent.ToString() + Separator + Name : Name;
    }
}
