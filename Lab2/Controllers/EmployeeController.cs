using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lab2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int pageSize = 10, int? positionId = null)
        {
            ViewBag.PageSize = pageSize;

            // Отримайте список всіх посад для відображення у фільтрі
            var positions = _context.Positions.ToList();
            ViewBag.Positions = new SelectList(positions, "PositionId", "Title");

            IQueryable<Employee> employeesQuery = _context.Employees.Include(e => e.Position);

            // Фільтрація за конкретною посадою, якщо вибрано
            if (positionId.HasValue)
            {
                employeesQuery = employeesQuery.Where(e => e.PositionId == positionId);
            }

            var totalRecords = employeesQuery.Count();

            // Отримайте список співробітників з бази даних з використанням пагінації
            var employees = employeesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Передайте вибрану посаду назад у фільтр
            ViewBag.SelectedPositionId = positionId;

            return View(employees);
        }




        [HttpGet]
        public IActionResult Create()
        {
            var positions = _context.Positions.ToList(); // Отримати список посад з бази даних
            ViewBag.Positions = new SelectList(_context.Positions, "PositionId", "Title");

            var employee = new Employee();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            var positions = _context.Positions.ToList();
            ViewBag.Positions = new SelectList(positions, "PositionId", "Title");
            ModelState.Remove("Position");
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Дані форми не пройшли валідацію");
                Console.WriteLine($"Посада (PositionId): {employee.PositionId}");
                return View(employee);
            }

            
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(); // Якщо співробітник не знайдений, повернути статус 404
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            // Отримайте URL попередньої сторінки з HTTP-заголовку Referer
            string refererUrl = Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(refererUrl))
            {
                return RedirectToAction("Index"); // Якщо немає посилання на попередню сторінку, перенаправте на Index
            }

            // Перенаправте на попередню сторінку
            return Redirect(refererUrl);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(); // Якщо співробітник не знайдений, повернути статус 404
            }

            ViewBag.Positions = new SelectList(_context.Positions, "PositionId", "Title");
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            ModelState.Remove("Position");
            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                if (existingEmployee == null)
                {
                    return NotFound(); // Якщо співробітник не знайдений, повернути статус 404
                }

                // Оновіть дані співробітника на основі вхідних даних
                existingEmployee.FullName = employee.FullName;
                existingEmployee.Age = employee.Age;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.Address = employee.Address;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.PassportData = employee.PassportData;
                existingEmployee.PositionId = employee.PositionId;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Positions = new SelectList(_context.Positions, "PositionId", "Title");
            return View(employee);
        }
    }
}
