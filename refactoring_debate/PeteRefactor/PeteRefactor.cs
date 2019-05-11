using System;
using System.Collections.Generic;

namespace refactoring_debate.PeteRefactor
{
    delegate decimal Discount(decimal amount, int yearsAccountHeld);

    public static class Discounts
    {
        public static decimal NotRegistered(decimal amount, int yearsAccountHeld) => amount;

        public static decimal SimpleCustomer(decimal amount, int yearsAccountHeld) => 
            (amount * 0.9m) * LoyaltyDiscount(yearsAccountHeld);

        public static decimal ValuableCustomer(decimal amount, int yearsAccountHeld) =>
            (amount * 0.7m) * LoyaltyDiscount(yearsAccountHeld);

        public static decimal MostValuableCustomer(decimal amount, int yearsAccountHeld) =>
            (amount * 0.5m) * LoyaltyDiscount(yearsAccountHeld);

        private static decimal LoyaltyDiscount(int yearsAccountHeld) => 1 - (Math.Min(yearsAccountHeld, 5) / 100);
    }

    public class DiscountCalculator
    {   
        private Dictionary<int, Discount> applyDiscountFor = new Dictionary<int, Discount>()
        {
            { 0, Discounts.NotRegistered },
            { 1, Discounts.SimpleCustomer },
            { 2, Discounts.ValuableCustomer },
            { 3, Discounts.MostValuableCustomer }
        };

        public decimal Calculate(decimal amount, int accountType, int yearsAccountHeld) =>
            this.applyDiscountFor[accountType](amount, yearsAccountHeld);
    }
}