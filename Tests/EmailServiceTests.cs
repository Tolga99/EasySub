using API.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.Threading.Tasks;

public class EmailServiceTests
{
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.SetupGet(x => x["EmailSettings:SmtpServer"]).Returns("smtp.example.com");
        mockConfig.SetupGet(x => x["EmailSettings:Port"]).Returns("587");
        mockConfig.SetupGet(x => x["EmailSettings:Username"]).Returns("test@example.com");
        mockConfig.SetupGet(x => x["EmailSettings:Password"]).Returns("password123");

        _emailService = new EmailService(mockConfig.Object);
    }

    [Fact]
    public async Task SendEmailAsync_ShouldReturnTrue_WhenEmailIsSent()
    {
        // Arrange
        string to = "test@recipient.com";
        string subject = "Test Subject";
        string body = "Test Body";

        // Act
        bool result = await _emailService.SendEmailAsync(to, subject, body);

        // Assert

    }
}