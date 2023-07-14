using AspNetCoreMVCApp.Data;
using AspNetCoreMVCApp.DTO.OfficeDTO;
using AspNetCoreMVCApp.Models;
using AspNetCoreMVCApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVCApp.Controllers
{
    [Authorize]
    public class OfficeController : Controller
    {

        private readonly OfficeRepository _officeRepository;

        public OfficeController(OfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            var indexData = await _officeRepository.GetAllRecords();

            int dataCount = indexData.Count();
            int dataNumberPerPage = 10;
            int TotalPages = (int)Math.Ceiling(dataCount / (double)dataNumberPerPage);

            bool HasPreviousPage = pageNumber > 1;
            bool HasNextPage = pageNumber < TotalPages;

            var tableRecords = indexData.Skip((pageNumber - 1) * dataNumberPerPage).Take(dataNumberPerPage).ToList();

            ViewBag.Offices = tableRecords;
            ViewBag.PageNumber = pageNumber;
            ViewBag.prevPage = HasPreviousPage;
            ViewBag.nextPage = HasNextPage;

            return await Task.Run(() => View());

        }

        public async Task<JsonResult> GetOfficeByGuid(Guid guid)
        {

            OfficeGetDTO? office = await _officeRepository.GetOfficeByGuid(guid);

            if (office == null)
                TempData["error"] = "An error occured.";

            return await Task.Run(() => Json(office));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Office _office)
        {

            try
            {

                await _officeRepository.Create(_office);
                TempData["success"] = "Office created successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Office _office)
        {

            try
            {

                await _officeRepository.Edit(_office);
                TempData["success"] = "Office updated successfully.";

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
        public async Task<IActionResult> Delete(Office _office)
        {

            try
            {

                await _officeRepository.Delete(_office);

                TempData["success"] = "Office deleted successfully.";

            }
            catch (Exception)
            {
                TempData["error"] = "An error occured.";
            }

            return await Task.Run(() => RedirectToAction("Index"));
            
        }

    }
}