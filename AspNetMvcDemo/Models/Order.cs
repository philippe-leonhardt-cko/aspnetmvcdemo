using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetMvcDemo.Models
{
    public class Order
    {
        public int Total { get; set; }
        public string Currency { get; set; }
        public Customer Cust { get; set; }
        public string CardToken { get; set; }

        public static Order FromCustomer(Customer customer)
        {
            return new Order { Cust = customer };
        }
    }
}