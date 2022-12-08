namespace JackHenrySample.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(string to, string from, string subject, string message);
    }
}
