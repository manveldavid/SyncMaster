namespace SyncMaster.Models;

public class SyncInfo
{
    public string From { get; set; }
    public string To { get; set; }
    public override string ToString() =>
        $"{From} -> {To}";
}
