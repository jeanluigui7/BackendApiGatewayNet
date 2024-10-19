using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilitary
{
    public class UtilService : IUtilService
    {
        private readonly IConfiguration _configuration;
        public UtilService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool saveFile(IList<IFormFile> files, string typeRoute, string guid = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(typeRoute))
                {
                    string route = getRoute(typeRoute);

                    foreach (IFormFile file in files)
                    {
                        if (file.Length > 0)
                        {
                            string[] commandPicture = file.FileName.Split(".");
                            string filePath = Path.Combine(route, guid == "" ? file.FileName : commandPicture[0] + "_" + guid + "." + commandPicture[commandPicture.Length - 1]);
                            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;

            }
            catch (Exception e)
            {
                Logger.Logger.EscribirLog(e);
                return false;
            }
        }
        public string getRoute(string typeRoute)
        {
             string route = String.Empty;
            //    switch (typeRoute)
            //    {
            //        case ImageRouteConstants.User:
            //            {
            //                route = _configuration["ResourcesTarget:RouteUser"];
            //            }
            //            break;
            //        case ImageRouteConstants.Product:
            //            {
            //                route = _configuration["ResourcesTarget:RouteProduct"];
            //            }
            //            break;
            //        case ImageRouteConstants.Store:
            //            {
            //                route = _configuration["ResourcesTarget:RouteStore"];
            //            }
            //            break;
            //    }
            return route;
        }

        public string ConvertToBase64(string url, string extension)
        {
            string base64 = "";

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(60);
                    var response = client.GetByteArrayAsync(url);
                    response.Wait();
                    var bytes = response.Result;
                    base64 = $"data:image/{extension};base64," + Convert.ToBase64String(bytes);
                }
            }
            catch (AggregateException ex)
            {
            }
            return base64;
        }

        public string ConvertUrlToBase64(string url, string extension)
        {
            string base64Result = string.Empty;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(url);
                    string base64String = Convert.ToBase64String(data);
                    base64Result = $"data:image/{extension};base64," + base64String;
                    return base64Result;
                }
            }
            catch (Exception)
            {
                base64Result = string.Empty;
            }
            return base64Result;
        }

    }
}
