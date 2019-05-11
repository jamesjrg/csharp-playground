//file AccountStatus.cs:

namespace refactoring_debate.SOLIDRefactor
{
  public class MostValuableCustomerDiscountCalculator : IAccountDiscountCalculator
  {
    public decimal ApplyDiscount(decimal price)
    {
      return price - (Constants.DISCOUNT_FOR_MOST_VALUABLE_CUSTOMERS * price);
    }
  }
}