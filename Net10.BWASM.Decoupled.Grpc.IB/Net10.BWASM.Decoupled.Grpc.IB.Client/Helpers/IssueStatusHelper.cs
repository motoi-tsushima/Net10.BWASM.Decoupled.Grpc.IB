using Google.Protobuf.WellKnownTypes;

namespace Net10.BWASM.Decoupled.Grpc.IB.Client.Helpers;

public static class IssueStatusHelper
{
    public static string GetDisplayName(int status) => status switch
    {
        0 => "未着手",
        1 => "着手中",
        2 => "解決失敗",
        3 => "課題確認不能",
        4 => "解決済み",
        _ => "不明"
    };

    public static IEnumerable<(int Value, string Label)> GetAllStatuses()
    {
        yield return (0, "未着手");
        yield return (1, "着手中");
        yield return (2, "解決失敗");
        yield return (3, "課題確認不能");
        yield return (4, "解決済み");
    }

    public static string FormatDateTime(Timestamp? ts, string format = "yyyy/MM/dd HH:mm")
    {
        if (ts is null) return string.Empty;
        return ts.ToDateTime().ToLocalTime().ToString(format);
    }
}
