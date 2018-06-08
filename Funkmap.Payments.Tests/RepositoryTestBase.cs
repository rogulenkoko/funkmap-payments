using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Funkmap.Payments.Core;
using Funkmap.Payments.Core.Abstract;
using Funkmap.Payments.Data;
using Funkmap.Payments.Data.Abstract;
using Funkmap.Payments.Data.Tools;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Funkmap.Payments.Tests
{
    public class RepositoryTestBase
    {
        protected readonly IPaymentsUnitOfWorkFactory _unitOfWorkFactory;

        public RepositoryTestBase()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            FunkmapConfigurationProvider.Configuration = configuration;

            IDbConnectionFactory connectionFactory = new DbConnectionFactory(configuration);
            PostgreEnvironmentTool environmentTool = new PostgreEnvironmentTool(connectionFactory);

            var options = new EnvironmentToolOptions()
            {
                DeleteIfExists = true
            };

            environmentTool.DropAllTablesAsync().GetAwaiter().GetResult();

            var result = environmentTool.TryCreateUserTableIfNotExistsAsync(options).GetAwaiter().GetResult();
            result = result && environmentTool.TryCreateProductTableIfNotExistsAsync(options).GetAwaiter().GetResult();
            result = result && environmentTool.TryCreateOrderTableIfNotExistsAsync(options).GetAwaiter().GetResult();
            Assert.True(result);

            _unitOfWorkFactory = new PaymentsUnitOfWorkFactory(connectionFactory);
        }
    }
}
