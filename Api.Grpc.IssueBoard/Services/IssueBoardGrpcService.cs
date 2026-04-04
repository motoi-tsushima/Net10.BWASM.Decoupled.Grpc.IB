using Api.Grpc.IssueBoard.Data;
using Api.Grpc.IssueBoard.Mapping;
using Api.Grpc.IssueBoard.Models;
using Grpc.Core;
using IssueBoard.Grpc;
using Microsoft.EntityFrameworkCore;

namespace Api.Grpc.IssueBoard.Services;

public class IssueBoardGrpcService(IssuesDbContext db, ILogger<IssueBoardGrpcService> logger)
    : global::IssueBoard.Grpc.IssueBoardService.IssueBoardServiceBase
{
    public override async Task<GetIssuesReply> GetIssues(GetIssuesRequest request, ServerCallContext context)
    {
        var issues = await db.Issues
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(context.CancellationToken);

        var reply = new GetIssuesReply();
        reply.Issues.AddRange(issues.Select(i => i.ToDto()));
        return reply;
    }

    public override async Task<GetIssueReply> GetIssue(GetIssueRequest request, ServerCallContext context)
    {
        var issue = await db.Issues.FindAsync([request.Id], context.CancellationToken);
        if (issue is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Issue {request.Id} not found."));

        return new GetIssueReply { Issue = issue.ToDto() };
    }

    public override async Task<CreateIssueReply> CreateIssue(CreateIssueRequest request, ServerCallContext context)
    {
        var issue = new Issue
        {
            AuthorName = request.AuthorName,
            Category = string.IsNullOrEmpty(request.Category) ? null : request.Category,
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            Status = 0,
        };

        db.Issues.Add(issue);
        await db.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Issue created: Id={Id}, Title={Title}", issue.Id, issue.Title);
        return new CreateIssueReply { Issue = issue.ToDto() };
    }

    public override async Task<UpdateIssueReply> UpdateIssue(UpdateIssueRequest request, ServerCallContext context)
    {
        var issue = await db.Issues.FindAsync([request.Id], context.CancellationToken);
        if (issue is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Issue {request.Id} not found."));

        issue.Category = string.IsNullOrEmpty(request.Category) ? null : request.Category;
        issue.Title = request.Title;
        issue.Description = request.Description;
        issue.Status = request.Status;
        issue.Resolution = string.IsNullOrEmpty(request.Resolution) ? null : request.Resolution;
        issue.ResolverName = string.IsNullOrEmpty(request.ResolverName) ? null : request.ResolverName;
        issue.ResolvedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Issue updated: Id={Id}", issue.Id);
        return new UpdateIssueReply { Issue = issue.ToDto() };
    }

    public override async Task<DeleteIssueReply> DeleteIssue(DeleteIssueRequest request, ServerCallContext context)
    {
        var issue = await db.Issues.FindAsync([request.Id], context.CancellationToken);
        if (issue is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Issue {request.Id} not found."));

        db.Issues.Remove(issue);
        await db.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Issue deleted: Id={Id}", request.Id);
        return new DeleteIssueReply { Success = true };
    }
}
