using ASMSDataAccessLayer.ContactsDAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class RepositoryBase<T, Id> : IRepositoryBase<T, Id> where T : class, new()
    {
        protected readonly MyContext _myContext;
        public RepositoryBase(MyContext myContext)
        {
            _myContext = myContext;
        }
        public bool Add(T entity)
        {
            try
            {
                _myContext.Set<T>().Add(entity);
                return _myContext.SaveChanges() > 0 ? true : false;
                
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public bool Delete(T entity)
        {
            try
            {
                _myContext.Set<T>().Remove(entity);
                return _myContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeEntities = null)
        {
            try
            {
                IQueryable<T> query = _myContext.Set<T>();

                if (filter != null)
                {
                    //select * from tableName where condition
                    query = query.Where(filter);//where koşulu eklendi.
                }

                if (includeEntities != null)
                {
                    //var x = includeEntities.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var result = includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in result)
                    {
                        //inner join yaptı
                        query = query.Include(item);
                    }
                }

                if (orderBy!=null)
                {
                    return orderBy(query);
                }
                return query;
            }
            
            catch (Exception)
            {

                throw;
            }
        }

        public T GetById(Id id) 
        {
            try
            {
                return _myContext.Set<T>().Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeEntities = null)
        {
            IQueryable<T> query = _myContext.Set<T>();
            if (filter!=null)
            {
                //select * from tableName where condition
                query = query.Where(filter);//where koşulu eklendi.
            }
            //ilişkili oldugu tabloyu dahil etmek için (inner Join)
            if (includeEntities!=null)
            {
                //var x = includeEntities.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var result = includeEntities.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in result)
                {
                    //inner join yaptı
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }

        public bool Update(T entity)
        {
            try
            {
                _myContext.Set<T>().Update(entity);
                return _myContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
