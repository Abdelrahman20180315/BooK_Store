using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repositories
{
    public class BookRepository : IBookstoreRepositories<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
                {
                    new Book
                    {
                        Id=1, Title="C# Programming",
                        Description="No Description",
                        ImageUrl="App.jpeg",
                        Author=new Author()
                    },
                    new Book
                    {
                        Id=2, Title="Java Programming",
                        Description="Nothing",
                         ImageUrl="Cprog.jpg",
                        Author=new Author()
                    },
                    new Book
                    {
                        Id=3, Title="Python Programming",
                        Description="No data",
                        ImageUrl="icon.jpeg",
                        Author=new Author()
                    },

                };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book=find(id);
            books.Remove(book);
        }

        public Book find(int id)
        {
            var book= books.SingleOrDefault(b => b.Id ==id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(b => b.Title.Contains(term)).ToList();
        }

        public void Update(int id,Book newBook)
        {
            var book = find(id);
            book.Id = newBook.Id;
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
