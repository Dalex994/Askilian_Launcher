﻿#pragma checksum "..\..\..\..\..\MVVM\View\HomeView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C7F55C08F44789FD4A2C9239B85A6E64682FBC72249709C3E79DAC8E925E0A3C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using Askilian_Launcher_WPF.MVVM.View;
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


namespace Askilian_Launcher_WPF.MVVM.View {
    
    
    /// <summary>
    /// HomeView
    /// </summary>
    public partial class HomeView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\..\..\MVVM\View\HomeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Discord;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\MVVM\View\HomeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button YouTube;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\..\MVVM\View\HomeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Twitch;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\..\MVVM\View\HomeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button TikTok;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\..\MVVM\View\HomeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Facebook;
        
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
            System.Uri resourceLocater = new System.Uri("/Askilian Launcher;component/mvvm/view/homeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MVVM\View\HomeView.xaml"
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
            this.Discord = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\..\..\..\MVVM\View\HomeView.xaml"
            this.Discord.Click += new System.Windows.RoutedEventHandler(this.Discord_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.YouTube = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\..\..\MVVM\View\HomeView.xaml"
            this.YouTube.Click += new System.Windows.RoutedEventHandler(this.YouTube_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Twitch = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\..\..\MVVM\View\HomeView.xaml"
            this.Twitch.Click += new System.Windows.RoutedEventHandler(this.Twitch_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TikTok = ((System.Windows.Controls.Button)(target));
            
            #line 89 "..\..\..\..\..\MVVM\View\HomeView.xaml"
            this.TikTok.Click += new System.Windows.RoutedEventHandler(this.TikTok_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Facebook = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\..\..\..\MVVM\View\HomeView.xaml"
            this.Facebook.Click += new System.Windows.RoutedEventHandler(this.Facebook_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

