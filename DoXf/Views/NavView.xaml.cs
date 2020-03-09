using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DoXf.Views
{
    public partial class NavView
    {
        public NavView()
        {
            InitializeComponent();
        }

        public bool HasBackButton
        {
            get => (bool)GetValue(HasBackButtonProperty);
            set => SetValue(HasBackButtonProperty, value);
        }
        public static readonly BindableProperty HasBackButtonProperty =
            BindableProperty.Create(nameof(HasBackButton), typeof(bool), typeof(NavView), true);
    }
}
