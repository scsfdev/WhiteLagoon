using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value= v.Id.ToString()
            });
            ViewData["VillaList"] = list;
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber obj) 
        {
            // One way to remove model validation.
            //ModelState.Remove("Villa");
            // The other way is to change from Model (Entities) >> by adding [ValidateNever] tag but need to adjust Project File with ItemGroup --> <FrameworkReference Include="Microsoft.AspNetCore.App" />

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == villaId);

            if(obj is null)
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

                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }
            return View();
        }


        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(v => v.Id == obj.Id);
            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
