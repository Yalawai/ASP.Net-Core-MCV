using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WDP2024Assignment2.Models;

using WDP2024Assignment2.Data;

using Microsoft.EntityFrameworkCore;

namespace WDP2024Assignment2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

   public async Task<IActionResult> Index()
        {
            return View(await _context.AIImage.OrderByDescending(img => img.Like).ToListAsync());
        }

    public IActionResult Contact()
    {
        return View();
    }

    
    public IActionResult Reset()
    {

        var  imgs = _context.AIImage.ToList();
        foreach (var img in imgs){
            img.canIncreaseLike = true;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
