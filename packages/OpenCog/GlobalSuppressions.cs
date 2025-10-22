// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Framework code needs to handle all exceptions gracefully")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Not applicable in Bot Framework context")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Template is appropriate parameter name in this context")]
[assembly: SuppressMessage("Performance", "CA1801:Review unused parameters", Justification = "Parameters required for interface compatibility and future extensibility")]
[assembly: SuppressMessage("AsyncUsage", "UseAsyncSuffix:Use Async suffix", Justification = "Method name consistency with existing framework patterns")]
[assembly: SuppressMessage("AsyncUsage", "UseConfigureAwait:Call ConfigureAwait", Justification = "Not applicable in Bot Framework context")]
[assembly: SuppressMessage("AsyncUsage", "AvoidAsyncVoid:Asynchronous method should not return void", Justification = "Event handler callback pattern")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1108:Block statements should not contain embedded comments", Justification = "Inline comments improve code readability")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1119:Statement should not use unnecessary parenthesis", Justification = "Parentheses improve readability")]
[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:Arithmetic expressions should declare precedence", Justification = "Expression precedence is clear from context")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1116:Split parameters should start on line after declaration", Justification = "Parameter formatting is clear")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1117:Parameters should be on same line or separate lines", Justification = "Parameter formatting is clear")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace should be followed by blank line", Justification = "Compact formatting for readability")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1402:File may only contain a single type", Justification = "Related helper types in same file")]
[assembly: SuppressMessage("StyleCop.CSharp.SpecialRules", "SA1412:Store files as UTF-8 with byte order mark", Justification = "UTF-8 without BOM is preferred")]