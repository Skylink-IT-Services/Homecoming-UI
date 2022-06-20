using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace homecoming.webapp.ViewModel
{
    public class RoomViewModel
    {
        [Key]
        public int RoomId { get; set; }
        public int AccomodationId { get; set; }
        public AccomodationViewModel Accomodation { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsBooked { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual List<RoomImagesViewModel> RoomGallary { get; set; }
        public virtual RoomTypeViewModel RoomDetails { get; set; }
        [Display(Name ="Choose Room Images")]
        [NotMapped]
        public IFormFileCollection ImageList { get; set; }
    }
}
