using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual OrderMaster Order { get; set; } = null!;
    }
}
