using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerProgram.Model
{
    /// <summary>
    /// This object represents the OrderDetails table in RetailerRewards database
    /// </summary>
    internal class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public float UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
