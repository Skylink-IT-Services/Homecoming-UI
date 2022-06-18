using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace homecoming.webapp.ViewModel
{
    public class BusinessUserViewModel
    {
        [Key]
        public int BusinessId { get; set; }
        public string AspUser { get; set; }
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

        [Display(Name = "Choose CoverPhoto")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual List<AccomodationViewModel> GetAccomodations { get; set; }
    }
}
