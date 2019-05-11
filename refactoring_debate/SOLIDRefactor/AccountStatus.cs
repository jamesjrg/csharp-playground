using System;
using System.Collections.Generic;
using System.Linq;

namespace refactoring_debate.SOLIDRefactor
{
    public class AccountStatus: IEquatable<AccountStatus>
    {
        public AccountStatus(int type, string description)
        {
            Type = type;
            Description = description;
        }

        public readonly int Type;

        public readonly string Description;

        public static AccountStatus NotRegistered = new AccountStatus(1, "Not registered");
        public static AccountStatus SimpleCustomer = new AccountStatus(2, "Simple customer");
        public static AccountStatus ValuableCustomer = new AccountStatus(3, "Valuable customer");
        public static AccountStatus MostValuableCustomer = new AccountStatus(4, "Most valuable customer");

        public static IEnumerable<AccountStatus> List()
        {
            return new[] {NotRegistered, SimpleCustomer, ValuableCustomer, MostValuableCustomer};
        }

        public static AccountStatus FromString(string input)
        {
            return List().Single(r => String.Equals(r.Description, input, StringComparison.OrdinalIgnoreCase));
        }

        public static AccountStatus FromValue(int type)
        {
            return List().Single(r => r.Type == type);
        }
        
        public bool Equals(AccountStatus other)
        {
            return Type == other.Type && string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AccountStatus) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }
    }
}