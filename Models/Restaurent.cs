using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Restaurent
    {
        public Restaurent()
        {
            Branches = new HashSet<Branch>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string RestaurentName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public long ContactNumber { get; set; }
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
