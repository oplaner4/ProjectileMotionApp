
namespace ProjectileMotionWeb.Models
{
    public class _EditorForHtmlAdditionalDataModel
    {
        public _EditorForHtmlAdditionalDataModel(_EditorForHtmlAttributesModel htmlAttributes)
        {
            this.HtmlAttributes = htmlAttributes;
        }

        public _EditorForHtmlAdditionalDataModel ()
        {
            this.HtmlAttributes = new _EditorForHtmlAttributesModel();
        }

        public _EditorForHtmlAttributesModel HtmlAttributes {get; set;}
    }
}