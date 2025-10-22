// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Components.OpenCog
{
    using System;

    /// <summary>
    /// Attribute to mark OpenCog actions for automatic registration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OpenCogActionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCogActionAttribute"/> class.
        /// </summary>
        /// <param name="declarativeType">The declarative type name for the action.</param>
        public OpenCogActionAttribute(string declarativeType)
        {
            DeclarativeType = declarativeType ?? throw new ArgumentNullException(nameof(declarativeType));
        }

        /// <summary>
        /// Gets the declarative type name for the action.
        /// </summary>
        public string DeclarativeType { get; }
    }
}