using Microsoft.AspNetCore.Mvc;
using PrimeiraAPI.Model;
using PrimeiraAPI.ViewModel;

namespace PrimeiraAPI.Controllers;

[ApiController]
[Route("api/v1/employee")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException();
    }

    [HttpPost]
    public IActionResult Add([FromForm] EmployeeViewModel employeeViewModel)
    {
        var filePath = Path.Combine("Storage", employeeViewModel.Photo.FileName);

        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        employeeViewModel.Photo.CopyTo(fileStream);
        
        var employee = new Employee(employeeViewModel.Name, employeeViewModel.Age, filePath);
        
        _employeeRepository.Add(employee);

        return Ok();
    }

    [HttpGet]
    [Route("{id}/download")]
    public IActionResult DownloadPhoto(int id)
    {
        var employee = _employeeRepository.Get(id);

        var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

        return File(dataBytes, "image/png");
    }

    [HttpGet]
    public IActionResult Get()
    {
        var employess = _employeeRepository.Get();

        return Ok(employess);
    }
}