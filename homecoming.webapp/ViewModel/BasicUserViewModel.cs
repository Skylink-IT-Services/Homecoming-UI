using System;
using System.ComponentModel.DataAnnotations;

namespace homecoming.webapp.ViewModel
{
    public class BasicUserViewModel
    {
        [Key]
        public int BasicUserId { get; set; }
        public string AspUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell_No { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
