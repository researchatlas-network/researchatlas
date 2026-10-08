namespace ResearchAtlas.Application.Abstractions.Messaging;

public interface IEmailSender
{
    Task SendAsync(
        IReadOnlyCollection<string> recipients,
        string subject,
        string body,
        CancellationToken cancellationToken = default);
}
