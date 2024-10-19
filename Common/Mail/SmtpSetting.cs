using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Mail
{
    /// <summary>
    /// Class SmtpSetting.
    /// </summary>
    public class SmtpSetting
    {
        /// <summary>
        /// Gets or sets the SMTP host.
        /// </summary>
        /// <value>The SMTP host.</value>
        [Required]
        [MaxLength(80)]
        public string SmtpHost { get; set; }
        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>The SMTP port.</value>
        public int SmtpPort { get; set; }
        /// <summary>
        /// Gets or sets the SMTP email.
        /// </summary>
        /// <value>The SMTP email.</value>
        [Required]
        [MaxLength(80)]
        public string SmtpEmail { get; set; }
        /// <summary>
        /// Gets or sets the SMTP password.
        /// </summary>
        /// <value>The SMTP password.</value>
        [Required]
        [MaxLength(50)]
        public string SmtpPassword { get; set; }
        /// <summary>
        /// Gets or sets the SMTP SSL.
        /// </summary>
        /// <value>The SMTP SSL.</value>
        public bool SmtpSsl { get; set; }
        public string SmtpDisplayname { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
