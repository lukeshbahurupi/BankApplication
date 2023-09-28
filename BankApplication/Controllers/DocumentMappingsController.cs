using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using BankApplication.Models;

namespace BankApplication.Controllers
{
    public class DocumentMappingsController : Controller
    {
        private BankApplication_dbContext db = new BankApplication_dbContext();

        // GET: DocumentMappings
        public ActionResult Index(string id)
        {
            var documentMappings = db.DocumentMappings.Include(d => d.DocumentMaster).Include(d => d.LoanType);
            return View(documentMappings.Where(el => el.LoanCode == id).ToList());
        }
        public ActionResult Documents()
        {
            return View(db.DocumentMasters.ToList());
        }
        public ActionResult LoanType()
        {
            return View(db.LoanTypes.ToList());
        }
       public ActionResult Static()
        {
            
            return View(db.LoanTypes.ToList());
        }
        public ActionResult PartialTable(string id)
        {
            var documentMappings = db.DocumentMappings.Include(d => d.DocumentMaster).Include(d => d.LoanType);
            return PartialView("_Static", documentMappings.Where(el => el.LoanCode == id).ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
