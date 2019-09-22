namespace ProjectileMotionWeb.Models
{
    public class BaseModel
    {
        public BaseModel ()
        {
            Layout = new LayoutModel("Stránka bez názvu");
            DefaultEditorForAdditionalData = new _EditorForHtmlAdditionalDataModel();
            PagePreviouslyVisited = false;
        }


        public BaseModel (LayoutModel layout)
        {
            Layout = layout;
        }

        public bool PagePreviouslyVisited { get; set; }

        public LayoutModel Layout { get; set; }

        public _EditorForHtmlAdditionalDataModel DefaultEditorForAdditionalData { get; set; }
    }
}