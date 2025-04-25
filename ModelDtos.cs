using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentsSystems
{
    public class ModelDtos
    {
        public static string apiurl = "https://api-v2.payerurl.com/api/payment";
        /// <summary>
        /// currency should be small letters e.g usd p.s based on documentation
        /// order_id is a unique int
        /// </summary>
        public record PaymentRequestBody()
        {
            public int Order_id { get; set; } = Random.Shared.Next(10000)*Random.Shared.Next(8);
           
             public required string Firstname { get; set; }
            public required string Lastname { get; set; }
            public required string Email { get; set; }
            public required string Currency { get;set; }
            public required string Redirectto { get; set; }
            public required string NotifyUrl { get; set; }
            public required string Cancelurl { get; set; }
            public ICollection<Items> Items { get; set; } = [];
            public int Amount { get; set; } = 0;

        }
        /// <summary>
        /// no spaces allowed based on documentation replace "" with _ 
        /// </summary>
        public record Items()
        {
            public required string Name { get; set; }
            public required int Qty { get; set; }
            public required decimal Price { get; set; }  
        }
       
        public record PayerUrlCredentials
        {
            public required string Payerurl_public_key { get; set; }
            public required string Payerurl_secret_key { get; set; }
            public string Payerurl_Url { get; set; } = apiurl;
        }
        /// <summary>
        /// Api Parameters
        /// </summary>
      
    }
}
