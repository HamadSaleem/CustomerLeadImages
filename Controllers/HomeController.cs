
using CustomerLeadImages.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerLeadImages.Controllers;
public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) { _db = db; }
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalCustomers = await _db.Customers.CountAsync();
        ViewBag.TotalLeads = await _db.Leads.CountAsync();
        return View();
    }
}
