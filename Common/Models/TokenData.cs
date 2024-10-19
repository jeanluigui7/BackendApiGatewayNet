using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class TokenData
    {
        public string token { get; set; }
        public DateTimeOffset expiration { get; set; }
    }
}
