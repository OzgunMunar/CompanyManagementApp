using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.CustomerDTO;
using AspNetCoreMVCApp.DTO.OrderDTO;
using AspNetCoreMVCApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVCApp.Repositories
{
    public interface IOrderRepository:IRepositoryBase<Order>
    {
        Task<Tuple<List<OrderGetDTO>, List<CustomerNameAndGuidDTO>>> GetAllRecords();
        Task<OrderGetByGuidDTO> GetOrderByGuid(Guid guid);
    }
    public class OrderRepository : IOrderRepository
    {

        private readonly IRepositoryBase<Order> _repository;
        private readonly MyCompanyDbContext _companyDbContext;

        public OrderRepository(IRepositoryBase<Order> repository, MyCompanyDbContext companyDbContext)
        {
            _repository = repository;
            _companyDbContext = companyDbContext;
        }
        
        public async Task<Tuple<List<OrderGetDTO>, List<CustomerNameAndGuidDTO>>> GetAllRecords()
        {

            #region DatabaseQueries

            List<OrderGetDTO> orderData = await (from _order in _companyDbContext.Order

                                                 join _customer in _companyDbContext.Customer
                                                 on _order.OrderCustomerGuid equals _customer.CustomerGuid

                                                 where _order.IsActive == true

                                                 select new OrderGetDTO
                                                 {

                                                     _OrderGuid = _order.OrderGuid,
                                                     _OrderDate = _order.OrderDate,
                                                     _IsOrderShipped = _order.OrderIsShipped,
                                                     _OrderComments = _order.OrderComments,
                                                     _OrderCustomerGuid = _order.OrderCustomerGuid,
                                                     _CustomerName = _customer.CustomerName,
                                                     _IsActive = _order.IsActive

                                                 })
                                                .ToListAsync();

            List<CustomerNameAndGuidDTO> customerData = await (from _customer in _companyDbContext.Customer

                                                               where _customer.IsActive == true

                                                               select new CustomerNameAndGuidDTO
                                                               {
                                                                   CustomerGuid = _customer.CustomerGuid,
                                                                   CustomerName = _customer.CustomerName
                                                               })
                                                               .ToListAsync();

            #endregion

            return await Task.Run(() => Tuple.Create(orderData, customerData));
            
        }

        public async Task<OrderGetByGuidDTO> GetOrderByGuid(Guid guid)
        {

            #region Database Query

            OrderGetByGuidDTO? order = await (from _order in _companyDbContext.Order

                                              join _customer in _companyDbContext.Customer
                                              on _order.OrderCustomerGuid equals _customer.CustomerGuid

                                              where _order.OrderGuid == guid && _order.IsActive == true

                                              select new OrderGetByGuidDTO
                                              {

                                                  _OrderGuid = _order.OrderGuid,
                                                  _OrderDate = _order.OrderDate,
                                                  _IsOrderShipped = _order.OrderIsShipped,
                                                  _OrderComments = _order.OrderComments,
                                                  _OrderCustomerGuid = _customer.CustomerGuid,
                                                  _CustomerName = _customer.CustomerName,
                                                  _IsActive = _customer.IsActive

                                              })
                     .FirstOrDefaultAsync();

            #endregion

            return await Task.Run(() => order);

        }

        public async Task Create(Order entity)
        {
            entity.OrderGuid = Guid.NewGuid();
            entity.IsActive = true;
            await _repository.Create(entity);
        }

        public async Task Edit(Order entity)
        {
            await _repository.Edit(entity);
        }

        public async Task Delete(Order entity)
        {
            entity.IsActive = false;
            await _repository.Delete(entity);
        }

        public async Task DeletePermanently(Order entity)
        {
            await _repository.DeletePermanently(entity);
        }

    }
}