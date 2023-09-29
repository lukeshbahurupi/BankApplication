﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankApplication.Models;

namespace BankApplication.Controllers
{
    public class ApplicationFormController : Controller
    {
        private BankApplication_dbContext db = new BankApplication_dbContext();

        // GET: ApplicationForm
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.ApplicationForms.ToList());
        }
        public ActionResult ListView()
        {
            return View(db.ApplicationForms.ToList());
        }
        // GET: ApplicationForm/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationForm applicationForm = db.ApplicationForms.Find(id);
            if (applicationForm == null)
            {
                return HttpNotFound();
            }
            return View(applicationForm);
        }

        // GET: ApplicationForm/Create
        [Authorize(Roles ="User")]
        public ActionResult Create()
        {
            User model = db.Users.FirstOrDefault(el => el.UserName == User.Identity.Name);
            if(model != null)
            {
                ViewBag.data = model;
                return View();
            }
             return RedirectToAction("Login","Accounts"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Create([Bind(Include = "Gender,BirthDate,Age,AadhaarCardNumber,PermanentAddress,CurrentAddress,LoanType,RequiredLoanAmount,RateOfInterest,LoanTenureInMonth,LoanEMI,Summary")] ApplicationForm applicationForm)
        {
            User data = db.Users.FirstOrDefault(el => el.UserName == User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    List<FileDetail> fileDetails = new List<FileDetail>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            FileDetail detail = new FileDetail()
                            {
                                FileName = fileName,
                                Extension = Path.GetExtension(fileName),
                                Id = Guid.NewGuid()
                            };
                            fileDetails.Add(detail);
                            var path = Path.Combine(Server.MapPath("~/App_Data/Upload"), detail.Id + detail.Extension);
                            file.SaveAs(path);
                        }
                        else
                        {
                            TempData["Error"] = "Please Upload Documents!";
                            if (data != null)
                            {
                                ViewBag.data = data;
                                return View();
                            }
                            return RedirectToAction("Login", "Accounts");
                        }
                    }
                    applicationForm.ApplicationDate = DateTime.Today;
                    applicationForm.CustemerName = data.UserName;
                    applicationForm.EmailId = data.Email;
                    applicationForm.PancardNumber = data.UniqueId;
                    applicationForm.PhoneNumber = data.MobileNumber;
                    applicationForm.FileDetails = fileDetails;
                    db.ApplicationForms.Add(applicationForm);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //User model = db.Users.FirstOrDefault(el => el.UserName == User.Identity.Name);
            if (data != null)
            {
                ViewBag.data = data;
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: ApplicationForm/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            //User model = db.Users.FirstOrDefault(el => el.UserName == User.Identity.Name);
            //if (model != null)
            //{
            //    ViewBag.data = model;               
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationForm applicationForm = db.ApplicationForms.Find(id);
            if (applicationForm == null)
            {
                return HttpNotFound();
            }
            return View(applicationForm);
        }

        // POST: ApplicationForm/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ApplicationID,LoanStatus")] ApplicationForm applicationForm)
        {
            
            //if (ModelState.IsValid)
            //{
                    ApplicationForm Form = db.ApplicationForms.Find(applicationForm.ApplicationID);
                try
                {
                    Form.LoanStatus = applicationForm.LoanStatus;
                    db.Entry(Form).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            //}
           return View(Form);
        }

        // GET: ApplicationForm/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationForm applicationForm = db.ApplicationForms.Find(id);
            if (applicationForm == null)
            {
                return HttpNotFound();
            }
            return View(applicationForm);
        }

        // POST: ApplicationForm/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationForm applicationForm = db.ApplicationForms.Find(id);
            db.ApplicationForms.Remove(applicationForm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/Upload/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                FileDetail fileDetail = db.FileDetails.Find(guid);
                if (fileDetail == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                db.FileDetails.Remove(fileDetail);
                db.SaveChanges();

                //Delete file from the file system
                var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), fileDetail.Id + fileDetail.Extension);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
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
