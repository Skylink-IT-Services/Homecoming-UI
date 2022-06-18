using System.ComponentModel.DataAnnotations;

namespace homecoming.webapp.ViewModel
{
    public class RoomImagesViewModel
    {
        [Key]
        public int RoomImageId { get; set; }
        public int RoomId { get; set; }
        public RoomViewModel Room { get; set; }
        public string ImageUrl { get; set; }
    }
}
