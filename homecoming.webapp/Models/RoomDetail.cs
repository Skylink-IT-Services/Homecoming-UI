using homecoming.webapp.ViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace homecoming.api.Model
{
    public class RoomDetail
    {
        [Key]
        public int RoomDetailId { get; set; }
        public int RoomId { get; set; }
        public virtual RoomTypeViewModel Room { get; set; }
        public string Type { get; set; }
        public int NumberOfBeds { get; set; }
        public string Description { get; set; }
        public Boolean Television { get; set; }
        public Boolean Wifi { get; set; }
        public Boolean Air_condition { get; set; }
        public Boolean Private_bathroom { get; set; }
    }
}
