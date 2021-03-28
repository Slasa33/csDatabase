using System;
using System.Collections.Generic;
using System.Text;

namespace csDatabase
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Product { get; set; }
        public int Price { get; set; }
    }
}
