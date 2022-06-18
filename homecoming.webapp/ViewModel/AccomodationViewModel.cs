using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace homecoming.webapp.ViewModel
{
    public class AccomodationViewModel
    {
        [Key]
        public int AccomodationId { get; set; }
        public int BusinessId { get; set; }
        public BusinessUserViewModel Business { get; set; }
        public string CoverPhoto { get; set; }
        public string AccomodationName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual List<AccomodationPhotos> AccomodationGallary { get; set; }
        public virtual List<RoomViewModel> AccomodationRooms { get; }
        [NotMapped]
        public IFormFile CoverImage { get; set; }
        [NotMapped]
        public IFormFileCollection ImageList { get; set; }
    }
}
