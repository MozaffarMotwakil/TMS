namespace TMS.API.Controllers
{
    public class DepositTransactionDTO
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal AmountToDeposit { get; set; }
    }
}
