
namespace ProjectileMotionWeb.Models
{
    public class _EditorForHtmlAdditionalDataModel
    {
        public _EditorForHtmlAdditionalDataModel(_EditorForHtmlAttributesModel htmlAttributes)
        {
            HtmlAttributes = htmlAttributes;
        }

        public _EditorForHtmlAdditionalDataModel ()
        {
            HtmlAttributes = new _EditorForHtmlAttributesModel();
        }

        public _EditorForHtmlAttributesModel HtmlAttributes {get; set;}
    }
}