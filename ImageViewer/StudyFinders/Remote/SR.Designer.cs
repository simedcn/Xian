//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3620
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.StudyFinders.Remote {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearCanvas.ImageViewer.StudyFinders.Remote.SR", typeof(SR).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Multiple wildcard queries cannot be performed on the ModalitiesInStudy tag..
        /// </summary>
        public static string ExceptionModalitiesInStudyCannotPerformMultipleWildcardQueries {
            get {
                return ResourceManager.GetString("ExceptionModalitiesInStudyCannotPerformMultipleWildcardQueries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Association rejected ({0})..
        /// </summary>
        public static string MessageFormatAssociationRejected {
            get {
                return ResourceManager.GetString("MessageFormatAssociationRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection failed ({0})..
        /// </summary>
        public static string MessageFormatConnectionFailed {
            get {
                return ResourceManager.GetString("MessageFormatConnectionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The connection timeout expired ({0})..
        /// </summary>
        public static string MessageFormatConnectTimeoutExpired {
            get {
                return ResourceManager.GetString("MessageFormatConnectTimeoutExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The query operation failed ({0})..
        /// </summary>
        public static string MessageFormatQueryOperationFailed {
            get {
                return ResourceManager.GetString("MessageFormatQueryOperationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The remote server cancelled the query ({0})..
        /// </summary>
        public static string MessageFormatRemoteServerCancelledFind {
            get {
                return ResourceManager.GetString("MessageFormatRemoteServerCancelledFind", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected message was received; aborted association..
        /// </summary>
        public static string MessageUnexpectedMessage {
            get {
                return ResourceManager.GetString("MessageUnexpectedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected network error has occurred..
        /// </summary>
        public static string MessageUnexpectedNetworkError {
            get {
                return ResourceManager.GetString("MessageUnexpectedNetworkError", resourceCulture);
            }
        }
    }
}
