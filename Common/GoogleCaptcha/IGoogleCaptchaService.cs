using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.GoogleCaptcha
{
    public interface IGoogleCaptchaService
    {
        bool ValidateCaptcha(string token);
    }
}
