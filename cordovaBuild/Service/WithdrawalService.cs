using cordovaBuild.Common;
using cordovaBuild.Data.Model;
using cordovaBuild.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Service
{
    public class WithdrawalService
    {

        public async Task<bool> Withdraw(decimal amountToWithdraw, string accountNumber, string user)
        {
            var accountBalanceRepo = new AccountStatementRepository();
            var currentBalance = await accountBalanceRepo.GetCurrentBalance(user);

            var returnValue = false;
            
            var withdrawalRepo = new WithdrawalRepository();

            var charges = Decimal.Multiply(amountToWithdraw, (decimal)0.10);
            var actualAmount = amountToWithdraw - (charges);

            currentBalance = currentBalance - charges;
            returnValue = await accountBalanceRepo.CreateSync(new AccountStatement
            {
                User = user,
                Debit = charges,
                CurrentBalance = currentBalance,
                TransactionDate = Helper.SetDateForMongo(DateTime.Now),
                Description = "cordovaBuild transaction fee"
            });

            if (returnValue)
            {
                currentBalance = currentBalance - actualAmount;
                await accountBalanceRepo.CreateSync(new AccountStatement
                {
                    User = user,
                    Debit = actualAmount,
                    CurrentBalance = currentBalance,
                    TransactionDate = Helper.SetDateForMongo(DateTime.Now),
                    Description = "Actual withdrawn amount"
                });
            }
            else
            {
                return returnValue;
            }

            if (returnValue)
            {
                await withdrawalRepo.CreateSync(new Withdrawal
                {
                    User = user,
                    AccountNumber = accountNumber,
                    AmountToWithdraw = amountToWithdraw,
                    ActualAmount = actualAmount,
                    DateWithdrawal = Helper.SetDateForMongo(DateTime.Now)
                });
            }
            else
            {
                return returnValue;
            }

            return false;
        }
    }
}
