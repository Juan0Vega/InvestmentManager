using Microsoft.AspNetCore.Mvc;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(string id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
                return NotFound($"No se encontró el cliente con ID {id}");

            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest("Datos de cliente inválidos.");

            await _clientService.CreateAsync(client);
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }
    }
}
