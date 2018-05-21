
namespace Funkmap.Payments.Core.Parameters
{
    public class ProductFilter
    {
        public ProductFilter()
        {
            Skip = 0;
            Take = 10;
        }
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
