using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace VisualProvision.Controls.AutoCompleteView
{
    public enum SuggestionPlacement
    {
        Bottom,
        Top,
    }

    public partial class AutoCompleteView : ContentView
    {
        private Layout<View> optionalSuggestionsPanelContainer;
        private SuggestionPlacement suggestionPlacement = SuggestionPlacement.Bottom;
        private PropertyInfo searchMemberCachePropertyInfo;
        private ObservableCollection<object> availableSuggestions;

        public event EventHandler OnSuggestionOpen;

        public event EventHandler OnSuggestionClose;

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteView), string.Empty);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoCompleteView), default(DataTemplate));
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), new List<object>());
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(AutoCompleteView), string.Empty);
        public static readonly BindableProperty CurrentTextProperty = BindableProperty.Create(nameof(CurrentText), typeof(string), typeof(AutoCompleteView), string.Empty, BindingMode.OneWayToSource);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteView), null, BindingMode.TwoWay);
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(AutoCompleteView), default(ICommand));
    
        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create(nameof(SuggestionBackgroundColor), typeof(Color), typeof(AutoCompleteView), Color.White);
        public static readonly BindableProperty SuggestionBorderColorProperty = BindableProperty.Create(nameof(SuggestionBorderColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty PlaceholderTextColorProperty = BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AutoCompleteView), Font.Default.FontSize);

        public static readonly BindableProperty SearchMemberProperty = BindableProperty.Create(nameof(SearchMember), typeof(string), typeof(AutoCompleteView), string.Empty);

        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(AutoCompleteView), 1.5d);

        public static readonly BindableProperty ExpanderIconProperty = BindableProperty.Create(nameof(ExpanderIcon), typeof(ImageSource), typeof(AutoCompleteView), null);
        public static readonly BindableProperty RotateExpanderIconProperty = BindableProperty.Create(nameof(RotateExpanderIcon), typeof(bool), typeof(AutoCompleteView), true);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public string EmptyText
        {
            get { return (string)GetValue(EmptyTextProperty); }
            set { SetValue(EmptyTextProperty, value); }
        }

        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            set { SetValue(CurrentTextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color SuggestionBackgroundColor
        {
            get { return (Color)GetValue(SuggestionBackgroundColorProperty); }
            set { SetValue(SuggestionBackgroundColorProperty, value); }
        }

        public Color SuggestionBorderColor
        {
            get { return (Color)GetValue(SuggestionBorderColorProperty); }
            set { SetValue(SuggestionBorderColorProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Color PlaceholderTextColor
        {
            get { return (Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            private set { SetValue(SelectedItemProperty, value); }
        }

        public string SearchMember
        {
            get { return (string)GetValue(SearchMemberProperty); }
            set { SetValue(SearchMemberProperty, value); }
        }

        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public double SeparatorHeight
        {
            get { return (double)GetValue(SeparatorHeightProperty); }
            set { SetValue(SeparatorHeightProperty, value); }
        }

        public bool ShowSeparator
        {
            get { return suggestedItemsRepeaterView.ShowSeparator; }
            set { suggestedItemsRepeaterView.ShowSeparator = value; }
        }

        public bool OpenOnFocus { get; set; }

        public int MaxResults { get; set; }

        public bool AllowFreeText { get; set; }

        public SuggestionPlacement SuggestionPlacement
        {
            get => suggestionPlacement;

            set
            {
                suggestionPlacement = value;

                OnPropertyChanged();
                PlaceSuggestionPanel();
            }
        }

        public ImageSource ExpanderIcon
        {
            get { return (ImageSource)GetValue(ExpanderIconProperty); }
            set { SetValue(ExpanderIconProperty, value); }
        }

        public bool RotateExpanderIcon
        {
            get { return (bool)GetValue(RotateExpanderIconProperty); }
            set { SetValue(RotateExpanderIconProperty, value); }
        }

        public Layout<View> OptionalSuggestionsPanelContainer
        {
            get => optionalSuggestionsPanelContainer;

            set
            {
                optionalSuggestionsPanelContainer = value;

                if (value == null)
                {
                    return;
                }

                OnPropertyChanged();
                PlaceSuggestionPanel();
            }
        }

        public AutoCompleteView()
        {
            InitializeComponent();

            availableSuggestions = new ObservableCollection<object>();

            suggestedItemsRepeaterView.SelectedItemCommand = new Command(SuggestedRepeaterItemSelected);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedItemProperty.PropertyName)
            {
                UpdateSelectedAndText();
            }

            if (propertyName == SearchMemberProperty.PropertyName)
            {
                searchMemberCachePropertyInfo = null;
            }

            if (propertyName == PlaceholderTextColorProperty.PropertyName)
            {
                mainEntry.TextColor = PlaceholderTextColor;
            }

            if (propertyName == ItemTemplateProperty.PropertyName)
            {
                suggestedItemsRepeaterView.ItemTemplate = ItemTemplate;
            }

            if (propertyName == SuggestionBackgroundColorProperty.PropertyName)
            {
                suggestedItemsOuterContainer.BackgroundColor = SuggestionBackgroundColor;
            }

            if (propertyName == SuggestionBorderColorProperty.PropertyName)
            {
                suggestedItemsOuterContainer.BorderColor = SuggestionBorderColor;
            }

            if (propertyName == EmptyTextProperty.PropertyName)
            {
                suggestedItemsRepeaterView.EmptyText = EmptyText;
            }

            if (propertyName == SeparatorColorProperty.PropertyName)
            {
                suggestedItemsRepeaterView.SeparatorColor = SeparatorColor;
            }

            if (propertyName == SeparatorHeightProperty.PropertyName)
            {
                suggestedItemsRepeaterView.SeparatorHeight = SeparatorHeight;
            }

            if (propertyName == ExpanderIconProperty.PropertyName)
            {
                expanderImage.Source = ExpanderIcon;
            }
        }

        private void UpdateSelectedAndText()
        {
            if (SelectedItem != null)
            {
                var propertyInfo = GetSearchMember(SelectedItem.GetType());

                var selectedItem = ItemsSource.Cast<object>().SingleOrDefault(x => propertyInfo.GetValue(x).ToString() == propertyInfo.GetValue(SelectedItem).ToString());

                if (selectedItem != null)
                {
                    try
                    {
                        mainEntry.TextChanged -= SearchTextTextChanged;
                        mainEntry.Text = propertyInfo.GetValue(SelectedItem).ToString();
                    }
                    finally
                    {
                        mainEntry.TextChanged += SearchTextTextChanged;
                    }

                    mainEntry.TextColor = TextColor;
                }
            }

            if (OpenOnFocus)
            {
                if (mainEntry.IsFocused)
                {
                    FilterSuggestions(mainEntry.Text);
                }
                else
                {
                    ShowHideListbox(false);
                }
            }
        }

        private void SearchTextFocused(object sender, FocusEventArgs e)
        {
            UpdateSelectedAndText();
        }

        private void SearchTextUnfocused(object sender, FocusEventArgs e)
        {
            UpdateSelectedAndText();

            if (AllowFreeText && SelectedItem == null)
            {
                availableSuggestions.Clear();
            }
        }

        private void SearchTextTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(mainEntry.Text))
            {
                SelectedItem = null;

                if (availableSuggestions.Any())
                {
                    availableSuggestions.Clear();

                    ShowHideListbox(false);
                }

                return;
            }

            FilterSuggestions(mainEntry.Text);
        }

        private void FilterSuggestions(string text, bool openSuggestionPanel = true)
        {
            var filteredSuggestions = ItemsSource.Cast<object>();

            if (!string.IsNullOrEmpty(text) && text.Length > 0 && filteredSuggestions.Any())
            {
                var property = GetSearchMember(filteredSuggestions.First().GetType());

                if (property == null)
                {
                    throw new Exception($"No property matching SearchMember value '{SearchMember}'");
                }

                if (property.PropertyType != typeof(string))
                {
                    throw new Exception($"Property '{SearchMember}' must be of type string");
                }

                filteredSuggestions = filteredSuggestions.Where(x => property.GetValue(x).ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(x => property.GetValue(x).ToString());
            }

            availableSuggestions = new ObservableCollection<object>(MaxResults > 0 ? filteredSuggestions.Take(MaxResults) : filteredSuggestions);

            ShowHideListbox(openSuggestionPanel);
        }

        private PropertyInfo GetSearchMember(Type type)
        {
            if (searchMemberCachePropertyInfo != null)
            {
                return searchMemberCachePropertyInfo;
            }

            if (string.IsNullOrEmpty(SearchMember))
            {
                throw new Exception("You must specify SearchMember property");
            }

            searchMemberCachePropertyInfo = type.GetRuntimeProperty(SearchMember);

            if (searchMemberCachePropertyInfo == null)
            {
                throw new Exception($"There's no corrisponding property the matches SearchMember value '{SearchMember}'");
            }

            if (searchMemberCachePropertyInfo.PropertyType != typeof(string))
            {
                throw new Exception($"Property '{SearchMember}' must be of type string");
            }

            return searchMemberCachePropertyInfo;
        }

        private void SuggestedRepeaterItemSelected(object selectedItem)
        {
            mainEntry.Text = GetSelectedText(selectedItem);
            mainEntry.TextColor = TextColor;

            ShowHideListbox(false);

            availableSuggestions.Clear();

            SelectedItem = selectedItem;

            SelectedItemCommand?.Execute(selectedItem);
        }

        private string GetSelectedText(object selectedItem)
        {
            var property = selectedItem.GetType().GetRuntimeProperty(SearchMember);

            if (property == null)
            {
                throw new Exception($"There's no corrisponding property the matches DisplayMember value '{SearchMember}'");
            }

            return property.GetValue(selectedItem).ToString();
        }

        private void PlaceSuggestionPanel()
        {
            if (OptionalSuggestionsPanelContainer == null)
            {
                suggestedItemsContainerTop.IsVisible = false;
                suggestedItemsContainerBottom.IsVisible = false;

                if (SuggestionPlacement == SuggestionPlacement.Bottom)
                {
                    if (suggestedItemsContainerTop.Children.Any())
                    {
                        var suggestionPanel = suggestedItemsContainerTop.Children.First();

                        suggestedItemsContainerTop.Children.Remove(suggestionPanel);
                        suggestedItemsContainerBottom.Children.Add(suggestionPanel);
                    }
                }
                else
                {
                    if (suggestedItemsContainerBottom.Children.Any())
                    {
                        var suggestionPanel = suggestedItemsContainerBottom.Children.First();

                        suggestedItemsContainerBottom.Children.Remove(suggestionPanel);
                        suggestedItemsContainerTop.Children.Add(suggestionPanel);
                    }
                }
            }
            else
            {
                if (suggestedItemsContainerTop.Children.Any())
                {
                    var suggestionPanel = suggestedItemsContainerTop.Children.First();

                    suggestedItemsContainerTop.Children.Remove(suggestionPanel);
                    OptionalSuggestionsPanelContainer.Children.Add(suggestionPanel);
                }

                if (suggestedItemsContainerBottom.Children.Any())
                {
                    var suggestionPanel = suggestedItemsContainerBottom.Children.First();

                    suggestedItemsContainerBottom.Children.Remove(suggestionPanel);
                    OptionalSuggestionsPanelContainer.Children.Add(suggestionPanel);
                }
            }
        }

        private void ShowHideListbox(bool show)
        {
            if (show)
            {
                suggestedItemsRepeaterView.ItemsSource = availableSuggestions;

                if (!suggestedItemsContainerTop.IsVisible && !suggestedItemsContainerBottom.IsVisible)
                {
                    OnSuggestionOpen?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                if (suggestedItemsContainerTop.IsVisible || suggestedItemsContainerBottom.IsVisible)
                {
                    mainEntry.Unfocus();
                    Unfocus();

                    OnSuggestionClose?.Invoke(this, new EventArgs());
                }
            }

            if (SuggestionPlacement == SuggestionPlacement.Top)
            {
                suggestedItemsContainerTop.IsVisible = show;
            }
            else
            {
                suggestedItemsContainerBottom.IsVisible = show;
            }
                
            if (RotateExpanderIcon)
            {
                AnimateExpanderIconRotation(show);
            }               
        }

        private void AnimateExpanderIconRotation(bool expanded)
        {
            if (expanded)
            {
                expanderImage.RotateTo(180, 250, Easing.CubicIn);
            }
            else
            {
                expanderImage.RotateTo(0, 150, Easing.CubicOut);
            }
        }
    }
}