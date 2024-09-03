using System;
using System.Windows.Forms;

namespace Events
{
    internal class MainAccount : BankAccount
    {
        public BankAccount SavingAccount { get; set; }

        public MainAccount(decimal balance) : base(balance)
        {

        }

        public override void WithDraw(decimal amount)
        {
            if (Balance + SavingAccount.Balance >= amount)
            {
                if (amount > Balance)
                {
                    decimal sub = amount - Balance;
                    Balance = 0;
                    SavingAccount.Balance -= sub;
                }
                else
                {
                    Balance -= amount;
                }
            }
            else
            {
                base.OnOverdrawnEventHandler(amount - Balance - SavingAccount.Balance);
            }
        }
    }

    internal class SavingAccount : BankAccount
    {
        public SavingAccount(decimal balance) : base(balance)
        {
        }
        public override void WithDraw(decimal amount)
        {
            if (this.Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                base.OnOverdrawnEventHandler(amount - Balance);
            }
        }
        
    }

    internal class BankAccount
    {
        public EventHandler OverdrawnEventHandler;

        public decimal Balance { get; set; }
        public virtual void WithDraw(decimal amount)
        {

        }

        public BankAccount(decimal balance)
        {
            Balance = balance;
            OverdrawnEventHandler = new EventHandler(OverdrawnEventMethod);
        }

        public virtual void OnOverdrawnEventHandler(decimal subAmount)
        {
            if (OverdrawnEventHandler != null)
                OverdrawnEventHandler.Invoke(this, new OverdrawnEventArgs(subAmount));
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        private void OverdrawnEventMethod(object sender, EventArgs e)
        {
            OverdrawnEventArgs args = (OverdrawnEventArgs)e;

            if (sender is MainAccount)
            {
                MainAccount main = sender as MainAccount;
                MessageBox.Show("Balansinda pul yoxdur " + args.SubAmount + " " + (main.Balance + main.SavingAccount.Balance));
            }
            else if (sender is SavingAccount)
            {
                SavingAccount sav = sender as SavingAccount;
                MessageBox.Show("Balansinda pul yoxdur " + args.SubAmount + " " + sav.Balance);

            }

        }

        public decimal GetBalance() => Balance;
    }

    internal class OverdrawnEventArgs : EventArgs
    {
        public OverdrawnEventArgs(decimal subAmount)
        {
            SubAmount = subAmount;
        }

        public decimal SubAmount { get; set; }
    }
}
