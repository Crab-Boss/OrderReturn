using System;
using System.Collections.Generic;
using System.Text;

namespace OrderReturn
{
    public class OrderReturnDbProperties
    {
        public static string DbTablePrefix { get; set; } = "";

        public static string DbSchema { get; set; } = "dbo";


        public const string ConnectionStringName = "Default";
    }
}
