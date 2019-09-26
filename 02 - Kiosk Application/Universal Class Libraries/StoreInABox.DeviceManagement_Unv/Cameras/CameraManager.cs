using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace StoreInABox.DeviceManagement_Unv
{
    public class CameraManager : INotifyPropertyChanged, IDisposable
    {
        private const int MaxSupportedCameras = 2;

        // for testing
        public Task Initalized { get; set; }

        public ObservableCollection<Tuple<string, string>> AllCamerasMeta { get; set; } = new ObservableCollection<Tuple<string, string>>();

        public ObservableCollection<Camera> AllCameras { get; set; }

        private List<Tuple<string, string>> _selectedCamerasInfo = new List<Tuple<string, string>>();

        private ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = MaxSupportedCameras };

        public event PropertyChangedEventHandler PropertyChanged;

        public Tuple<string, string> AddSelectedCameraInfo {
            set {
                _selectedCamerasInfo.Add(value);
            } }

        static CameraManager()
        {
            CameraManager.Current = new CameraManager();

        }

        public static CameraManager Current { get; private set; }
        public bool SaveAllOutput { get; set; }

        private CameraManager()
        {
            this.AllCameras = new ObservableCollection<Camera>();
        }

        public async Task InitalizeAsync()
        {
            await this.UpdateAllCamerasAsync();
        }

        public async Task UpdateAllSnapshotsAsync()
        {
            foreach (var cam in this.AllCameras)
            {
                await cam.TakeSnapshotAsync();
            }
        }

        public async Task UpdateAllCamerasAsync()
        {
            this.AllCameras.Clear();

            await GetAllCamsAsync();

            try
            {
                foreach (var camInfo in this.AllCamerasMeta)
                {
                    var cam = new Camera(camInfo.Item2);
                    await cam.InitializeCameraAsync();
                    this.AllCameras.Add(cam);
                }
            }
            catch (System.Runtime.InteropServices.COMException exp)
            {
                // Warn about camera already in use
                if ((uint)exp.HResult == 0xC00D3704)
                {
                    throw new CameraInUseException(exp.Message);
                }
                else
                {
                    throw exp;
                }
            }
        }

        // Returns a dictionary with name: device name, value: device id
        // Used in UI to allow selection of specifc camera
        // Note; this (async void) is not the best way to achieve async initialization of bindable properties
        // you could run into problems with error handling using 'async void'.  For a better, but
        // too obfuscated pattern for this article, see https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
        public async Task GetAllCamsAsync()
        {
             // Finds all video capture devices
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            foreach (var device in devices)
            {
                AllCamerasMeta.Add(new Tuple<string,string>(device.Name, device.Id));
            }
        }

        public void Dispose()
        {
            foreach (var cam in this.AllCameras)
            {
                cam.Dispose();
            }
        }
    }

    public class CameraInUseException : Exception
    {
        //create custruction and pass a error message to 
        //base class i.e.System exception class
        public CameraInUseException(String msg) : base(msg) { }
    }
} 