using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskManager.Models;
using TaskManager.Models.Response;

namespace TaskManager.Controllers
{
    public class ExploreController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public ExploreController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /*public class ExploreCourseParams
        {
            public int numOfRecordsPerPage { get; set; }
            public int pageNo { get; set; }
            public string? batchStatus { get; set; }
        }*/


        // [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userID = user.UserName;
            var liveLearnServer = _configuration["LiveLearnServer"];
            ViewBag.liveLearnServer = liveLearnServer;


            var data = "numOfRecordsPerPage=100&pageNo=0&batchStatus=" + "40,10";

            /*var exploreParams = new ExploreCourseParams
            {
                numOfRecordsPerPage = 100,
                pageNo = 0,
                batchStatus = "40,10"
            };

            var json = JsonConvert.SerializeObject(exploreParams);*/
            var exdata = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            List<Courses> CoursesInfo = new List<Courses>();

            var url = liveLearnServer +"explore"; //"http://localhost:8080/evidya/explore";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, exdata);

            //Storing the response details recieved from web api
            var ExploreResponse = response.Content.ReadAsStringAsync().Result;
            //Deserializing the response recieved from web api and storing into the Course list
            CoursesInfo = JsonConvert.DeserializeObject<List<Courses>>(ExploreResponse); 
            foreach(var items in CoursesInfo)
            {
                items.BatchStartDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(items.BatchStartDate)).ToString("MMM dd, yyyy");
                items.BatchEndDate = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(items.BatchEndDate)).ToString("MMM dd, yyyy");
                if (items.CourseImage != null)
                {
                    items.CourseImageFullPath = "https://ample.omnex.com/resources/userspace/Courses/" + items.CourseID + "/Images/Framework/" + items.CourseImage;
                }
                else
                {
                    items.CourseImageFullPath = "https://ample.omnex.com/resources/product/images/NewIcons/svg/course.svg";
                }
            }
            return View(CoursesInfo);

        }
    }
}
