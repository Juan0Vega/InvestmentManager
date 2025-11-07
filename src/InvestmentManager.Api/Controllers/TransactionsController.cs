using Microsoft.AspNetCore.Mvc;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;
using InvestmentManager.Application.DTOs;
using InvestmentManager.Application.Common;

namespace InvestmentManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransationsByIdAsync(id);
            if (transaction == null)
                return NotFound($"No se encontró la transacción con ID {id}");

            return Ok(transaction);
        }

        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                var id = await _transactionService.CreateAsync(transaction);
                return Ok(new { Message = "Transacción creada con éxito", TransactionId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("CancelSubscription")]
        public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionDTO dto)
        {
            try
            {
                var result = await _transactionService.CancelSubscription(dto);
                return Ok(new { message = result });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}
