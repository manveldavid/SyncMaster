namespace SyncMaster.Common;
public static class FileActions
{
    public static void CompareFiles(FileInfo mainFile,  FileInfo subFile)
    {
        if(mainFile.LastWriteTime >  subFile.LastWriteTime)
        {
            AddNewFile(mainFile, subFile.DirectoryName);
        }
    }

    public static void AddNewFile(FileInfo file, string destination)
    {
        Marks.NewFiles.Add(file);
        Marks.NewFilesDestination.Add(file, destination);
    }
    public static void AddDeletedFile(FileInfo file) 
    {
        Marks.DeletedFiles.Add(file);
    }

    public static void DeleteFile(FileInfo file)
    {
        if(File.Exists(file.FullName))
        {
            try
            {
                if((File.GetAttributes(file.FullName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    var attributes = File.GetAttributes(file.FullName);
                    attributes &= ~FileAttributes.ReadOnly;
                    File.SetAttributes(file.FullName, attributes);
                }

                File.Delete(file.FullName);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public static void CopyFile(FileInfo file, string destination)
    {
        try
        {
            File.Copy(file.FullName, destination + Models.Directory.Separator + file.Name, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static void DeleteFiles()
    {
        Marks.DeletedFiles.ForEach(file => FileActions.DeleteFile(file));
    }
    public static void CopyFiles()
    {
        Marks.NewFiles.ForEach(file => FileActions.CopyFile(file, Marks.NewFilesDestination[file]));
    }
}
