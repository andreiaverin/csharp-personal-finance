using System;

namespace PersonalFinanceDemo
{
    public class Functions
    {
        /// <summary>
        /// Effective Interest Rate
        /// </summary>
        /// <param name="rate">Nominal interest rate</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>
        public static double EFFECT(double rate, int compPeriods)
        {
            return Math.Pow((1 + rate / compPeriods), compPeriods) - 1;
        }

        /// <summary>
        /// Nominal Interest Rate
        /// </summary>
        /// <param name="rate">Effective interest rate</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>        
        public static double NOMINAL(double rate, int compPeriods)
        {
            return compPeriods * (Math.Pow(rate + 1, 1 / (double)compPeriods) - 1);
        }

        /// <summary>
        /// Future value of investment
        /// </summary>
        /// <param name="initial">Initial capital</param>
        /// <param name="rate">Effective interest rate (annual)</param>
        /// <param name="periods">Total number of periods</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>
        /// <param name="payment">Amount of the constant periodic payment</param>
        public static double FV(double initial, double rate, int periods, int compPeriods, double payment)
        {
            // Calculate the nominal interest rate
            double nominal = NOMINAL(rate / 100, compPeriods);

            // Re-calculate nominal rate with regard to the compounding periods
            double compRate = nominal / compPeriods;

            // Calculate first part
            double firstPart = initial * Math.Pow(1 + compRate, periods);

            // May not divide by 0
            if (compRate == 0) return 0;

            // Get the future value
            double result = firstPart + payment * (Math.Pow((1 + compRate), periods) - 1) / compRate;

            // Format the output
            return Math.Round(Math.Abs(result), 4);
        }

        /// <summary>
        /// Present value of investment (initial capital)
        /// </summary>
        /// <param name="futureValue">Initial capital</param>
        /// <param name="rate">Effective interest rate</param>
        /// <param name="periods">Total number of periods</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>
        /// <param name="payment">Amount of the constant periodic payment</param>
        public static double PV(double futureValue, double rate, int periods, int compPeriods, double payment)
        {
            // Calculate the nominal interest rate
            double nominal = NOMINAL(rate / 100, compPeriods);

            // Re-calculate nominal rate with regard to the compounding periods
            double compRate = nominal / compPeriods;

            // Get the future value
            double result = (futureValue - Math.Abs(payment) * (Math.Pow((1 + compRate), periods) - 1) / compRate) / Math.Pow(1 + compRate, periods);

            // Format the output (negative)
            return -1 * Math.Round(Math.Abs(result), 4);
        }

        /// <summary>
        /// Constant periodic payment
        /// </summary>
        /// <param name="presentValue">Initial capital</param>
        /// <param name="futureValue">Future value of the investment</param>
        /// <param name="rate">Interest rate</param>
        /// <param name="periods">Total number of periods</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>
        public static double PMT(double presentValue, double futureValue, double rate, int periods, int compPeriods)
        {
            // Calculate the nominal interest rate
            double nominal = NOMINAL(rate / 100, compPeriods);

            // Re-calculate nominal rate with regard to the compounding periods
            double compRate = nominal / compPeriods;

            // Calculate first part
            double firstPart = Math.Abs(presentValue) * Math.Pow(1 + compRate, periods);

            // Get the periodic payments
            double result = (futureValue - firstPart) / ((Math.Pow((1 + compRate), periods) - 1) / compRate);

            // Format the output
            return -1 * Math.Round(Math.Abs(result), 4);
        }

        /// <summary>
        /// Interest rate
        /// </summary>
        /// <param name="initial">Initial capital</param>
        /// <param name="futureValue">Future value of the investment</param>
        /// <param name="periods">Total number of periods</param>
        /// <param name="compPeriods">Number of compounding periods per year</param>
        /// <param name="payment">Amount of the constant periodic payment</param>
        public static double INTRATE(double initial, double futureValue, int periods, int compPeriods, double payment)
        {
            // Instantiate the custom algorithm
            var seeker = new InterestRateSeeker(initial, periods, compPeriods, payment);

            // Declare the goal seek
            var goalSeeker = new TridentGoalSeek.GoalSeek(seeker);

            // Seek for the expected future value
            var result = goalSeeker.SeekResult(Convert.ToDecimal(futureValue));

            // Use the found input variable
            return Math.Round(Convert.ToDouble(result.InputVariable), 2);
        }

        /// <summary>
        /// Used to find the Interest Rate using the Goal Seek algorithm
        /// </summary>
        class InterestRateSeeker : TridentGoalSeek.IGoalSeekAlgorithm
        {
            public double PresentValue { get; set; }
            public int Periods { get; set; }
            public int CompPeriods { get; set; }
            public double Payment { get; set; }

            public InterestRateSeeker(double presentValue, int periods, int compPeriods, double payment)
            {
                PresentValue = presentValue;
                Periods = periods;
                CompPeriods = compPeriods;
                Payment = payment;
            }

            public decimal Calculate(decimal rate)
            {
                return Convert.ToDecimal(FV(PresentValue, Convert.ToDouble(rate), Periods, CompPeriods, Payment));
            }
        }
    }
}
