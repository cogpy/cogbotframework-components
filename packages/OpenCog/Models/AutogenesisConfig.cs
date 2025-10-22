// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Configuration for the autogenesis engine that creates new cognitive components dynamically.
    /// </summary>
    public class AutogenesisConfig
    {
        /// <summary>
        /// Gets or sets the unique identifier for the autogenesis configuration.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the autogenesis configuration.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether autogenesis is enabled.
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum synergy threshold for triggering autogenesis.
        /// </summary>
        [JsonProperty("synergyThreshold")]
        public double SynergyThreshold { get; set; } = 0.8;

        /// <summary>
        /// Gets or sets the maximum number of nodes that can be auto-generated.
        /// </summary>
        [JsonProperty("maxAutoNodes")]
        public int MaxAutoNodes { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum number of links that can be auto-generated.
        /// </summary>
        [JsonProperty("maxAutoLinks")]
        public int MaxAutoLinks { get; set; } = 500;

        /// <summary>
        /// Gets or sets the learning rate for the autogenesis process.
        /// </summary>
        [JsonProperty("learningRate")]
        public double LearningRate { get; set; } = 0.1;

        /// <summary>
        /// Gets or sets the mutation rate for evolutionary changes.
        /// </summary>
        [JsonProperty("mutationRate")]
        public double MutationRate { get; set; } = 0.05;

        /// <summary>
        /// Gets or sets the fitness evaluation criteria for generated components.
        /// </summary>
        [JsonProperty("fitnessThreshold")]
        public double FitnessThreshold { get; set; } = 0.6;

        /// <summary>
        /// Gets or sets the template patterns for generating new nodes.
        /// </summary>
        [JsonProperty("nodeTemplates")]
        public List<CognitiveNodeTemplate> NodeTemplates { get; set; } = new List<CognitiveNodeTemplate>();

        /// <summary>
        /// Gets or sets the template patterns for generating new links.
        /// </summary>
        [JsonProperty("linkTemplates")]
        public List<SynergyLinkTemplate> LinkTemplates { get; set; } = new List<SynergyLinkTemplate>();

        /// <summary>
        /// Gets or sets the evaluation interval for autogenesis processes (in milliseconds).
        /// </summary>
        [JsonProperty("evaluationInterval")]
        public int EvaluationIntervalMs { get; set; } = 5000;

        /// <summary>
        /// Gets or sets custom parameters for the autogenesis engine.
        /// </summary>
        [JsonProperty("customParameters")]
        public Dictionary<string, object> CustomParameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the timestamp when this configuration was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when this configuration was last updated.
        /// </summary>
        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Template for generating new cognitive nodes.
    /// </summary>
    public class CognitiveNodeTemplate
    {
        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the node type to generate.
        /// </summary>
        [JsonProperty("nodeType")]
        public string NodeType { get; set; } = "ConceptNode";

        /// <summary>
        /// Gets or sets the base properties for generated nodes.
        /// </summary>
        [JsonProperty("baseProperties")]
        public Dictionary<string, object> BaseProperties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the conditions that trigger this template.
        /// </summary>
        [JsonProperty("triggerConditions")]
        public Dictionary<string, object> TriggerConditions { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// Template for generating new synergy links.
    /// </summary>
    public class SynergyLinkTemplate
    {
        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the link type to generate.
        /// </summary>
        [JsonProperty("linkType")]
        public string LinkType { get; set; } = "SimilarityLink";

        /// <summary>
        /// Gets or sets the base properties for generated links.
        /// </summary>
        [JsonProperty("baseProperties")]
        public Dictionary<string, object> BaseProperties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the conditions that trigger this template.
        /// </summary>
        [JsonProperty("triggerConditions")]
        public Dictionary<string, object> TriggerConditions { get; set; } = new Dictionary<string, object>();
    }
}