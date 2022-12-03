using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace Notes.Controls.Behaviors
{

    class WindowBarBehavior : Behavior<Window>
    {
        public UIElement? TopBar { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            SetTopBar(AssociatedObject, TopBar);
        }

        public static readonly DependencyProperty AttachedTopBarProperty =
            DependencyProperty.RegisterAttached(
                "AttachedTopBar",
                typeof(UIElement),
                typeof(WindowBarBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static UIElement GetTopBar(UIElement target) =>
            (UIElement)target.GetValue(AttachedTopBarProperty);

        public static void SetTopBar(UIElement target, UIElement? value) =>
            target.SetValue(AttachedTopBarProperty, value);
    }
}
