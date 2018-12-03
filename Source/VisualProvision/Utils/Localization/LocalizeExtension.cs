using VisualProvision.Resources;
using System;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualProvision.Utils.Localization
{
    [ContentProperty("Text")]
    public class LocalizeExtension : IMarkupExtension
    {
        private static readonly Lazy<ResourceManager> ResourceManager = new Lazy<ResourceManager>(
            () => new ResourceManager(typeof(Translations)));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }

            var translation = ResourceManager.Value.GetString(Text);

            if (translation == null)
            {
                translation = $"MISSING TRANSLATION: {Text}";
            }

            return translation;
        }
    }
}
