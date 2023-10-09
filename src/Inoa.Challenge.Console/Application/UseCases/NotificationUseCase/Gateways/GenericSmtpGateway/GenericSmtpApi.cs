using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Inoa.Challenge.Console.Application.Models;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Abstractions;
using Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Models;
using Inoa.Challenge.Console.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Gateways.GenericSmtpGateway;

public class GenericSmtpApi : IGenericSmtpApi
{
    private readonly EmailSettings _options;
    private readonly ILogger<GenericSmtpApi> _logger;

    public GenericSmtpApi(ILogger<GenericSmtpApi> logger, IOptions<EmailSettings> options)
    {
        _options = options.Value;
        _logger = logger;
    }

    public void SendEmail(NotificationModel model)
    {
        var smtpClient = new SmtpClient(_options.Smtp)
        {
            Port = _options.Port ?? 25,
            Credentials = GetCredential(_options.Credentials),
            EnableSsl = _options.EnableSsl,
        };

        var emailSubject = CreateEmailSubject(model.Operation, model.AssetName);
        var emailBody = CreateEmailBody(GetHtmlTemplate(), model.Operation, model.AssetName, model.StartPrice, model.EndPrice);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_options.From),
            Subject = emailSubject,
            Body = emailBody,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(_options.To);

        smtpClient.Send(mailMessage);

        _logger.LogInformation("email sent to {receiver}", _options.To);
    }

    private NetworkCredential? GetCredential(Credentials? credentials)
    {
        if (credentials is null || string.IsNullOrWhiteSpace(credentials?.Username))
            return null;

        return new NetworkCredential(credentials.Username, credentials.Password);
    }

    private string GetHtmlTemplate()
    {
        var assembly = typeof(GenericSmtpApi).Assembly;
        var filePath = "Inoa.Challenge.Console.Application.UseCases.NotificationUseCase.Gateways.GenericSmtpGateway.template.html";

        Stream file = assembly.GetManifestResourceStream(filePath);

        using var sr = new StreamReader(file);
        var html = sr.ReadToEnd();
        return html;
    }

    private static string CreateEmailSubject(OperationType operation, string assetName) =>
        $"Oportunidade de {(operation == OperationType.Buy ? "compra" : "venda")} ação {assetName}";

    private string CreateEmailBody(string html, OperationType operation, string assetName, decimal startPrice, decimal endPrice)
    {
        string output = Regex.Replace(html, @"\[(\w+)\]", match =>
        {
            string propertyName = match.Groups[1].Value;
            switch (propertyName.ToLower())
            {
                case "assetname":
                    return assetName;
                case "operation":
                    return operation == OperationType.Buy ? "abaixou" : "subiu";
                case "startprice":
                    return startPrice.ToString();
                case "endprice":
                    return endPrice.ToString();
                case "action":
                    return operation == OperationType.Buy ? "comprar" : "vender";
                default:
                    return match.Value;
            }
        });

        return output;
    }
}