using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace homecoming.webapp.ViewModel
{
    public class RoomTypeViewModel
    {
        [Key]
        public int RoomDetailId { get; set; }
        public int RoomId { get; set; }
        public virtual RoomViewModel Room { get; set; }
        public string Type { get; set; }
        public int NumberOfBeds { get; set; }
        public string Description { get; set; }
        public Boolean Television { get; set; }
        public Boolean Wifi { get; set; }
        public Boolean Air_condition { get; set; }
        public Boolean Private_bathroom { get; set; }
        public virtual BedRoomType BedRoomTypes { get; set; }
    }
}
