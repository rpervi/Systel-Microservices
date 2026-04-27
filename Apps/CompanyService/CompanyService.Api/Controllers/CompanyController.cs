using CompanyService.Application.Interfaces;
using CompanyService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CompanyService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        // The API layer asks for the Interface, not the Implementation
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return StatusCode(companies.StatusCode, companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            return StatusCode(company.StatusCode, company); 
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] companymaster company)
        {
            if (company == null) return BadRequest();

            var result = await _companyService.CreateCompanyAsync(company);
            
            return StatusCode(result.StatusCode, result);
        }
    }
}
