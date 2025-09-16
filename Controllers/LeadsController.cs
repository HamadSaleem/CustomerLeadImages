
using CustomerLeadImages.Data;
using CustomerLeadImages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerLeadImages.Controllers;
public class LeadsController : Controller
{
    private readonly AppDbContext _db;
    public LeadsController(AppDbContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var items = await _db.Leads
            .Select(c => new OwnerListItem
            {
                Id = c.Id,
                Title = c.Company,
                ImageCount = _db.ProfileImages.Count(pi => pi.OwnerType == "lead" && pi.OwnerId == c.Id)
            })
            .ToListAsync();
        return View(items);
    }

    public IActionResult Create() => View(new Lead());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lead m)
    {
        if (!ModelState.IsValid) return View(m);
        _db.Leads.Add(m);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _db.Leads.FindAsync(id);
        if (entity == null) return NotFound();
        return View(entity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lead m)
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
        var entity = await _db.Leads.FindAsync(id);
        if (entity == null) return NotFound();
        var imgs = _db.ProfileImages.Where(x => x.OwnerType == "lead" && x.OwnerId == id);
        _db.ProfileImages.RemoveRange(imgs);
        _db.Leads.Remove(entity);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Upload(int id)
    {
        var entity = await _db.Leads.FindAsync(id);
        if (entity == null) return NotFound();
        ViewBag.OwnerType = "lead";
        return View("Details", entity);
    }

    public async Task<IActionResult> Gallery(int id)
    {
        var entity = await _db.Leads.FindAsync(id);
        if (entity == null) return NotFound();
        var images = await _db.ProfileImages
            .Where(i => i.OwnerType == "lead" && i.OwnerId == id)
            .OrderBy(i => i.CreatedOn)
            .ToListAsync();
        ViewBag.Title = entity.Company;
        return View(images);
    }
}
