using System;
using System.Threading.Tasks;
using Dapper;
using Funkmap.Payments.Core;
using Funkmap.Payments.Data.Abstract;

namespace Funkmap.Payments.Data.Tools
{
    public class PostgreEnvironmentTool
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PostgreEnvironmentTool(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> TryCreateProductTableIfNotExistsAsync(EnvironmentToolOptions options = null)
        {
            if (options == null) options = EnvironmentToolOptions.Default();

            bool result = true;

            try
            {
                using (var connection = _dbConnectionFactory.Connection())
                {
                    connection.Open();


                    if (options.DeleteIfExists)
                    {
                        var dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.ProductsTableName}""";
                        await connection.ExecuteAsync(dropTableScript);
                    }

                    var createProductTableScript = $@"CREATE TABLE IF NOT EXISTS ""{FunkmapDataConfigurationProvider.ProductsTableName}""
                                                    (
                                                      ""Id"" BIGSERIAL PRIMARY KEY,
                                                      ""Name"" character varying(20) NOT NULL,
                                                      ""Description"" character varying(100) NOT NULL,
                                                      ""MetaTitle"" character varying(20) NOT NULL,
                                                      ""MetaDescription"" character varying(20) NOT NULL,
                                                      ""Price"" real NOT NULL,
                                                      ""SellingCount"" bigint NOT NULL DEFAULT 0,
                                                      ""IsDeleted"" boolean NOT NULL DEFAULT True 
                                                    )
                                                    WITH (
                                                      OIDS=FALSE
                                                    );
                                                    ALTER TABLE ""{FunkmapDataConfigurationProvider.ProductsTableName}""
                                                      OWNER TO postgres;
                                                    CREATE UNIQUE INDEX ON public.product_test USING btree (""Name"" ASC NULLS LAST);";

                    await connection.ExecuteAsync(createProductTableScript);

                    if (options.NeedFeelStatic)
                    {
                        result = result && await FillStaticProductsAsync();
                    }

                    return result;
                }

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> FillStaticProductsAsync()
        {
            var proAccount = new Product()
            {
                Name = "pro_account",
                Description = "description",
                MetaDescription = "ProAccountDescription",
                MetaTitle = "ProAccountTitle",
                Price = 5
            };

            var specialProfile = new Product()
            {
                Name = "special_profile",
                Description = "description",
                MetaDescription = "SpecialProfileDescription",
                MetaTitle = "SpecialProfileTitle",
                Price = 5
            };

            bool result = true;

            using (var unitOfWork = new PaymentsUnitOfWorkFactory(_dbConnectionFactory).UnitOfWork())
            {

                result = await unitOfWork.ProductRepository.CreateAsync(proAccount);

                
                result = result && await unitOfWork.ProductRepository.CreateAsync(specialProfile);
            }

            return result;
        }
    }
}
