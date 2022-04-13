using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Report
{
    public string version = Application.version;
    public string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public double epoch = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    public string userId = "Test User";
    public string eventKey;
    public string eventValue;
    public Vector3 position;
    public ClockType clockInfo;
    public Vector3Int delta = Vector3Int.zero;
    public Report(string eventKey, string eventValue)
    {
        this.eventKey = eventKey;
        this.eventValue = eventValue;
    }
}

public static class Reporter
{
    private const string path = "./Assets/Logs/userLog.json";
    private static readonly StreamWriter Writer = new(path, true);

    //TODO: Report a KeyFrame event for clocks added, or the target time being created
    //TODO: Report a PlayerEvent for a clock being disassembled

    public static void ReportEvent(string userId, string eventKey, string eventValue)
    {
        Writer.WriteLine(JsonUtility.ToJson(new Report(eventKey, eventValue)));
        Writer.Flush();
    }
    
    public static void ReportSelect(Clock clock)
    {
        var report = new Report("SELECTED", "PlayerEvent")
        {
            position = clock.transform.position,
        };
        Writer.WriteLine(JsonUtility.ToJson(report));
        Writer.Flush();
    }
    
    public static void ReportDeselect(Clock clock)
    {
        var report = new Report("DESELECTED", "PlayerEvent")
        {
            position = clock.transform.position,
        };
        Writer.WriteLine(JsonUtility.ToJson(report));
        Writer.Flush();
    }
    
    public static void ReportMerge(Clock clock) {
        var report = new Report("MERGE", "PlayerEvent")
        {
            position = clock.transform.position,
            clockInfo = clock.info,
        };
        Writer.WriteLine(JsonUtility.ToJson(report));
        Writer.Flush();
    }
    
    public static void ReportMadeTime(Clock clock) {
        var report = new Report("MADE", "PlayerEvent")
        {
            position = clock.transform.position,
            clockInfo = clock.info,
        };
        Writer.WriteLine(JsonUtility.ToJson(report));
        Writer.Flush();
    }

    public static void Close() => Writer.Close();
}