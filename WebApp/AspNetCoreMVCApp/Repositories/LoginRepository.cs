using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.LoginDTO;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AspNetCoreMVCApp.Repositories
{
    public interface ILoginRepository
    {
        Task<LoginDTO> CheckLoginCredentialsAsync(LoginDTO loginCredentials);
    }
    public class LoginRepository : ILoginRepository
    {
        private readonly MyCompanyDbContext _dbContext;
        public LoginRepository(MyCompanyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoginDTO> CheckLoginCredentialsAsync(LoginDTO loginCredentials)
        {

            LoginDTO? credentials = await (from _emp in _dbContext.Employees

                                           join _empJob in _dbContext.EmployeeJob on _emp.EmployeeJobGuid equals _empJob.EmployeeJobTitleGuid

                                         where
                                         _emp.EmployeeEmail == loginCredentials.EmployeeEmail
                                         &&
                                         _emp.EmployeePassword == loginCredentials.EmployeePassword
                                         &&
                                         _emp.IsActive == true

                                         select new LoginDTO
                                         {
                                             EmployeeName = _emp.EmployeeName,
                                             EmployeeJobTitle = _empJob.EmployeeJobTitle
                                         })
                                        .FirstOrDefaultAsync();

            return await Task.Run(() => credentials);

        }
    }
}
