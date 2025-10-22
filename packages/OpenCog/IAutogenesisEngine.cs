// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Components.OpenCog.Models;

    /// <summary>
    /// Interface for the autogenesis engine that creates new cognitive components dynamically.
    /// </summary>
    public interface IAutogenesisEngine
    {
        /// <summary>
        /// Initializes the autogenesis engine with the given configuration.
        /// </summary>
        /// <param name="config">The autogenesis configuration.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task InitializeAsync(AutogenesisConfig config, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates new cognitive components for the specified synergy network.
        /// </summary>
        /// <param name="network">The target synergy network.</param>
        /// <param name="config">The autogenesis configuration.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The result of the autogenesis process.</returns>
        Task<AutogenesisResult> GenerateComponentsAsync(SynergyNetwork network, AutogenesisConfig config, CancellationToken cancellationToken = default);

        /// <summary>
        /// Evaluates the fitness of generated components.
        /// </summary>
        /// <param name="network">The network containing the components.</param>
        /// <param name="generatedNodes">The generated cognitive nodes.</param>
        /// <param name="generatedLinks">The generated synergy links.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The fitness evaluation result.</returns>
        Task<FitnessEvaluation> EvaluateFitnessAsync(SynergyNetwork network, List<CognitiveNode> generatedNodes, List<SynergyLink> generatedLinks, CancellationToken cancellationToken = default);

        /// <summary>
        /// Applies evolutionary mutations to existing components.
        /// </summary>
        /// <param name="network">The target network.</param>
        /// <param name="mutationRate">The rate of mutation to apply.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ApplyMutationsAsync(SynergyNetwork network, double mutationRate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a cognitive node based on the specified template and conditions.
        /// </summary>
        /// <param name="template">The node template to use.</param>
        /// <param name="triggerConditions">The conditions that triggered generation.</param>
        /// <returns>The generated cognitive node.</returns>
        CognitiveNode GenerateCognitiveNode(CognitiveNodeTemplate template, Dictionary<string, object> triggerConditions);

        /// <summary>
        /// Generates a synergy link based on the specified template and conditions.
        /// </summary>
        /// <param name="template">The link template to use.</param>
        /// <param name="sourceNodeId">The ID of the source node.</param>
        /// <param name="targetNodeId">The ID of the target node.</param>
        /// <param name="triggerConditions">The conditions that triggered generation.</param>
        /// <returns>The generated synergy link.</returns>
        SynergyLink GenerateSynergyLink(SynergyLinkTemplate template, string sourceNodeId, string targetNodeId, Dictionary<string, object> triggerConditions);
    }

    /// <summary>
    /// Represents the result of an autogenesis process.
    /// </summary>
    public class AutogenesisResult
    {
        /// <summary>
        /// Gets or sets the generated cognitive nodes.
        /// </summary>
        public List<CognitiveNode> GeneratedNodes { get; set; } = new List<CognitiveNode>();

        /// <summary>
        /// Gets or sets the generated synergy links.
        /// </summary>
        public List<SynergyLink> GeneratedLinks { get; set; } = new List<SynergyLink>();

        /// <summary>
        /// Gets or sets the conditions that triggered the autogenesis.
        /// </summary>
        public Dictionary<string, object> TriggerConditions { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the fitness score of the generated components.
        /// </summary>
        public double FitnessScore { get; set; }

        /// <summary>
        /// Gets or sets whether the autogenesis was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets any error messages from the autogenesis process.
        /// </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents the result of fitness evaluation for generated components.
    /// </summary>
    public class FitnessEvaluation
    {
        /// <summary>
        /// Gets or sets the overall fitness score (0.0 to 1.0).
        /// </summary>
        public double OverallFitness { get; set; }

        /// <summary>
        /// Gets or sets the individual fitness scores for each node.
        /// </summary>
        public Dictionary<string, double> NodeFitnessScores { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the individual fitness scores for each link.
        /// </summary>
        public Dictionary<string, double> LinkFitnessScores { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the network connectivity improvement score.
        /// </summary>
        public double ConnectivityImprovement { get; set; }

        /// <summary>
        /// Gets or sets the synergy enhancement score.
        /// </summary>
        public double SynergyEnhancement { get; set; }

        /// <summary>
        /// Gets or sets whether the components meet the fitness threshold.
        /// </summary>
        public bool MeetsFitnessThreshold { get; set; }

        /// <summary>
        /// Gets or sets detailed evaluation metrics.
        /// </summary>
        public Dictionary<string, object> DetailedMetrics { get; set; } = new Dictionary<string, object>();
    }
}