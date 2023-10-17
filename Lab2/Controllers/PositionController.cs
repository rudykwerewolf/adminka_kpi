using Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Position> positions = _context.Positions.ToList();
            return View(positions);
        }
    }
}
