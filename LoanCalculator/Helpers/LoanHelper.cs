using LoanCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {

            //Calculate monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            //Create Loop from 1 to term
            for (int i = 1; i <= loan.Term; i++)
            {
                //Calculate LoanPayment schedule
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                //Assigns new LoanPyament object with updated values
                LoanPayment loanPayment = new();
                loanPayment.Month = i;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //push payments into loan
                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            //return loan
            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            var monthlyRate = CalcMonthlyRate(rate);

            //converting decimals to double
            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));


            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}
