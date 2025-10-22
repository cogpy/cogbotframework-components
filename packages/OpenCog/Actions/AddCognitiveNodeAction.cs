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
    /// Action to add a cognitive node to a synergy network.
    /// </summary>
    [OpenCogAction("Microsoft.Bot.Components.OpenCog.AddCognitiveNode")]
    public class AddCognitiveNodeAction : Dialog
    {
        /// <summary>
        /// Gets or sets the ID of the target network.
        /// </summary>
        /// <value>An expression which evaluates to the network ID.</value>
        [JsonProperty("networkId")]
        public StringExpression NetworkId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the cognitive node.
        /// </summary>
        /// <value>An expression which evaluates to the node name.</value>
        [JsonProperty("nodeName")]
        public StringExpression NodeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the cognitive node.
        /// </summary>
        /// <value>An expression which evaluates to the node type.</value>
        [JsonProperty("nodeType")]
        public StringExpression NodeType { get; set; } = "ConceptNode";

        /// <summary>
        /// Gets or sets the initial attention value for the node.
        /// </summary>
        /// <value>An expression which evaluates to the attention value.</value>
        [JsonProperty("attentionValue")]
        public NumberExpression AttentionValue { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the initial confidence for the node.
        /// </summary>
        /// <value>An expression which evaluates to the confidence value.</value>
        [JsonProperty("confidence")]
        public NumberExpression Confidence { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the initial strength for the node.
        /// </summary>
        /// <value>An expression which evaluates to the strength value.</value>
        [JsonProperty("strength")]
        public NumberExpression Strength { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the property path to store the created node ID.
        /// </summary>
        /// <value>An expression which evaluates to a property path.</value>
        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; } = "dialog.nodeId";

        /// <summary>
        /// Begins the dialog and adds a cognitive node to the network.
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
                var networkId = NetworkId?.GetValue(dialogContext.State);
                if (string.IsNullOrEmpty(networkId))
                {
                    throw new ArgumentException("Network ID is required");
                }

                var nodeName = NodeName?.GetValue(dialogContext.State) ?? $"Node_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var nodeType = NodeType?.GetValue(dialogContext.State) ?? "ConceptNode";
                var attentionValue = Math.Max(0.0, Math.Min(1.0, (double)(AttentionValue?.GetValue(dialogContext.State) ?? 1.0)));
                var confidence = Math.Max(0.0, Math.Min(1.0, (double)(Confidence?.GetValue(dialogContext.State) ?? 1.0)));
                var strength = Math.Max(0.0, Math.Min(1.0, (double)(Strength?.GetValue(dialogContext.State) ?? 1.0)));

                // Create the cognitive node
                var node = new CognitiveNode
                {
                    Name = nodeName,
                    NodeType = nodeType,
                    AttentionValue = attentionValue,
                    Confidence = confidence,
                    Strength = strength
                };

                // Add metadata about the creation context
                node.Metadata["CreatedByAction"] = "AddCognitiveNodeAction";
                node.Metadata["CreationContext"] = new
                {
                    ActivityId = dialogContext.Context.Activity.Id,
                    ActivityType = dialogContext.Context.Activity.Type,
                    Timestamp = DateTime.UtcNow
                };

                // Add the node to the network
                var success = orchestrator.AddCognitiveNode(networkId, node);

                if (!success)
                {
                    throw new InvalidOperationException($"Failed to add node to network {networkId}");
                }

                // Store the node ID in the result property
                var resultProperty = ResultProperty?.GetValue(dialogContext.State) ?? "dialog.nodeId";
                dialogContext.State.SetValue(resultProperty, node.Id);

                return await dialogContext.EndDialogAsync(new 
                { 
                    NodeId = node.Id, 
                    NodeName = node.Name, 
                    NodeType = node.NodeType,
                    Success = true 
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                return await dialogContext.EndDialogAsync(new { Error = ex.Message, Success = false }, cancellationToken);
            }
        }

        /// <summary>
        /// Gets a descriptive summary of the action.
        /// </summary>
        /// <returns>A summary string.</returns>
        public override string GetVersion()
        {
            return $"{GetType().Name}[{NodeName?.ToString() ?? "undefined"}]";
        }
    }
}