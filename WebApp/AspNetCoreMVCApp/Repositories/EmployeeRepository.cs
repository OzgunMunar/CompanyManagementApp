using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.EmployeeDTO;
using AspNetCoreMVCApp.DTO.EmployeeJobDTO;
using AspNetCoreMVCApp.DTO.OfficeDTO;
using AspNetCoreMVCApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Repositories
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<Tuple<List<EmployeeGetDTO>, List<OfficeCityAndGuidDTO>, List<EmployeeNameAndGuidDTO>, List<EmployeeJobDTO>>> GetAllRecords();
        Task<EmployeeGetByGuidDTO> GetEmployeeByGuid(Guid guid);
    }
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly IRepositoryBase<Employee> _repository;
        private readonly MyCompanyDbContext _companyDbContext;

        public EmployeeRepository(IRepositoryBase<Employee> repository, MyCompanyDbContext companyDbContext)
        {
            _repository = repository;
            _companyDbContext = companyDbContext;
        }

        public async Task<Tuple<List<EmployeeGetDTO>, List<OfficeCityAndGuidDTO>, List<EmployeeNameAndGuidDTO>, List<EmployeeJobDTO>>> GetAllRecords()
        {

            #region Database Queries

            List<EmployeeGetDTO> employees = await (from _employee in _companyDbContext.Employees

                                                    join _office in _companyDbContext.Office on _employee.EmployeeOfficeGuid equals _office.OfficeGuid

                                                    join _manager in _companyDbContext.Employees on _employee.EmployeeManagerGuid equals _manager.EmployeeGuid

                                                    join _employeeJob in _companyDbContext.EmployeeJob on _employee.EmployeeJobGuid equals _employeeJob.EmployeeJobTitleGuid

                                                    where _employee.IsActive == true

                                                    select new EmployeeGetDTO
                                                    {
                                                        _EmployeeGuid = _employee.EmployeeGuid,
                                                        _EmployeeName = _employee.EmployeeName,
                                                        _EmployeePhoneNumber = _employee.EmployeePhoneNumber,
                                                        _EmployeeEmail = _employee.EmployeeEmail,
                                                        _EmployeePassword = _employee.EmployeePassword,
                                                        _EmployeeOfficeGuid = _employee.EmployeeOfficeGuid,
                                                        _EmployeeJobGuid = _employee.EmployeeJobGuid,
                                                        _EmployeeJobTitle = _employeeJob.EmployeeJobTitle,
                                                        _EmployeeOffice = _office.OfficeCity,
                                                        _EmployeeManagerGuid = _employee.EmployeeManagerGuid,
                                                        _EmployeeManager = _manager.EmployeeName,
                                                        _EmployeeStartDate = _employee.EmployeeStartDate,

                                                    }).ToListAsync();


            List<OfficeCityAndGuidDTO> officeData = await (from _office in _companyDbContext.Office

                                                           where _office.IsActive == true

                                                           select new OfficeCityAndGuidDTO
                                                           {
                                                               OfficeGuid = _office.OfficeGuid,
                                                               OfficeCity = _office.OfficeCity
                                                           }).ToListAsync();


            List<EmployeeNameAndGuidDTO> employeeData = (from _employee in employees

                                                         select new EmployeeNameAndGuidDTO
                                                         {
                                                             EmployeeGuid = _employee._EmployeeGuid,
                                                             EmployeeName = _employee._EmployeeName
                                                         }).ToList();

            List<EmployeeJobDTO> employeeJobs = await (from _employeeJob in _companyDbContext.EmployeeJob

                                                       where _employeeJob.IsActive == true

                                                       select new EmployeeJobDTO
                                                       {
                                                           _EmployeeJobGuid = _employeeJob.EmployeeJobTitleGuid,
                                                           _EmployeeJobTitle = _employeeJob.EmployeeJobTitle
                                                       })
                                                        .ToListAsync();



            #endregion


            return await Task.Run(() => Tuple.Create(employees, officeData, employeeData, employeeJobs));

        }

        public async Task<EmployeeGetByGuidDTO> GetEmployeeByGuid(Guid guid)
        {

            #region Database Query

            EmployeeGetByGuidDTO? employee = await (from _employee in _companyDbContext.Employees

                                                    join _office in _companyDbContext.Office on _employee.EmployeeOfficeGuid equals _office.OfficeGuid

                                                    join _manager in _companyDbContext.Employees on _employee.EmployeeManagerGuid equals _manager.EmployeeGuid

                                                    join _employeeJob in _companyDbContext.EmployeeJob on _employee.EmployeeJobGuid equals _employeeJob.EmployeeJobTitleGuid

                                                    where _employee.EmployeeGuid == guid && _employee.IsActive == true

                                                    select new EmployeeGetByGuidDTO
                                                    {

                                                        _EmployeeGuid = _employee.EmployeeGuid,
                                                        _EmployeeName = _employee.EmployeeName,
                                                        _EmployeePhoneNumber = _employee.EmployeePhoneNumber,
                                                        _EmployeeEmail = _employee.EmployeeEmail,
                                                        _EmployeePassword = _employee.EmployeePassword,
                                                        _EmployeeOfficeGuid = _office.OfficeGuid,
                                                        _EmployeeOffice = _office.OfficeCity,
                                                        _EmployeeJobGuid = _employee.EmployeeJobGuid,
                                                        _EmployeeJobTitle = _employeeJob.EmployeeJobTitle,
                                                        _EmployeeManager = _manager.EmployeeName,
                                                        _EmployeeManagerGuid = _employee.EmployeeManagerGuid,
                                                        _EmployeeStartDate = _employee.EmployeeStartDate,
                                                        _IsActive = _employee.IsActive

                                                    }).FirstOrDefaultAsync();

            #endregion

            return await Task.Run(() => employee);

        }

        public async Task Create(Employee entity)
        {
            entity.EmployeeGuid = Guid.NewGuid();
            entity.IsActive = true;
            await _repository.Create(entity);
        }

        public async Task Edit(Employee entity)
        {
            await _repository.Edit(entity);
        }

        public async Task Delete(Employee entity)
        {
            entity.IsActive = false;
            await _repository.Delete(entity);
        }

        public async Task DeletePermanently(Employee entity)
        {
            await _repository.DeletePermanently(entity);
        }

    }
}