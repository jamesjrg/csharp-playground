using System.Collections.Generic;
using Xunit;
using SuccincT.PatternMatchers.GeneralMatcher;

namespace refactoring_debate.ArnoRefactor
{
    public class DiscountTestsV2
    {
        public enum CustomerType { Unregistered, Simple, Valuable, MostValuable }

        public class Customer
        {
            public CustomerType CustomerType { get; }
            public int Years { get; }

            public Customer(CustomerType customerType, int years)
            {
                CustomerType = customerType;
                Years = customerType == CustomerType.Unregistered ? 0 : years;
            }
        }

        private static Dictionary<CustomerType, int> CustomerDiscountMap =
            new Dictionary<CustomerType, int>
            {
                [CustomerType.Unregistered] = 0,
                [CustomerType.Simple] = 1,
                [CustomerType.Valuable] = 3,
                [CustomerType.MostValuable] = 5
            };

        private static int YearsDiscount(int years) =>
            years.Match().To<int>()
                 .Where(y => y > 5).Do(5)
                 .Else(y => y)
                 .Result();

        private static (int typeDiscount, int yearsDiscount) AccountDiscount(Customer customer) =>
            customer.Match().To<(int, int)>()
                    .Where(c => c.CustomerType == CustomerType.Unregistered).Do((0, 0))
                    .Else(c => (CustomerDiscountMap[c.CustomerType], YearsDiscount(c.Years)))
                    .Result();

        private static decimal ApplyDiscount(decimal price, decimal discount) =>
            price - price * discount / 100.0m;

        private static decimal ReducePriceBy((int type, int years) discount, decimal price) =>
            ApplyDiscount(ApplyDiscount(price, discount.type), discount.years);

        public static decimal CalculateDiscountPrice(Customer account, decimal price) =>
            ReducePriceBy(AccountDiscount(account), price);

        [Fact]
        public void Tests()
        {
            Assert.Equal(94.05000m, CalculateDiscountPrice(new Customer(CustomerType.MostValuable, 1), 100.0m));
            Assert.Equal(92.15000m, CalculateDiscountPrice(new Customer(CustomerType.Valuable, 6), 100.0m));
            Assert.Equal(98.01000m, CalculateDiscountPrice(new Customer(CustomerType.Simple, 1), 100.0m));
            Assert.Equal(100.0m, CalculateDiscountPrice(new Customer(CustomerType.Unregistered, 0), 100.0m));
        }
    }
}
