using MongoDB.Bson;
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

namespace cordovaBuild.Controllers
{
    public class SearchSpaceController : Controller
    {
        // GET: SearchSpace
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(Search search)
        {
            try
            {
                var searchRepo = new SearchRepository();
                var documentId = ObjectId.GenerateNewId();
                searchRepo.Create(new Search 
                {
                    Id = documentId,
                    Address = search.Address,
                    Username = User.Identity.Name,
                    Radius = search.Radius,
                    lat = search.lat,
                    lng = search.lng,
                    SearchDate = Helper.SetDateForMongo(DateTime.UtcNow)
                });

                return Json(new { success = true, responseText = "Added.", id = documentId.ToString(), isAuthenticated = User.Identity.IsAuthenticated }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}