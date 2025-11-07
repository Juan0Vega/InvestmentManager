using Microsoft.AspNetCore.Mvc;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundsController : ControllerBase
    {
        private readonly IFundService _fundService;

        public FundsController(IFundService fundService)
        {
            _fundService = fundService;
        }

        [HttpGet("GetAllFunds")]
        public async Task<ActionResult<IEnumerable<Fund>>> GetAllFunds()
        {
            try
            {
                var funds = await _fundService.GetAllAsync();
                return Ok(funds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener los fondos: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fund>> GetFundById(int id)
        {
            try
            {
                var fund = await _fundService.GetByIdAsync(id);
                if (fund == null)
                    return NotFound($"No se encontró el fondo con ID {id}");

                return Ok(fund);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al buscar el fondo: {ex.Message}" });
            }
        }

        [HttpPost("CreateFund")]
        public async Task<ActionResult> CreateFund([FromBody] Fund fund)
        {
            try
            {
                if (fund == null)
                    return BadRequest("Datos de fondo inválidos.");

                var result = await _fundService.CreateFundAsync(fund);
                return CreatedAtAction(nameof(GetFundById), new { id = result.FundId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al crear el fondo: {ex.Message}" });
            }
        }
    }
}
