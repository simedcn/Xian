﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageServer.Services.Shreds.StreamingServer {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class ImageStreamingServerSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ImageStreamingServerSettings defaultInstance = ((ImageStreamingServerSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ImageStreamingServerSettings())));
        
        public static ImageStreamingServerSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://+:1000/wado/")]
        public string Address {
            get {
                return ((string)(this["Address"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("65536")]
        public int StreamBufferSize {
            get {
                return ((int)(this["ReadBufferSize"]));
            }
        }
    }
}
