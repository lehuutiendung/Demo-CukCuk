using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repository
{
    public interface ICustomerRepository:IBaseRepository<Customer>
    {
        /*List<object> GetCustomer();

        Object GetCustomerById(Guid customerId);

        Object FilterCustomer(int pageSize, int pageNumber, String fullName, String customerCode, String phoneNumber);

        Object NewCustomerCode();

        int AddCustomer(Customer customer);

        int DeleteCustomer(Guid customerId);

        int UpdateCustomer(Guid customerId, Customer customer);*/

    }
}
