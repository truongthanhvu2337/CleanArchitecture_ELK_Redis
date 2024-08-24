using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; } 
        public int quantity { get; set; }
        public double unitPrice { get; set; }

        public virtual ICollection<OrderDetail> Items { get; set; }

    }
}
