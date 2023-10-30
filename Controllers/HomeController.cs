using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Assignment_Chefs_N_Dishes.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Chefs__N_Dishes.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        List<Chef> CountDish = _context.Chefs.Include(d => d.AllDishes).ToList();
        return View("Index", CountDish);
    }

    [HttpGet("dishes")]
    public IActionResult Dishes()
    {
        List<Dishe> dishes = _context.Dishes.Include(d => d.Creator).ToList();
        return View("Dishes", dishes);
    }

    [HttpGet("chefs/new")]
    public IActionResult AddChef()
    {

        return View("AddChef");
    }
    [HttpPost("chef/create")]
    public IActionResult CreateChef(Chef NewChef)
    {
        if(ModelState.IsValid)
        {
            var today = DateTime.Today;
            var age = today.Year - NewChef.DateOfBirth.Year;
            if(NewChef.DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }        
            if(age >= 18)
            {
                _context.Chefs.Add(NewChef);
                _context.SaveChanges();
                return RedirectToAction("Index", NewChef);
            }
            else
            {
                ModelState.AddModelError("DateOfBirth", "Chef must be at least 18 years old.");
                return View("AddChef");
            }
        }
        else
        {
            return View("AddChef");
        }
        
    }

    [HttpGet("dishes/new")]
    public IActionResult AddDish()
    {
        List<Chef> chefs = _context.Chefs.ToList();
        Dishe newDish = new Dishe();
        ViewData["Chefs"] = chefs;
        return View("AddDish", newDish);
    }
    [HttpPost("diches/create")]
    public IActionResult CreateDishes(Dishe NewDish)
    {
        if(ModelState.IsValid)
        {
            _context.Dishes.Add(NewDish);
            _context.SaveChanges();
            return RedirectToAction("Dishes");
        }
        else
        {
            return View("AddDish");
        }
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
