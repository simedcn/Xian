﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.InputManagement {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class InputManagementSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static InputManagementSettings defaultInstance = ((InputManagementSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new InputManagementSettings())));
        
        public static InputManagementSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// The context menu delay, in milliseconds, if a tool is assigned the same button as that which normally invokes the context menu.
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The context menu delay, in milliseconds, if a tool is assigned the same button as" +
            " that which normally invokes the context menu.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("300")]
        public int ContextMenuDelay {
            get {
                return ((int)(this["ContextMenuDelay"]));
            }
            set {
                this["ContextMenuDelay"] = value;
            }
        }
    }
}
