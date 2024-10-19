using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mail
{
    public class MailBodyMessage
    {
        /// <summary>
        /// Gets or sets el nombre del email.
        /// </summary>
        /// <value>The name.</value>
        public string Asunto { get; set; }
        /// <summary>
        /// Gets or sets the correo del destinatario.
        /// </summary>
        /// <value>The correo.</value>
        public string Correo { get; set; }
        /// <summary>
        /// Gets or sets the custom message .
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public IEnumerable<MailFile> Files { get; set; }
    }
    /// <summary>
    /// Class MailFile.
    /// </summary>
    public class MailFile
    {
        public string Filename { get; set; }
        public byte[] Content { get; set; }
    }

    public class EmailTemplate
    {
        public int ModuleId { get; set; }
        public int TemplateId { get; set; }
        public string SubjectGeneral { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string ImageBackground { get; set; }
        public string LinkRedirect { get; set; }
        public string TitleButton { get; set; }
    }
}
