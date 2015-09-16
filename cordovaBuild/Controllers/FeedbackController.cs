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
    public class FeedbackController : Controller
    {
        // GET: Feedback
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Feedback feedback)
        {
            try
            {
                var feedbackRepo = new FeedbackRepository();

                feedbackRepo.Create(new Feedback
                {
                    Message = feedback.Message,
                    Email = feedback.Email,
                    DatePosted = DateTime.UtcNow
                });

                return Json(new { success = true, responseText = "Added." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}