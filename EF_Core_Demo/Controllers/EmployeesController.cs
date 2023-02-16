using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_Core_Demo.Data;
using EF_Core_Demo.Models;
using EF_Core_Demo.Repository;

namespace EF_Core_Demo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBonusRepository _bonusRepository;

        public EmployeesController(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IBonusRepository bonusRepository)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _bonusRepository = bonusRepository;
        }
        public async Task<IActionResult> Index(int companyId = 0)
        {
            var employees = _bonusRepository.GetEmployeesWithCompany(companyId);

            return View(employees);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _companyRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _employeeRepository.Find(id.Value);

            IEnumerable<SelectListItem> companyList = _companyRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _employeeRepository.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            var employee = _employeeRepository.Find(id);

            if (employee is not null)
                return true;

            return false;
        }
    }
}
