using System;
using System.Collections.Generic;

namespace Api.Grpc.IssueBoard.Models;

public partial class Issue
{
    public int Id { get; set; }

    public string AuthorName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? Category { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    /// <summary>
    /// 0:未着手, 1:着手中, 2:解決失敗, 3:課題確認不能, 4:解決済み
    /// </summary>
    public int Status { get; set; }

    public string? Resolution { get; set; }

    public string? ResolverName { get; set; }

    public DateTime? ResolvedAt { get; set; }
}
