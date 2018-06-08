using System;
using System.Threading.Tasks;
using Dapper;
using Funkmap.Payments.Core.Models;
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
                                                      ""IsDeleted"" boolean NOT NULL DEFAULT False
                                                    )
                                                    WITH (
                                                      OIDS=FALSE
                                                    );
                                                    ALTER TABLE ""{FunkmapDataConfigurationProvider.ProductsTableName}""
                                                      OWNER TO postgres;
                                                    CREATE UNIQUE INDEX ON ""{FunkmapDataConfigurationProvider.ProductsTableName}"" USING btree (""Name"" ASC NULLS LAST);";

                    await connection.ExecuteAsync(createProductTableScript);

                    if (options.NeedFeelStatic)
                    {
                        result = await FillStaticProductsAsync();
                    }

                    return result;
                }

            }
            catch (Exception)
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

            bool result;

            using (var unitOfWork = new PaymentsUnitOfWorkFactory(_dbConnectionFactory).UnitOfWork())
            {

                result = await unitOfWork.ProductRepository.CreateAsync(proAccount);


                result = result && await unitOfWork.ProductRepository.CreateAsync(specialProfile);
            }

            return result;
        }


        public async Task<bool> TryCreateOrderTableIfNotExistsAsync(EnvironmentToolOptions options = null)
        {
            if (options == null) options = EnvironmentToolOptions.Default();

            try
            {
                using (var connection = _dbConnectionFactory.Connection())
                {
                    connection.Open();


                    if (options.DeleteIfExists)
                    {
                        var dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.OrderTableName}""";
                        await connection.ExecuteAsync(dropTableScript);
                    }

                    var createOrderTableScript = $@"CREATE TABLE IF NOT EXISTS ""{FunkmapDataConfigurationProvider.OrderTableName}""
                                                    (
                                                      ""Id"" BIGSERIAL PRIMARY KEY,
                                                      ""CreatorLogin"" character varying(20) NOT NULL references ""{FunkmapDataConfigurationProvider.UserTableName}""(""Login""),
                                                      ""ProductId"" BIGSERIAL NOT NULL references ""{FunkmapDataConfigurationProvider.ProductsTableName}""(""Id""),
                                                      ""OrderPrice"" real NOT NULL,
                                                      ""Currency"" character varying(20) NOT NULL,
                                                      ""CreatedOn"" timestamp without time zone NOT NULL,
                                                      ""UpdatedOn"" timestamp without time zone NOT NULL
                                                    )
                                                    WITH (
                                                      OIDS=FALSE
                                                    );
                                                    ALTER TABLE ""{FunkmapDataConfigurationProvider.OrderTableName}""
                                                      OWNER TO postgres;";

                    await connection.ExecuteAsync(createOrderTableScript);

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> TryCreateUserTableIfNotExistsAsync(EnvironmentToolOptions options = null)
        {
            if (options == null) options = EnvironmentToolOptions.Default();

            try
            {
                using (var connection = _dbConnectionFactory.Connection())
                {
                    connection.Open();


                    if (options.DeleteIfExists)
                    {
                        var dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.UserTableName}""";
                        await connection.ExecuteAsync(dropTableScript);
                    }

                    var createUserTableScript = $@"CREATE TABLE IF NOT EXISTS ""{FunkmapDataConfigurationProvider.UserTableName}""
                                                    (
                                                      ""Login"" character varying(20) NOT NULL PRIMARY KEY,
                                                      ""Name"" character varying(20) NOT NULL,
                                                      ""Email"" character varying(40) NOT NULL,
                                                      ""PaymentInfoJson"" text
                                                    )
                                                    WITH (
                                                      OIDS=FALSE
                                                    );
                                                    ALTER TABLE ""{FunkmapDataConfigurationProvider.UserTableName}""
                                                      OWNER TO postgres;";

                    await connection.ExecuteAsync(createUserTableScript);

                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DropAllTablesAsync()
        {
            using (var connection = _dbConnectionFactory.Connection())
            {
                var dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.OrderTableName}""";
                await connection.ExecuteAsync(dropTableScript);

                dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.ProductsTableName}""";
                await connection.ExecuteAsync(dropTableScript);

                dropTableScript = $@"DROP TABLE IF EXISTS ""{FunkmapDataConfigurationProvider.UserTableName}""";
                await connection.ExecuteAsync(dropTableScript);

            }


        }
    }
}
