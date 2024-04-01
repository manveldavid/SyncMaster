using SyncMaster.Common;
using SyncMaster.Models;
using System.Diagnostics;
using System.Text.Json;

var fileName = "SyncDirectories.json";
if (!File.Exists(fileName))
{
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    File.Create(filePath).Close();
    Process.Start(new ProcessStartInfo() { FileName = filePath, UseShellExecute = true });
    return;
}

Console.WriteLine("Scaning...");

var fileData = File.ReadAllText(fileName);

var syncInfos = JsonSerializer.Deserialize<IEnumerable<SyncInfo>>(fileData);

foreach (var syncInfo in syncInfos!)
{
    DirectoryActions.Sync(syncInfo.From, syncInfo.To);
}

Console.WriteLine("\n\nSync end");
Console.ReadLine();