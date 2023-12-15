using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using NuGet.Protocol;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Immutable;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;
        private Author _currentAuthor;

        public BooksController(LibraryContext context, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            var tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
            if (tempData["Author"] != null)
            {
                _currentAuthor = JsonConvert.DeserializeObject<Author>(tempData["Author"].ToString());
            }
            _context = context;
        }

        //Action: Save Books
        public async Task<IActionResult> Save()
        {
            var fromDb = await _context.Authors.FirstOrDefaultAsync(m => m.AuthorID == _currentAuthor.AuthorID);
            if (TempData["data"] != null)
            {
                List<Book>? books = JsonConvert.DeserializeObject<List<Book>>(TempData["data"].ToString());
                foreach (var book in books)
                {
                    book.Author = fromDb;
                    book.Genre = _context.Genres.FirstOrDefault(m => m.Name == book.Genre.Name);
                    _context.Books.Add(book);
                }              
            }

            if(await _context.SaveChangesAsync() > 0)
            {
                TempData.Remove("data");
                TempData["saveStatus"] = true;
            }

            return Redirect("/Authors/Details/" + fromDb.AuthorID.ToString());
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        //Add Books to ToSave List of Books
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook([Bind("Title,PageNum,Genre", Prefix = "Item2")] Book book)
        {
            var fromDb = await _context.Authors.AsNoTracking().FirstOrDefaultAsync(m => m.AuthorID == _currentAuthor.AuthorID);
            book.Author = fromDb;
            if (ModelState.IsValid)
            {
                if (TempData["data"] != null)
                {
                    List<Book>? books = JsonConvert.DeserializeObject<List<Book>>(TempData["data"].ToString());
                    books.Add(book);
                    TempData["data"] = JsonConvert.SerializeObject(books);
                    TempData.Keep("data");
                }
                else
                {
                    TempData["data"] = JsonConvert.SerializeObject(new List<Book> { book });
                    TempData.Keep("data");
                }
                TempData["saveStatus"] = false;
                return Redirect("/Authors/Details/" + book.Author?.AuthorID.ToString());
            }
            return Problem("Нету книг для сохранения.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,PageNum,Genre", Prefix = "Item2")] Book book)
        {
            var fromDb = await _context.Authors.FirstOrDefaultAsync(m => m.AuthorID == _currentAuthor.AuthorID);
            book.Author = fromDb;
            book.Genre = await _context.Genres.Where(g => g.Name == book.Genre.Name).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return Redirect("/Authors/Details/" + fromDb?.AuthorID.ToString());
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Author).Where(b => b.Id == id).FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            var genres = _context.Genres.Select(g => new SelectListItem() { Text = g.Name }).ToList();
            return View(new Tuple<Book, List<SelectListItem>>(book, genres));
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PageNum,Genre")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Authors/Details/" + _currentAuthor.AuthorID);
            }

            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryContext.Books'  is null.");
            }
            var book = await _context.Books.Include(b => b.Author).Where(b => b.Id == id).FirstOrDefaultAsync();
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return Redirect("/Authors/Details/" + book.Author.AuthorID);
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
