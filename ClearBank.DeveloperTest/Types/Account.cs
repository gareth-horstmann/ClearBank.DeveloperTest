namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        /// <summary>
        /// Debit the account balance.
        /// </summary>
        /// <param name="amount">The amount to credit the account with</param>
        /// <remarks>
        ///     This method encapsulates the behaviour of debiting the accounts balance to the Account model itself.
        /// </remarks>
        internal void Debit(decimal amount)
        {
            // Note that this call could reduce the balance to below zero.  The assumption is that the balance
            // might be allowed to reduce to below zero in certain cases, and these rules will be covered by the rules
            // defined in the validators.  So, this method is not checking whether the amount is greater than the
            // balance and throwing an exception becuase this might be a valid scenario.  As these rules were not
            // part of the original code, and this is a refactoring exercise, it has also been left out here.
            Balance -= amount;
        }
    }
}
