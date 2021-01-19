using System;
using ValidationCore;

namespace ProgramForSumExponentialNumbers
{
    class Program
    {
        private const int _sizeOfMantissa = 39;

        static void Main(string[] args)
        {
            Console.WriteLine("Input new number in exponential form. Example of correct input (1,54e+5). " +
            $"Size of mantissa should be no more than {_sizeOfMantissa} characters");
            Console.WriteLine("If you want to see sum of added numbers press \"=\"");

            ExponentialNumber sum = new ExponentialNumber();

            while (true)
            {

                string input = Console.ReadLine().Trim();
                if (input.Contains("="))
                {
                    break;
                }

                string mantissa;
                short order;

                try
                {
                    Validation.ParseString(input, out mantissa, out order);
                }
                catch (Exception x)
                {
                    Console.WriteLine(x.Message);
                    continue;
                }

                ExponentialNumber custNumber = ExponentialNumber.ExponentialNumberCreate(input);

                sum += custNumber;

            }

            Console.WriteLine(sum.ToString());

            Console.WriteLine("Press any key to continue:");
            Console.ReadKey();

        }
    }
}

