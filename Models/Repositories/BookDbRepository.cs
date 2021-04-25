using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repositories
{
    public class BookDbRepository : IBookstoreRepositories<Book>
    {
        BookStoreDbContext DB;
        public BookDbRepository(BookStoreDbContext _DB)
        {
            DB = _DB;
        }
        
        public void Add(Book entity)
        {
            DB.Books.Add(entity);
            DB.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = find(id);
            DB.Books.Remove(book);
            DB.SaveChanges();
        }

        public Book find(int id)
        {
            var book = DB.Books.Include(a=>a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return DB.Books.Include(a=>a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            DB.Update(newBook);
            DB.SaveChanges();
        }

        public List<Book> Search(string term)
        {
            var result = DB.Books.Include(a => a.Author).Where(b => b.Title.Contains(term)
             || (b.Description.Contains(term))
             || (b.Author.FullName.Contains(term))).ToList();

            return result;
        }
    }
}
