﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DustInTheWind.VeloCity.Domain {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DustInTheWind.VeloCity.Domain.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error reading the {0} value from the configuration file..
        /// </summary>
        internal static string ConfigurationElement_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("ConfigurationElement_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not open the configuration file..
        /// </summary>
        internal static string ConfigurationOpen_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("ConfigurationOpen_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error reading the database..
        /// </summary>
        internal static string DataAccess_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("DataAccess_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database file does not exist: &apos;{0}&apos;.
        /// </summary>
        internal static string DatabaseFileNotFound_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("DatabaseFileNotFound_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database was not found. Connection string: {0}.
        /// </summary>
        internal static string DatabaseNotFound_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("DatabaseNotFound_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to open the database file..
        /// </summary>
        internal static string DatabaseOpen_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("DatabaseOpen_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no sprint in the database..
        /// </summary>
        internal static string NoSprint_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("NoSprint_DefaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The sprint with number {0} does not exist in the database..
        /// </summary>
        internal static string SprintDoesNotExist_DefaultErrorMessage {
            get {
                return ResourceManager.GetString("SprintDoesNotExist_DefaultErrorMessage", resourceCulture);
            }
        }
    }
}
