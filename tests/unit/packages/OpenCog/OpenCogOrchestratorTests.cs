// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Components.OpenCog;
using Microsoft.Bot.Components.OpenCog.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Microsoft.Bot.Components.OpenCog.Tests
{
    /// <summary>
    /// Unit tests for the OpenCog orchestrator.
    /// </summary>
    public class OpenCogOrchestratorTests
    {
        private readonly Mock<ILogger<OpenCogOrchestrator>> _mockLogger;
        private readonly Mock<ICognitiveSynergyManager> _mockSynergyManager;
        private readonly Mock<IAutogenesisEngine> _mockAutogenesisEngine;
        private readonly OpenCogOrchestrator _orchestrator;

        public OpenCogOrchestratorTests()
        {
            _mockLogger = new Mock<ILogger<OpenCogOrchestrator>>();
            _mockSynergyManager = new Mock<ICognitiveSynergyManager>();
            _mockAutogenesisEngine = new Mock<IAutogenesisEngine>();
            
            _orchestrator = new OpenCogOrchestrator(
                _mockLogger.Object,
                _mockSynergyManager.Object,
                _mockAutogenesisEngine.Object);
        }

        [Fact]
        public async Task InitializeAsync_WithValidConfig_ShouldInitializeSuccessfully()
        {
            // Arrange
            var config = new AutogenesisConfig
            {
                Name = "TestConfig",
                Enabled = true,
                SynergyThreshold = 0.7,
                LearningRate = 0.1
            };

            // Act & Assert
            await _orchestrator.InitializeAsync(config);
            
            // Verify that synergy manager and autogenesis engine were initialized
            _mockSynergyManager.Verify(sm => sm.InitializeAsync(config, default), Times.Once);
            _mockAutogenesisEngine.Verify(ae => ae.InitializeAsync(config, default), Times.Once);
        }

        [Fact]
        public void CreateSynergyNetwork_WithValidName_ShouldCreateNetwork()
        {
            // Arrange
            var networkName = "TestNetwork";
            var config = new AutogenesisConfig { Name = "TestConfig" };

            // Act
            var network = _orchestrator.CreateSynergyNetwork(networkName, config);

            // Assert
            Assert.NotNull(network);
            Assert.Equal(networkName, network.Name);
            Assert.Single(_orchestrator.SynergyNetworks);
        }

        [Fact]
        public void AddCognitiveNode_WithValidNetworkAndNode_ShouldReturnTrue()
        {
            // Arrange
            var network = _orchestrator.CreateSynergyNetwork("TestNetwork");
            var node = new CognitiveNode
            {
                Name = "TestNode",
                NodeType = "ConceptNode"
            };

            // Act
            var result = _orchestrator.AddCognitiveNode(network.Id, node);

            // Assert
            Assert.True(result);
            Assert.Contains(node.Id, network.Nodes.Keys);
        }

        [Fact]
        public void CreateSynergyLink_WithValidNetworkAndLink_ShouldReturnTrue()
        {
            // Arrange
            var network = _orchestrator.CreateSynergyNetwork("TestNetwork");
            var sourceNode = new CognitiveNode { Name = "SourceNode" };
            var targetNode = new CognitiveNode { Name = "TargetNode" };
            
            _orchestrator.AddCognitiveNode(network.Id, sourceNode);
            _orchestrator.AddCognitiveNode(network.Id, targetNode);

            var link = new SynergyLink
            {
                SourceNodeId = sourceNode.Id,
                TargetNodeId = targetNode.Id,
                LinkType = "SimilarityLink"
            };

            // Act
            var result = _orchestrator.CreateSynergyLink(network.Id, link);

            // Assert
            Assert.True(result);
            Assert.Contains(link.Id, network.Links.Keys);
        }

        [Fact]
        public void GetOverallSynergyScore_WithNoNetworks_ShouldReturnZero()
        {
            // Act
            var score = _orchestrator.GetOverallSynergyScore();

            // Assert
            Assert.Equal(0.0, score);
        }

        [Fact]
        public void GetOverallSynergyScore_WithNetworks_ShouldReturnAverageScore()
        {
            // Arrange
            var network1 = _orchestrator.CreateSynergyNetwork("Network1");
            var network2 = _orchestrator.CreateSynergyNetwork("Network2");

            // Add some nodes and links to create measurable synergy
            var node1 = new CognitiveNode { Name = "Node1", AttentionValue = 0.8 };
            var node2 = new CognitiveNode { Name = "Node2", AttentionValue = 0.9 };
            
            _orchestrator.AddCognitiveNode(network1.Id, node1);
            _orchestrator.AddCognitiveNode(network1.Id, node2);

            var link = new SynergyLink
            {
                SourceNodeId = node1.Id,
                TargetNodeId = node2.Id,
                Strength = 0.7
            };
            _orchestrator.CreateSynergyLink(network1.Id, link);

            // Act
            var score = _orchestrator.GetOverallSynergyScore();

            // Assert
            Assert.True(score >= 0.0);
            Assert.True(score <= 1.0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateSynergyNetwork_WithInvalidName_ShouldThrowException(string invalidName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _orchestrator.CreateSynergyNetwork(invalidName));
        }

        [Fact]
        public void AddCognitiveNode_WithInvalidNetworkId_ShouldReturnFalse()
        {
            // Arrange
            var node = new CognitiveNode { Name = "TestNode" };

            // Act
            var result = _orchestrator.AddCognitiveNode("InvalidNetworkId", node);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateSynergyLink_WithInvalidNetworkId_ShouldReturnFalse()
        {
            // Arrange
            var link = new SynergyLink
            {
                SourceNodeId = "SourceId",
                TargetNodeId = "TargetId"
            };

            // Act
            var result = _orchestrator.CreateSynergyLink("InvalidNetworkId", link);

            // Assert
            Assert.False(result);
        }
    }
}