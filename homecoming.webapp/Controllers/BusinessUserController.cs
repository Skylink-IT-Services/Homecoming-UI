using homecoming.api.Model;
using homecoming.webapp.Data;
using homecoming.webapp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<AccomodationViewModel> accomodationList = null;
            using (var client = new HttpClient())
            {
                string id = userManager.GetUserId(HttpContext.User);
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync($"accomodation/GetByAspId/{id}");
                var result = response;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<AccomodationViewModel>>();
                    read.Wait();
                    accomodationList = read.Result;
                }
                else
                {
                    accomodationList = Enumerable.Empty<AccomodationViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(accomodationList);
        }

       
        public IActionResult AddAccomodation()
        {
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
                        AccomodationName = accomodation.AccomodationName,
                        Description = accomodation.Description,
                        Rating = 0,
                        Location = accomodation.Location
                    };
                    //stringify object data to maltipart/form-data
                    formdata.Headers.ContentType.MediaType = "multipart/form-data";
                    formdata.Add(new StringContent(businessObj.BusinessId.ToString()), "BusinessId");
                    formdata.Add(new StringContent(businessObj.AccomodationName), "AccomodationName");
                    formdata.Add(new StringContent(businessObj.Description), "Description");
                    formdata.Add(new StringContent(businessObj.Rating.ToString()), "Rating");
                    formdata.Add(new StringContent(businessObj.Location), "Location");



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
            room.Description = "None";
            room.Price = 0.00M;
            room.RoomDetails = new List<RoomTypeViewModel>();
            room.RoomDetails.Add(new RoomTypeViewModel { RoomDetailId=1});
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
                    roomObj.RoomDetails = new List<RoomTypeViewModel>();
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
                        foreach(var item in roomObj.RoomDetails)
                        {
                            RoomTypeViewModel type = new RoomTypeViewModel()
                            {
                                RoomId = int.Parse(insertedRoomId),
                                Type = item.Type,
                                Description = item.Description,
                                NumberOfBeds = item.NumberOfBeds,
                                Television = item.Television,
                                Air_condition = item.Air_condition,
                                Wifi = item.Wifi,
                                Private_bathroom = item.Private_bathroom
                            };
                            db.RoomDetails.Add(type);
                            db.SaveChanges();
                        }
                        return RedirectToAction("AccomodationList", "BusinessUser");
                    }
                }
            }
            return BadRequest();
        }
        public async Task<IActionResult> EditRoom() 
        {
            return View();
        }
        public async Task<IActionResult> ViewRoom(int id) 
        {
            RoomViewModel  accomRoomIfo= null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Config.BaseUrl);
                var response = await client.GetAsync($"room/getroom/{id}");
                if (response.IsSuccessStatusCode)
                {                  
                    accomRoomIfo = await response.Content.ReadAsAsync<RoomViewModel>();
                }
                else
                {
                    accomRoomIfo = null;
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            return View(accomRoomIfo);
        }
        public async Task<IActionResult> DeleteRoom() { return View(); }
    }
}
