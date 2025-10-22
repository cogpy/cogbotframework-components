// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a cognitive node in the OpenCog framework.
    /// Each node encapsulates a specific cognitive capability or knowledge unit.
    /// </summary>
    public class CognitiveNode
    {
        /// <summary>
        /// Gets or sets the unique identifier for the cognitive node.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the type of cognitive node (e.g., "ConceptNode", "PredicateNode", "SchemaNode").
        /// </summary>
        [JsonProperty("nodeType")]
        public string NodeType { get; set; } = "ConceptNode";

        /// <summary>
        /// Gets or sets the name/label of the cognitive node.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the attention value (importance/relevance) of this node.
        /// </summary>
        [JsonProperty("attentionValue")]
        public double AttentionValue { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the confidence level of this cognitive node.
        /// </summary>
        [JsonProperty("confidence")]
        public double Confidence { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the strength/weight of this cognitive node.
        /// </summary>
        [JsonProperty("strength")]
        public double Strength { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets metadata associated with this cognitive node.
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the list of incoming synergy links from other nodes.
        /// </summary>
        [JsonProperty("incomingLinks")]
        public List<string> IncomingLinks { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of outgoing synergy links to other nodes.
        /// </summary>
        [JsonProperty("outgoingLinks")]
        public List<string> OutgoingLinks { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the timestamp when this node was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when this node was last updated.
        /// </summary>
        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether this node is active in the current cognitive process.
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; } = true;
    }
}