using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ATS.CodeLibrary.Email
{
    public class MailHelper
    {
        private string smtpServer;
        string fromAddress;
        string fromDisplayName;

        /// <summary>
        /// Send email message to one or more people
        /// </summary>
        /// <param name="smtpServer">SMTP Server name (example relay2.atsna.atsauto.net)</param>
        /// <param name="fromAddress">Email address with @atsautomation.com ending</param>
        /// <param name="fromDisplayName">Display name on email</param>
        public MailHelper(string smtpServer, string fromAddress, string fromDisplayName)
        {
            this.smtpServer = smtpServer;
            this.fromAddress = fromAddress;
            this.fromDisplayName = fromDisplayName;
        }

        /// <summary>
        /// Send email message to one or more people
        /// </summary>
        /// <param name="message">The full message that makes up the body of the email</param>
        /// <param name="toAddresses">List of addresses separated by semicolons (;)</param>
        /// <param name="subject">Email subject</param>
        /// <param name="filesToAttach">Full paths of files to attach</param>
        public void SendMessage(string message, string toAddresses, string subject, IEnumerable<string> filesToAttach = null)
        {
            SendMessage(message, toAddresses, subject, smtpServer, fromAddress, fromDisplayName, filesToAttach);
        }

        /// <summary>
        /// Send email message to one or more people
        /// </summary>
        /// <param name="message">The full message that makes up the body of the email</param>
        /// <param name="toAddresses">List of addresses of addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="filesToAttach">Full paths of files to attach</param>
        public void SendMessage(string message, IEnumerable<string> toAddresses, string subject, IEnumerable<string> filesToAttach = null)
        {
            SendMessage(message, toAddresses, subject, smtpServer, fromAddress, fromDisplayName, filesToAttach);
        }

        /// <summary>
        /// Send email message to one or more people
        /// </summary>
        /// <param name="message">The full message that makes up the body of the email</param>
        /// <param name="toAddresses">List of addresses separated by semicolons (;)</param>
        /// <param name="subject">Email subject</param>
        /// <param name="smtpServer">SMTP Server name (example relay2.atsna.atsauto.net)</param>
        /// <param name="fromAddress">Email address with @atsautomation.com ending</param>
        /// <param name="fromDisplayName">Display name on email</param>
        /// <param name="filesToAttach">Full paths of files to attach</param>
        public static void SendMessage(string message, string toAddresses, string subject, string smtpServer, string fromAddress, string fromDisplayName, IEnumerable<string> filesToAttach = null)
        {
            using (MailMessage msg = new MailMessage())
            {
                string[] addresses = toAddresses.Split(new char[] { ';' });
                foreach (string recip in addresses)
                {
                    if (recip.Length > 0)
                    {
                        msg.To.Add(recip);
                    }
                }

                if (filesToAttach != null)
                {
                    foreach (var file in filesToAttach)
                    {
                        var attach = new Attachment(file);
                        msg.Attachments.Add(attach);
                    }
                }

                msg.From = new MailAddress(fromAddress, fromDisplayName);
                msg.Subject = subject;


                AlternateView plainView = AlternateView.CreateAlternateViewFromString(message, null, "text/plain");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");

                msg.AlternateViews.Add(plainView);
                msg.AlternateViews.Add(htmlView);

                if (!string.IsNullOrWhiteSpace(smtpServer))
                {
                    SmtpClient smtp = new SmtpClient(smtpServer);
                    smtp.Send(msg);
                }
                else
                {
                    throw new Exception("SMTP Server location not found in configuration");
                }

            }
        }

        /// <summary>
        /// Send email message to one or more people
        /// </summary>
        /// <param name="message">The full message that makes up the body of the email</param>
        /// <param name="toAddresses">List of addresses of addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="smtpServer">SMTP Server name (example relay2.atsna.atsauto.net)</param>
        /// <param name="fromAddress">Email address with @atsautomation.com ending</param>
        /// <param name="fromDisplayName">Display name on email</param>
        /// <param name="filesToAttach">Full paths of files to attach</param>
        public static void SendMessage(string message, IEnumerable<string> toAddresses, string subject, string smtpServer, string fromAddress, string fromDisplayName, IEnumerable<string> filesToAttach = null)
        {
            var to = string.Join(";", toAddresses);
            SendMessage(message, to, subject, smtpServer, fromAddress, fromDisplayName, filesToAttach);
        }
    }
}
