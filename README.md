# PayerUrl_DotNet
PayerUrl payment intergration with c# and asp.net
so this is part of my private Repos but just thought it could share since the only payerUrl documentation is in PHP
all you need is to 

##
```
builder.Services.PayerUrl();
// add this as a dependancy in your middleware
// and then you will have access to the IIPayerUrl interface

IIPayerUrl.paymentRequest(/*your payer url credentils /Keys*/,/*The Request Body*/)

the Request Body can be results of static methods that help you format your data to theis Models
```
##
if You are using MVC controllers
You can just.
##
```
  [Route("/Api/PayInvoice")]
  public async Task<IActionResult>InvoicePay([FromQuery]Guid invoice, [FromServices]IPayment payment, [FromServices]IIPayerUrl iPayerUrl)
  {
      string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{HttpContext.Request.PathBase}";

   
      var tr = await payment.PayInvoice(invoice, iPayerUrl, baseUrl);
      if (string.IsNullOrEmpty(tr))
      {
          return RedirectToAction("InvoiceView",invoice); 
      
      }
     
      return PartialView("~/Views/index/Components/invstate.cshtml",tr);
  }

public class payment{
  public static string PayInvoice (InvoiceId,[FromServices],IIPayerurlpayment string URLPoint ){
        var payeUrlConfiguration = ....[///db interaction to get your conifguration ];
        or Grab it from your appsetings whihc i dont recommend .,,
  var PaymentRequestBody = new PaymentRequestBody()
  {
      Order_id = int.Parse(inv.InvoiceNumber),
      Firstname = inv.Customer_Users.FirstName,
      Lastname = inv.Customer_Users.LastName,
      Email = inv.Customer_Users.Email,
      Currency = inv.Payment.CurrencyCode.ToString().ToLower(),
      Items = items,
      Amount = Decimal.ToInt32(inv.Amount),
      Redirectto =UrlPoints,
      Cancelurl = UrlPoints,
      NotifyUrl = UrlPoints,

  };
Note:EVERYTHING IN THE PaymentRequestBody IS NEEDED DONT IGNORE ANYTHING
 then just call the
var paymentLink = await iPayerUrl.PaymentRequest(payeUrlConfiguration, PaymentRequestBody);
 
}
}
```
##
the file ModelDtos

just maps the modles to the expected models at payer url's endpoint 

the result will be a string formatted as a link 

if you are using mvc RazorViews you can embed this in a partial view or if you hae a fornt end Framework You can redirect your URL to this incomming string 

Feel Free to Star this if it at any point helped you 

#OSS_For_All
