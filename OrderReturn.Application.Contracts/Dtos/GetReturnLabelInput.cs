using System;
using System.Collections.Generic;
using System.Text;

namespace OrderReturn.Dtos
{
    public class GetReturnLabelInput
    {
        public string OrderNumber { get; set; }
        public string FirstAndLastName { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string ReturnReason { get; set; }
    }
}
