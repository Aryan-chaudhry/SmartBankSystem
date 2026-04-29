namespace SmartBank.Data.DTO
{
    public class AccountresponseDTO
    {
        public int AccountId {get; set;}

        public string AccountNumber {get; set;}

        public decimal Balance {get; set;}

        public string Status {get; set;}

        public string AccountType {get; set;}
    }
}