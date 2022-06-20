using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homecoming.webapp.ViewModel
{
    public class LocationViewModel
    {
        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

}
