
using CustomerLeadImages.Data;
using CustomerLeadImages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerLeadImages.Controllers;
public class CustomersController : Controller
{
    private readonly AppDbContext _db;
    public CustomersController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var items = await _db.Customers
            .Select(c => new OwnerListItem
            {
                Id = c.Id,
                Title = c.Name,
                ImageCount = _db.ProfileImages.Count(pi => pi.OwnerType == "customer" && pi.OwnerId == c.Id)
            })
            .ToListAsync();
        return View(items);
    }

    public IActionResult Create() => View(new Customer());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Customer m)
    {
        if (!ModelState.IsValid) return View(m);
        _db.Customers.Add(m);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return NotFound();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer m)
    {
        if (id != m.Id) return NotFound();
        if (!ModelState.IsValid) return View(m);
        _db.Entry(m).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return NotFound();
        var imgs = _db.ProfileImages.Where(x => x.OwnerType == "customer" && x.OwnerId == id);
        _db.ProfileImages.RemoveRange(imgs);
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Upload(int id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return NotFound();
        ViewBag.OwnerType = "customer";
        return View("Details", entity);
    }

    public async Task<IActionResult> Gallery(int id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return NotFound();
        var images = await _db.ProfileImages
            .Where(i => i.OwnerType == "customer" && i.OwnerId == id)
            .OrderBy(i => i.CreatedOn)
            .ToListAsync();
        ViewBag.Title = entity.Name;
        return View(images);
    }
}
