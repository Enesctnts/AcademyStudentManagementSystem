using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASMSDataAccessLayer.ContactsDAL
{
    public interface IRepositoryBase<T,Id> where T : class,new()
    {

        //includeEntities inner join ile ilişkide oldugu entity getirecektir.
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null,Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null, string includeEntities = null);

        //alttaki filter = null yap hata almak için böyle yapıyoz :((
        T GetFirstOrDefault(Expression<Func<T, bool>> filter=null, string includeEntities=null);
        T GetById(Id id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);

    }
}
