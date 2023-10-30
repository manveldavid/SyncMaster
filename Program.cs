using SyncMaster.Common;
using System.Diagnostics;
using System.Reflection;

var fileName = "SyncDirectories.txt";
if (!File.Exists(fileName))
{
    File.Create(fileName).Close();
    var filePath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, fileName);
    Process.Start("explorer ", filePath);
    return;
}
var fileData = File.ReadAllText(fileName);
var directoriesPairs = fileData.Split(';');
foreach (var directoriesPair in directoriesPairs)
{
    var clearData = directoriesPair;
    clearData = clearData.Replace("\n", "");
    clearData = clearData.Replace("\r", "");
    clearData = clearData.Replace(" ", "");
    if (string.IsNullOrEmpty(clearData)) continue;
    var dirs = clearData.Split(',');
    DirectoryActions.Sync(dirs.First(), dirs.Last());
}

Task.Delay(3000).Wait();