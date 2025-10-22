# Microsoft Bot Components - OpenCog Autonomous Orchestrator

## Overview

The OpenCog Autonomous Orchestrator is a cutting-edge cognitive synergy architecture designed for the Microsoft Bot Framework. It provides autogenesis capabilities for dynamic component creation and coordination, enabling bots to evolve and adapt their cognitive capabilities autonomously.

## Features

### 🧠 Cognitive Synergy Networks
- **Dynamic Node Creation**: Create cognitive nodes representing different processing capabilities
- **Synergy Links**: Connect nodes with weighted relationships for information flow
- **Emergence Detection**: Automatically detect when new cognitive capabilities emerge from node interactions

### 🔄 Autogenesis Engine
- **Evolutionary Algorithms**: Generate new cognitive components using evolutionary principles
- **Fitness Evaluation**: Assess the effectiveness of generated components
- **Adaptive Architecture**: Continuously evolve the bot's cognitive architecture based on usage patterns

### 🎯 Autonomous Orchestration  
- **Self-Organization**: Networks self-organize based on interaction patterns
- **Attention Management**: Dynamic attention allocation across cognitive nodes
- **Load Balancing**: Automatically distribute cognitive load for optimal performance

## Quick Start

### Installation

For C# projects:
```bash
dotnet add package Microsoft.Bot.Components.OpenCog
```

For JavaScript/TypeScript projects:
```bash
npm install @microsoft/bot-components-opencog
```

### Basic Usage

#### Creating a Synergy Network

```csharp
// In your bot's startup configuration
services.AddSingleton<IOpenCogOrchestrator, OpenCogOrchestrator>();

// In your dialog
var config = new AutogenesisConfig
{
    Name = "MainNetwork",
    SynergyThreshold = 0.7,
    LearningRate = 0.1,
    Enabled = true
};

var network = orchestrator.CreateSynergyNetwork("MainCognitiveNetwork", config);
```

#### Adding Cognitive Nodes

```csharp
var intentNode = new CognitiveNode
{
    Name = "IntentProcessor",
    NodeType = "PredicateNode",
    AttentionValue = 0.8,
    Confidence = 0.9
};

orchestrator.AddCognitiveNode(network.Id, intentNode);
```

#### Triggering Autogenesis

```csharp
// Evaluate synergy and trigger autogenesis if thresholds are met
await orchestrator.EvaluateSynergyAsync();

// Or force autogenesis evaluation
await orchestrator.ProcessActivityAsync(turnContext);
```

### Using in Adaptive Dialogs

The OpenCog package provides declarative actions that can be used in Bot Framework Composer:

#### Create Synergy Network Action
```json
{
  "$kind": "Microsoft.Bot.Components.OpenCog.CreateSynergyNetwork",
  "networkName": "=concat('Network_', formatDateTime(utcNow(), 'yyyyMMdd'))",
  "synergyThreshold": 0.7,
  "learningRate": 0.1,
  "resultProperty": "dialog.networkId"
}
```

#### Add Cognitive Node Action
```json
{
  "$kind": "Microsoft.Bot.Components.OpenCog.AddCognitiveNode",
  "networkId": "=dialog.networkId",
  "nodeName": "UserIntentProcessor",
  "nodeType": "PredicateNode",
  "attentionValue": 0.8,
  "resultProperty": "dialog.nodeId"
}
```

#### Trigger Autogenesis Action
```json
{
  "$kind": "Microsoft.Bot.Components.OpenCog.TriggerAutogenesis",
  "forceExecution": false,
  "resultProperty": "dialog.autogenesisResult"
}
```

## Core Concepts

### Cognitive Nodes
Cognitive nodes represent discrete cognitive capabilities or knowledge units within the bot's architecture. Each node has:

- **Type**: ConceptNode, PredicateNode, SchemaNode, or LinkNode
- **Attention Value**: Importance/relevance score (0.0 to 1.0)
- **Confidence**: Certainty level (0.0 to 1.0)  
- **Strength**: Processing weight (0.0 to 1.0)
- **Metadata**: Custom properties for specialized behavior

### Synergy Links
Synergy links connect cognitive nodes and enable information flow and collaborative processing:

- **Link Types**: InheritanceLink, SimilarityLink, ImplicationLink
- **Strength**: Connection weight affecting information transfer
- **Bidirectionality**: Whether information flows both ways
- **Activation Tracking**: Usage statistics for optimization

### Autogenesis Process
The autogenesis engine continuously evaluates the cognitive architecture and generates new components:

1. **Opportunity Analysis**: Identifies gaps and potential improvements
2. **Component Generation**: Creates new nodes and links based on templates
3. **Fitness Evaluation**: Assesses the effectiveness of generated components
4. **Integration**: Adds successful components to the network
5. **Optimization**: Removes weak components and strengthens successful ones

## Configuration

### AutogenesisConfig Properties

| Property | Description | Default |
|----------|-------------|---------|
| `SynergyThreshold` | Minimum synergy score for emergence detection | 0.8 |
| `MaxAutoNodes` | Maximum nodes that can be auto-generated | 100 |
| `MaxAutoLinks` | Maximum links that can be auto-generated | 500 |
| `LearningRate` | Rate of adaptation for the network | 0.1 |
| `MutationRate` | Rate of evolutionary changes | 0.05 |
| `FitnessThreshold` | Minimum fitness for component acceptance | 0.6 |
| `EvaluationIntervalMs` | How often to evaluate synergy (milliseconds) | 5000 |

## Events

The orchestrator provides events for monitoring cognitive evolution:

```csharp
orchestrator.CognitiveEmergence += (sender, args) => {
    Console.WriteLine($"New capabilities emerged: {string.Join(", ", args.NewCapabilities)}");
    Console.WriteLine($"Synergy score: {args.SynergyScore}");
};

orchestrator.AutogenesisTriggered += (sender, args) => {
    Console.WriteLine($"Generated {args.GeneratedNodes.Count} nodes and {args.GeneratedLinks.Count} links");
};
```

## Advanced Features

### Custom Node Templates
Define templates for generating specific types of cognitive nodes:

```csharp
var template = new CognitiveNodeTemplate
{
    Name = "ConversationAnalyzer",
    NodeType = "SchemaNode",
    BaseProperties = {
        ["AnalysisType"] = "Sentiment",
        ["ProcessingMode"] = "Realtime"
    },
    TriggerConditions = {
        ["MinConversationLength"] = 3,
        ["RequiredConfidence"] = 0.7
    }
};

config.NodeTemplates.Add(template);
```

### Custom Link Templates
Define templates for generating synergy links:

```csharp
var linkTemplate = new SynergyLinkTemplate
{
    Name = "SemanticSimilarity",
    LinkType = "SimilarityLink",
    BaseProperties = {
        ["SimilarityAlgorithm"] = "Cosine",
        ["Threshold"] = 0.8
    }
};

config.LinkTemplates.Add(linkTemplate);
```

### Performance Monitoring
Monitor cognitive load and network performance:

```csharp
var synergyManager = serviceProvider.GetService<ICognitiveSynergyManager>();
var loadAnalysis = synergyManager.AnalyzeCognitiveLoad(network);

Console.WriteLine($"Overall Load: {loadAnalysis.OverallLoad}");
Console.WriteLine($"Bottlenecks: {string.Join(", ", loadAnalysis.BottleneckNodes)}");
foreach (var recommendation in loadAnalysis.OptimizationRecommendations)
{
    Console.WriteLine($"Recommendation: {recommendation}");
}
```

## Integration with Bot Framework

The OpenCog orchestrator integrates seamlessly with the Bot Framework ecosystem:

- **Turn Context Processing**: Automatically processes each bot interaction
- **Adaptive Dialogs**: Provides declarative actions for visual design tools
- **Middleware Integration**: Can be added as middleware to existing bots
- **State Management**: Preserves cognitive state across conversations
- **Telemetry**: Integrates with Bot Framework analytics and logging

## Best Practices

1. **Start Simple**: Begin with a basic network and let autogenesis grow it
2. **Monitor Performance**: Regularly check cognitive load and synergy scores
3. **Tune Thresholds**: Adjust synergy and fitness thresholds based on your bot's behavior
4. **Use Templates**: Define node and link templates for consistent component generation
5. **Handle Events**: Listen to emergence and autogenesis events for insights
6. **Test Gradually**: Enable autogenesis in controlled environments first

## Examples

See the `/examples` directory for complete implementation examples:

- **Basic Chat Bot**: Simple integration with intent recognition
- **Enterprise Assistant**: Advanced cognitive architecture for complex tasks
- **Learning Bot**: Demonstrates adaptive behavior and knowledge evolution
- **Multi-Domain Bot**: Cross-domain knowledge synthesis and transfer

## License

MIT License - see LICENSE file for details.

## Contributing

We welcome contributions! Please see CONTRIBUTING.md for guidelines.

## Support

For issues and questions:
- GitHub Issues: [botframework-components/issues](https://github.com/Microsoft/botframework-components/issues)
- Stack Overflow: Tag with `botframework` and `opencog`
- Documentation: [Bot Framework Docs](https://docs.microsoft.com/azure/bot-service/)