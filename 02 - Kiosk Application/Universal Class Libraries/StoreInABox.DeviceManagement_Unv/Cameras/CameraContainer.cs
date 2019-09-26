using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace StoreInABox.DeviceManagement_Unv
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CameraContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        public ObservableCollection<Camera> Cameras { get; set; } = new ObservableCollection<Camera>();

        [JsonProperty]
        public List<string> CameraMetadata
        {
            get
            {
                return new List<string>(this.Cameras?.Select(p => p.DeviceId));
            }
        }

        public string Name { get; set; }

        [JsonConstructor]
        public CameraContainer(List<string> cameraMetadata)
        {
            this.Cameras = new ObservableCollection<Camera>();

            if (cameraMetadata != null)
            {
                foreach (var camId in cameraMetadata)
                {
                    var camera = CameraManager.Current.AllCameras.Single(p => p.DeviceId == camId);
                    this.Cameras.Add(camera);
                }
            }
        }

        // json serializer does not preserve object references to have to refresh after load.
        public void UpdateCameras()
        {
            var cameraMetadataOld = CameraMetadata;
            this.Cameras.Clear();

            if (cameraMetadataOld != null)
            {
                foreach (var camId in cameraMetadataOld)
                {
                    var camera = CameraManager.Current.AllCameras.Single(p => p.DeviceId == camId);
                    this.Cameras.Add(camera);
                }
            }
        }

        public CameraContainer()
        {
        }
    }
}
