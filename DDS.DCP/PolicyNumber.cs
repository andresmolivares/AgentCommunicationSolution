#nullable enable

using System;

namespace DDS.DCP
{
    /// <summary>
    /// Represents a class type for a policy number
    /// </summary>
    public class PolicyNumber
    {
        protected PolicyNumber() { }

        protected PolicyNumber(string value)
        {
            Unformatted = value;
            var policyNumber = Parse(value);
            Prefix = policyNumber.Prefix;
            Suffix = policyNumber.Suffix;
            Term = policyNumber.Term;
        }

        public string? Unformatted { get; private set; }

        public string? Prefix { get; private set; }

        public string? Suffix { get; private set; }

        public string? Term { get; set; }

        public static PolicyNumber Parse(string value)
        {
            var policyPrefix = string.Empty;
            var policySuffix = string.Empty;
            var policyTerm = string.Empty;

            ///<remarks>
            /// When value length is 12 or greater, use first 12 character to create PolicyNumber
            /// </remarks>
            if (value.Length >= 12)
            {
                var fullValue = value.Substring(0, 12);
                policyPrefix = fullValue.Substring(0, 3);
                policySuffix = fullValue.Substring(3, 7);
                policyTerm = fullValue.Substring(10, 2);

                return new PolicyNumber
                {
                    Prefix = policyPrefix,
                    Suffix = policySuffix,
                    Term = policyTerm,
                    Unformatted = value
                };
            }
            ///<remarks>
            /// When length less than 12, build suffix and prefix
            /// </remarks>
            for (var i = 0; i < value.Length; i++)
            {
                var x = value[i];
                // Collect letter or white space characters for policy prefix
                if (i < 3 && (char.IsLetter(x) || char.IsWhiteSpace(x)))
                    policyPrefix += x;
                // Collect digit or white space characters for policy suffix
                if (i >= 3 || char.IsDigit(x))
                    policySuffix += x;
            }
            if (string.IsNullOrWhiteSpace(policyPrefix))
                throw new ArgumentException($"Invalid policy number input value: {value}");
            // Build policy prefix
            policyPrefix = policyPrefix.PadRight(3);
            // Build length based suffix
            if (policySuffix.Length <= 7)
            {
                // when the policy number is shor, no term number is applied
                policySuffix = policySuffix.PadLeft(7);
            }
            else
            {
                // when policy number is long, term number is remaining characters
                policyTerm = policySuffix.Substring(7, 2);
                policySuffix = policySuffix.Substring(0, 7);
            }
            // Return PolicyNumber instance
            return new PolicyNumber
            {
                Prefix = policyPrefix,
                Suffix = policySuffix,
                Term = policyTerm,
                Unformatted = value
            };
        }

        public override string ToString()
        {
            return $"{Prefix}{Suffix}{Term}";
        }

        public string ToStringWithoutTerm()
        {
            return $"{Prefix}{Suffix}";
        }
    }
}
