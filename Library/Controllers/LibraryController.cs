using Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class LibraryController : Controller
    {
        private readonly LibraryContext _context;
        public LibraryController(LibraryContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            return View(await _context.Authors.ToListAsync());
        }
    }
}
