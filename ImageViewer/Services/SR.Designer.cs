﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.Services {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.ImageViewer.Services.SR", typeof(SR).Assembly);
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
        ///   Looks up a localized string similar to Example Group.
        /// </summary>
        internal static string ExampleGroup {
            get {
                return ResourceManager.GetString("ExampleGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rm 101.
        /// </summary>
        internal static string ExampleLocation {
            get {
                return ResourceManager.GetString("ExampleLocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Example Server.
        /// </summary>
        internal static string ExampleServer {
            get {
                return ResourceManager.GetString("ExampleServer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} (Offline)
        ///The configuration is currently unavailable.
        ///Please ensure all required services are running..
        /// </summary>
        internal static string FormatLocalDataStoreConfigurationUnavailable {
            get {
                return ResourceManager.GetString("FormatLocalDataStoreConfigurationUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server Name: {0}
        ///AE Title: {1}
        ///Host: {2}
        ///Listening Port: {3}
        ///Interim Storage Directory: {4}.
        /// </summary>
        internal static string FormatLocalDataStoreDetails {
            get {
                return ResourceManager.GetString("FormatLocalDataStoreDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server Name: {0}
        ///AE Title: {1}
        ///Host: {2}
        ///Listening Port: {3}.
        /// </summary>
        internal static string FormatServerDetails {
            get {
                return ResourceManager.GetString("FormatServerDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Streaming Header Port: {0}
        ///Streaming Image Port: {1}.
        /// </summary>
        internal static string FormatStreamingDetails {
            get {
                return ResourceManager.GetString("FormatStreamingDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Location: {0}.
        /// </summary>
        internal static string Location {
            get {
                return ResourceManager.GetString("Location", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Activity Monitor service is currently unavailable.
        /// </summary>
        internal static string MessageActivityMonitorServiceUnavailable {
            get {
                return ResourceManager.GetString("MessageActivityMonitorServiceUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to start reindex operation, possibly because a conflicting operation is currently running.  If this problem persists, please contact your system administrator..
        /// </summary>
        internal static string MessageFailedToStartReindex {
            get {
                return ResourceManager.GetString("MessageFailedToStartReindex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reindex status unknown.
        /// </summary>
        internal static string MessageReindexStatusUnknown {
            get {
                return ResourceManager.GetString("MessageReindexStatusUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to My Studies.
        /// </summary>
        internal static string MyDataStoreTitle {
            get {
                return ResourceManager.GetString("MyDataStoreTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to My Servers.
        /// </summary>
        internal static string MyServersTitle {
            get {
                return ResourceManager.GetString("MyServersTitle", resourceCulture);
            }
        }
    }
}
