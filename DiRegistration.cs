using Microsoft.Extensions.DependencyInjection;

namespace PaymentsSystems
{
    public static class PayerDiUrlRegistration
    {
        ///<summary>
        ///Registers PayerUrl as aDependnacy
        ///iExpect to have your body moded and keys available also httpclient as a dependcancy is required 
        ///</summary>
        public static IServiceCollection PayerUrl(this IServiceCollection services) {
          
            services.AddScoped<IIPayerUrl, PayerurlImplementation>();
                return services;
        }
    }
}
