using System;
using Xamarin.Forms;

namespace VisualProvision.Pages
{
    public partial class ResultsPage
    {
        public ResultsPage()
        {
            InitializeComponent();
            ApplyTemplate();

            SubscribeToDeploymentUpdates();
        }

        private void SubscribeToDeploymentUpdates()
        {
            btnDone.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(IsEnabledProperty.PropertyName) && btnDone.IsEnabled)
                {
                    AnimationView.OnFinish += AnimationViewOnShowTransitionWhenFinished;
                    AnimationView.Loop = false;
                    return;
                }
            };
        }

        private void AnimationViewOnShowTransitionWhenFinished(object sender, EventArgs e)
        {
            AnimationView.OnFinish -= AnimationViewOnShowTransitionWhenFinished;
            AnimationView.Animation = "02-check-button.json";
            AnimationView.OnFinish += AnimationViewOnShowButtonWhenFinished;
        }

        private async void AnimationViewOnShowButtonWhenFinished(object sender, EventArgs e)
        {
            AnimationView.OnFinish -= AnimationViewOnShowButtonWhenFinished;

            btnDone.Opacity = 0;
            btnDone.IsVisible = true;
            await btnDone.FadeTo(1);

            AnimationView.IsVisible = false;
        }
    }
}