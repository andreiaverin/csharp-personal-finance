using System;
using System.Threading;

namespace PersonalFinanceDemo
{
    class Program
    {

        private const string _welcomeString = "Welcome to Personal Financial Planner!\n";
        private const string _optionsString =
            "Please, select one of the options below:\n" +
            "0: Exit the program.\n" +
            "1: Calculate the effective interest rate of the investment.\n" +
            "2: Calculate the future value of the investment.\n" +
            "3: Calculate the constant periodic payment.\n" +
            "4: Calculate the initial capital required for the investment.\n";

        private const string _optionsNextString = "Please, select one of the options (0-4): ";

        // Command codes
        private const string _exitCode = "0";
        private const string _calculateInterestRate = "1";
        private const string _calculateFutureValue = "2";
        private const string _calculatePayment = "3";
        private const string _calculatePresentValue = "4";

        private const string _interestRateIs = "Required interest rate is: ";
        private const string _presentValueIs = "Required present value is: ";
        private const string _futureValueWillBe = "Future value will be: ";
        private const string _paymentsWillBe = "Required periodic payments will be: ";

        private const string _enterPresentValue = "Enter the present value of the invesment: ";
        private const string _enterFutureValue = "Enter the future value of the investment: ";
        private const string _enterPeriods = "Enter the total number of periods: ";
        private const string _enterCompPeriods = "Enter the number of compounding periods per year: ";
        private const string _enterPayment = "Enter the amount of periodic payments: ";
        private const string _enterEffectiveRate = "Enter the annual effective interest rate (%): ";

        private const int _defaultPresentValue = -20000;
        private const int _defaultCompundingPeriods = 12;
        private const int _defaultTotalPeriods = 120;
        private const int _defaultPayment = -100;

        static void Main(string[] args)
        {
            // Get the welcome message
            string welcome = _welcomeString + _optionsString;

            // Show the welcome message to user
            Console.Write(welcome);

            // User action code
            string code = string.Empty;

            // Wait for the code from user
            while (code != _exitCode)
            {
                // Read the user input
                code = Console.ReadLine();

                try
                {
                    // Handle the code
                    switch (code)
                    {
                        case _calculateInterestRate:
                            {
                                double presentValue = AskDouble(_enterPresentValue, _defaultPresentValue);
                                double futureValue = AskDouble(_enterFutureValue);
                                int periods = AskInteger(_enterPeriods, _defaultTotalPeriods);
                                int compPeriods = AskInteger(_enterCompPeriods, _defaultCompundingPeriods);
                                double payment = AskDouble(_enterPayment, _defaultPayment);

                                double rate = Functions.INTRATE(presentValue, futureValue, periods, compPeriods, payment);
                                Console.WriteLine(_interestRateIs + rate.ToString() + "%");

                                break;
                            }
                        case _calculateFutureValue:
                            {
                                double presentValue = AskDouble(_enterPresentValue, _defaultPresentValue);
                                double rate = AskDouble(_enterEffectiveRate);
                                int periods = AskInteger(_enterPeriods, _defaultTotalPeriods);
                                int compPeriods = AskInteger(_enterCompPeriods, _defaultCompundingPeriods);
                                double payment = AskDouble(_enterPayment, _defaultPayment);

                                double futureValue = Functions.FV(presentValue, rate, periods, compPeriods, payment);
                                Console.WriteLine(_futureValueWillBe + futureValue.ToString());

                                break;
                            }
                        case _calculatePayment:
                            {
                                double presentValue = AskDouble(_enterPresentValue, _defaultPresentValue);
                                double futureValue = AskDouble(_enterFutureValue);
                                double rate = AskDouble(_enterEffectiveRate);
                                int periods = AskInteger(_enterPeriods, _defaultTotalPeriods);
                                int compPeriods = AskInteger(_enterCompPeriods, _defaultCompundingPeriods);

                                double payment = Math.Round(Functions.PMT(presentValue, futureValue, rate, periods, compPeriods), 4);
                                Console.WriteLine(_paymentsWillBe + payment.ToString());

                                break;
                            }
                        case _calculatePresentValue:
                            {
                                double futureValue = AskDouble(_enterFutureValue);
                                double rate = AskDouble(_enterEffectiveRate);
                                int periods = AskInteger(_enterPeriods, _defaultTotalPeriods);
                                int compPeriods = AskInteger(_enterCompPeriods, _defaultCompundingPeriods);
                                double payment = AskDouble(_enterPayment, _defaultPayment);

                                double presentValue = Functions.PV(futureValue, rate, periods, compPeriods, payment);
                                Console.WriteLine(_presentValueIs + presentValue.ToString());

                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    // Display the exception text
                    Console.WriteLine(ex.Message);
                }

                // Line-break for better readability
                Console.WriteLine();

                // Display the options again
                Console.Write(_optionsNextString);
            }
        }

        private static double AskDouble(string message, double defaultValue = 0)
        {
            Console.Write(message);
            string separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string sendKey = (defaultValue == 0) ? "0" + separator + "0" : defaultValue.ToString();
            System.Windows.Forms.SendKeys.SendWait(sendKey);
            return double.Parse(Console.ReadLine());            
        }

        private static int AskInteger(string message, int defaultValue)
        {
            Console.Write(message);
            System.Windows.Forms.SendKeys.SendWait(defaultValue.ToString());
            return int.Parse(Console.ReadLine());
        }
    }
}
