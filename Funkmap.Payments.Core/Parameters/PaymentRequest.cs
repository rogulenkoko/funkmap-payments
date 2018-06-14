using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Funkmap.Payments.Core
{
    public abstract class PaymentRequest
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Currency { get; set; }

        public string ProductParameterJson { get; set; }

        [JsonIgnore]
        public ProductParameterBase ProductParameter
        {
            get
            {
                var productName = JObject.Parse(ProductParameterJson).SelectToken("name").ToString();

                switch (productName)
                {
                    case "proaccount": return JsonConvert.DeserializeObject<ProAccountParameter>(ProductParameterJson);
                    default: return null;
                }
            }
            private set { }
        }
    }

    public abstract class ProductParameterBase
    {
        public string Name { get; set; }
    }

    public class ProAccountParameter : ProductParameterBase
    {
        public string Login { get; set; }
    }
}