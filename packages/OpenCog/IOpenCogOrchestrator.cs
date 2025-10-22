// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Components.OpenCog.Models;

    /// <summary>
    /// Interface for the OpenCog autonomous orchestrator.
    /// Manages cognitive synergy networks and coordinates bot framework components.
    /// </summary>
    public interface IOpenCogOrchestrator
    {
        /// <summary>
        /// Gets the current synergy networks managed by the orchestrator.
        /// </summary>
        IReadOnlyDictionary<string, SynergyNetwork> SynergyNetworks { get; }

        /// <summary>
        /// Initializes the OpenCog orchestrator with the given configuration.
        /// </summary>
        /// <param name="config">The autogenesis configuration.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InitializeAsync(AutogenesisConfig config, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes a bot activity through the cognitive synergy networks.
        /// </summary>
        /// <param name="turnContext">The turn context for the current conversation.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ProcessActivityAsync(ITurnContext turnContext, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new synergy network with the specified configuration.
        /// </summary>
        /// <param name="networkName">The name of the network to create.</param>
        /// <param name="config">Optional configuration for the network.</param>
        /// <returns>The created synergy network.</returns>
        SynergyNetwork CreateSynergyNetwork(string networkName, AutogenesisConfig config = null);

        /// <summary>
        /// Adds a cognitive node to a specific synergy network.
        /// </summary>
        /// <param name="networkId">The ID of the target network.</param>
        /// <param name="node">The cognitive node to add.</param>
        /// <returns>True if the node was added successfully; otherwise, false.</returns>
        bool AddCognitiveNode(string networkId, CognitiveNode node);

        /// <summary>
        /// Creates a synergy link between two cognitive nodes.
        /// </summary>
        /// <param name="networkId">The ID of the target network.</param>
        /// <param name="link">The synergy link to create.</param>
        /// <returns>True if the link was created successfully; otherwise, false.</returns>
        bool CreateSynergyLink(string networkId, SynergyLink link);

        /// <summary>
        /// Evaluates the cognitive synergy across all networks and triggers autogenesis if needed.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EvaluateSynergyAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the overall cognitive synergy score across all networks.
        /// </summary>
        /// <returns>The overall synergy score.</returns>
        double GetOverallSynergyScore();

        /// <summary>
        /// Event raised when new cognitive capabilities emerge through synergy.
        /// </summary>
        event EventHandler<CognitiveEmergenceEventArgs> CognitiveEmergence;

        /// <summary>
        /// Event raised when autogenesis creates new components.
        /// </summary>
        event EventHandler<AutogenesisEventArgs> AutogenesisTriggered;
    }

    /// <summary>
    /// Event arguments for cognitive emergence events.
    /// </summary>
    public class CognitiveEmergenceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the network where emergence occurred.
        /// </summary>
        public SynergyNetwork Network { get; set; }

        /// <summary>
        /// Gets or sets the synergy score at the time of emergence.
        /// </summary>
        public double SynergyScore { get; set; }

        /// <summary>
        /// Gets or sets the new capabilities that emerged.
        /// </summary>
        public List<string> NewCapabilities { get; set; } = new List<string>();
    }

    /// <summary>
    /// Event arguments for autogenesis events.
    /// </summary>
    public class AutogenesisEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the network where autogenesis occurred.
        /// </summary>
        public SynergyNetwork Network { get; set; }

        /// <summary>
        /// Gets or sets the generated nodes.
        /// </summary>
        public List<CognitiveNode> GeneratedNodes { get; set; } = new List<CognitiveNode>();

        /// <summary>
        /// Gets or sets the generated links.
        /// </summary>
        public List<SynergyLink> GeneratedLinks { get; set; } = new List<SynergyLink>();

        /// <summary>
        /// Gets or sets the trigger conditions that caused autogenesis.
        /// </summary>
        public Dictionary<string, object> TriggerConditions { get; set; } = new Dictionary<string, object>();
    }
}