using homecoming.webapp.ViewModel;
using System;
using System.Collections.Generic;

namespace homecoming.webapp.Models
{
    public class BusinessViewModel
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string CoverPhotoUrl { get; set; }
        public string Tel_No { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Province { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
       // public virtual List<AccomodationViewModel> GetAccomodations { get; set; }
}
}
