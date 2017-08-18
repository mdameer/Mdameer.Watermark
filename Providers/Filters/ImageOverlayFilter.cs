using ImageProcessor;
using ImageProcessor.Imaging;
using Orchard.ContentManagement;
using Orchard.FileSystems.Media;
using Orchard.Localization;
using Orchard.MediaLibrary.Models;
using Orchard.MediaProcessing.Descriptors.Filter;
using Orchard.MediaProcessing.Services;
using System.Drawing;
using System.IO;

namespace Mdameer.Watermark.Providers.Filters
{
    public class ImageOverlayFilter : IImageFilterProvider
    {
        private readonly IContentManager _contentManager;
        private readonly IStorageProvider _storageProvider;

        public ImageOverlayFilter(
            IContentManager contentManager,
            IStorageProvider storageProvider)
        {
            _contentManager = contentManager;
            _storageProvider = storageProvider;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeFilterContext describe)
        {
            describe.For("Graphics", T("Graphics"), T("Graphics"))
                .Element("ImageOverlay", T("Image Overlay"), T("Add image overlay."),
                         ApplyFilter,
                         DisplayFilter,
                         "ImageOverlayFilter"
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            if (context.State.ImageId == null)
            {
                return;
            }

            int imageId = context.State.ImageId;
            var watermarkMediaPart = _contentManager.Get<MediaPart>(imageId);

            if (watermarkMediaPart == null)
            {
                return;
            }

            if (context.Media.CanSeek)
            {
                context.Media.Seek(0, SeekOrigin.Begin);
            }

            var watermarkImage = Image.FromStream(_storageProvider.GetFile(Path.Combine(watermarkMediaPart.FolderPath, watermarkMediaPart.FileName)).OpenRead());
            var originalImage = Image.FromStream(context.Media);

            int width = ParseUtils.ParseInt((string)context.State.Width);
            int height = ParseUtils.ParseInt((string)context.State.Height);
            ContentAlignment alignment = ParseUtils.ParseEnum<ContentAlignment>((string)context.State.Alignment);
            int margin = ParseUtils.ParseInt((string)context.State.Margin);
            int opacity = ParseUtils.ParseInt((string)context.State.Opacity);
            var position = new Point();

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    position.Y = margin;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    position.Y = (originalImage.Height - height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    position.Y = originalImage.Height - height - margin;
                    break;
            }

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    position.X = margin;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    position.X = (originalImage.Width - width) / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    position.X = originalImage.Width - width - margin;
                    break;
            }

            var result = new MemoryStream();
            if (context.Media.CanSeek)
            {
                context.Media.Seek(0, SeekOrigin.Begin);
            }

            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
            {
                imageFactory.Load(context.Media)
                    .Overlay(new ImageLayer
                    {
                        Image = watermarkImage,
                        Size = new Size(width, height),
                        Opacity = opacity,
                        Position = position
                    })
                    .Save(result);

                context.Media = result;
            }
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            return T("Add image overlay");
        }
    }
}