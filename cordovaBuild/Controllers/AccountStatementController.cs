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
    public class AccountStatementController : Controller
    {
        // GET: AccountStatement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> TenDollarPromo()
        {
            try
            {
                var accountStatement = new AccountStatementRepository();

                var promo = new AccountStatement 
                { 
                    User = User.Identity.Name,
                    Credit = 10,
                    CurrentBalance = 10,
                    TransactionDate = Helper.SetDateForMongo(DateTime.Now),
                    Description = "Early adopter $10 promo"                    
                };

                accountStatement.Create(promo);
                return Json(new { success = true, responseText = "Added." }, JsonRequestBehavior.AllowGet);
                
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}