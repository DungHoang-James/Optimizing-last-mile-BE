using System.Text;
using OptimizingLastMile.Models.Commons;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OptimizingLastMile.Services.Emails;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;

    public EmailService(ISendGridClient sendGridClient)
    {
        this._sendGridClient = sendGridClient;
    }

    public async Task<GenericResult> SendEmail(string email, string password)
    {
        StringBuilder message = new StringBuilder();

        message.AppendLine("Thank you! Now you are a part of company.");
        message.AppendLine("This is your password");
        message.AppendLine($"Password: {password}");
        message.AppendLine();
        message.AppendLine("Please! Do not send your password to anyone. Change new password after you login.");
        message.AppendLine();
        message.AppendLine("Thanks and best regards.");

        var msg = new SendGridMessage()
        {
            From = new EmailAddress("tuongminh.nguyen2000@gmail.com"),
            Subject = "Information use to login"
        };

        msg.AddContent(MimeType.Text, message.ToString());
        msg.AddTo(new EmailAddress(email));

        var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);

        var error = Errors.Auth.CannotSendEmail();

        return response.IsSuccessStatusCode ? GenericResult.Ok() : GenericResult.Fail(error);
    }
}
