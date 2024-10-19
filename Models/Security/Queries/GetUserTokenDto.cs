using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Security.Queries
{
    public class GetUserTokenDto
    {
        public int UserID { get; set; }
        public string Token { get; set; }
    }
}
