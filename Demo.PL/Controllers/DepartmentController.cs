using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        public IDepartmentRepository _departmentRepository { get; }

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        public IActionResult Create()
       {
            return View();
       }

        [HttpPost]
        public IActionResult Create(Department newDepartment)
        {
            if (ModelState.IsValid)
            {
               int Res = _departmentRepository.Add(newDepartment);
                if(Res > 0)
                {
                    TempData["Massage"] = "new Department is Created";
                }
                return RedirectToAction("Index");
            }
            return View(newDepartment);
        }

        public IActionResult Details(int ?id , string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var department = _departmentRepository.GetById(id.Value);
            if (department is null) return NotFound();
            return View(viewName, department);
        }

        public IActionResult Edit(int ?id)
        {
            return Details(id , "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department _Department , [FromRoute]int id)
        {
            if (_Department.Id != id) return BadRequest();
            if (ModelState.IsValid)
            {
                try {
                    _departmentRepository.Update(_Department);
                    return RedirectToAction("Index");
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty , ex.Message);
                }

      
            }
            return View(_Department);   
        }

        public IActionResult Delete([FromRoute]int ? id)
        {
            if(id is null) return BadRequest();
            try
            {
                _departmentRepository.Delete(_departmentRepository.GetById(id.Value));
            }catch(System.Exception ex)
            {
                ModelState.AddModelError (string.Empty , ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
