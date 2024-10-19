using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class EntityResponse<T>
    {
        public int Code { get; set; }
        public T Payload { get; set; }
        public string Message { get; set; }
        public int TypeMessage { get; set; }
    }
}
