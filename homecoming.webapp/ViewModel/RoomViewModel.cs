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
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual List<RoomImagesViewModel> RoomGallary { get; set; }
        public virtual List<RoomTypeViewModel> RoomDetails { get; set; }
        [NotMapped]
        public IFormFileCollection ImageList { get; set; }
    }
}
