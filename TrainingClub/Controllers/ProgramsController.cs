using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainingClub.Models;
using TrainingClub.ModelsDB;

namespace TrainingClub.Controllers
{
    public class ProgramsController : Controller
    {
        public RaceDBEntities ProgramDB;
        public ProgramsController()
        {
            ProgramDB = new RaceDBEntities();
        }
        // GET: Programs
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.roleName))
                return View("Index");
            
            return View("Readonly");
            
        }
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New([Bind(Exclude = "id")] ProgramViewModel Newprogram)
        {
            if (ModelState.IsValid)
            {
                Program program = new Program();
                program.Id = Newprogram.Id;
                program.ProgramName = Newprogram.ProgramName;
                program.ProgramFee = Newprogram.ProgramFee;
                program.NumberOfStudents = Newprogram.NumberOfStudents;
                program.Description = Newprogram.Descirption;
                using (ProgramDB)
                {
                    try
                    {
                        // TODO: Add insert logic here
                        ProgramDB.Programs.Add(program);

                        ProgramDB.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }

                }

            }
            return RedirectToAction("New");
        }
        //GET

        public ActionResult Details(int id)
        {
            
            var program = ProgramDB.Programs.SingleOrDefault(c => c.Id == id);
            if (program == null)
                return HttpNotFound();

            ProgramViewModel curProgram = new ProgramViewModel();
            curProgram.Id = program.Id;
            curProgram.ProgramName = program.ProgramName;
            curProgram.ProgramFee = program.ProgramFee;
            curProgram.NumberOfStudents = program.NumberOfStudents;
            curProgram.Descirption = program.Description;
            return View(curProgram);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
         
               using (ProgramDB)
                {
                ProgramViewModel curProgram = new ProgramViewModel();
                Program program = ProgramDB.Programs.Where(x => x.Id == id).FirstOrDefault<Program>();
                curProgram.ProgramName = program.ProgramName;
                curProgram.ProgramFee = program.ProgramFee;
                curProgram.Descirption = program.Description;
                curProgram.NumberOfStudents = program.NumberOfStudents;
                curProgram.Id = program.Id;


                    return View(curProgram);
                }
            
        }

        [HttpPost]
        public ActionResult Edit(ProgramViewModel program)
        {
            if (ModelState.IsValid)
            {
                using (ProgramDB)
                {
                    Program curProgram = new Program();
                    curProgram.ProgramName = program.ProgramName;
                    curProgram.ProgramFee = program.ProgramFee;
                    curProgram.NumberOfStudents = program.NumberOfStudents;
                    curProgram.Id = program.Id;
                    curProgram.Description = program.Descirption;

                    ProgramDB.Entry(curProgram).State = EntityState.Modified;
                    ProgramDB.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("New");

        }

        [HttpPost]
       
        public ActionResult Delete(int id)
        {

            using (ProgramDB)
            {
                Program program = ProgramDB.Programs.Where(x => x.Id == id).FirstOrDefault<Program>();
               ProgramDB.Programs.Remove(program);
                ProgramDB.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


        protected override void Dispose(bool disposing) 
        {
            ProgramDB.Dispose();
        }
        public ActionResult GetData()
        {
            using (ProgramDB)
            {
                List<Program> programList = ProgramDB.Programs.ToList();
                return Json(new { data = programList }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}