using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj) 
        {
            if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The desc cannot match wtih Name.");
            }
            if(ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index", "Villa");
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == villaId);

            // Filtering result.
            //var objList = _db.Villas.Where(v => v.Price > 40 && v.Occupancy > 2);
            //var obj2 = _db.Villas.Find(villaId);
            //Villa? obj3 = _db.Villas.FirstOrDefault(v => v.Price > 40 && v.Occupancy > 2);

            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();

                return RedirectToAction("Index", "Villa");
            }
            return View();
        }


        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Remove(obj);
                _db.SaveChanges();

                return RedirectToAction("Index", "Villa");
            }
            return View();
        }
    }
}
