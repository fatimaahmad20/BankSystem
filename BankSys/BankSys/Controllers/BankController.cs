using BankSys.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankSys.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        ApplicationDbContext myDB = new ApplicationDbContext();
        private decimal balance;

        // GET: bank
        public ActionResult Home()
        {

            string username = User.Identity.GetUserName();
            var user = myDB.Users.FirstOrDefault(u => u.UserName == username);

            return View(user);
        }

        [HttpGet]
        public ActionResult Withdrawl(string Id)
        {
            var user = myDB.Users.FirstOrDefault(u => u.Id == Id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmWithdrawl(string Id, int withdrawlAmount)
        {
            var user = myDB.Users.FirstOrDefault(u => u.Id == Id);

            if (user.Balance > withdrawlAmount)
            {
                Detials dt = new Detials()
                {
                    userId = Id,
                    amount = withdrawlAmount,
                    action = "withdrawl",
                    date = DateTime.Now,
                };
                myDB.Detials.Add(dt);
                user.Balance -= withdrawlAmount;
                myDB.SaveChanges();
                return RedirectToAction("Home");

            }
            else if (user.Balance < withdrawlAmount)
            {
                throw new ApplicationException("insufficient funds");
            }
            else if (withdrawlAmount <= 0)
            {
                throw new ApplicationException("invalid withdrawl amount");
            }
            else return RedirectToAction("Home");


        }


        public ActionResult Withdrawl100(string Id)
        {
            var user = myDB.Users.FirstOrDefault(u => u.Id == Id);
            if (user.Balance > 100)
            {
                Detials dt = new Detials()
                {
                    userId = Id,
                    amount = 100,
                    action = "quick draw",
                    date = DateTime.Now,
                };
                myDB.Detials.Add(dt);
                user.Balance -= 100;
                myDB.SaveChanges();
                return RedirectToAction("Home");

            }
            if (balance < 100)
            {
                throw new ApplicationException("insufficient funds");
            }

            return RedirectToAction("Home");
        }




        public ActionResult BalanceInquery(string Id)
        {

            var user = myDB.Users.FirstOrDefault(i => i.Id == Id);

            if (user == null)
            {
                throw new ApplicationException("no account exists with that id");
            }

            return View(user);
        }





        [HttpGet]
        public ActionResult Deposit(string Id)
        {
            var user = myDB.Users.FirstOrDefault(i => i.Id == Id);
            return View(user);
        }
        [HttpPost]
        public ActionResult confirmDeposit(string Id, int amount)
        {
            var user = myDB.Users.FirstOrDefault(i => i.Id == Id);
            if (amount <= 0)
            {
                throw new ApplicationException("invalid deposit amount");
            }

            user.Balance += amount;
            myDB.SaveChanges();
            return RedirectToAction("Home");
        }

        public ActionResult Detials(string Id)
        {
            var detials = myDB.Detials.Where(i => i.userId == Id).ToList();
            return View(detials);
        }

        [HttpGet]
        public ActionResult Transfer(string Id)
        {
            var user = myDB.Users.FirstOrDefault(i => i.Id == Id);
            return View(user);
        }

        [HttpPost]
        public ActionResult TransferFunds(string fromAccountId, string toAccountId, int transferAmount)
        {
            var user = myDB.Users.FirstOrDefault(i => i.UserName == fromAccountId);
            var reciver = myDB.Users.FirstOrDefault(i => i.UserName == toAccountId);

            if (reciver == null)
            {
                throw new ApplicationException("reciver not found");
                return RedirectToAction("transferfunds", new { Id = fromAccountId });
            }
            if (transferAmount <= 0)
            {
                throw new ApplicationException("transfer amount must be positive");
            }
            else if (transferAmount == 0)
            {
                throw new ApplicationException("invalid transfer amount");
            }


            if (user.Balance < transferAmount)
            {
                throw new ApplicationException("insufficient funds");
                return RedirectToAction("transferfunds", new { Id = fromAccountId });
            }
            else
            {
                user.Balance -= transferAmount;
                reciver.Balance += transferAmount;
                myDB.SaveChanges();
            }

            return RedirectToAction("Home");
        }

    }

}