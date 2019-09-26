using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StoreInABox.DeviceManagement_Unv;
using Windows.UI.Core;
using Windows.Storage;
using StoreInABox.Container_Unv;
using Newtonsoft.Json;

namespace StoreInABox.TouchscreenApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        Windows.Storage.StorageFolder AppLocalFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        Frame rootFrame;


        public CameraManager CameraManager { get { return CameraManager.Current; } }

        public Store Store { get; set; }


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            rootFrame = Window.Current.Content as Frame;


            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            await CoreWindow.GetForCurrentThread().Dispatcher.RunTaskAsync(async () => { await InitalizeCamStoreAsync(); });
        }

        private async Task InitalizeCamera()
        {
            await CameraManager.InitalizeAsync();

            await CameraManager.UpdateAllSnapshotsAsync();
        }

        private async Task InitalizeCamStoreAsync()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values["SmartCard"] == null ||
                (string) localSettings.Values["SmartCard"] == "" ||
                localSettings.Values["PredictionKey"] == null ||
                (string) localSettings.Values["PredictionKey"] == "" ||
                localSettings.Values["PredictionUri"] == null ||
                (string)localSettings.Values["PredictionUri"] == "")
            {
                DisplayNoConfigAsync();
            }
            try
            {
                await InitalizeCamera();
            }
            catch (CameraInUseException cex)
            {
                // Warn about camera already in use
                await DisplayCameraInUseAsync();
                Application.Current.Exit();
            }

                var task = Task.Run<IStorageItem>(async () => { return await AppLocalFolder.TryGetItemAsync("store.json"); });
                var storeFile = task.Result;

                if (storeFile != null)
                {
                    this.LoadStoreAsync();
                }
                else
                {
                    this.Store = new Store() { Name = "Store-In-A-Box Prototype" };
                }
        }



        private async void LoadStoreAsync()
        {
            StorageFile storeJson = await AppLocalFolder.GetFileAsync("store.json");
            this.Store = JsonConvert.DeserializeObject<Store>(await FileIO.ReadTextAsync(storeJson), new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            // json serializer does not preserve object references so have to refresh after load.
            foreach (var shelf in this.Store.Content)
            {
                shelf.CameraContainer.UpdateCameras();
                foreach (var cam in shelf.CameraContainer.Cameras)
                {
                    cam.IsAvailable = false;
                }
            }
        }

        private async void DisplayNoConfigAsync()
        {
            ContentDialog noConfigDialog = new ContentDialog()
            {
                Title = "Kiosk Not Configured",
                Content = "This kiosk has not yet been configured, or has a faulty configuration. Please contact your kiosk administrator.",
                CloseButtonText = "Ok"
            };

            await noConfigDialog.ShowAsync();
        }

        private async Task DisplayCameraInUseAsync()
        {
            ContentDialog noConfigDialog = new ContentDialog()
            {
                Title = "Camera In Use",
                Content = "Another application is using one of the cameras.  Please ensure cameras are not in use before starting the kiosk. ",
                CloseButtonText = "Ok"
            };

            await noConfigDialog.ShowAsync();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            CameraManager.Dispose();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        private async void OnResuming(object sender, object e)
        {
            await CoreWindow.GetForCurrentThread().Dispatcher.RunTaskAsync(async () => { await InitalizeCamera(); });
        }
    }



    // Start mediacapture in STA thread as required https://github.com/Microsoft/Windows-task-snippets/blob/master/tasks/UI-thread-task-await-from-background-thread.md

    public static class DispatcherTaskExtensions
    {
        public static async Task<T> RunTaskAsync<T>(this CoreDispatcher dispatcher,
            Func<Task<T>> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            await dispatcher.RunAsync(priority, async () =>
            {
                try
                {
                    taskCompletionSource.SetResult(await func());
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                }
            });
            return await taskCompletionSource.Task;
        }

        // There is no TaskCompletionSource<void> so we use a bool that we throw away.
        public static async Task RunTaskAsync(this CoreDispatcher dispatcher,
            Func<Task> func, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal) =>
            await RunTaskAsync(dispatcher, async () => { await func(); return false; }, priority);

    }


}
