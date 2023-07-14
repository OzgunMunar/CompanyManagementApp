using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.EmployeeDTO;
using AspNetCoreMVCApp.DTO.EmployeeJobDTO;
using AspNetCoreMVCApp.DTO.OfficeDTO;
using AspNetCoreMVCApp.Models;
using AspNetCoreMVCApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVCApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly EmployeeRepository _employeeRepository;
        public EmployeeController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            var indexData = await _employeeRepository.GetAllRecords();

            List<EmployeeGetDTO> employees = indexData.Item1;
            List<OfficeCityAndGuidDTO> officeData = indexData.Item2;
            List<EmployeeNameAndGuidDTO> employeeData = indexData.Item3;
            List<EmployeeJobDTO> employeeJobs = indexData.Item4;

            int dataCount = employees.Count();
            int dataNumberPerPage = 10;
            int TotalPages = (int)Math.Ceiling(dataCount / (double)dataNumberPerPage);

            bool HasPreviousPage = pageNumber > 1;
            bool HasNextPage = pageNumber < TotalPages;

            var tableRecords = employees.Skip((pageNumber - 1) * dataNumberPerPage).Take(dataNumberPerPage).ToList();

            ViewBag.Employees = tableRecords;
            ViewBag.EmployeeData = employeeData;        //Manager Dropdown
            ViewBag.OfficeData = officeData;            //Office Dropdown
            ViewBag.EmployeeJobData = employeeJobs;     //EmployeeJobs Dropdown
            ViewBag.PageNumber = pageNumber;
            ViewBag.prevPage = HasPreviousPage;
            ViewBag.nextPage = HasNextPage;

            return await Task.Run(() => View());

        }

        public async Task<JsonResult> GetEmployeeByGuid(Guid guid)
        {

            EmployeeGetByGuidDTO? employee = await _employeeRepository.GetEmployeeByGuid(guid);

            if (employee == null)
                TempData["error"] = "An error occured.";

            return await Task.Run(() => Json(employee));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee emp)
        {

            try
            {

                await _employeeRepository.Create(emp);
                
                TempData["success"] = "Employee created successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee emp)
        {

            try
            {

                await _employeeRepository.Edit(emp);
                TempData["success"] = "Employee updated successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Employee _employee)
        {

            try
            {

                await _employeeRepository.Delete(_employee);
                
                TempData["success"] = "Employee deleted successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));

        }

    }
}