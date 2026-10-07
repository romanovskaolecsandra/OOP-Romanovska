using System;
using System.Collections.Generic;

namespace lab9V2
{
    public abstract class Payment
    {
        public double Amount { get; set; }

        public Payment(double amount)
        {
            Amount = amount;
        }

        public abstract void Process();
    }

    public class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; }

        public CreditCardPayment(double amount, string cardNumber) : base(amount)
        {
            CardNumber = cardNumber;
        }

        public override void Process()
        {
            if (CardNumber == null || CardNumber.Length != 16)
            {
                throw new ArgumentException("Неправильний номер картки! Має бути 16 цифр.");
            }

            Console.WriteLine("Успішно оплачено карткою " + CardNumber + " на суму " + Amount + " грн.");
        }
    }

    public class PayPalPayment : Payment
    {
        public string PayPalEmail { get; set; }

        public PayPalPayment(double amount, string email) : base(amount)
        {
            PayPalEmail = email;
        }

        public override void Process()
        {
            if (PayPalEmail == null || !PayPalEmail.Contains("@"))
            {
                throw new ArgumentException("Некоректний email для PayPal: " + PayPalEmail);
            }

            Console.WriteLine("Успішно оплачено через PayPal (" + PayPalEmail + ") на суму " + Amount + " грн.");
        }
    }

    public class CryptoPayment : Payment
    {
        public string CryptoAddress { get; set; }

        public CryptoPayment(double amount, string cryptoAddress) : base(amount)
        {
            CryptoAddress = cryptoAddress;
        }

        public override void Process()
        {
            if (CryptoAddress == null || !CryptoAddress.StartsWith("0x"))
            {
                throw new ArgumentException("Некоректна крипто-адреса! Має починатися з '0x'.");
            }

            Console.WriteLine("Успішний крипто-платіж на адресу " + CryptoAddress + " на суму " + Amount + " грн.");
        }
    }

    public class PaymentService
    {
        public void SendAll(List<Payment> items)
        {
            Console.WriteLine("--- Обробка всіх платежів ---");
            foreach (var item in items)
            {
                try
                {
                    item.Process();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
            Console.WriteLine("--- Обробку завершено ---");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Payment> list = new List<Payment>();

            list.Add(new CreditCardPayment(500, "1111222233334444"));
            list.Add(new PayPalPayment(250, "student@test.com"));
            list.Add(new CryptoPayment(1000, "0xAbCdEf123456"));

            list.Add(new CreditCardPayment(50, "123"));
            list.Add(new PayPalPayment(120, "wrong_email.com"));

            PaymentService service = new PaymentService();
            service.SendAll(list);

            Console.ReadLine();
        }
    }
}
