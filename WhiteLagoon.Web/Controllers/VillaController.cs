using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                if(obj.Image != null)
                {

                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index), "Villa");
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == villaId);

            // Filtering result.
            //var objList = _db.Villas.Where(v => v.Price > 40 && v.Occupancy > 2);
            //var obj2 = _db.Villas.Find(villaId);
            //Villa? obj3 = _db.Villas.FirstOrDefault(v => v.Price > 40 && v.Occupancy > 2);

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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction(nameof(Index), "Villa");
            }
            return View();
        }


        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(v => v.Id == obj.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();

                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index), "Villa");
            }
            TempData["error"] = "The villa could not be deleted.";
            return View();
        }
    }
}
