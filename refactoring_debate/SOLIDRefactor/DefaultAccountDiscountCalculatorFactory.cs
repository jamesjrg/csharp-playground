using System;

namespace refactoring_debate.SOLIDRefactor
{
    public class DefaultAccountDiscountCalculatorFactory : IAccountDiscountCalculatorFactory
    {
        public IAccountDiscountCalculator GetAccountDiscountCalculator(AccountStatus accountStatus)
        {
            IAccountDiscountCalculator calculator;

            if (accountStatus.Type == AccountStatus.NotRegistered.Type)
            {
                calculator = new NotRegisteredDiscountCalculator();
            }
            else if (accountStatus.Type == AccountStatus.SimpleCustomer.Type)
            {
                calculator = new SimpleCustomerDiscountCalculator();
            }
            else if (accountStatus.Type == AccountStatus.ValuableCustomer.Type)
            {
                calculator = new ValuableCustomerDiscountCalculator();
            }
            else if (accountStatus.Type == AccountStatus.MostValuableCustomer.Type)
            {
                calculator = new MostValuableCustomerDiscountCalculator();
            }
            else
            {
                throw new NotImplementedException();
            }

            return calculator;
        }
    }
}