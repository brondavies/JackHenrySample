using JackHenrySample.Interfaces;

namespace JackHenrySample.Implementation
{
    public class StubEmailService : IEmailService
    {
        /// <summary>
        /// Returns true if the email is successfully sent
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool SendEmail(string to, string from, string subject, string message)
        {
            //This would send an email
            return true;
        }
    }
}
