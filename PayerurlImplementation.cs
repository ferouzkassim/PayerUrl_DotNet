
using System.Net;

using System.Net.Http.Headers;

using System.Security.Cryptography;
using System.Text;

using System.Text.Json;
using System.Threading.Tasks;
using static PaymentsSystems.ModelDtos;

namespace PaymentsSystems
{
    public interface IIPayerUrl
    {
        Task<string> PaymentRequest(PayerUrlCredentials payerUrlCredentials, PaymentRequestBody paymentRequestBody);
    }
    public class PayerurlImplementation : IIPayerUrl
    {
        private readonly IHttpClientFactory httpClientFactoryclient;
      
        public PayerurlImplementation(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactoryclient = httpClientFactory;
         
        }
        public async Task<string?> PaymentRequest(PayerUrlCredentials payerUrlCredentials, PaymentRequestBody paymentRequestBody)
        {
            var BearerToken = TokenBuilder( payerUrlCredentials,  paymentRequestBody);
            var FormAsString = FormstringBuilder(paymentRequestBody);
            Console.WriteLine(FormAsString);
            var content = new StringContent(FormAsString, Encoding.UTF8, "application/x-www-form-urlencoded");
            try
            {
                var i = httpClientFactoryclient.CreateClient();

                i.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", BearerToken));
                var posted = await i.PostAsync(apiurl, content);
                Console.WriteLine(await posted.Content.ReadAsStringAsync());
                Console.WriteLine(posted.StatusCode); 
                posted.EnsureSuccessStatusCode();
                if (posted.IsSuccessStatusCode)
                {

                    var paymentLink = await posted.Content.ReadAsStreamAsync();
                    Console.WriteLine($"iam in and {
                    paymentLink}");
                    var tr = JsonSerializer.Deserialize<Dictionary<string, string>>(paymentLink);
                    return tr.Values.FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
                
            }
            return string.Empty;
        }
        private  string TokenBuilder(PayerUrlCredentials payerUrlCredentials,PaymentRequestBody paymentRequestBody)
        {
            var AuthStr =  FormstringBuilder(paymentRequestBody);

            using HMACSHA256 hMACSHA = new();
            hMACSHA.Key =Encoding.UTF8.GetBytes(payerUrlCredentials.Payerurl_secret_key.Trim());
            var HashedBytes = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(AuthStr));
            var Signature = BitConverter.ToString(HashedBytes).Replace("-","").ToLower();
            Console.WriteLine(Signature);
            byte[] bytes =(Encoding.UTF8.GetBytes($"{payerUrlCredentials.Payerurl_public_key.Trim()}:{Signature}"));
            var authorisationString = Convert.ToBase64String(bytes);
            Console.WriteLine(authorisationString);
            return authorisationString;
        }
        private  string FormstringBuilder(PaymentRequestBody PRB)
        {
            IEnumerable<KeyValuePair<string,string>> ItemsCollections = PRB.Items.Select((item,i) => new []{
           new KeyValuePair<string, string>($"items[{i}][name]", item.Name.Replace(" ", "_")),
            new KeyValuePair<string, string>($"items[{i}][qty]", item.Qty.ToString()),
            new KeyValuePair<string, string>($"items[{i}][price]", item.Price.ToString())
            }).SelectMany(r=>r);
         
            var BodyForrm = new SortedDictionary<string, string>
            {   
                { "order_id", PRB.Order_id.ToString() },
                { "amount", PRB.Amount.ToString() },
            
                { "currency",PRB.Currency.ToLower() },
                { "billing_fname", PRB.Firstname },
                { "billing_lname", PRB.Lastname },
                { "billing_email", PRB.Email },

                { "redirect_to",string.Concat(PRB.Redirectto,$"id={PRB.Order_id}?Action=true")},
                { "notify_url",string.Concat(PRB.Redirectto,$"id={PRB.Order_id}?Action=Notify")},
                { "cancel_url",string.Concat(PRB.Redirectto,$"id={PRB.Order_id}?Action=false")},
                { "type", "php" }
            };
            Console.WriteLine(BodyForrm);
            foreach (var item in ItemsCollections)
            { 
                BodyForrm[item.Key] = item.Value;
            } 
            var AuthStr = string.Join("&", BodyForrm.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));

            return AuthStr;
        }
       

    }
    
}
