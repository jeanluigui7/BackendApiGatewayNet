using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Security.Queries
{
    public class GetUserPersonQueryDto
    {
        public TokenData tokenData { get; set; }
        public UserPersonDto userPerson { get; set; }
    }
}
