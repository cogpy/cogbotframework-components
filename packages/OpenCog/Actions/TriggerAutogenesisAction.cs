// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Actions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdaptiveExpressions.Properties;
    using Microsoft.Bot.Builder.Dialogs;
    using Newtonsoft.Json;

    /// <summary>
    /// Action to trigger the autogenesis process for cognitive synergy evaluation.
    /// </summary>
    [OpenCogAction("Microsoft.Bot.Components.OpenCog.TriggerAutogenesis")]
    public class TriggerAutogenesisAction : Dialog
    {
        /// <summary>
        /// Gets or sets whether to force autogenesis regardless of thresholds.
        /// </summary>
        /// <value>An expression which evaluates to a boolean value.</value>
        [JsonProperty("forceExecution")]
        public BoolExpression ForceExecution { get; set; } = false;

        /// <summary>
        /// Gets or sets the property path to store the autogenesis results.
        /// </summary>
        /// <value>An expression which evaluates to a property path.</value>
        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; } = "dialog.autogenesisResult";

        /// <summary>
        /// Begins the dialog and triggers the autogenesis process.
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
                var forceExecution = ForceExecution?.GetValue(dialogContext.State) ?? false;

                // Get current synergy score before autogenesis
                var preSynergyScore = orchestrator.GetOverallSynergyScore();

                // Store information about the autogenesis trigger
                var autogenesisContext = new
                {
                    TriggeredAt = DateTime.UtcNow,
                    TriggeredBy = "TriggerAutogenesisAction",
                    ForceExecution = forceExecution,
                    PreSynergyScore = preSynergyScore,
                    ActivityId = dialogContext.Context.Activity.Id,
                    ActivityType = dialogContext.Context.Activity.Type,
                    UserInput = dialogContext.Context.Activity.Text
                };

                // Set up event handlers to capture autogenesis results
                AutogenesisEventArgs autogenesisResult = null;
                CognitiveEmergenceEventArgs emergenceResult = null;

                void OnAutogenesisTriggered(object sender, AutogenesisEventArgs e)
                {
                    autogenesisResult = e;
                }

                void OnCognitiveEmergence(object sender, CognitiveEmergenceEventArgs e)
                {
                    emergenceResult = e;
                }

                try
                {
                    // Subscribe to events
                    orchestrator.AutogenesisTriggered += OnAutogenesisTriggered;
                    orchestrator.CognitiveEmergence += OnCognitiveEmergence;

                    // Trigger the evaluation and potential autogenesis
                    await orchestrator.EvaluateSynergyAsync(cancellationToken);

                    // Wait a moment for events to be processed
                    await Task.Delay(100, cancellationToken);
                }
                finally
                {
                    // Unsubscribe from events
                    orchestrator.AutogenesisTriggered -= OnAutogenesisTriggered;
                    orchestrator.CognitiveEmergence -= OnCognitiveEmergence;
                }

                // Get post-evaluation synergy score
                var postSynergyScore = orchestrator.GetOverallSynergyScore();

                // Compile results
                var result = new
                {
                    Context = autogenesisContext,
                    PreSynergyScore = preSynergyScore,
                    PostSynergyScore = postSynergyScore,
                    SynergyImprovement = postSynergyScore - preSynergyScore,
                    AutogenesisTriggered = autogenesisResult != null,
                    CognitiveEmergenceDetected = emergenceResult != null,
                    AutogenesisDetails = autogenesisResult != null ? new
                    {
                        NetworkId = autogenesisResult.Network?.Id,
                        NetworkName = autogenesisResult.Network?.Name,
                        GeneratedNodesCount = autogenesisResult.GeneratedNodes?.Count ?? 0,
                        GeneratedLinksCount = autogenesisResult.GeneratedLinks?.Count ?? 0,
                        TriggerConditions = autogenesisResult.TriggerConditions
                    } : null,
                    EmergenceDetails = emergenceResult != null ? new
                    {
                        NetworkId = emergenceResult.Network?.Id,
                        NetworkName = emergenceResult.Network?.Name,
                        SynergyScore = emergenceResult.SynergyScore,
                        NewCapabilities = emergenceResult.NewCapabilities
                    } : null,
                    Success = true
                };

                // Store the results in the specified property
                var resultProperty = ResultProperty?.GetValue(dialogContext.State) ?? "dialog.autogenesisResult";
                dialogContext.State.SetValue(resultProperty, result);

                return await dialogContext.EndDialogAsync(result, cancellationToken);
            }
            catch (Exception ex)
            {
                var errorResult = new 
                { 
                    Error = ex.Message, 
                    Success = false,
                    Timestamp = DateTime.UtcNow
                };

                return await dialogContext.EndDialogAsync(errorResult, cancellationToken);
            }
        }

        /// <summary>
        /// Gets a descriptive summary of the action.
        /// </summary>
        /// <returns>A summary string.</returns>
        public override string GetVersion()
        {
            return $"{GetType().Name}[Force={ForceExecution?.ToString() ?? "false"}]";
        }
    }
}