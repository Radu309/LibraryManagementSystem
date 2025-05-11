using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers;

[Authorize(Roles = "User,Admin")]
public class BooksController : Controller
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string title, string author)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(b => b.Title.Contains(title));

        if (!string.IsNullOrEmpty(author))
            query = query.Where(b => b.Author.Contains(author));

        var books = await query.OrderBy(b => b.Id).ToListAsync();
        return View(books); 
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
        if (book == null) return NotFound();

        return View(book);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            book.AvailableCopies = book.Quantity;
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Book book)
    {
        if (id != book.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Books.FindAsync(book.Id);
                if (existing == null) return NotFound();

                int borrowed = existing.Quantity - existing.AvailableCopies;

                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.Quantity = book.Quantity;
                existing.AvailableCopies = book.Quantity - borrowed;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
        if (book == null) return NotFound();

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id) => _context.Books.Any(e => e.Id == id);
    
    [HttpPost]
    public async Task<IActionResult> Borrow(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        if (book.AvailableCopies <= 0)
        {
            TempData["Error"] = "No available copies to borrow.";
        }
        else
        {
            book.AvailableCopies--;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"You borrowed: {book.Title}";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        if (book.AvailableCopies >= book.Quantity)
        {
            TempData["Error"] = "All copies have already been returned.";
        }
        else
        {
            book.AvailableCopies++;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"You returned: {book.Title}";
        }

        return RedirectToAction(nameof(Index));
    }

}
