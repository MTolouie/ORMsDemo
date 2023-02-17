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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}