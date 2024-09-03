// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis#preconditions-allownull-and-disallownull

using System.Diagnostics.CodeAnalysis;

// Pell.App.AddressExample();

namespace Pell
{
    public class AddressInfo
    {
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }

        public required int PostalCode { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }

        [MemberNotNull(nameof(City), nameof(State))]
        public void SetPostalInfo(string city, string state)
        {
            this.City = city;
            this.State = state;
        }
    }

    public class NullabilityOptions
    {
        public required string IsRequired { get; set; }
        public string? IsNullable { get; set; }
        public string PinkiePromiseIsNeverNull { get; set; } = null!;
    }

    public static class App
    {
        public static class NullabilityMethods
        {
            public static void NotNull([NotNull] string? input)
            {
                ArgumentNullException.ThrowIfNullOrEmpty(input);
                // continue to process with non-null input variable
            }

            public static bool NotNullWhen([NotNullWhen(true)] string? input)
            {
                return !string.IsNullOrEmpty(input);
            }

            public static void MaybeNull([MaybeNull] string input)
            {
                input = null;
            }

            public static bool MaybeNullWhen([MaybeNullWhen(false)] string input)
            {
                return false;
            }

            [return: NotNullIfNotNull(nameof(input))]
            public static string? NotNullIfNotNull(string? input)
            {
                return input;
            }
        }

        private static void LogIsNull(string propertyName, IConvertible? value)
        {
            Console.WriteLine($"{propertyName} {(value == null ? "is null" : "is NOT null")}");
        }

        private static void LogIsNull(AddressInfo address)
        {
            LogIsNull(nameof(address.AddressLine1), address.AddressLine1);
            LogIsNull(nameof(address.AddressLine2), address.AddressLine2);
            LogIsNull(nameof(address.City), address.City);
            LogIsNull(nameof(address.State), address.State);
            LogIsNull(nameof(address.PostalCode), address.PostalCode);
        }

        public static void AddressExample()
        {
            var address = new AddressInfo
            {
                AddressLine1 = "123 Main",
                PostalCode = 90210
            };

            LogIsNull(address);

            // address.City "may be null here"

            address.SetPostalInfo("Beverly Hills", "California");

            // address.City "is not null here"
        }

        public static void StaticAnalyzerExample1()
        {

            string? input = null;

            // input may be null here

            NullabilityMethods.NotNull(input);

            // input is not null here

            string output = input;
        }

        public static void StaticAnalyzerExample2()
        {

            string? input = null;

            // input may be null here

            if (NullabilityMethods.NotNullWhen(input))
            {
                // input is not null here
                string output = input;
            }
            else
            {
                // input may be null here
                string? output = input;
            }
        }

        public static void StaticAnalyzerExample3()
        {

            string input = "Test";

            // input is not null here

            if (NullabilityMethods.MaybeNullWhen(input))
            {
                // input is not null here
                string output = input;
            }
            else
            {
                // input may be null here
                string? output = input;
            }
        }

        public static void StaticAnalyzerExample4()
        {

            string input = "Test";

            // input is not null here

            NullabilityMethods.MaybeNull(input);

            // input may be null here

            string? output = input;
        }

        public static void StaticAnalyzerExample5()
        {

            string inputNotNull = "Test";

            // inputNotNull is not null here

            NullabilityMethods.NotNullIfNotNull(inputNotNull);

            // inputNotNull may be null here

            string outputNotNull = inputNotNull;

            // with a null value

            string? inputIsNull = null;

            // inputIsNull may be null here

            NullabilityMethods.NotNullIfNotNull(inputIsNull);

            // inputIsNull may be null here

            string? outputIsNull = inputIsNull;

        }

        public static void NotNullIfNotNullGotcha(string? input)
        {
            NullabilityMethods.NotNullIfNotNull(input);

            // input may be null here
        }

    }

}

