using cordovaBuild.Common;
using cordovaBuild.Data.Model;
using cordovaBuild.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace cordovaBuild.Controllers
{
    public class PostSpaceController : Controller
    {
        // GET: PostSpace
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        
        [Authorize]
        public async Task<ActionResult> List(string username)
        {
            var postRepository = new PostRepository();
            var spaces = await postRepository.GetSpacesByUser(User.Identity.Name);
            return View(spaces);
        }
        
        public async Task<ActionResult> HeatMap()
        {
            var searchRepository = new SearchRepository();
            var search = await searchRepository.GetAllSearches();


            return View(search);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Photo()
        {
            var photo = Request.Files["filePhoto"];
            var id = Request["Id"];
            var dir = Server.MapPath("~/Upload/" + User.Identity.Name);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            var filename = Guid.NewGuid() + Path.GetExtension(photo.FileName);
            var fullPath = Path.Combine(dir, filename);
            photo.SaveAs(fullPath);

            var postRepo = new PostRepository();
            await postRepo.UpdatePhoto(id, fullPath);

            return Json(new { success = true, responseText = "Added." }, JsonRequestBehavior.AllowGet);
        }        

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post(Space space)
        {
            try
            {
                var postRepo = new PostRepository();

                switch (space.AvailabilityType)
                {
                    case "Specific Date" :
                        if (space.Availability.SpecificDate == DateTime.MinValue)
                        {
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            return Json(new { success = false, responseText = "Invalid specific date." }, JsonRequestBehavior.AllowGet);
                        }
                        break;
                    case "Date Range":
                        if (space.Availability.DateFrom == DateTime.MinValue || space.Availability.DateTo == DateTime.MinValue)
                        {
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            return Json(new { success = false, responseText = "Invalid date range." }, JsonRequestBehavior.AllowGet);
                        }
                        break;
                }
                space.Availability.DateFrom = DateTime.SpecifyKind(space.Availability.DateFrom, DateTimeKind.Utc);
                space.Availability.DateTo = DateTime.SpecifyKind(space.Availability.DateTo, DateTimeKind.Utc);
                space.Availability.SpecificDate = DateTime.SpecifyKind(space.Availability.SpecificDate, DateTimeKind.Utc);

                var data = new Data.Model.Space
                {
                    User = User.Identity.Name,
                    Address = space.Address,
                    latitude = space.latitude,
                    longitude = space.longitude,
                    VehicleType = space.VehicleType,
                    NumberOfSlot = space.NumberOfSlot,
                    AvailabilityType = space.AvailabilityType,
                    Availability = space.Availability,
                    Price = space.Price,
                    Instructions = space.Instructions,
                    DatePosted = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };

                await postRepo.CreateSync(data);

                

                var accountStatement = new AccountStatementRepository();
                var amount = await accountStatement.GetCurrentBalance(User.Identity.Name) + 10;
                var promo = new AccountStatement
                {
                    User = User.Identity.Name,
                    Credit = 10,
                    CurrentBalance = amount,
                    TransactionDate = Helper.SetDateForMongo(DateTime.Now),
                    Description = "Early adopter $10 promo"
                };

                accountStatement.Create(promo);


                return Json(new { success = true, responseText = "Added.", id = data.Id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}