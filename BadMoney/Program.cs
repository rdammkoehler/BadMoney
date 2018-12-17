using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks.Sources;
using System.Transactions;

/*
 * The following code demonstrates how a floating point number rounding error can
 * impact arithmetic operations in C#. Note that the float data-type has a rounding
 * error immediately, where the double data-type gets through 3 iterations before
 * the error shows up. Clearly, repeated operations exacerbates the problem. After
 * 15 million iterations the calculations will be off by several significant digits.
 *
 * In the C# programming language, the decimal data-type is the recommended data-type
 * for currency operations. This code uses decimal as the baseline of correctness.
 * decimal is so large/wide that it is unreasonable to expect an issue in calculations
 * that we might commonly experience in non-scientific computing. The range of the
 * decimal data-type is ±1.0 x 10^-28 to ±7.9228 x 10^28. 
 * 
 * The Currency Class included here shows one technique of using a fixed point 
 * representation. Using an adjustment value of 1,000,000 extends the capability of
 * the Currency class to operate well beyond the limits of the double data-type.
 * A larger adjustment value will result in even greater range. However, parsing 
 * limitations in this implementation create an artificial upper and lower bound
 * for initialization. So, if you bump into those limits you may need to do some
 * math in order to get the value you'd like. Further work on the Currency class can
 * overcome those limitations.
 *   
 * What Every Computer Scientist Should Know About Floating-Point Arithmetic
 * https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html
 */
namespace BadMoney
{
    internal class Currency
    {
        private const decimal Adjustment = 1000000m;
        private static readonly BigInteger BiAdjustment = new BigInteger(Adjustment);
        private readonly BigInteger _value;

        public Currency(string val)
        {
            // because of decimal.Parse the limit here is the same as ulong (64-bits, unsigned) 
            // this could be adjusted by using a custom parser
            _value = new BigInteger((ulong) (decimal.Parse(val) * Adjustment));
        }

        private Currency(BigInteger val)
        {
            _value = val;
        }

        public static Currency operator +(Currency a, Currency b)
        {
            return new Currency(a._value + b._value);
        }

        public static Currency operator -(Currency a, Currency b)
        {
            return new Currency(a._value - b._value);
        }

        public static Currency operator *(Currency a, Currency b)
        {
            return new Currency(a._value * b._value / BiAdjustment);
        }

        public static Currency operator /(Currency a, Currency b)
        {
            return new Currency(a._value / b._value * BiAdjustment);
        }

        public override string ToString()
        {
            return $"{((decimal) (_value)) / Adjustment}";
        }
    }

    public class Program
    {
        private const string A = "1000.00";
        private const string B = "23.23";
        private const string C = "45.20";
        private const int Iterations = 15;

        public static void Main(string[] _)
        {
            const int width = 20;
            var floats = UsingFloat(); //        32-bits, -3.4 x 10^38 to + 3.4 x 10^38
            var doubles = UsingDouble(); //      64-bits, ±5.0 x 10^-324 to ±1.7 x 10^308
            var decimals = UsingDecimal(); //    128-bits, (±7.9 x 10^28) / 10^0 to 28
            var currencies = UsingCurrency(); //  technically no bounds*
                                    
            Console.WriteLine("formula: a + (b * b + c)");
            Console.WriteLine($"a = {A,7}");
            Console.WriteLine($"b = {B,7}");
            Console.WriteLine($"c = {C,7}");
            Console.WriteLine(
                $"{"decimal",width}{"currency",width}{"float",width}{"double",width}{"diff(C-m)",width}{"diff(f-m)",width}{"diff(d-m)",width}");
            for (var idx = 0; idx < Iterations; idx++)
            {
                var diffFm = "*****";
                try
                {
                    var attempt = decimal.Parse(floats[idx].ToString("R")) - decimals[idx];
                    diffFm = $"{attempt}";
                }
                catch (FormatException)
                {
                    //because float doesn't get big enough we will eventually have a failure here.
                    diffFm = "*****";
                }

                var diffDm = decimal.Parse(doubles[idx].ToString("R")) - decimals[idx];
                var diffCm = decimal.Parse(currencies[idx].ToString()) - decimals[idx];
                Console.WriteLine(
                    $"{decimals[idx],width}{currencies[idx],width}{floats[idx],width}{doubles[idx],width}{diffCm,width}{diffFm,width}{diffDm,width}");
            }
        }

        private static T[] DoMaths<T>(T a, T b, T c, Func<T, T, T, T> maths)
        {
            var values = new T[Iterations];

            for (var idx = 0; idx < Iterations; idx++)
            {
                a = maths(a, b, c);
                values[idx] = a;
            }

            return values;
        }

        private static decimal[] UsingDecimal()
        {
            return DoMaths(decimal.Parse(A), decimal.Parse(B), decimal.Parse(C), (a, b, c) => a + (b * b + c));
        }

        private static float[] UsingFloat()
        {
            return DoMaths(float.Parse(A), float.Parse(B), float.Parse(C), (a, b, c) => a + (b * b + c));
        }

        private static double[] UsingDouble()
        {
            return DoMaths(double.Parse(A), double.Parse(B), double.Parse(C), (a, b, c) => a + (b * b + c));
        }

        private static Currency[] UsingCurrency()
        {
            return DoMaths(new Currency(A), new Currency(B), new Currency(C), (a, b, c) => a + (b * b + c));
        }
    }
}