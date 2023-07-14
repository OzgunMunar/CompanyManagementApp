using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.OfficeDTO;
using AspNetCoreMVCApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Repositories
{
    public interface IOfficeRepository : IRepositoryBase<Office>
    {
        Task<List<OfficeGetDTO>> GetAllRecords();
        Task<OfficeGetDTO> GetOfficeByGuid(Guid guid);
    }
    public class OfficeRepository : IOfficeRepository
    {

        private readonly IRepositoryBase<Office> _repository;
        private readonly MyCompanyDbContext _companyDbContext;

        public OfficeRepository(IRepositoryBase<Office> repository, MyCompanyDbContext companyDbContext)
        {
            _repository = repository;
            _companyDbContext = companyDbContext;
        }

        public async Task<List<OfficeGetDTO>> GetAllRecords()
        {

            #region DatabaseQueries

            List<OfficeGetDTO> officeData = await (from _office in _companyDbContext.Office

                                                   where _office.IsActive == true

                                                   select new OfficeGetDTO
                                                   {
                                                       _OfficeGuid = _office.OfficeGuid,
                                                       _OfficeAddress = _office.OfficeAddress,
                                                       _OfficeCity = _office.OfficeCity,
                                                       _OfficePhoneNumber = _office.OfficePhoneNumber,
                                                       _OfficePostalCode = _office.OfficePostalCode,
                                                       _IsActive = _office.IsActive
                                                   })
                                            .ToListAsync();

            #endregion

            return await Task.Run(() => officeData);

        }

        public async Task<OfficeGetDTO> GetOfficeByGuid(Guid guid)
        {

            #region Database Query

            OfficeGetDTO? office = await (from _office in _companyDbContext.Office

                                            where _office.IsActive == true && _office.OfficeGuid == guid

                                            select new OfficeGetDTO
                                            {
                                                _OfficeGuid = _office.OfficeGuid,
                                                _OfficeAddress = _office.OfficeAddress,
                                                _OfficeCity = _office.OfficeCity,
                                                _OfficePhoneNumber = _office.OfficePhoneNumber,
                                                _OfficePostalCode = _office.OfficePostalCode,
                                                _IsActive = _office.IsActive
                                            })
                                            .FirstOrDefaultAsync();

            #endregion

            return await Task.Run(() => office);


        }

        public async Task Create(Office entity)
        {
            entity.OfficeGuid = Guid.NewGuid();
            entity.IsActive = true;
            await _repository.Create(entity);
        }

        public async Task Edit(Office entity)
        {
            await _repository.Edit(entity);
        }

        public async Task Delete(Office entity)
        {
            entity.IsActive = false;
            await _repository.Delete(entity);
        }

        public async Task DeletePermanently(Office entity)
        {
            await _repository.DeletePermanently(entity);
        }

    }
}