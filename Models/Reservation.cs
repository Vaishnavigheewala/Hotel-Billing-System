using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ReservationuserId { get; set; }
        public int TableId { get; set; }
        public int? ReservationTablesId { get; set; }
        public int TimeslteId { get; set; }
        public int? ReservationTimeslotsId { get; set; }
        public int BranchId { get; set; }
        public int? ReservationBranchesId { get; set; }
        public int RestaurentId { get; set; }
        public int? ResevationRestId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationStatus { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Branch ReservationBranches { get; set; }
        public virtual DiningTable ReservationTables { get; set; }
        public virtual Timeslot ReservationTimeslots { get; set; }
        public virtual Usermaster Reservationuser { get; set; }
        public virtual Restaurent ResevationRest { get; set; }


        //public IEnumerable<DiningTable>? ReservationTablesList { get; set; }
        //public IEnumerable<Timeslot>? ReservationTimeslotsList { get; set; }



    }
}
