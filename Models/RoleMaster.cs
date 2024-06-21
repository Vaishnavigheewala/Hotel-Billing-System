using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotelBillingMVC.Models
{
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            Usermasters = new HashSet<Usermaster>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool ActiveFlag { get; set; }


        
        public virtual ICollection<Usermaster>? Usermasters { get; set; }
    }
}
