// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Actions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdaptiveExpressions.Properties;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Components.OpenCog.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Action to create a new synergy network within the OpenCog orchestrator.
    /// </summary>
    [OpenCogAction("Microsoft.Bot.Components.OpenCog.CreateSynergyNetwork")]
    public class CreateSynergyNetworkAction : Dialog
    {
        /// <summary>
        /// Gets or sets the name of the network to create.
        /// </summary>
        /// <value>An expression which evaluates to the network name.</value>
        [JsonProperty("networkName")]
        public StringExpression NetworkName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the synergy threshold for emergence detection.
        /// </summary>
        /// <value>An expression which evaluates to the synergy threshold.</value>
        [JsonProperty("synergyThreshold")]
        public NumberExpression SynergyThreshold { get; set; } = 0.7;

        /// <summary>
        /// Gets or sets the learning rate for the network.
        /// </summary>
        /// <value>An expression which evaluates to the learning rate.</value>
        [JsonProperty("learningRate")]
        public NumberExpression LearningRate { get; set; } = 0.1;

        /// <summary>
        /// Gets or sets the property path to store the created network ID.
        /// </summary>
        /// <value>An expression which evaluates to a property path.</value>
        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; } = "dialog.networkId";

        /// <summary>
        /// Begins the dialog and creates a new synergy network.
        /// </summary>
        /// <param name="dialogContext">The dialog context.</param>
        /// <param name="options">Optional dialog options.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dialogContext, object options = null, CancellationToken cancellationToken = default)
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            try
            {
                // Get the OpenCog orchestrator from services
                var orchestrator = dialogContext.Context.TurnState.Get<IOpenCogOrchestrator>();
                if (orchestrator == null)
                {
                    throw new InvalidOperationException("OpenCog orchestrator not found in turn state");
                }

                // Evaluate expressions
                var networkName = NetworkName?.GetValue(dialogContext.State) ?? $"Network_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var synergyThreshold = (double)(SynergyThreshold?.GetValue(dialogContext.State) ?? 0.7);
                var learningRate = (double)(LearningRate?.GetValue(dialogContext.State) ?? 0.1);

                // Create autogenesis config for the network
                var config = new AutogenesisConfig
                {
                    Name = $"{networkName}_Config",
                    SynergyThreshold = synergyThreshold,
                    LearningRate = learningRate,
                    Enabled = true
                };

                // Create the synergy network
                var network = orchestrator.CreateSynergyNetwork(networkName, config);

                // Store the network ID in the result property
                var resultProperty = ResultProperty?.GetValue(dialogContext.State) ?? "dialog.networkId";
                dialogContext.State.SetValue(resultProperty, network.Id);

                return await dialogContext.EndDialogAsync(new { NetworkId = network.Id, NetworkName = network.Name }, cancellationToken);
            }
            catch (Exception ex)
            {
                return await dialogContext.EndDialogAsync(new { Error = ex.Message }, cancellationToken);
            }
        }

        /// <summary>
        /// Gets a descriptive summary of the action.
        /// </summary>
        /// <returns>A summary string.</returns>
        public override string GetVersion()
        {
            return $"{GetType().Name}[{NetworkName?.ToString() ?? "undefined"}]";
        }
    }
}