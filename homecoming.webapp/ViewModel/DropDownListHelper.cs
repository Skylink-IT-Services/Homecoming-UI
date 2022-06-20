using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace homecoming.webapp.ViewModel
{
    public class DropDownListHelper
    { 
        private static List<BedRoomType> defaultDedRoomTypes = new List<BedRoomType>
        {
                new BedRoomType {BedRoom = "Single" },
                new BedRoomType {BedRoom ="Double" },
                new BedRoomType {BedRoom =" Dexule"},
                new BedRoomType {BedRoom ="Triple" },
                new BedRoomType {BedRoom =" Twin"} ,
                new BedRoomType {BedRoom ="Queen" },
                new BedRoomType {BedRoom ="King"}
        };
        private static List<int> numberOfRooms = new List<int>() { 1,2,3,4,5};

        private async static Task<List<LocationViewModel>> GetLocations()
        {
            List<LocationViewModel> locations = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync("location");
                if (response.IsSuccessStatusCode)
                {
                    locations = await response.Content.ReadAsAsync<List<LocationViewModel>>();
                }
            }
            locations.Insert(0, new LocationViewModel { LocationId = 0, LocationName = "----Select Location----" });
            return locations;
        }

        public static List<BedRoomType> ListOfBedRoomTypes { get { return defaultDedRoomTypes; } }
        public static List<int> ListOfNumberOfRooms { get { return numberOfRooms; } }
        public static List<LocationViewModel> Geo { get { return  GetLocations().Result; } }

    }
}
