using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class DiningTable
    {
        public DiningTable()
        {
            OrderMasters = new HashSet<OrderMaster>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public int BranchId { get; set; }
        public int? TablesId { get; set; }
        public int SeatNo { get; set; }
        public string Capacity { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Branch Tables { get; set; }
        public virtual ICollection<OrderMaster> OrderMasters { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
