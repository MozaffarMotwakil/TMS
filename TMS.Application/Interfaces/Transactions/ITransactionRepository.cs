using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Application.DTOs.Transactions;
using TMS.Domain.Entities.Transactions;
using TMS.Domain.Enums.Transactions;

namespace TMS.Application.Interfaces.Transactions
{
    public interface ITransactionRepository
    {

        public Task<Transaction?> GetByIdAsync(int Id);

        public Task<IEnumerable<Transaction>> GetAllAsync();

        public Task<int?> DepositAsync(string AccountNumber, decimal Amount);

        public Task<int?> WithdrawAsync(string AccountNumber, decimal Amount);

        public Task<int?> TransferAsync(string FromAccountNumber, string ToAccountNumber, decimal Amount);

    }
}
