using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.CustomerDTO;
using AspNetCoreMVCApp.DTO.EmployeeDTO;
using AspNetCoreMVCApp.Models;
using AspNetCoreMVCApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Composition.Convention;

namespace AspNetCoreMVCApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {

        private readonly CustomerRepository _customerRepository;
        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            #region Database Queries

            var indexData = await _customerRepository.GetAllRecords();

            List<CustomerGetDTO> customerData = indexData.Item1;
            List<EmployeeNameAndGuidDTO> employeesNameAndGuid = indexData.Item2;

            #endregion
            
            int dataCount = customerData.Count();
            int dataNumberPerPage = 10;
            int TotalPages = (int)Math.Ceiling(dataCount / (double)dataNumberPerPage);

            bool HasPreviousPage = pageNumber > 1;
            bool HasNextPage = pageNumber < TotalPages;

            var tableRecords = customerData.Skip((pageNumber - 1) * dataNumberPerPage).Take(dataNumberPerPage).ToList();

            ViewBag.Customers = tableRecords;
            ViewBag.EmployeeData = employeesNameAndGuid;  //Employee who sold to the customer Dropdown
            ViewBag.PageNumber = pageNumber;
            ViewBag.prevPage = HasPreviousPage;
            ViewBag.nextPage = HasNextPage;

            return await Task.Run(() => View());

        }

        public async Task<JsonResult> GetCustomerByGuid(Guid guid)
        {

            CustomerGetByGuidDTO? customer = await _customerRepository.GetCustomerByGuid(guid);

            if (customer == null)
                TempData["error"] = "An error occured.";

            return await Task.Run(() => Json(customer));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer _customer)
        {

            try
            {

                await _customerRepository.Create(_customer);
                TempData["success"] = "Customer created successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer _customer)
        {

            try
            {

                await _customerRepository.Edit(_customer);
                TempData["success"] = "Customer updated successfully.";

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
        public async Task<IActionResult> Delete(Customer customer)
        {

            try
            {

                await _customerRepository.Delete(customer);
                TempData["success"] = "Customer deleted successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));

        }

        

    }
}