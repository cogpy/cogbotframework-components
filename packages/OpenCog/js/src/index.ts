/**
 * @module bot-components-opencog
 */

/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

// Export core models
export * from './models/CognitiveNode';
export * from './models/SynergyNetwork';
export * from './models/SynergyLink';
export * from './models/AutogenesisConfig';

// Export interfaces
export * from './interfaces/IOpenCogOrchestrator';
export * from './interfaces/ICognitiveSynergyManager';
export * from './interfaces/IAutogenesisEngine';

// Export implementations
export * from './OpenCogOrchestrator';
export * from './CognitiveSynergyManager';
export * from './AutogenesisEngine';

// Export actions
export * from './actions/CreateSynergyNetworkAction';
export * from './actions/AddCognitiveNodeAction';
export * from './actions/TriggerAutogenesisAction';

// Export utilities
export * from './utils/OpenCogUtils';

// Export component registration
export * from './OpenCogBotComponent';

// Version
export const VERSION = require('../package.json').version;