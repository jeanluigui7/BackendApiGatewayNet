using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{

    public class MailService : IMailService
    {
        private SmtpSetting _settings;

        public async Task<bool> Send(MailBodyMessage content)
        {
            DateTime fecha = DateTime.UtcNow;
            var message = new MailMessage
            {
                From = new MailAddress(_settings.SmtpEmail, _settings.SmtpDisplayname),
                Body = content.Message,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                Priority = MailPriority.Normal,
                IsBodyHtml = true,
                //BodyTransferEncoding = TransferEncoding.QuotedPrintable
            };
            var correos = content.Correo.Split(',');
            foreach (var correo in correos)
            {
                if (string.IsNullOrWhiteSpace(correo)) continue;

                message.To.Add(correo.Trim());
            }
            message.Subject = content.Asunto;

            if (content.Files != null)
            {
                foreach (var file in content.Files)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(file.Content), file.Filename));
                }
            }

            var result = false;
            try
            {
                using (message)
                using (var client = new SmtpClient
                {
                    Host = _settings.SmtpHost,
                    Port = _settings.SmtpPort,
                    EnableSsl = _settings.SmtpSsl,
                    Timeout = 5000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = _settings.UseDefaultCredentials,
                })
                {
                    if (!_settings.UseDefaultCredentials)
                    {
                        client.Credentials = new NetworkCredential(_settings.SmtpEmail, _settings.SmtpPassword);
                    }
                    await client.SendMailAsync(message);
                }
                result = true;

            }
            catch (Exception e)
            {
                Logger.Logger.EscribirLog(e);
            }
            return result;
        }

        public void SetEmailSetting(SmtpSetting setting)
        {
            _settings = setting;
        }


    
    }
}
