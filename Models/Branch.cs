using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Branch
    {
        public Branch()
        {
            DiningTables = new HashSet<DiningTable>();
            Reservations = new HashSet<Reservation>();
            Timeslots = new HashSet<Timeslot>();
        }

        public int Id { get; set; }
        public int RestaurentId { get; set; }
        public int? RestBranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public long ContactNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Restaurent? RestBranch { get; set; }
        public virtual ICollection<DiningTable> DiningTables { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Timeslot> Timeslots { get; set; }
    }
}
