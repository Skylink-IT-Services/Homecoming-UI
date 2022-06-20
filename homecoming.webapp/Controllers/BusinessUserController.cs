using homecoming.api.Model;
using homecoming.webapp.Data;
using homecoming.webapp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace homecoming.webapp.Controllers
{
    public class BusinessUserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private ApplicationDbContext db;
        public BusinessUserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        [Authorize(Roles ="BusinessUser")]
        public IActionResult ManageBusiness()
        {
         return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageBusiness(BusinessUserViewModel business)
        {
            if(business != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Config.BaseUrl);
                    var formdata = new MultipartFormDataContent();
                    var businessObj = new BusinessUserViewModel()
                    {
                        BusinessName = business.BusinessName,
                        Email = userManager.GetUserName(HttpContext.User),
                        Tel_No = business.Tel_No,
                        AddressLine1 = business.AddressLine1,
                        City = business.City,
                        Zipcode = business.Zipcode,
                        Province = business.Province,
                        AspUser = userManager.GetUserId(HttpContext.User)
                    };
                    //data
                    formdata.Headers.ContentType.MediaType = "multipart/form-data";
                    formdata.Add(new StringContent(businessObj.BusinessName),"BusinessName");
                    formdata.Add(new StringContent(businessObj.Tel_No),"Tel_No");
                    formdata.Add(new StringContent(businessObj.AddressLine1), "AddressLine1");
                    formdata.Add(new StringContent(businessObj.City), "City");
                    formdata.Add(new StringContent(businessObj.Zipcode),"Zipcode");
                    formdata.Add(new StringContent(businessObj.Province) ,"Province");


                    //file
                    if (business.ImageFile !=null) {
                        var fileStreamContent = new StreamContent(business.ImageFile.OpenReadStream());
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                        formdata.Add(fileStreamContent, name: business.ImageFile.Name, fileName: business.ImageFile.FileName);
                    }
                    var response = await client.PostAsync("business", formdata);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = response.Content.ReadAsStringAsync().Result;
                        return RedirectToAction("AccomodationList","BusinessUser");
                    }
                }
            }
            return View();
           
        }

        [Authorize(Roles = "BusinessUser")]
        public async Task<IActionResult> AccomodationList()
        {
            BusinessUserViewModel accomodationList = null;
            using (var client = new HttpClient())
            {
                string id = userManager.GetUserId(HttpContext.User);
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync($"business/GetBusinessByAspUserId/{id}");
                var result = response;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<BusinessUserViewModel>();
                    read.Wait();
                    accomodationList = read.Result;
                }
                else
                {
                    accomodationList = null;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(accomodationList);
        }

       
        public IActionResult AddAccomodation()
        { 
            ViewBag.location = DropDownListHelper.Geo;
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> AddAccomodation(AccomodationViewModel accomodation)
        {
            if (accomodation != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Config.BaseUrl);


                    //get businness id of logged in user using identity
                    int businessIdentifier = 0;
                    var responseFromBusinssEntity = await client.GetAsync($"business/getid/{userManager.GetUserId(HttpContext.User)}");
                    if (responseFromBusinssEntity.IsSuccessStatusCode)
                    {
                        var responseData = responseFromBusinssEntity.Content.ReadAsStringAsync().Result;
                        businessIdentifier = Convert.ToInt32(responseData);
                    }


                    var formdata = new MultipartFormDataContent();
                    var businessObj = new AccomodationViewModel()
                    {
                        BusinessId = businessIdentifier,
                        LocationId = accomodation.GeoLocation.LocationId,
                        AccomodationName = accomodation.AccomodationName,
                        Description = accomodation.Description,
                        Rating = 0,
                        GeoLocation = accomodation.GeoLocation
                    };
                    //stringify object data to maltipart/form-data
                    formdata.Headers.ContentType.MediaType = "multipart/form-data";
                    formdata.Add(new StringContent(businessObj.BusinessId.ToString()), "BusinessId");
                    formdata.Add(new StringContent(businessObj.LocationId.ToString()), "LocationId");
                    formdata.Add(new StringContent(businessObj.AccomodationName), "AccomodationName");
                    formdata.Add(new StringContent(businessObj.Description), "Description");
                    formdata.Add(new StringContent(businessObj.Rating.ToString()), "Rating");



                    //upload file to zaure blob storage
                    if (accomodation.CoverImage != null && accomodation.ImageList != null)
                    {
                        var fileStreamContent = new StreamContent(accomodation.CoverImage.OpenReadStream());
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                        formdata.Add(fileStreamContent, name: accomodation.CoverImage.Name, fileName: accomodation.CoverImage.FileName);

                        foreach(var img in accomodation.ImageList)
                        {
                            fileStreamContent = new StreamContent(img.OpenReadStream());
                            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                            formdata.Add(fileStreamContent, name: img.Name, fileName: img.FileName);
                        }
                    }

                    // post the data to web api httppost
                    var response = await client.PostAsync("accomodation", formdata);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = response.Content.ReadAsStringAsync().Result;
                        return RedirectToAction("AccomodationList", "BusinessUser");
                    }
                }
            }
            return BadRequest();
        }

        public IActionResult AddRoom()
        {
            RoomViewModel room = new RoomViewModel();
            DropDownListHelper.ListOfBedRoomTypes.Insert(0, new BedRoomType { Id = 0, BedRoom = "----Select BedRoom----" });
            room.Description = "";
            room.Price = 0.00M;
            room.RoomDetails = new RoomTypeViewModel();
            room.RoomDetails = new RoomTypeViewModel { RoomDetailId = 1 };
            room.RoomDetails.BedRoomTypes = new BedRoomType();
            ViewBag.bedrooms = DropDownListHelper.ListOfBedRoomTypes;
            DropDownListHelper.ListOfNumberOfRooms.Insert(0, 1);
            ViewBag.numberOfRooms = DropDownListHelper.ListOfNumberOfRooms;
            return View(room);
        }
        [HttpPost]
        public async Task<IActionResult> AddRoom(RoomViewModel roomInfo, int id)
        {
            if (roomInfo != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Config.BaseUrl);
                    var payload = new MultipartFormDataContent();
                    var roomObj = new RoomViewModel()
                    {
                        AccomodationId = id,
                        Description = roomInfo.Description,
                        Price = roomInfo.Price,
                    };
                    roomObj.RoomDetails = new RoomTypeViewModel();
                    roomObj.RoomDetails = roomInfo.RoomDetails;

                    //stringify object data to maltipart/form-data
                    payload.Headers.ContentType.MediaType = "multipart/form-data";
                    payload.Add(new StringContent(roomObj.AccomodationId.ToString()), "AccomodationId");
                    payload.Add(new StringContent(roomObj.Description), "Description");
                    payload.Add(new StringContent(roomObj.Price.ToString()), "Price");


                    //upload file to zaure blob storage
                    if (roomInfo.ImageList != null)
                    {
                        StreamContent fileStreamContent;
                        foreach (var img in roomInfo.ImageList)
                        {
                            fileStreamContent = new StreamContent(img.OpenReadStream());
                            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                            payload.Add(fileStreamContent, name: img.Name, fileName: img.FileName);
                        }
                    }

                    // post the data to web api httppost
                    var response = await client.PostAsync("room", payload);
                    if (response.IsSuccessStatusCode)
                    {
                        var insertedRoomId = response.Content.ReadAsStringAsync().Result;
                       
                            RoomTypeViewModel type = new RoomTypeViewModel()
                            {
                                RoomId = int.Parse(insertedRoomId),
                                Type = roomObj.RoomDetails.BedRoomTypes.BedRoom,
                                Description = roomObj.RoomDetails.Description,
                                NumberOfBeds = roomObj.RoomDetails.NumberOfBeds,
                                Television = roomObj.RoomDetails.Television,
                                Air_condition = roomObj.RoomDetails.Air_condition,
                                Wifi = roomObj.RoomDetails.Wifi,
                                Private_bathroom = roomObj.RoomDetails.Private_bathroom
                            };
                            db.RoomDetails.Add(type);
                            db.SaveChanges();
                        return RedirectToAction("AccomodationList", "BusinessUser");
                    }
                }
            }
            return BadRequest();
        }

        public async Task<IActionResult> EditRoom(int id) 
        {
            RoomViewModel room = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);

                var response = await client.GetAsync($"room/{id}");
                if (response.IsSuccessStatusCode)
                {
                    room = await response.Content.ReadAsAsync<RoomViewModel>();
                   
                }
            }
            return View(room);
        }
        public async Task<IActionResult> EditAccomodation()
        {
            return View();
        }
        public async Task<IActionResult> ViewRoomDetail()
        {
            return View();
        }
        public async Task<IActionResult> ViewRoom(int id) 
        {
            AccomodationViewModel AccomodationDetails= null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync($"accomodation/{id}");
                if (response.IsSuccessStatusCode)
                {                  
                    AccomodationDetails = await response.Content.ReadAsAsync<AccomodationViewModel>();
                }
                else
                {
                    AccomodationDetails = null;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(AccomodationDetails);
        }
        public async Task<IActionResult> DeleteRoom() { return View(); }

        public async Task<IActionResult> UpdateBusinessProfile()
        {
            BusinessUserViewModel BusinessList = null;
            using (var client = new HttpClient())
            {
                string i = userManager.GetUserId(HttpContext.User);
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync($"business/GetBusinessByAspUserId/{i}");
                var result = response;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<BusinessUserViewModel>();
                    read.Wait();
                    BusinessList = read.Result;
                }
                else
                {
                    BusinessList = null;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(BusinessList);
        }

        [HttpPost]
        public  async Task<IActionResult> UpdateBusinessProfile(int id,BusinessUserViewModel model)
        {
            return View();
        }
    }
}
