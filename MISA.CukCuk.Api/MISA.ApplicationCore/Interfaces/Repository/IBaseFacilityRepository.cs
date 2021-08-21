using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repository
{
    /// <summary>
    /// Base cho Department, Position
    /// </summary>
    /// <typeparam name="FacilityEntity"></typeparam>
    public interface IBaseFacilityRepository<FacilityEntity>
    {
        List<object> GetAll();

        Object GetById(Guid facilityEntityId);
    }
}
