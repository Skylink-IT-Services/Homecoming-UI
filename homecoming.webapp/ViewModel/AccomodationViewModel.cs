using homecoming.webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homecoming.webapp.ViewModel
{
    public class AccomodationViewModel
    {
        public int AccomodationId { get; set; }
        public int BusinessId { get; set; }
        public BusinessViewModel Business { get; set; }
        public string CoverPhoto { get; set; }
        public string AccomodationName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public  List<AccomodationPhotos> AccomodationGallary { get; set; }
    }
}
