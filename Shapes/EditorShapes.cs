using System.IO;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Orchard.MediaLibrary.Models;

namespace Mdameer.Watermark.Shapes
{
    public class EditorShapes : IShapeTableProvider {
        private readonly IContentManager _contentManager;

        public EditorShapes(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public void Discover(ShapeTableBuilder builder) {
            
        }

        [Shape]
        public void MediaLibraryPickerEditor(
            TextWriter Output,
            dynamic Display,
            dynamic Shape,
            string Value,
            string Name,
            string FieldName,
            string DisplayName,
            bool Required,
            string Hint,
            bool ShowSaveWarning) {

            int val = ParseUtils.ParseInt(Value);

            var media = _contentManager.Get<MediaPart>(val);

            Output.Write(Display.MediaLibraryPicker(
                FieldName: FieldName,
                DisplayName: DisplayName,
                Required: Required,
                Hint: Hint,
                ShowSaveWarning: ShowSaveWarning,
                ContentItems: media != null ? new ContentItem[] { media.ContentItem } : Enumerable.Empty<ContentItem>()
                ));
        }
    }
}
