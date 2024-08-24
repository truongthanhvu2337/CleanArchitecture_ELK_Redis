using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get;}
    }
}
