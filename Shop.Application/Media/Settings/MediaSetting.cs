namespace Shop.Application.Media.Settings
{
    public class MediaSetting : ISetting
    {
        public bool AutoGenerateAbsoluteUrl { get; set; } = false;
        public long DefaultMaximumSizeUpload { get; set; } = 100 * 1024; //100mb
        public bool EnablePictureZoom { get; set; } = true;
        public int DefaultPictureQuality { get; set; } = 80;
        public int DefaultMaximumPictureSize { get; set; } = 1920;
        public string DefaultPictureAvatar { get; set; } = "no-avatar.png";
        public string DefaultPictureEntity { get; set; } = "no-entity.png";
    }

}
