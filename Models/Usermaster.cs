using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBillingMVC.Models
{
    public partial class Usermaster
    {
        public Usermaster()
        {
            Feedbacks = new HashSet<Feedback>();
            OrderMasters = new HashSet<OrderMaster>();
            Reservations = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(15, ErrorMessage = "Username cannot be longer than 15 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Password must be minimum 6 characters, at least one letter and one number")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        //[Phone(ErrorMessage = "Invalid Phone Number")]
        //[RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage = "Phone number must be 10 digits")]

        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;
        public int RoleId { get; set; }
        public bool ActiveFlag { get; set; }

        [JsonIgnore]
        public virtual RoleMaster? Role { get; set; } = null!;
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<OrderMaster> OrderMasters { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
