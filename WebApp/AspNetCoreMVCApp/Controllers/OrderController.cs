using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.CustomerDTO;
using AspNetCoreMVCApp.DTO.OrderDTO;
using AspNetCoreMVCApp.Models;
using AspNetCoreMVCApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVCApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            var indexData = await _orderRepository.GetAllRecords();

            List<OrderGetDTO> orderData = indexData.Item1;
            List<CustomerNameAndGuidDTO> customerData = indexData.Item2;

            int dataCount = orderData.Count();
            int dataNumberPerPage = 10;
            int TotalPages = (int)Math.Ceiling(dataCount / (double)dataNumberPerPage);

            bool HasPreviousPage = pageNumber > 1;
            bool HasNextPage = pageNumber < TotalPages;

            var tableRecords = orderData.Skip((pageNumber - 1) * dataNumberPerPage).Take(dataNumberPerPage).ToList();

            ViewBag.Orders = tableRecords;
            ViewBag.CustomerData = customerData;
            ViewBag.PageNumber = pageNumber;
            ViewBag.prevPage = HasPreviousPage;
            ViewBag.nextPage = HasNextPage;

            return await Task.Run(() => View());

        }

        public async Task<JsonResult> GetOrderByGuid(Guid guid)
        {

            OrderGetByGuidDTO? order = await _orderRepository.GetOrderByGuid(guid);

            if (order == null)
                TempData["error"] = "An error occured.";

            return await Task.Run(() => Json(order));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order _order)
        {

            try
            {

                await _orderRepository.Create(_order);
                TempData["success"] = "Order created successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order _order)
        {

            try
            {

                await _orderRepository.Edit(_order);
                TempData["success"] = "Order updated successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
                throw;
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Order order)
        {

            try
            {

                await _orderRepository.Delete(order);

                TempData["success"] = "Order deleted successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

    }
}