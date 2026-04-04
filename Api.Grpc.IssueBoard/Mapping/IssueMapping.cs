using Api.Grpc.IssueBoard.Models;
using Google.Protobuf.WellKnownTypes;
using IssueBoard.Grpc;

namespace Api.Grpc.IssueBoard.Mapping;

public static class IssueMapping
{
    public static IssueDto ToDto(this Issue issue)
    {
        var dto = new IssueDto
        {
            Id = issue.Id,
            AuthorName = issue.AuthorName,
            CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(issue.CreatedAt, DateTimeKind.Utc)),
            Category = issue.Category ?? string.Empty,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status,
            Resolution = issue.Resolution ?? string.Empty,
            ResolverName = issue.ResolverName ?? string.Empty,
        };

        if (issue.ResolvedAt.HasValue)
            dto.ResolvedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(issue.ResolvedAt.Value, DateTimeKind.Utc));

        return dto;
    }
}
