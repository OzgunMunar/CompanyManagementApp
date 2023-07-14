using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.CustomerDTO;
using AspNetCoreMVCApp.DTO.EmployeeDTO;
using AspNetCoreMVCApp.DTO.OrderDTO;
using AspNetCoreMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Repositories
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Tuple<List<CustomerGetDTO>, List<EmployeeNameAndGuidDTO>>> GetAllRecords();
        Task<CustomerGetByGuidDTO> GetCustomerByGuid(Guid guid);
    }
    public class CustomerRepository : ICustomerRepository
    {

        private readonly IRepositoryBase<Customer> _repositoryBase;
        private readonly MyCompanyDbContext _companyDbContext;

        public CustomerRepository(IRepositoryBase<Customer> repositoryBase, MyCompanyDbContext companyDbContext)
        {
            _companyDbContext = companyDbContext;
            _repositoryBase = repositoryBase;
        }

        public async Task<Tuple<List<CustomerGetDTO>, List<EmployeeNameAndGuidDTO>>> GetAllRecords()
        {

            #region Database Queries

            List<CustomerGetDTO> customerData = await (from _customer in _companyDbContext.Customer

                                                       join _employee in _companyDbContext.Employees
                                                       on _customer.CustomerBoughtFromEmployeeGuid equals _employee.EmployeeGuid

                                                       where _customer.IsActive == true

                                                       select new CustomerGetDTO
                                                       {
                                                           _CustomerGuid = _customer.CustomerGuid,
                                                           _CustomerName = _customer.CustomerName,
                                                           _CustomerPhoneNumber = _customer.CustomerPhoneNumber,
                                                           _CustomerPostalCode = _customer.CustomerPostalCode,
                                                           _CustomerBoughtFromEmployeeGuid = _customer.CustomerBoughtFromEmployeeGuid,
                                                           _CustomerBoughtFromEmployeeName = _employee.EmployeeName,
                                                           _CustomerAddress = _customer.CustomerAddress,
                                                           _IsActive = _customer.IsActive
                                                       })
                                                .ToListAsync();

            List<EmployeeNameAndGuidDTO> employeesNameAndGuid = await (from _employee in _companyDbContext.Employees

                                                                       where _employee.IsActive == true

                                                                       select new EmployeeNameAndGuidDTO
                                                                       {
                                                                           EmployeeGuid = _employee.EmployeeGuid,
                                                                           EmployeeName = _employee.EmployeeName
                                                                       })
                                                                .ToListAsync();

            #endregion

            return await Task.Run(() => Tuple.Create(customerData, employeesNameAndGuid));

        }

        public async Task<CustomerGetByGuidDTO> GetCustomerByGuid(Guid guid)
        {

            #region Database Query

            CustomerGetByGuidDTO? customer = await (from _customer in _companyDbContext.Customer

                                                    join _emp in _companyDbContext.Employees
                                                    on _customer.CustomerBoughtFromEmployeeGuid equals _emp.EmployeeGuid

                                                    where _customer.CustomerGuid == guid && _customer.IsActive == true

                                                    select new CustomerGetByGuidDTO
                                                    {
                                                        _CustomerGuid = _customer.CustomerGuid,
                                                        _CustomerName = _customer.CustomerName,
                                                        _CustomerPhoneNumber = _customer.CustomerPhoneNumber,
                                                        _CustomerAddress = _customer.CustomerAddress,
                                                        _CustomerPostalCode = _customer.CustomerPostalCode,
                                                        _CustomerBoughtFromEmployeeGuid = _emp.EmployeeGuid,
                                                        _CustomerBoughtFromEmployeeName = _emp.EmployeeName,
                                                        _IsActive = _customer.IsActive
                                                    }).FirstOrDefaultAsync();

            #endregion

            return await Task.Run(() => customer);

        }

        public async Task Create(Customer entity)
        {
            entity.CustomerGuid = Guid.NewGuid();
            entity.IsActive = true;
            await _repositoryBase.Create(entity);
        }

        public async Task Edit(Customer entity)
        {
            await _repositoryBase.Edit(entity);
        }

        public async Task Delete(Customer entity)
        {
            entity.IsActive = false;
            await _repositoryBase.Delete(entity);
        }

        public async Task DeletePermanently(Customer entity)
        {
            await _repositoryBase.DeletePermanently(entity);
        }

    }
}