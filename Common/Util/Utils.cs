using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Util
{
    public static class Utils
    {
        private static List<string> lstExtensions = new List<string>
        {
           "jpg","png","jpeg","png"
        };
        private static List<string> lstNamesNotPermit = new List<string>
        {
            ";",":",">","<","/",".","*","%","$"
        };
        public static bool ValidateFilesIncludes(string extension)
        {
            return lstExtensions.Contains(extension.ToLower());
        }
        public static bool ValidateFilesNamesNotPermit(string fileName)
        {
            return lstNamesNotPermit.Contains(fileName.ToLower());
        }
        public static string GetTextDateForMyHistoryProducts(DateTime date)
        {

            string text;
            if (date.Date == DateTime.Now.AddDays(-1).Date)
            {
                text = "Ayer";
            }
            else if (date.Date == DateTime.Now.Date)
            {

                text = "Hoy";
            }
            else
            {
                text = date.ToString("dddd, dd MMMM yyyy");
            }

            return text;
        }

        public static JObject GetObjectError(Exception e)
        {
            JObject logJson = new JObject();
            logJson.Add("action", "exception");
            logJson.Add("data", new JObject
                {
                    { "message", e.Message},
                    { "stackTrace", e.StackTrace}
                });
            return logJson;
        }
        public static JObject GetObjecDynamictError(dynamic e)
        {
            JObject logJson = new JObject();
            logJson.Add("action", "exception");
            logJson.Add("data", new JObject
                {
                    { "message", e.Message},
                });
            return logJson;
        }

        public static string Extract(this string input, int len)
        {
            if (string.IsNullOrEmpty(input) || input.Length < len)
            {
                return input;
            };

            return input.Substring(0, len);
        }
        /// <summary>
        /// Returns the input string with the first character converted to uppercase
        /// </summary>
        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public static IList<DateTime> GetDateByStartAndEndDate(DateTime start, DateTime end)
        {
            var firstDate = new DateTime(start.Year, start.Month, start.Day);
            var lastDate = new DateTime(end.Year, end.Month, end.Day);

            var firstDayOfWeekDates = new List<DateTime>();
            firstDayOfWeekDates.Add(firstDate);

            while (lastDate > firstDate)
            {
                firstDate = firstDate.AddDays(1);
                firstDayOfWeekDates.Add(firstDate);
            }



            return firstDayOfWeekDates;
        }
        public static IList<DateTime> GetDateByMonthId(int month)
        {
            var year = DateTime.Now.Year;
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                   .Select(day => new DateTime(year, month, day)) // Map each day to a date
                   .ToList();
        }
        public static bool ListFilesValidate(List<IFormFile> files)
        {
            bool validate = true;
            foreach (IFormFile file in files)
            {
                string filename = Reverse(file.FileName);
                filename = Reverse(filename.Split('.')[0]);
                validate = ValidateFilesIncludes(filename);
                if (!validate)
                {
                    break;
                }
            }
            return validate;
        }

        public static bool ListFilesNamesValidate(List<IFormFile> files)
        {
            bool validate = true;
            foreach (IFormFile file in files)
            {
                string filename = file.FileName.Split(".")[0];
                validate = ValidateFilesNamesNotPermit(filename);
                if (validate)
                {
                    break;
                }
            }
            return validate;
        }

        public static bool FilesMaxSizeValidate(List<IFormFile> files, string maxSize)
        {
            bool validate = true;
            if (files.Count > 0)
            {
                foreach (IFormFile file in files)
                {
                    if (file.Length > Convert.ToInt64(maxSize))
                    {
                        validate = false;
                        break;
                    }
                }
            }
            return validate;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
