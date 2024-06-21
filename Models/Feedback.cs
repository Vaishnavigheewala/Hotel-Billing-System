using System;
using System.Collections.Generic;

namespace HotelBillingMVC.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Responce { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int? FeedIdId { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual Usermaster? FeedId { get; set; }
    }
}
