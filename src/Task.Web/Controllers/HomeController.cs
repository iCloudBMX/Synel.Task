using Microsoft.AspNetCore.Mvc;
using Task.Application.Contracts;
using Task.Application.Services;
using Task.Domain;
using Task.Domain.Entities;

namespace Task.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEmployeeService employeeService;
    private readonly ICsvService csvService;

    public HomeController(
        IEmployeeService employeeService,
        ICsvService csvService)
    {
        this.employeeService = employeeService;
        this.csvService = csvService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        [FromQuery] GridParameters parameters)
    {
        ViewData[Constants.ErrorViewData] = TempData[Constants.ErrorViewData];
        ViewData[Constants.MessageViewData] = TempData[Constants.MessageViewData];

        var result = await this.employeeService
            .RetrieveEmployeesAsync(parameters);

        if (!result.Succeeded)
        {
            ViewData[Constants.ErrorViewData] = result.Message;
            return RedirectToAction("Index");
        }

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] IFormFile formFile)
    {
        if (formFile.ContentType != "text/csv")
        {
            TempData[Constants.ErrorViewData] = Errors.InvalidFileType;
            return RedirectToAction("Index");
        }

        var employees = await this.csvService
            .ReadDataAsync<Employee>(formFile);

        var result = await this.employeeService
            .AddEmployeeRangeAsync(employees);

        if (!result.Succeeded)
        {
            TempData[Constants.ErrorViewData] = result.Message;
            return RedirectToAction("Index");
        }

        TempData["Message"] = string.Format(Constants.FileImportMessage, employees.Count);

        return RedirectToAction("Index");
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit([FromRoute]Guid id)
    {
        var result = await this.employeeService
            .RetrieveEmployeeByIdAsync(id);

        if (!result.Succeeded)
        {
            ViewData[Constants.ErrorViewData] = result.Message;
        }

        return View("Edit", result.Data);
    }

    [HttpPost("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Employee employee)
    {
        var result = await this.employeeService
            .ModifyEmployeeAsync(employee);

        if (!result.Succeeded)
        {
            ViewData[Constants.ErrorViewData] = result.Message;
            return View("Edit");
        }

        return RedirectToAction("Index");
    }

    [HttpGet("delete/{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id)
    {
        var result = await this.employeeService
            .RemoveEmployeeAsync(id);

        if (!result.Succeeded)
        {
            TempData[Constants.ErrorViewData] = result.Message;
        }
        else
        {
            TempData[Constants.MessageViewData] = Constants.RecordRemovedMessage;
        }

        return RedirectToAction("Index");
    }
}