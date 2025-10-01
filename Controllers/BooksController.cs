using BookApp.Exceptions;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Controllers
{
    public class BooksController : Controller
    {
        //  DB (static list)
        //private static List<Book> _books = null; //this is for null exception test
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "ASP.NET Core Basics", Author = "John Doe", Price = 200 },
            new Book { Id = 2, Title = "C# Fundamentals", Author = "Jane Smith", Price = 300 }
        };

        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        // GET: Books
        public IActionResult Index()
        {
            try
            {
                if (_books == null)
                    throw new BookDatabaseException("Failed to load book list from database.");
                return View(_books);
            }
            catch (BookDatabaseException ex)
            {
                _logger.LogError(ex, "Database error in Index:");
                TempData["Error"] = "Book list could not be loaded. Try again later.";
                return View(new List<Book>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books");
                TempData["Error"] = "Unable to load books.";
                return View(new List<Book>());
            }
        }

        // GET: Books/Details/5
        public IActionResult Details(int id)
        {

            try
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                    throw new BookNotFoundException(id);

                return View(book);
            }
            catch(BookNotFoundException ex)
            {
                _logger.LogError(ex, "Error fetching details");
                TempData["Error"] = $"Unable to load details for book with ID {id}.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching details");
                TempData["Error"] = "Unable to load book details.";
                return RedirectToAction("Index");
            }
        }

        // GET: Books/Create
        public IActionResult Create() => View();

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            try { 
                if (!ModelState.IsValid)
                    throw new InvalidBookDataException("Book data is not valid.");
                book.Id = _books.Max(b => b.Id) + 1;
                _books.Add(book);

                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("Index");
            }
            catch (InvalidBookDataException ex)
            {
                _logger.LogError(ex, "Error creating book with Title '{Title}' and Author '{Author}'", book.Title, book.Author);

                TempData["Error"] = "Could not create book. The data provided is invalid.";

                return View(book);
            }
            catch (Exception ex) {

                _logger.LogError(ex, "Error creating book");
                TempData["Error"] = "Could not create book.";
                return View(book);

            }
        
        }

        // GET: Books/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading book for edit");
                TempData["Error"] = "Unable to load book for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.Id) {
                return NotFound();
            }
            
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                var existing = _books.FirstOrDefault(b => b.Id == id);
                if (existing == null)
                {
                    return NotFound();
                }

                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.Price = book.Price;
                TempData["Success"] = "Book updated successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing book");
                TempData["Error"] = "Could not update book.";
                return View(book);
            }
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var book = _books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    _books.Remove(book);
                    TempData["Success"] = "Book removed successfully!";
                }
                else
                {
                    TempData["Error"] = "Book not found.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book");
                TempData["Error"] = "Could not delete book.";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
