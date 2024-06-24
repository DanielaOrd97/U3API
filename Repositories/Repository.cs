using Microsoft.EntityFrameworkCore;
using U3API.Models;

namespace U3Api.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly LabsysteDoubledContext context;

        public Repository(LabsysteDoubledContext context)
        {
            this.context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public virtual T? Get(object id)
        {
            return context.Find<T>(id);
        }

        public virtual void Insert(T entity)
        {

            context.Add(entity); context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public virtual void Delete(T id)
        {
            context.Remove(id); context.SaveChanges();
        }
    }
}
