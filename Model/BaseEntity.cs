using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public int StatusID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        // paginated
        public int NroRow { get; set; }
        public int TotalRow { get; set; }

        // token
        public string Token { get; set; }
    }
}
