using IdentityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingClub.Models;
using TrainingClub.ModelsDB;

namespace TrainingClub.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private RaceDBEntities DB;
        public BookingsController()
        {
           
            DB = new RaceDBEntities();
        }
        // GET: Bookings
      
       
        public ActionResult New()
        {
            BookingViewModel curBooking = new BookingViewModel();
         
                curBooking.SelectCategory = (from objCat in DB.Categories
                                             select new SelectListItem()
                                             {
                                                 Text = objCat.CategoryName,
                                                 Value = objCat.CategoryName,
                                                 Selected = true
                                             });
                curBooking.SelectProgram = (from objCat in DB.Programs
                                            select new SelectListItem()
                                            {
                                                Text = objCat.ProgramName,
                                                Value = objCat.Id.ToString(),
                                                Selected = true
                                            });

                return View(curBooking);
           
        }
        [HttpPost]
        public ActionResult New(BookingViewModel newBooking)
        {
            if (ModelState.IsValid)
            {
              
                    //doesnot exceed the number of poeple it can accomodate
                    Program temp = DB.Programs.Where(x => x.Id == newBooking.ProgramID).FirstOrDefault<Program>();
                    if (temp.NumberOfStudents > newBooking.NumberOfPeople)
                    {
                        temp.NumberOfStudents = temp.NumberOfStudents - newBooking.NumberOfPeople;
                        DB.Entry(temp).State = EntityState.Modified;

                    }
                    else
                    {
                        return HttpNotFound("No Avaliable Space");
                    }
                    try
                    {
                        Booking curBooking = new Booking();
                        curBooking.Id = newBooking.Id;
                        curBooking.Name = newBooking.Name;
                        curBooking.NumberOfStudents = newBooking.NumberOfPeople;
                        curBooking.Email = newBooking.Email;
                        curBooking.Class = temp.ProgramName; ;
                        curBooking.Category = newBooking.category;
                        curBooking.Amount = temp.ProgramFee;
                        curBooking.TotalAmount = temp.ProgramFee * newBooking.NumberOfPeople;
                        curBooking.UserId = User.Identity.Name;
                    curBooking.ProgramID = newBooking.ProgramID;
                        // TODO: Add insert logic here
                        DB.Bookings.Add(curBooking);
                        DB.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
            }
            return RedirectToAction("New");
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.roleName))
            {
                return View("Index");
            }
            return View("Index-User");
        }
        [Authorize(Roles = RoleName.roleName)]
        public ActionResult Delete(int id)
        {

            using (DB)
            {
                Booking curBooking = DB.Bookings.Where(x => x.Id == id).FirstOrDefault<Booking>();
                BookingViewModel program = new BookingViewModel();

                Program temp = DB.Programs.Where(x => x.ProgramName == curBooking.Class).FirstOrDefault<Program>();

                temp.NumberOfStudents = temp.NumberOfStudents + curBooking.NumberOfStudents;
                DB.Entry(temp).State = EntityState.Modified;
               
                DB.Bookings.Remove(curBooking);
                DB.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
    
                BookingViewModel curBooking = new BookingViewModel();

                Booking booking = DB.Bookings.Where(x => x.Id == id).FirstOrDefault<Booking>();
                curBooking.Id = booking.Id;
                curBooking.Name = booking.Name;
                curBooking.Email = booking.Email;
                curBooking.TotalAmount = booking.TotalAmount;
                curBooking.NumberOfPeople = booking.NumberOfStudents;
                curBooking.Amount = booking.Amount;
                curBooking.userId = booking.UserId;
                 Session["Username"] = booking.UserId;
            curBooking.ProgramID = booking.ProgramID;
            curBooking.SelectCategory = (from objCat in DB.Categories
                                             select new SelectListItem()
                                             {
                                                 Text = objCat.CategoryName,
                                                 Value = objCat.CategoryName,
                                                 
                                                 Selected = true
                                             });
                curBooking.SelectProgram = (from objCat in DB.Programs
                                            select new SelectListItem()
                                            {
                                                Text = objCat.ProgramName,
                                                Value = objCat.Id.ToString(),
                                                Selected = true
                                            });
           
            Program temp = DB.Programs.Where(x => x.Id == curBooking.ProgramID).FirstOrDefault<Program>();
            if(temp!=null)
                curBooking.Amount = temp.ProgramFee;
          

                return View(curBooking);
             
            
        }
        [HttpPost]
        public ActionResult Edit(BookingViewModel newBooking)
        {
            if (ModelState.IsValid)
            {
              
                    Program temp = DB.Programs.Where(x => x.Id == newBooking.ProgramID).FirstOrDefault<Program>();

                if (newBooking == null)
                    return RedirectToAction("Index");

                Booking curBooking = new Booking();
                    curBooking.Id = newBooking.Id;
                    curBooking.Name = newBooking.Name;
                    curBooking.NumberOfStudents = newBooking.NumberOfPeople;
                    curBooking.Email = newBooking.Email;
                    curBooking.Class = temp.ProgramName; ;
                    curBooking.Category = newBooking.category;
                    curBooking.Amount = temp.ProgramFee;
                    curBooking.TotalAmount = temp.ProgramFee * newBooking.NumberOfPeople;
                if (!User.IsInRole(RoleName.roleName))
                    curBooking.UserId = User.Identity.Name;
                else
                    curBooking.UserId = Session["Username"].ToString();



                    DB.Entry(curBooking).State = EntityState.Modified;
                    DB.SaveChanges();
                    return RedirectToAction("Index");               
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
           DB.Dispose();
        }
        public ActionResult GetData()
        {
            using (DB)
            {
                List<Booking> bookingList = new List<Booking>();
                if (User.IsInRole(RoleName.roleName))
                {
                    bookingList = DB.Bookings.ToList();
                }
                else
                {
                    foreach (Booking cur in DB.Bookings)
                    {
                        if (cur.UserId == User.Identity.Name)
                        {
                            bookingList.Add(cur);
                        }
                    } 
                }

                return Json(new { data = bookingList }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
