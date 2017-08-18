using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
using System;
using System.Drawing;
using System.Web.Mvc;

namespace Mdameer.Watermark.Forms
{
    public class TextWatermarkFilterForm : IFormProvider
    {
        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public TextWatermarkFilterForm(
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
                        Id: "TextWatermarkFilter",
                         _Text: Shape.Textbox(
                            Id: "text", Name: "Text",
                            Title: T("Text"),
                            Value: "",
                            Description: T("The Text to use as watermark."),
                            Classes: new[] { "text small" }),
                         _RightToLeft: Shape.Checkbox(
                            Id: "rightToLeft", Name: "RightToLeft",
                            Title: T("Right To Left"),
                            Value: false,
                            Description: T("Text direction is right to left or not.")),
                        _FontFamily: Shape.SelectList(
                            Id: "fontFamily", Name: "FontFamily",
                            Title: T("Font Family"),
                            Description: T("Select the font family of the watermark text."),
                            Size: 1,
                            Multiple: false),
                        _FontSize: Shape.Textbox(
                            Id: "fontSize", Name: "FontSize",
                            Title: T("Font Size"),
                            Value: 13,
                            Description: T("The font size of the watermark text."),
                            Classes: new[] { "text small" }),
                        _FontColor: Shape.Textbox(
                            Id: "fontColor", Name: "FontColor",
                            Title: T("Font Color"),
                            Value: "",
                            Description: T("The font color of the watermark text."),
                            Classes: new[] { "text small" }),
                        _FontStyle: Shape.SelectList(
                            Id: "fontStyle", Name: "FontStyle",
                            Title: T("Font Style"),
                            Description: T("Select the font style of the watermark text."),
                            Size: 1,
                            Multiple: false),
                        _Underline: Shape.Checkbox(
                            Id: "underline", Name: "Underline",
                            Title: T("Underline"),
                            Value: true),
                        _Strikeout: Shape.Checkbox(
                            Id: "strikeout", Name: "Strikeout",
                            Title: T("Strikeout"),
                            Value: true),
                       _Alignment: Shape.SelectList(
                            Id: "alignment", Name: "Alignment",
                            Title: T("Alignment"),
                            Description: T("Select the alignment of the watermark text."),
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
                            Classes: new[] { "text small" }),
                        _DropShadow: Shape.Checkbox(
                            Id: "dropShadow", Name: "DropShadow",
                            Title: T("DropShadow"),
                            Value: true,
                            Description: T("Add shadow to watermark text."))
                        );

                    foreach (var fontFamily in FontFamily.Families)
                    {
                        f._FontFamily.Add(new SelectListItem { Value = fontFamily.Name, Text = fontFamily.Name });
                    }

                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopLeft.ToString(), Text = T("Top Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopCenter.ToString(), Text = T("Top Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.TopRight.ToString(), Text = T("Top Right").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleLeft.ToString(), Text = T("Middle Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleCenter.ToString(), Text = T("Middle Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.MiddleRight.ToString(), Text = T("Middle Right").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomLeft.ToString(), Text = T("Bottom Left").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomCenter.ToString(), Text = T("Bottom Center").Text });
                    f._Alignment.Add(new SelectListItem { Value = ContentAlignment.BottomRight.ToString(), Text = T("Bottom Right").Text });

                    f._FontStyle.Add(new SelectListItem { Value = FontStyle.Regular.ToString(), Text = T("Regular").Text });
                    f._FontStyle.Add(new SelectListItem { Value = FontStyle.Bold.ToString(), Text = T("Bold").Text });
                    f._FontStyle.Add(new SelectListItem { Value = FontStyle.Italic.ToString(), Text = T("Italic").Text });
                    f._FontStyle.Add(new SelectListItem { Value = (FontStyle.Bold | FontStyle.Italic).ToString().Replace(", ", "-"), Text = T("Bold & Italic").Text });

                    return f;
                };

            context.Form("TextWatermarkFilter", form);
        }
    }
}