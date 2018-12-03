using VisualProvision.Pages;
using VisualProvision.Services;
using VisualProvision.Services.Management;
using VisualProvision.ViewModels;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace VisualProvision.Services
{
    public class NavigationService
    {
        protected Page CurrentMainPage
        {
            get => Application.Current.MainPage;
            set => Application.Current.MainPage = value;
        }

        public async Task InitializeAsync()
        {
            var azureService = DependencyService.Resolve<IAzureService>(DependencyFetchTarget.GlobalInstance);
            bool previouslyAuthenticated = await azureService.HasSavedCredentialsAsync();

            if (previouslyAuthenticated)
            {
                await ClearNavigateToAsync<SubscriptionViewModel>();
            }
            else
            {
                await ClearNavigateToAsync<LoginViewModel>();
            }
        }

        public Task NavigateToAsync<TViewModel>()
            where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter)
            where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateBackAsync()
        {
            return CurrentMainPage?.Navigation.PopAsync();
        }

        public Task ClearNavigateToAsync<T>()
        {
            return InternalNavigateToAsync(typeof(T), null, true);
        }

        public Task RemovePreviousAsync<T>()
        {
            if (CurrentMainPage is CustomNavigationPage mainNavigation)
            {
                INavigation navigation = mainNavigation.Navigation;
                int stackSize = navigation.NavigationStack.Count;

                if (stackSize > 1)
                {
                    Page page = navigation.NavigationStack[stackSize - 2];

                    if (page.BindingContext?.GetType() == typeof(T))
                    {
                        HandlePagePopped(page);
                        navigation.RemovePage(page);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool clearStack = false)
        {
            (Page page, BaseViewModel vm) result = CreateAndBindPage(viewModelType, parameter);

            if (CurrentMainPage is CustomNavigationPage mainNavigation)
            {
                Page currentPage = mainNavigation.CurrentPage;

                if (currentPage != null)
                {
                    HandlePageNavigated(currentPage);
                }

                await PushAsync(mainNavigation, result.page, clearStack);
                await result.vm.NavigatedToAsync(parameter);
            }
        }

        private async Task PushAsync(CustomNavigationPage navigationPage, Page page, bool clearStack)
        {
            INavigation navigation = navigationPage.Navigation;
            int stackSize = navigation.NavigationStack.Count;

            if (clearStack && stackSize > 0)
            {
                navigation.InsertPageBefore(page, navigation.NavigationStack[0]);
                await navigation.PopToRootAsync(false);
            }
            else
            {
                await navigationPage.PushAsync(page);
            }
        }

        private (Page page, BaseViewModel vm) CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Can't locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            BaseViewModel viewModel = Activator.CreateInstance(viewModelType) as BaseViewModel;
            page.BindingContext = viewModel;

            return (page, viewModel);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private void OnPoppedToRoot(object sender, NavigationEventArgs e)
        {
            HandlePagePopped(e.Page);
        }

        private void OnPagePopped(object sender, NavigationEventArgs e)
        {
            HandlePagePopped(e.Page);
        }

        private void HandlePagePopped(Page page)
        {
            if (page?.BindingContext is BaseViewModel vm)
            {
                var t = Task.Factory.StartNew(
                async () =>
                {
                    await vm.UnloadAsync();
                })
                .ContinueWith(
                    async x =>
                    {
                        if (x.Exception != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"{nameof(HandlePagePopped)} error: {x.Exception}");
                        }

                        if (CurrentMainPage != null)
                        {
                            int count = CurrentMainPage.Navigation.NavigationStack.Count;

                            if (count > 0)
                            {
                                Page currentPage = CurrentMainPage?.Navigation.NavigationStack[count - 1];

                                await (currentPage.BindingContext as BaseViewModel).NavigatedBackAsync();
                            }
                        }
                    },
                    System.Threading.CancellationToken.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void HandlePageNavigated(Page currentPage)
        {
            if (currentPage?.BindingContext is BaseViewModel vm)
            {
                var t = Task.Factory.StartNew(
                async () =>
                {
                    await vm.NavigatedFromAsync();
                })
                .ContinueWith(x =>
                {
                    if (x.Exception != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"{nameof(HandlePageNavigated)} error: {x.Exception}");
                    }
                });
            }
        }
    }
}
