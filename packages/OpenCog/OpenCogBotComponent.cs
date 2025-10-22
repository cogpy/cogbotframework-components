// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AdaptiveExpressions.Converters;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs.Declarative;
    using Microsoft.Bot.Builder.Dialogs.Declarative.Converters;
    using Microsoft.Bot.Components.OpenCog.Actions;
    using Microsoft.Bot.Components.OpenCog.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// OpenCog Bot Component for autonomous orchestration of cognitive synergy architecture.
    /// Provides autogenesis capabilities for dynamic component creation and coordination.
    /// </summary>
    public class OpenCogBotComponent : BotComponent
    {
        /// <inheritdoc/>
        public override void ConfigureServices(IServiceCollection services, IConfiguration componentConfiguration)
        {
            // Register OpenCog orchestrator services
            services.AddSingleton<IOpenCogOrchestrator, OpenCogOrchestrator>();
            services.AddSingleton<ICognitiveSynergyManager, CognitiveSynergyManager>();
            services.AddSingleton<IAutogenesisEngine, AutogenesisEngine>();

            // Register OpenCog actions
            var actionTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.CustomAttributes.Any(attr => attr.AttributeType == typeof(OpenCogActionAttribute)));

            foreach (Type type in actionTypes)
            {
                var attribute = type.GetCustomAttribute<OpenCogActionAttribute>();
                if (attribute != null)
                {
                    services.AddSingleton<DeclarativeType>(sp => new DeclarativeType(attribute.DeclarativeType, type));
                }
            }

            // Register JSON converters for OpenCog models
            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ObjectExpressionConverter<CognitiveNode>>>();
            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ObjectExpressionConverter<SynergyNetwork>>>();
            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ObjectExpressionConverter<AutogenesisConfig>>>();
            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ArrayExpressionConverter<CognitiveNode>>>();
            services.AddSingleton<JsonConverterFactory, JsonConverterFactory<ArrayExpressionConverter<SynergyLink>>>();
        }
    }
}