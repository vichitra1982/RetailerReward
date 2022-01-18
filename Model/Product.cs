using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerProgram.Model
{
    /// <summary>
    /// This object represents the Products table in RetailerRewards database
    /// </summary>
    internal class Product
    {
        public int ProductId { get; set; }
        public int ProductName { get; set; }
        public float UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
