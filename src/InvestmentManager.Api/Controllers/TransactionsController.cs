using Microsoft.AspNetCore.Mvc;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

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
        public async Task<ActionResult<Transaction>> GetTransactionById(string id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction == null)
                return NotFound($"No se encontró la transacción con ID {id}");

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Datos de transacción inválidos.");

            await _transactionService.CreateAsync(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.TransactionId }, transaction);
        }
    }
}
