﻿namespace Dfe.Spi.Translation.FunctionApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.Spi.Common.Http.Server;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Logging;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Translation.Application.Caches;
    using Dfe.Spi.Translation.Application.Definitions.Caches;
    using Dfe.Spi.Translation.Application.Definitions.Factories;
    using Dfe.Spi.Translation.Application.Definitions.Processors;
    using Dfe.Spi.Translation.Application.Factories;
    using Dfe.Spi.Translation.Application.Processors;
    using Dfe.Spi.Translation.Domain.Definitions;
    using Dfe.Spi.Translation.Domain.Definitions.SettingsProviders;
    using Dfe.Spi.Translation.FunctionApp.SettingsProviders;
    using Dfe.Spi.Translation.Infrastructure.AzureStorage;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Functions startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        private const string SystemErrorIdentifier = "T";

        /// <inheritdoc />
        public override void Configure(
            IFunctionsHostBuilder functionsHostBuilder)
        {
            if (functionsHostBuilder == null)
            {
                throw new ArgumentNullException(nameof(functionsHostBuilder));
            }

            // camelCase, if you please.
            JsonConvert.DefaultSettings =
                () => new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                };

            IServiceCollection serviceCollection =
                functionsHostBuilder.Services;

            AddSettingsProviders(serviceCollection);
            AddLogging(serviceCollection);
            AddCaches(serviceCollection);
            AddFactories(serviceCollection);

            HttpErrorBodyResultProvider httpErrorBodyResultProvider =
                new HttpErrorBodyResultProvider(
                    SystemErrorIdentifier,
                    HttpErrorMessages.ResourceManager);

            serviceCollection
                .AddSingleton<IHttpErrorBodyResultProvider>(httpErrorBodyResultProvider)
                .AddScoped<IMappingsResultStorageAdapter, MappingsResultStorageAdapter>()
                .AddScoped<IGetEnumerationMappingsProcessor, GetEnumerationMappingsProcessor>();
        }

        private static void AddSettingsProviders(
            IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IMappingsResultStorageAdapterSettingsProvider, MappingsResultStorageAdapterSettingsProvider>();
        }

        private static void AddFactories(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IMappingsResultCacheManagerFactory, MappingsResultCacheManagerFactory>();
        }

        private static void AddCaches(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IMappingsResultCache, MappingsResultCache>();
        }

        private static void AddLogging(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<ILogger>(CreateILogger)
                .AddScoped<ILoggerWrapper, LoggerWrapper>();
        }

        private static ILogger CreateILogger(IServiceProvider serviceProvider)
        {
            ILogger toReturn = null;

            ILoggerFactory loggerFactory =
                serviceProvider.GetService<ILoggerFactory>();

            string categoryName = LogCategories.CreateFunctionUserCategory(
                nameof(Dfe.Spi.Translation));

            toReturn = loggerFactory.CreateLogger(categoryName);

            return toReturn;
        }
    }
}