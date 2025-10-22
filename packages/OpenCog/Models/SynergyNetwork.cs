// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a synergy network that interconnects cognitive nodes.
    /// Manages the emergence of cognitive capabilities through node interactions.
    /// </summary>
    public class SynergyNetwork
    {
        /// <summary>
        /// Gets or sets the unique identifier for the synergy network.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the synergy network.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of cognitive nodes in this network.
        /// </summary>
        [JsonProperty("nodes")]
        public Dictionary<string, CognitiveNode> Nodes { get; set; } = new Dictionary<string, CognitiveNode>();

        /// <summary>
        /// Gets or sets the collection of synergy links between nodes.
        /// </summary>
        [JsonProperty("links")]
        public Dictionary<string, SynergyLink> Links { get; set; } = new Dictionary<string, SynergyLink>();

        /// <summary>
        /// Gets or sets the overall synergy score of the network.
        /// </summary>
        [JsonProperty("synergyScore")]
        public double SynergyScore { get; set; } = 0.0;

        /// <summary>
        /// Gets or sets the emergence threshold for new cognitive capabilities.
        /// </summary>
        [JsonProperty("emergenceThreshold")]
        public double EmergenceThreshold { get; set; } = 0.7;

        /// <summary>
        /// Gets or sets the network's learning rate for adaptation.
        /// </summary>
        [JsonProperty("learningRate")]
        public double LearningRate { get; set; } = 0.1;

        /// <summary>
        /// Gets or sets metadata for the synergy network.
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the timestamp when this network was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when this network was last updated.
        /// </summary>
        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Adds a cognitive node to the network.
        /// </summary>
        /// <param name="node">The cognitive node to add.</param>
        public void AddNode(CognitiveNode node)
        {
            if (node != null && !string.IsNullOrEmpty(node.Id))
            {
                Nodes[node.Id] = node;
                LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Adds a synergy link between two nodes.
        /// </summary>
        /// <param name="link">The synergy link to add.</param>
        public void AddLink(SynergyLink link)
        {
            if (link != null && !string.IsNullOrEmpty(link.Id))
            {
                Links[link.Id] = link;
                
                // Update node connections
                if (Nodes.ContainsKey(link.SourceNodeId))
                {
                    Nodes[link.SourceNodeId].OutgoingLinks.Add(link.Id);
                }
                
                if (Nodes.ContainsKey(link.TargetNodeId))
                {
                    Nodes[link.TargetNodeId].IncomingLinks.Add(link.Id);
                }
                
                LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Calculates the current synergy score of the network.
        /// </summary>
        /// <returns>The calculated synergy score.</returns>
        public double CalculateSynergyScore()
        {
            if (!Nodes.Any() || !Links.Any())
            {
                return 0.0;
            }

            var activeNodes = Nodes.Values.Where(n => n.IsActive).ToList();
            var activeLinks = Links.Values.Where(l => l.IsActive).ToList();

            if (!activeNodes.Any())
            {
                return 0.0;
            }

            // Calculate network connectivity
            double connectivity = (double)activeLinks.Count / (activeNodes.Count * (activeNodes.Count - 1));

            // Calculate average node attention
            double avgAttention = activeNodes.Average(n => n.AttentionValue);

            // Calculate average link strength
            double avgLinkStrength = activeLinks.Any() ? activeLinks.Average(l => l.Strength) : 0.0;

            // Combine metrics for overall synergy score
            SynergyScore = (connectivity * 0.4) + (avgAttention * 0.3) + (avgLinkStrength * 0.3);

            return SynergyScore;
        }
    }
}