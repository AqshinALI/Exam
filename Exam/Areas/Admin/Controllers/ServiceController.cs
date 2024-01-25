using Exam.Areas.Admin.ViewModels.Service;
using Exam.DAL;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.services.ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Service service = new Service
            {
                Name = vm.Name,
                Description = vm.Description,
                Icon = vm.Icon,
            };
            await _context.services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service service=await _context.services.FirstOrDefaultAsync(e=>e.Id==id);
            if (service == null) return NotFound();
            UpdateServiceVM vm = new UpdateServiceVM
            {
                Name = service.Name,
                Description = service.Description,
                Icon = service.Icon,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVM vm)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.services.FirstOrDefaultAsync(e => e.Id == id);
            if (service == null) return NotFound();
            service.Name = vm.Name;
            service.Description = vm.Description;
            service.Icon = vm.Icon;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.services.FirstOrDefaultAsync(e => e.Id == id);
            if (service == null) return NotFound();
            _context.services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }





    }
}
