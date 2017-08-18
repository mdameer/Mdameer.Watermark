using ImageProcessor;
using ImageProcessor.Imaging;
using Orchard.ContentManagement;
using Orchard.FileSystems.Media;
using Orchard.Localization;
using Orchard.MediaProcessing.Descriptors.Filter;
using Orchard.MediaProcessing.Services;
using System.Drawing;
using System.IO;

namespace Mdameer.Watermark.Providers.Filters
{
    public class TextWatermarkFilter : IImageFilterProvider
    {
        private readonly IContentManager _contentManager;
        private readonly IStorageProvider _storageProvider;

        public TextWatermarkFilter(
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
                .Element("TextWatermark", T("Text Watermark"), T("Add text watermark."),
                         ApplyFilter,
                         DisplayFilter,
                         "TextWatermarkFilter"
                );
        }

        public void ApplyFilter(FilterContext context)
        {
            if (context.Media.CanSeek)
            {
                context.Media.Seek(0, SeekOrigin.Begin);
            }

            var originalImage = Image.FromStream(context.Media);

            string text = context.State.Text;
            bool rightToLeft = ParseUtils.ParseBoolean((string)context.State.RightToLeft);
            FontFamily fontFamily = !string.IsNullOrWhiteSpace((string)context.State.FontFamily) ? new FontFamily((string)context.State.FontFamily) : FontFamily.GenericSansSerif;
            var fontSize = ParseUtils.ParseInt((string)context.State.FontSize);
            var fontStyle = ParseUtils.ParseEnum<FontStyle>(((string)context.State.FontStyle).Replace("-", ", "));
            var fontColor = ParseUtils.ParseColor((string)context.State.FontColor);
            var underline = ParseUtils.ParseBoolean((string)context.State.Underline);
            var strikeout = ParseUtils.ParseBoolean((string)context.State.Strikeout);
            ContentAlignment alignment = ParseUtils.ParseEnum<ContentAlignment>((string)context.State.Alignment);
            int margin = ParseUtils.ParseInt((string)context.State.Margin);
            int opacity = ParseUtils.ParseInt((string)context.State.Opacity);
            var dropShadow = ParseUtils.ParseBoolean((string)context.State.DropShadow);

            if (underline)
            {
                fontStyle |= FontStyle.Underline;
            }

            if (strikeout)
            {
                fontStyle |= FontStyle.Strikeout;
            }

            float width = 0;
            float height = 0;
            var position = new Point();

            using (Graphics graphics = Graphics.FromImage(originalImage))
            {
                using (Font font = new Font(fontFamily, fontSize, fontStyle, GraphicsUnit.Pixel))
                {
                    using (StringFormat drawFormat = new StringFormat(StringFormat.GenericTypographic))
                    {
                        SizeF textSize = graphics.MeasureString(text, font, new SizeF(originalImage.Width, originalImage.Height), drawFormat);
                        width = textSize.Width;
                        height = textSize.Height;
                    }
                }
            }

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
                    position.Y = (int)(originalImage.Height - height) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    position.Y = (int)(originalImage.Height - height) - margin;
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
                    position.X = (int)(originalImage.Width - width) / 2;
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    position.X = (int)(originalImage.Width - width) - margin;
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
                    .Watermark(new TextLayer
                    {
                        Text = text,
                        FontColor = fontColor,
                        DropShadow = dropShadow,
                        Opacity = opacity,
                        FontFamily = fontFamily,
                        FontSize = fontSize,
                        Position = position,
                        RightToLeft = rightToLeft,
                        Style = fontStyle
                    })
                    .Save(result);

                context.Media = result;
            }
        }

        public LocalizedString DisplayFilter(FilterContext context)
        {
            return T("Add text watermark");
        }
    }
}