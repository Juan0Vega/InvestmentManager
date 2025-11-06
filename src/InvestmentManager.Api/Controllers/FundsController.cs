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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fund>>> GetAllFunds()
        {
            var funds = await _fundService.GetAllAsync();
            return Ok(funds);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fund>> GetFundById(string id)
        {
            var fund = await _fundService.GetByIdAsync(id);
            if (fund == null)
                return NotFound($"No se encontró el fondo con ID {id}");

            return Ok(fund);
        }
    }
}
