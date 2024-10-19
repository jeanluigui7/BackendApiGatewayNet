using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserToken : BaseEntity
    {
        public int UserID { get; set; }
        public string Token { get; set; }
    }
}

