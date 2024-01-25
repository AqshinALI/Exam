using Exam.DAL;
using Exam.Models;
using Exam.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.services.ToListAsync();
            HomeVM vm= new HomeVM{
            Services= services,
            };
            return View(vm);
        }

    }
}