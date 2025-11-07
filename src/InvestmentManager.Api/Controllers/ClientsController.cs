using Microsoft.AspNetCore.Mvc;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;
using InvestmentManager.Application.Common;

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
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            try
            {
                var client = await _clientService.GetByIdAsync(id);
                return Ok(client);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detail = ex.Message });
            }
        }

        [HttpPost("CreateClient")]
        public async Task<ActionResult> CreateClient([FromBody] Client client)
        {
            try
            {
                if (client == null)
                    return BadRequest(new { error = "Datos de cliente inválidos." });

                await _clientService.CreateAsync(client);

                return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detail = ex.Message });
            }
        }

        [HttpGet("GetAllClients")]
        public async Task<ActionResult<List<Client>>> GetAllClients()
        {
            try
            {
                var client = await _clientService.GetAllClients();
            
                return Ok(client);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado.", detail = ex.Message });
            }
        }
    }
}
