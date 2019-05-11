using System;
using System.Collections.Generic;
using Xunit;

namespace refactoring_debate.MyRefactor
{
    public static class DiscountCalculator
    {
        public enum AccountType { Unregistered, Simple, Valuable, MostValuable }

        static readonly Dictionary<AccountType, decimal> AccountKindDiscounts = new Dictionary<AccountType, decimal>
        {
            [AccountType.Unregistered] = 1m,
            [AccountType.Simple] = 0.9m,
            [AccountType.Valuable] = 0.7m,
            [AccountType.MostValuable] = 0.5m
        };
        
        static decimal LoyaltyDiscount(AccountType type, int years)
        {
            return type == AccountType.Unregistered
                ? 1m
                : 1m - Math.Min(years, 5) / 100m;
        }

        public static decimal Calculate(decimal price, AccountType type, int years)
        {
            var loyaltyDiscount = LoyaltyDiscount(type, years);

            return price * AccountKindDiscounts[type] * loyaltyDiscount;
        }
    }

    public class Tests
    {
        [Fact]
        public void TestTheThings()
        {
            Assert.Equal(100m, DiscountCalculator.Calculate(100m, DiscountCalculator.AccountType.Unregistered, 1));
            Assert.Equal(49.5m, DiscountCalculator.Calculate(100m, DiscountCalculator.AccountType.MostValuable, 1));
            Assert.Equal(66.5m, DiscountCalculator.Calculate(100m, DiscountCalculator.AccountType.Valuable, 6));
            Assert.Equal(89.1m, DiscountCalculator.Calculate(100m, DiscountCalculator.AccountType.Simple, 1));
        }
    }
}