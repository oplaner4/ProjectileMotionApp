namespace ProjectileMotionWeb.Models
{
    public class _EditorForHtmlAttributesModel
    {
        public _EditorForHtmlAttributesModel()
        {
            @Class = "form-control";
            Style = "width:auto";
        }

        public string @Class { get; set; }

        public string Placeholder { get; set; }

        public string Style { get; set; }
    }


    public class _EditorForHtmlAttributesModel_Disabled : _EditorForHtmlAttributesModel
    {
        public string Disabled { get; set; }
    }


    public class _EditorForHtmlAttributesModel_Type : _EditorForHtmlAttributesModel
    {
        public _EditorForHtmlAttributesModel_Type(string type)
        {
            Type = type;
        }
        public string Type { get; set; }
    }


    public class _EditorForHtmlAttributesModel_TypeNumber : _EditorForHtmlAttributesModel_Type
    {
        public _EditorForHtmlAttributesModel_TypeNumber() : base("number")
        {
            Min = 0;
            Max = double.MaxValue;
            Step = 1.0;
        }

        public double Min { get; set; }

        public double Max { get; set; }

        public double Step { get; set; }

        public string Formnovalidate { get; set; }
    }

    public class _EditorForHtmlAttributesModel_Checked : _EditorForHtmlAttributesModel {
        public string Checked { get; set; }
    }
}