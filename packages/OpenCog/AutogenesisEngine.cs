// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Components.OpenCog.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Autogenesis engine implementation for dynamic creation of cognitive components.
    /// Uses evolutionary algorithms and emergent patterns to generate new cognitive architectures.
    /// </summary>
    public class AutogenesisEngine : IAutogenesisEngine
    {
        private readonly ILogger<AutogenesisEngine> _logger;
        private readonly Random _random;
        private AutogenesisConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutogenesisEngine"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public AutogenesisEngine(ILogger<AutogenesisEngine> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _random = new Random();
        }

        /// <inheritdoc/>
        public Task InitializeAsync(AutogenesisConfig config, CancellationToken cancellationToken = default)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            
            // Initialize default templates if none provided
            if (!_config.NodeTemplates.Any())
            {
                InitializeDefaultNodeTemplates();
            }

            if (!_config.LinkTemplates.Any())
            {
                InitializeDefaultLinkTemplates();
            }

            _logger.LogInformation("Autogenesis engine initialized with {NodeTemplates} node templates and {LinkTemplates} link templates",
                                 _config.NodeTemplates.Count, _config.LinkTemplates.Count);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task<AutogenesisResult> GenerateComponentsAsync(SynergyNetwork network, AutogenesisConfig config, CancellationToken cancellationToken = default)
        {
            if (network == null || config == null)
            {
                return new AutogenesisResult { Success = false, ErrorMessages = { "Invalid network or configuration" } };
            }

            var result = new AutogenesisResult();
            
            try
            {
                _logger.LogDebug("Starting autogenesis for network: {NetworkName}", network.Name);

                // Analyze network state and identify generation opportunities
                var opportunities = AnalyzeGenerationOpportunities(network);
                result.TriggerConditions = opportunities;

                // Generate new nodes based on templates and opportunities
                result.GeneratedNodes = await GenerateNodesAsync(network, opportunities, config, cancellationToken);

                // Generate new links to connect nodes optimally
                result.GeneratedLinks = await GenerateLinksAsync(network, result.GeneratedNodes, opportunities, config, cancellationToken);

                // Evaluate fitness of generated components
                var fitnessEvaluation = await EvaluateFitnessAsync(network, result.GeneratedNodes, result.GeneratedLinks, cancellationToken);
                result.FitnessScore = fitnessEvaluation.OverallFitness;

                // Accept or reject components based on fitness threshold
                if (fitnessEvaluation.MeetsFitnessThreshold)
                {
                    // Add accepted components to the network
                    foreach (var node in result.GeneratedNodes)
                    {
                        network.AddNode(node);
                    }

                    foreach (var link in result.GeneratedLinks)
                    {
                        network.AddLink(link);
                    }

                    result.Success = true;
                    _logger.LogInformation("Autogenesis successful: generated {NodeCount} nodes and {LinkCount} links for network {NetworkName}",
                                         result.GeneratedNodes.Count, result.GeneratedLinks.Count, network.Name);
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessages.Add($"Generated components did not meet fitness threshold: {fitnessEvaluation.OverallFitness:F3} < {config.FitnessThreshold:F3}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during autogenesis for network {NetworkName}", network.Name);
                result.Success = false;
                result.ErrorMessages.Add(ex.Message);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<FitnessEvaluation> EvaluateFitnessAsync(SynergyNetwork network, List<CognitiveNode> generatedNodes, List<SynergyLink> generatedLinks, CancellationToken cancellationToken = default)
        {
            var evaluation = new FitnessEvaluation();

            try
            {
                // Evaluate individual node fitness
                foreach (var node in generatedNodes)
                {
                    var nodeFitness = EvaluateNodeFitness(node, network);
                    evaluation.NodeFitnessScores[node.Id] = nodeFitness;
                }

                // Evaluate individual link fitness
                foreach (var link in generatedLinks)
                {
                    var linkFitness = EvaluateLinkFitness(link, network, generatedNodes);
                    evaluation.LinkFitnessScores[link.Id] = linkFitness;
                }

                // Calculate connectivity improvement
                evaluation.ConnectivityImprovement = CalculateConnectivityImprovement(network, generatedNodes, generatedLinks);

                // Calculate synergy enhancement
                evaluation.SynergyEnhancement = await CalculateSynergyEnhancementAsync(network, generatedNodes, generatedLinks, cancellationToken);

                // Calculate overall fitness
                var avgNodeFitness = evaluation.NodeFitnessScores.Any() ? evaluation.NodeFitnessScores.Values.Average() : 0.0;
                var avgLinkFitness = evaluation.LinkFitnessScores.Any() ? evaluation.LinkFitnessScores.Values.Average() : 0.0;

                evaluation.OverallFitness = (avgNodeFitness * 0.3) + (avgLinkFitness * 0.3) + 
                                          (evaluation.ConnectivityImprovement * 0.2) + (evaluation.SynergyEnhancement * 0.2);

                evaluation.MeetsFitnessThreshold = evaluation.OverallFitness >= _config.FitnessThreshold;

                // Add detailed metrics
                evaluation.DetailedMetrics["AvgNodeFitness"] = avgNodeFitness;
                evaluation.DetailedMetrics["AvgLinkFitness"] = avgLinkFitness;
                evaluation.DetailedMetrics["GeneratedNodeCount"] = generatedNodes.Count;
                evaluation.DetailedMetrics["GeneratedLinkCount"] = generatedLinks.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating fitness for generated components");
                evaluation.OverallFitness = 0.0;
            }

            return evaluation;
        }

        /// <inheritdoc/>
        public async Task ApplyMutationsAsync(SynergyNetwork network, double mutationRate, CancellationToken cancellationToken = default)
        {
            if (network == null || mutationRate <= 0.0)
            {
                return;
            }

            _logger.LogDebug("Applying mutations to network {NetworkName} with rate {MutationRate:F3}", network.Name, mutationRate);

            try
            {
                // Mutate node properties
                foreach (var node in network.Nodes.Values)
                {
                    if (_random.NextDouble() < mutationRate)
                    {
                        MutateNode(node);
                    }
                }

                // Mutate link properties
                foreach (var link in network.Links.Values)
                {
                    if (_random.NextDouble() < mutationRate)
                    {
                        MutateLink(link);
                    }
                }

                // Randomly create or remove links
                if (_random.NextDouble() < mutationRate)
                {
                    await ApplyStructuralMutationsAsync(network, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying mutations to network {NetworkName}", network.Name);
                throw;
            }
        }

        /// <inheritdoc/>
        public CognitiveNode GenerateCognitiveNode(CognitiveNodeTemplate template, Dictionary<string, object> triggerConditions)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var node = new CognitiveNode
            {
                Name = $"{template.Name}_{DateTime.UtcNow:yyyyMMddHHmmss}_{_random.Next(1000, 9999)}",
                NodeType = template.NodeType,
                AttentionValue = GetRandomValue(0.5, 1.0),
                Confidence = GetRandomValue(0.7, 1.0),
                Strength = GetRandomValue(0.6, 1.0)
            };

            // Apply base properties from template
            foreach (var kvp in template.BaseProperties)
            {
                node.Metadata[kvp.Key] = kvp.Value;
            }

            // Add trigger condition information
            node.Metadata["GenerationTriggers"] = triggerConditions;
            node.Metadata["GenerationTimestamp"] = DateTime.UtcNow;
            node.Metadata["TemplateUsed"] = template.Name;

            return node;
        }

        /// <inheritdoc/>
        public SynergyLink GenerateSynergyLink(SynergyLinkTemplate template, string sourceNodeId, string targetNodeId, Dictionary<string, object> triggerConditions)
        {
            if (template == null || string.IsNullOrEmpty(sourceNodeId) || string.IsNullOrEmpty(targetNodeId))
            {
                throw new ArgumentException("Invalid template or node IDs");
            }

            var link = new SynergyLink
            {
                LinkType = template.LinkType,
                SourceNodeId = sourceNodeId,
                TargetNodeId = targetNodeId,
                Strength = GetRandomValue(0.5, 0.9),
                Confidence = GetRandomValue(0.6, 1.0),
                AttentionValue = GetRandomValue(0.4, 0.8),
                IsBidirectional = _random.NextDouble() > 0.7 // 30% chance of bidirectional
            };

            // Apply base properties from template
            foreach (var kvp in template.BaseProperties)
            {
                link.Metadata[kvp.Key] = kvp.Value;
            }

            // Add generation metadata
            link.Metadata["GenerationTriggers"] = triggerConditions;
            link.Metadata["GenerationTimestamp"] = DateTime.UtcNow;
            link.Metadata["TemplateUsed"] = template.Name;

            return link;
        }

        private void InitializeDefaultNodeTemplates()
        {
            _config.NodeTemplates.AddRange(new[]
            {
                new CognitiveNodeTemplate
                {
                    Name = "AdaptiveProcessor",
                    NodeType = "SchemaNode",
                    BaseProperties = { ["ProcessingType"] = "Adaptive", ["Priority"] = "High" }
                },
                new CognitiveNodeTemplate
                {
                    Name = "PatternMatcher",
                    NodeType = "PredicateNode",
                    BaseProperties = { ["MatchingAlgorithm"] = "Fuzzy", ["Threshold"] = 0.8 }
                },
                new CognitiveNodeTemplate
                {
                    Name = "KnowledgeIntegrator",
                    NodeType = "ConceptNode",
                    BaseProperties = { ["IntegrationType"] = "Semantic", ["Scope"] = "Global" }
                },
                new CognitiveNodeTemplate
                {
                    Name = "EmergenceDetector",
                    NodeType = "SchemaNode",
                    BaseProperties = { ["DetectionMode"] = "Continuous", ["Sensitivity"] = "High" }
                }
            });
        }

        private void InitializeDefaultLinkTemplates()
        {
            _config.LinkTemplates.AddRange(new[]
            {
                new SynergyLinkTemplate
                {
                    Name = "InformationFlow",
                    LinkType = "InheritanceLink",
                    BaseProperties = { ["FlowDirection"] = "Bidirectional", ["Bandwidth"] = "High" }
                },
                new SynergyLinkTemplate
                {
                    Name = "ConceptualSimilarity",
                    LinkType = "SimilarityLink",
                    BaseProperties = { ["SimilarityType"] = "Semantic", ["Threshold"] = 0.7 }
                },
                new SynergyLinkTemplate
                {
                    Name = "CausalImplication",
                    LinkType = "ImplicationLink",
                    BaseProperties = { ["CausalDirection"] = "Forward", ["Certainty"] = "High" }
                }
            });
        }

        private Dictionary<string, object> AnalyzeGenerationOpportunities(SynergyNetwork network)
        {
            var opportunities = new Dictionary<string, object>();

            try
            {
                // Analyze network connectivity
                var connectivity = CalculateNetworkConnectivity(network);
                opportunities["NetworkConnectivity"] = connectivity;

                // Identify isolated nodes
                var isolatedNodes = network.Nodes.Values.Where(n => !n.IncomingLinks.Any() && !n.OutgoingLinks.Any()).ToList();
                opportunities["IsolatedNodeCount"] = isolatedNodes.Count;

                // Calculate attention distribution
                var attentionVariance = CalculateAttentionVariance(network);
                opportunities["AttentionVariance"] = attentionVariance;

                // Identify potential synergy gaps
                var synergyGaps = IdentifySynergyGaps(network);
                opportunities["SynergyGaps"] = synergyGaps;

                // Check if network size is below optimal threshold
                var nodeCount = network.Nodes.Count;
                var optimalSize = Math.Min(_config.MaxAutoNodes, nodeCount * 1.2); // 20% growth target
                opportunities["GrowthPotential"] = Math.Max(0, optimalSize - nodeCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing generation opportunities for network {NetworkName}", network.Name);
            }

            return opportunities;
        }

        private async Task<List<CognitiveNode>> GenerateNodesAsync(SynergyNetwork network, Dictionary<string, object> opportunities, AutogenesisConfig config, CancellationToken cancellationToken)
        {
            var generatedNodes = new List<CognitiveNode>();

            try
            {
                var growthPotential = (double)(opportunities.GetValueOrDefault("GrowthPotential") ?? 0.0);
                var nodesToGenerate = Math.Min((int)growthPotential, config.MaxAutoNodes - network.Nodes.Count);

                if (nodesToGenerate <= 0)
                {
                    return generatedNodes;
                }

                for (int i = 0; i < nodesToGenerate; i++)
                {
                    // Select template based on network needs
                    var template = SelectOptimalNodeTemplate(network, opportunities);
                    if (template != null)
                    {
                        var node = GenerateCognitiveNode(template, opportunities);
                        generatedNodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating nodes for network {NetworkName}", network.Name);
            }

            return generatedNodes;
        }

        private async Task<List<SynergyLink>> GenerateLinksAsync(SynergyNetwork network, List<CognitiveNode> newNodes, Dictionary<string, object> opportunities, AutogenesisConfig config, CancellationToken cancellationToken)
        {
            var generatedLinks = new List<SynergyLink>();

            try
            {
                var allNodes = network.Nodes.Values.Concat(newNodes).ToList();

                // Generate links for new nodes
                foreach (var newNode in newNodes)
                {
                    var potentialConnections = FindPotentialConnections(newNode, allNodes, network);
                    
                    foreach (var (targetNode, affinity) in potentialConnections.Take(3)) // Max 3 connections per new node
                    {
                        if (affinity > 0.5) // Connection threshold
                        {
                            var template = SelectOptimalLinkTemplate(network, opportunities);
                            if (template != null)
                            {
                                var link = GenerateSynergyLink(template, newNode.Id, targetNode.Id, opportunities);
                                generatedLinks.Add(link);
                            }
                        }
                    }
                }

                // Generate additional links to improve network connectivity
                var connectivityGaps = IdentifyConnectivityGaps(allNodes, network);
                foreach (var (sourceId, targetId, priority) in connectivityGaps.Take(5)) // Max 5 additional links
                {
                    if (priority > 0.6)
                    {
                        var template = SelectOptimalLinkTemplate(network, opportunities);
                        if (template != null)
                        {
                            var link = GenerateSynergyLink(template, sourceId, targetId, opportunities);
                            generatedLinks.Add(link);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating links for network {NetworkName}", network.Name);
            }

            return generatedLinks;
        }

        private double EvaluateNodeFitness(CognitiveNode node, SynergyNetwork network)
        {
            var fitness = 0.0;

            // Base fitness from node properties
            fitness += node.Confidence * 0.3;
            fitness += node.Strength * 0.3;

            // Fitness from potential connectivity
            var connectivityPotential = EstimateConnectivityPotential(node, network);
            fitness += connectivityPotential * 0.4;

            return Math.Min(1.0, fitness);
        }

        private double EvaluateLinkFitness(SynergyLink link, SynergyNetwork network, List<CognitiveNode> newNodes)
        {
            var fitness = 0.0;

            // Base fitness from link properties
            fitness += link.Confidence * 0.4;
            fitness += link.Strength * 0.3;

            // Fitness from network integration potential
            var integrationValue = EstimateLinkIntegrationValue(link, network, newNodes);
            fitness += integrationValue * 0.3;

            return Math.Min(1.0, fitness);
        }

        private double CalculateConnectivityImprovement(SynergyNetwork network, List<CognitiveNode> newNodes, List<SynergyLink> newLinks)
        {
            var currentConnectivity = CalculateNetworkConnectivity(network);
            
            // Simulate adding new components
            var simulatedNodeCount = network.Nodes.Count + newNodes.Count;
            var simulatedLinkCount = network.Links.Count + newLinks.Count;
            
            var maxPossibleLinks = simulatedNodeCount * (simulatedNodeCount - 1);
            var newConnectivity = maxPossibleLinks > 0 ? (double)simulatedLinkCount / maxPossibleLinks : 0.0;
            
            return Math.Max(0.0, newConnectivity - currentConnectivity);
        }

        private async Task<double> CalculateSynergyEnhancementAsync(SynergyNetwork network, List<CognitiveNode> newNodes, List<SynergyLink> newLinks, CancellationToken cancellationToken)
        {
            var currentSynergy = network.CalculateSynergyScore();
            
            // Simulate potential synergy with new components
            var potentialSynergyBoost = newNodes.Sum(n => n.AttentionValue * 0.1) + newLinks.Sum(l => l.Strength * 0.05);
            
            return Math.Min(1.0, potentialSynergyBoost);
        }

        private void MutateNode(CognitiveNode node)
        {
            // Randomly adjust node properties within reasonable bounds
            if (_random.NextDouble() < 0.5)
            {
                node.AttentionValue = Math.Max(0.1, Math.Min(1.0, node.AttentionValue + GetRandomValue(-0.1, 0.1)));
            }

            if (_random.NextDouble() < 0.3)
            {
                node.Confidence = Math.Max(0.1, Math.Min(1.0, node.Confidence + GetRandomValue(-0.05, 0.05)));
            }

            if (_random.NextDouble() < 0.3)
            {
                node.Strength = Math.Max(0.1, Math.Min(1.0, node.Strength + GetRandomValue(-0.05, 0.05)));
            }

            node.LastUpdated = DateTime.UtcNow;
        }

        private void MutateLink(SynergyLink link)
        {
            // Randomly adjust link properties
            if (_random.NextDouble() < 0.4)
            {
                link.Strength = Math.Max(0.1, Math.Min(1.0, link.Strength + GetRandomValue(-0.1, 0.1)));
            }

            if (_random.NextDouble() < 0.2)
            {
                link.Confidence = Math.Max(0.1, Math.Min(1.0, link.Confidence + GetRandomValue(-0.05, 0.05)));
            }

            link.LastUpdated = DateTime.UtcNow;
        }

        private async Task ApplyStructuralMutationsAsync(SynergyNetwork network, CancellationToken cancellationToken)
        {
            // Randomly add or remove links
            if (_random.NextDouble() < 0.5 && network.Links.Count < _config.MaxAutoLinks)
            {
                // Add a random link
                var nodes = network.Nodes.Values.Where(n => n.IsActive).ToList();
                if (nodes.Count >= 2)
                {
                    var source = nodes[_random.Next(nodes.Count)];
                    var target = nodes[_random.Next(nodes.Count)];
                    
                    if (source.Id != target.Id)
                    {
                        var template = _config.LinkTemplates.FirstOrDefault() ?? new SynergyLinkTemplate { Name = "MutationLink", LinkType = "SimilarityLink" };
                        var link = GenerateSynergyLink(template, source.Id, target.Id, new Dictionary<string, object> { ["MutationType"] = "StructuralAddition" });
                        network.AddLink(link);
                    }
                }
            }
            else if (network.Links.Any())
            {
                // Remove a weak link
                var weakLink = network.Links.Values.Where(l => l.Strength < 0.3).OrderBy(l => l.Strength).FirstOrDefault();
                if (weakLink != null)
                {
                    network.Links.Remove(weakLink.Id);
                }
            }
        }

        private CognitiveNodeTemplate SelectOptimalNodeTemplate(SynergyNetwork network, Dictionary<string, object> opportunities)
        {
            // Select template based on network analysis
            var isolatedCount = (int)(opportunities.GetValueOrDefault("IsolatedNodeCount") ?? 0);
            
            if (isolatedCount > 0)
            {
                return _config.NodeTemplates.FirstOrDefault(t => t.NodeType == "ConceptNode") ?? _config.NodeTemplates.FirstOrDefault();
            }

            return _config.NodeTemplates[_random.Next(_config.NodeTemplates.Count)];
        }

        private SynergyLinkTemplate SelectOptimalLinkTemplate(SynergyNetwork network, Dictionary<string, object> opportunities)
        {
            return _config.LinkTemplates[_random.Next(_config.LinkTemplates.Count)];
        }

        private double CalculateNetworkConnectivity(SynergyNetwork network)
        {
            if (network.Nodes.Count <= 1)
            {
                return 0.0;
            }

            var maxPossibleLinks = network.Nodes.Count * (network.Nodes.Count - 1);
            return maxPossibleLinks > 0 ? (double)network.Links.Count / maxPossibleLinks : 0.0;
        }

        private double CalculateAttentionVariance(SynergyNetwork network)
        {
            if (!network.Nodes.Any())
            {
                return 0.0;
            }

            var attentionValues = network.Nodes.Values.Select(n => n.AttentionValue).ToList();
            var mean = attentionValues.Average();
            var variance = attentionValues.Sum(a => Math.Pow(a - mean, 2)) / attentionValues.Count;
            
            return variance;
        }

        private List<string> IdentifySynergyGaps(SynergyNetwork network)
        {
            var gaps = new List<string>();
            
            // Identify nodes with low connectivity
            foreach (var node in network.Nodes.Values)
            {
                var connectionCount = node.IncomingLinks.Count + node.OutgoingLinks.Count;
                if (connectionCount < 2)
                {
                    gaps.Add($"LowConnectivity_{node.Id}");
                }
            }

            return gaps;
        }

        private List<(CognitiveNode Node, double Affinity)> FindPotentialConnections(CognitiveNode newNode, List<CognitiveNode> allNodes, SynergyNetwork network)
        {
            var connections = new List<(CognitiveNode, double)>();

            foreach (var node in allNodes.Where(n => n.Id != newNode.Id))
            {
                var affinity = CalculateNodeAffinity(newNode, node);
                connections.Add((node, affinity));
            }

            return connections.OrderByDescending(c => c.Item2).ToList();
        }

        private double CalculateNodeAffinity(CognitiveNode node1, CognitiveNode node2)
        {
            var typeAffinity = node1.NodeType == node2.NodeType ? 0.3 : 0.1;
            var attentionAffinity = 1.0 - Math.Abs(node1.AttentionValue - node2.AttentionValue);
            var strengthAffinity = 1.0 - Math.Abs(node1.Strength - node2.Strength);

            return (typeAffinity + attentionAffinity * 0.4 + strengthAffinity * 0.3);
        }

        private List<(string SourceId, string TargetId, double Priority)> IdentifyConnectivityGaps(List<CognitiveNode> allNodes, SynergyNetwork network)
        {
            var gaps = new List<(string, string, double)>();

            // Find node pairs with high potential but no direct connection
            for (int i = 0; i < allNodes.Count; i++)
            {
                for (int j = i + 1; j < allNodes.Count; j++)
                {
                    var node1 = allNodes[i];
                    var node2 = allNodes[j];

                    // Check if they're already connected
                    var isConnected = node1.OutgoingLinks.Any(linkId =>
                        network.Links.TryGetValue(linkId, out var link) && link.TargetNodeId == node2.Id);

                    if (!isConnected)
                    {
                        var priority = CalculateNodeAffinity(node1, node2);
                        gaps.Add((node1.Id, node2.Id, priority));
                    }
                }
            }

            return gaps.OrderByDescending(g => g.Item3).ToList();
        }

        private double EstimateConnectivityPotential(CognitiveNode node, SynergyNetwork network)
        {
            // Higher potential for nodes that can connect well with existing nodes
            var avgNetworkAttention = network.Nodes.Values.Any() ? network.Nodes.Values.Average(n => n.AttentionValue) : 0.5;
            var attentionCompatibility = 1.0 - Math.Abs(node.AttentionValue - avgNetworkAttention);
            
            return attentionCompatibility * node.Confidence;
        }

        private double EstimateLinkIntegrationValue(SynergyLink link, SynergyNetwork network, List<CognitiveNode> newNodes)
        {
            // Higher value for links that improve network connectivity
            var sourceExists = network.Nodes.ContainsKey(link.SourceNodeId) || newNodes.Any(n => n.Id == link.SourceNodeId);
            var targetExists = network.Nodes.ContainsKey(link.TargetNodeId) || newNodes.Any(n => n.Id == link.TargetNodeId);

            if (!sourceExists || !targetExists)
            {
                return 0.0;
            }

            return link.Strength * link.Confidence;
        }

        private double GetRandomValue(double min, double max)
        {
            return min + (_random.NextDouble() * (max - min));
        }
    }
}