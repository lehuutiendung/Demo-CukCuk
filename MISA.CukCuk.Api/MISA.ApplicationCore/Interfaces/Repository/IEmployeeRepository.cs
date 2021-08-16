using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repository
{
    public interface IEmployeeRepository:IBaseRepository<Employee>
    {
        /*List<object> GetEmployee();

        Object GetEmployeeById(Guid employeeId);

        Object FilterEmployee(int pageSize, int pageNumber, String fullName, String employeeCode, String phoneNumber);

        Object NewEmployeeCode();

        int AddEmployee(Employee employee);

        int DeleteEmployee(Guid employeeId);

        int UpdateEmployee(Guid employeeId, Employee employee);*/
    }
}
