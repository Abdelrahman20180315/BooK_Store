using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repositories
{
    public class AuthorRepository : IBookstoreRepositories<Author>
    {
        List<Author> Authors = new List<Author>()
        {
            new Author
            {
                Id=1 , FullName="Abdelrahman Ramadan"
            },
            new Author
            {
                Id=2, FullName="Khaled ElSsaadani" 

            },
            new Author
            {
                Id=3, FullName="Elzero"
            },
        };
        public void Add(Author entity)
        {
            entity.Id = Authors.Max(A => A.Id) + 1;
            Authors.Add(entity);
        }

        public void Delete(int id)
        {
            var Author= find(id);
            Authors.Remove(Author);
        }

        public Author find(int id)
        {
            var Author = Authors.SingleOrDefault(a => a.Id == id);
            return Author;
        }

        public IList<Author> List()
        {
            return Authors;
        }

        public List<Author> Search(string term)
        {
            return Authors.Where(a => a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            var Author = find(id);
            Author.Id = newAuthor.Id;
            Author.FullName = newAuthor.FullName;
        }
    }
}
