using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilitary
{
    public interface IUtilService
    {
        Boolean saveFile(IList<IFormFile> files, string route, string guid = "");
        string getRoute(string typeRoute);
        public string ConvertToBase64(string url, string extension);
        public string ConvertUrlToBase64(string url, string extension);
    }
}
