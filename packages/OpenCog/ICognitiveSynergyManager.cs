// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Components.OpenCog.Models;

    /// <summary>
    /// Interface for managing cognitive synergy between bot framework components.
    /// </summary>
    public interface ICognitiveSynergyManager
    {
        /// <summary>
        /// Initializes the cognitive synergy manager.
        /// </summary>
        /// <param name="config">The autogenesis configuration.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InitializeAsync(AutogenesisConfig config, CancellationToken cancellationToken = default);

        /// <summary>
        /// Processes a bot activity through the cognitive synergy network.
        /// </summary>
        /// <param name="network">The synergy network to process through.</param>
        /// <param name="turnContext">The turn context for the current conversation.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ProcessActivityAsync(SynergyNetwork network, ITurnContext turnContext, CancellationToken cancellationToken = default);

        /// <summary>
        /// Identifies emergent capabilities in a synergy network.
        /// </summary>
        /// <param name="network">The synergy network to analyze.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A list of identified emergent capabilities.</returns>
        Task<List<string>> IdentifyEmergentCapabilitiesAsync(SynergyNetwork network, CancellationToken cancellationToken = default);

        /// <summary>
        /// Optimizes the synergy links in a network based on usage patterns.
        /// </summary>
        /// <param name="network">The network to optimize.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task OptimizeNetworkAsync(SynergyNetwork network, CancellationToken cancellationToken = default);

        /// <summary>
        /// Analyzes cognitive load distribution across the network.
        /// </summary>
        /// <param name="network">The network to analyze.</param>
        /// <returns>A cognitive load analysis result.</returns>
        CognitiveLoadAnalysis AnalyzeCognitiveLoad(SynergyNetwork network);
    }

    /// <summary>
    /// Represents the result of cognitive load analysis.
    /// </summary>
    public class CognitiveLoadAnalysis
    {
        /// <summary>
        /// Gets or sets the overall cognitive load score (0.0 to 1.0).
        /// </summary>
        public double OverallLoad { get; set; }

        /// <summary>
        /// Gets or sets the load distribution across cognitive nodes.
        /// </summary>
        public Dictionary<string, double> NodeLoadDistribution { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the bottleneck nodes in the network.
        /// </summary>
        public List<string> BottleneckNodes { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the underutilized nodes in the network.
        /// </summary>
        public List<string> UnderutilizedNodes { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets recommendations for optimization.
        /// </summary>
        public List<string> OptimizationRecommendations { get; set; } = new List<string>();
    }
}