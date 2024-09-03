using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Healper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController
            (IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository , IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public IActionResult Index(string Name)
        {
            IEnumerable<EmployeeViewModel> MappedEmployees;
            IEnumerable<Employee>employees;
            if (string.IsNullOrEmpty(Name))
            {
                employees = _employeeRepository.GetAll();
            }else 
                employees = _employeeRepository.GetByName(Name);

            MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(MappedEmployees);
        }

      
        public IActionResult Add()
        {
            var Departments = _departmentRepository.GetAll();
            ViewBag.Departments = Departments;
            return View();
        }

        [HttpPost]
        public IActionResult Add(EmployeeViewModel employee)
        {
            if(ModelState.IsValid)
            {
                employee.ImageName = DocumentSettings.UplodeFile(employee.Image, "Images");
                Employee MappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(employee);
                int Res = _employeeRepository.Add(MappedEmployee);
                if(Res > 0)
                {
                    TempData["Massage"] = "new Employee Added";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Details(int? id , string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound();

            var MappedEmployee = _mapper.Map<Employee , EmployeeViewModel>(employee);

            return View(ViewName, MappedEmployee);
        }

        public IActionResult Edit(int id)
        {
            var Departments = _departmentRepository.GetAll();
            ViewBag.Departments = Departments;
            return Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employee , [FromRoute] int id)
        {
            if (employee.Id != id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employee);
                    _employeeRepository.Update(MappedEmployee);
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(employee);
        }

        public IActionResult Delete(int id) {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employee , [FromRoute] int id)
        {
            if(id != employee.Id) return BadRequest();
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employee);
                int effected = _employeeRepository.Delete(MappedEmployee);
                if(effected > 0 && employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "Images");
                }
                return RedirectToAction(nameof(Index));
            }catch(System.Exception e) {
                ModelState.AddModelError(string.Empty , e.Message);
            }
            return View(employee);
        }
    }
}
