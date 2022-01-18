using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerProgram.Model
{
    internal class RewardsPerCustomer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public DateTime OrderStart { get; set; }
        public DateTime OrderEnd { get; set; }
        public double OrderTotal { get; set; }
        public double RewardsGained { get; set; }
    }
}
