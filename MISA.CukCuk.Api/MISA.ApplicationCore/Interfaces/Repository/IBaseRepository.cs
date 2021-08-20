using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repository
{
    public interface IBaseRepository<Entity>
    {
        List<object> GetAll<Entity>();

        Object GetById(Guid entityId);

        Object Filter(int pageSize, int pageNumber, String filter);

        Object NewCode();

        int Add(Entity entity);

        int Delete(Guid entityId);

        int DeleteMultiple(List<Guid> listId);

        int Update(Guid entityId, Entity entity);

    }
}
