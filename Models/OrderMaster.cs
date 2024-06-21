using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class OrderMaster
    {
        public OrderMaster()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? OrderuserId { get; set; }
        public int TableId { get; set; }
        public int? OrderTablesId { get; set; }
        public string? CustName { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public string? OrderStatus { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual DiningTable? OrderTables { get; set; }
        public virtual Usermaster? Orderuser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
