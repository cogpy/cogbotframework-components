// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a synergy link between two cognitive nodes.
    /// Links enable information flow and collaborative processing between nodes.
    /// </summary>
    public class SynergyLink
    {
        /// <summary>
        /// Gets or sets the unique identifier for the synergy link.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the type of synergy link (e.g., "InheritanceLink", "SimilarityLink", "ImplicationLink").
        /// </summary>
        [JsonProperty("linkType")]
        public string LinkType { get; set; } = "SimilarityLink";

        /// <summary>
        /// Gets or sets the ID of the source cognitive node.
        /// </summary>
        [JsonProperty("sourceNodeId")]
        public string SourceNodeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the target cognitive node.
        /// </summary>
        [JsonProperty("targetNodeId")]
        public string TargetNodeId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strength/weight of the synergy link.
        /// </summary>
        [JsonProperty("strength")]
        public double Strength { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the confidence level of the synergy link.
        /// </summary>
        [JsonProperty("confidence")]
        public double Confidence { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the attention value of the synergy link.
        /// </summary>
        [JsonProperty("attentionValue")]
        public double AttentionValue { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets whether the link is bidirectional.
        /// </summary>
        [JsonProperty("isBidirectional")]
        public bool IsBidirectional { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the link is currently active.
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the activation frequency of this link.
        /// </summary>
        [JsonProperty("activationCount")]
        public long ActivationCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the last activation timestamp.
        /// </summary>
        [JsonProperty("lastActivated")]
        public DateTime? LastActivated { get; set; }

        /// <summary>
        /// Gets or sets metadata associated with this synergy link.
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the timestamp when this link was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when this link was last updated.
        /// </summary>
        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Activates the synergy link, incrementing usage statistics.
        /// </summary>
        public void Activate()
        {
            ActivationCount++;
            LastActivated = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Strengthens the synergy link based on usage patterns.
        /// </summary>
        /// <param name="reinforcement">The amount to strengthen the link by.</param>
        public void Strengthen(double reinforcement)
        {
            Strength = Math.Min(1.0, Strength + reinforcement);
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Weakens the synergy link due to lack of use or negative feedback.
        /// </summary>
        /// <param name="decay">The amount to weaken the link by.</param>
        public void Weaken(double decay)
        {
            Strength = Math.Max(0.0, Strength - decay);
            if (Strength <= 0.1)
            {
                IsActive = false;
            }
            LastUpdated = DateTime.UtcNow;
        }
    }
}