using Book_Store.Models;
using Book_Store.Models.Repositories;
using Book_Store.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepositories<Book> bookRepository;
        private readonly IBookstoreRepositories<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookstoreRepositories<Book> bookRepository,
            IBookstoreRepositories<Author> authorRepository,
            IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();

            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.find(id);

            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors=FillSelection()
            };

            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel Model)
        {
            if (ModelState.IsValid) {

                try
                {
                    string fileName = UploadFile(Model.File) ?? string.Empty;
                   
                    if (Model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an Author from the list!";

                        return View(GetAllAuthors());
                    }
                    var author = authorRepository.find(Model.AuthorId);
                    Book book = new Book
                    {
                        Id = Model.BookId,
                        Title = Model.Title,
                        Description = Model.Description,
                        Author = author,
                        ImageUrl=fileName

                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "You have to fill All Required Fields!");

            return View(GetAllAuthors());

        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl
            };

            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel ViewModel)
        {
            try
            {
                string fileName = UploadFile(ViewModel.File, ViewModel.ImageUrl);
                var author = authorRepository.find(ViewModel.AuthorId);
                Book book = new Book
                {
                    Id = ViewModel.BookId,
                    Title = ViewModel.Title,
                    Description = ViewModel.Description,
                    Author = author,
                    ImageUrl = fileName
                };
                bookRepository.Update(ViewModel.BookId,book);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception)
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelection()
        {
            var Authors =authorRepository.List().ToList();
            Authors.Insert(0,new Author { Id=-1,FullName="--- Please Choose Author ---"});
            return Authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vModel = new BookAuthorViewModel
            {
                Authors = FillSelection()
            };
            return(vModel);
        }
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                string FullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(FullPath, FileMode.Create));

                return file.FileName;
            }
            return null;
        }
        string UploadFile(IFormFile file ,string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");               
                string newPath = Path.Combine(uploads, file.FileName);

                string OldPath = Path.Combine(uploads, imageUrl);

                if (newPath != OldPath)
                {

                    //Delete old Path
                    System.IO.File.Delete(OldPath);
                    //Save New File
                    file.CopyTo(new FileStream(newPath, FileMode.Create));

                }
                return file.FileName;
            }
            return imageUrl;
        }
        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);

            return View("Index",result);
        }
    }
}
