using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
using System;
using System.Drawing;
using System.Web.Mvc;

namespace Mdameer.Watermark.Forms
{
    public class ImageOverlayFilterForm : IFormProvider
    {
        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public ImageOverlayFilterForm(
            IShapeFactory shapeFactory)
        {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context)
        {
            Func<IShapeFactory, object> form =
                shape =>
                {
                    var f = Shape.Form(
                        Id: "ImageOverlayFilter",
                        _ImageId: Shape.MediaLibraryPickerEditor(
                            Name: "ImageId",
                            FieldName: "ImageId",
                            DisplayName: "Image",
                            Required: true,
                            Hint: T("The Image.").Text,
                            ShowSaveWarning: true,
                            Value: "0"
                            ),
                         _Width: Shape.Textbox(
                            Id: "width", Name: "Width",
                            Title: T("Width"),
                            Value: 0,
                            Description: T("The Width."),
                            Classes: new[] { "text small" }),
                        _Height: Shape.Textbox(
                            Id: "height", Name: "Height",
                            Title: T("Height"),
                            Value: 0,
                            Description: T("The Height."),
                            Classes: new[] { "text small" }),
                       _Alignment: Shape.SelectList(
                            Id: "alignment", Name: "Alignment",
                            Title: T("Alignment"),
                            Description: T("Select the alignment of the overlay image."),
                            Size: 1,
                            Multiple: false),
                        _Margin: Shape.Textbox(
                            Id: "margin", Name: "Margin",
                            Title: T("Margin"),
                            Value: 0,
                            Description: T("The margin in pixels."),
                            Classes: new[] { "text small" }),
                        _Opacity: Shape.Textbox(
                            Id: "opacity", Name: "Opacity",
                            Title: T("Opacity"),
                            Value: 80,
                            Description: T("The opacity of the overlay image, between 0 and 100."),
                            Classes: new[] { "text small" })
                        );

                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopLeft.ToString(), Text = T("Top Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopCenter.ToString(), Text = T("Top Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopRight.ToString(), Text = T("Top Right").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleLeft.ToString(), Text = T("Middle Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleCenter.ToString(), Text = T("Middle Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleRight.ToString(), Text = T("Middle Right").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomLeft.ToString(), Text = T("Bottom Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomCenter.ToString(), Text = T("Bottom Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomRight.ToString(), Text = T("Bottom Right").Text });

                    return f;
                };

            context.Form("ImageOverlayFilter", form);
        }
    }
}