using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Timeslot
    {
        public Timeslot()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public int BranchId { get; set; }
        public int? TimeslotId { get; set; }
        public DateTime ReservationDay { get; set; }
        public string? MealType { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Branch? TimeslotNavigation { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
