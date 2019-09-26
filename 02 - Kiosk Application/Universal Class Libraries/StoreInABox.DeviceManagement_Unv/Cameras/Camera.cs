using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Newtonsoft.Json;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace StoreInABox.DeviceManagement_Unv
{


    [JsonObject(MemberSerialization.OptIn)]
    public class Camera: INotifyPropertyChanged, IDisposable
    {
        private MediaCapture _mediaCapture;
        private ManualResetEvent _snapshotTaken = new ManualResetEvent(false);
        private LowLagPhotoCapture _lowLagCapture;

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty]
        public string DeviceId { get; private set; }
        [JsonProperty]
        public bool IsAvailable { get; set; } = true;
        public SoftwareBitmap LastImage{ get; set; }
        public SoftwareBitmapSource LastImageSource { get; set; }

        public Camera(string deviceId)
        {
            this.DeviceId = deviceId;
            _mediaCapture = new MediaCapture();
        }

        public async Task InitializeCameraAsync()
        {
            // Asynchronously initialize this instance.
            var mediaInitSettings = new MediaCaptureInitializationSettings { VideoDeviceId = this.DeviceId };
            mediaInitSettings.StreamingCaptureMode = StreamingCaptureMode.Video;
            await _mediaCapture.InitializeAsync(mediaInitSettings);
            await this.SetCameraResolutionAsync(1280, 720);
            _lowLagCapture = await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));
        }

        public async Task TakeSnapshotAsync(bool save = false)
        {
            // Camera device streaming may shutdown.  Have to re-initalize if this happens
            // real application should handle this by registering for CameraStreamStateChange event - 
            // one of many camera state changes to handle.  For an example, see...
            // https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/BasicFaceDetection/cs/Scenario2_DetectInWebcam.xaml.cs

            if (this._mediaCapture.CameraStreamState == Windows.Media.Devices.CameraStreamState.Shutdown)
            {
                _mediaCapture.Dispose();
                _mediaCapture = new MediaCapture();
                await InitializeCameraAsync();

            }
            var capturedPhoto = await _lowLagCapture.CaptureAsync();
            // Convert to format that allows display by XAML as source.
            this.LastImage = SoftwareBitmap.Convert(capturedPhoto.Frame.SoftwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied); ;

            var source = new SoftwareBitmapSource();
            await source.SetBitmapAsync(this.LastImage);
            this.LastImageSource = source;

            if (save)
            {
                await SaveSoftwareBitmapToFile(this.LastImage, "mypic");
            }
        }

        private async Task SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, string fileName)
        {
            var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
            StorageFile file = await myPictures.SaveFolder.CreateFileAsync(fileName + ".jpg", CreationCollisionOption.GenerateUniqueName);

            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {

                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);

                // The following properties must be set to avoid a COM error:
                // 'the codec is in the wrong state. (exception from hresult: 0x88982f04)'
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;

                // Set additional encoding parameters, if needed
                //encoder.BitmapTransform.ScaledWidth = 320;
                //encoder.BitmapTransform.ScaledHeight = 240;
                //encoder.BitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }

                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }


            }
        }

        private async Task<bool> SetCameraResolutionAsync(int pr_width, int pr_height)
        {

            System.Collections.Generic.IReadOnlyList<IMediaEncodingProperties> PreviewPropsList = _mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.VideoPreview);
            for (int i = 0; i < PreviewPropsList.Count; i++)
            {
                IMediaEncodingProperties EncodingProps = PreviewPropsList[i];
                VideoEncodingProperties spVideoEncodingProperties = (VideoEncodingProperties)EncodingProps;
                UInt32 propwidth = spVideoEncodingProperties.Width;
                UInt32 propheight = spVideoEncodingProperties.Height;
                if (propwidth == pr_width && propheight == pr_height)
                {
                    await _mediaCapture.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, EncodingProps);
                    return true;
                }
            }

            // requested resolution not supported by device
            return false;
        }

        public void Dispose()
        {
            _mediaCapture.Dispose();
        }

    }
}
