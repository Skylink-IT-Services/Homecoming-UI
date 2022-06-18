using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homecoming.webapp.ViewModel
{
    
    public class AccomodationPhotos
    {
        [Key]
        public int AccomodationPhotoId { get; set; }
        public int AccomodationId { get; set; }
        public string ImageUrl { get; set; }
    }
}
