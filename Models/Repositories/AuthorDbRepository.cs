using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repositories
{
    public class AuthorDbRepository : IBookstoreRepositories<Author>
    {
        BookStoreDbContext DB;
        public AuthorDbRepository(BookStoreDbContext _DB)
        {
            DB = _DB;
        }
        public void Add(Author entity)
        {
            DB.Authors.Add(entity);
            DB.SaveChanges();
        }

        public void Delete(int id)
        {
            var Author = find(id);
            DB.Authors.Remove(Author);
            DB.SaveChanges();
        }

        public Author find(int id)
        {
            var Author = DB.Authors.SingleOrDefault(a => a.Id == id);
            return Author;
           
        }

        public IList<Author> List()
        {
            return DB.Authors.ToList();
            DB.SaveChanges();
        }

        public List<Author> Search(string term)
        {
            return DB.Authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            DB.Update(newAuthor);
            DB.SaveChanges();
        }
    }
}
