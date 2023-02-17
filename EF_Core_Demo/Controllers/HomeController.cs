using EF_Core_Demo.Models;
using EF_Core_Demo.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EF_Core_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBonusRepository _bonusRepository;

        public HomeController(IBonusRepository bonusRepository)
        {
            _bonusRepository = bonusRepository;
        }

        public IActionResult Index()
        {
            var companies = _bonusRepository.GetAllCompaniesWithEmployees();
            return View(companies);
        }

        public IActionResult AddTestRecords()
        {
            Company company = new Company()
            {
                Name = "Test" + Guid.NewGuid().ToString(),
                Address = "test address",
                City = "test city",
                PostalCode = "test postalCode",
                State = "test state",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Email = "test Email",
                Name = "Test Name " + Guid.NewGuid().ToString(),
                Phone = " test phone",
                Title = "Test Manager"
            });

            company.Employees.Add(new Employee()
            {
                Email = "test Email 2",
                Name = "Test Name 2" + Guid.NewGuid().ToString(),
                Phone = " test phone 2",
                Title = "Test Manager 2"
            });

            _bonusRepository.AddCompanyWithEmployeesWithTransaction(company);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveTestRecords()
        {
            int[] companyIds = _bonusRepository.FIlterCompaniesByName("Test").Select(s=>s.CompanyId).ToArray();
            _bonusRepository.RemoveRange(companyIds);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}