using DBL.Entities;
using DBL.Models;
using DBL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customer = new Customers
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Segment = dto.Segment
            };
            var created = await _repository.Create(customer);
            return Ok(created);
        }
    }
}
