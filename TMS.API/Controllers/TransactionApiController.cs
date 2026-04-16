using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.People;
using TMS.Application.DTOs.Transactions;
using TMS.Application.Interfaces.Transactions;
using TMS.Application.Services.Transactions;
using TMS.Domain.Entities.Transactions;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionApiController : ControllerBase
    {
        private readonly ITransactionService _TransactionService;

        [HttpGet("GetTransactionById", Name = "GetTransactionById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(int id)
        {
            if (id < 0)
            {
                return BadRequest($"المعرف {id} خاطئ");
            }
            var TransactionDTO = await _TransactionService.GetByIdAsync(id);

           return TransactionDTO is null
                ? NotFound("لم يتم العثور على العملية")
                : Ok(TransactionDTO);

        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {

            var result = await _TransactionService.GetAllAsync();

            return Ok(result);

        }

        [HttpPost("Deposit")]
        public async Task<ActionResult<TransactionDTO>> AddDeposit(DepositTransactionDTO dto)
        {
            
           bool result = await _TransactionService.DepositAsync(dto.AccountNumber, dto.AmountToDeposit);
        }


    }
}
