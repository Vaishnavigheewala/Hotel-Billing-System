using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Menu
    {
        public Menu()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string ItemName { get; set; } = null!;
        public double Price { get; set; }
        public string Description { get; set; } = null!;
        public bool ActiveFlag { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
