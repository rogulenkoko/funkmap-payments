using System;
using System.Collections.Generic;
using System.Text;
using Funkmap.Payments.Core.Models;
using Funkmap.Payments.Data.Entities;

namespace Funkmap.Payments.Data.Mappers
{
    public static class ProductMapper
    {
        public static Product ToModel(this ProductEntity source, string locale)
        {
            if (source == null) return null;
            return new Product()
            {
                Title = source.Name,
                //Description = source.
            };
        }
    }
}
