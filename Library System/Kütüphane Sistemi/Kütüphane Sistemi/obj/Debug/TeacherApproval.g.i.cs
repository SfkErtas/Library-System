﻿#pragma checksum "..\..\TeacherApproval.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5EE5409663CFFEF3937A20D1C2E4DCAAB6C85D5BAC12C13279AF5B6B75406B68"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Kütüphane_Sistemi;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Kütüphane_Sistemi {
    
    
    /// <summary>
    /// TeacherApproval
    /// </summary>
    public partial class TeacherApproval : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\TeacherApproval.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReturnAdmin;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\TeacherApproval.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dg5;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\TeacherApproval.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnApprove;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Kütüphane Sistemi;component/teacherapproval.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TeacherApproval.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnReturnAdmin = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\TeacherApproval.xaml"
            this.btnReturnAdmin.Click += new System.Windows.RoutedEventHandler(this.btnReturnAdmin_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dg5 = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.btnApprove = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\TeacherApproval.xaml"
            this.btnApprove.Click += new System.Windows.RoutedEventHandler(this.btnApprove_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

