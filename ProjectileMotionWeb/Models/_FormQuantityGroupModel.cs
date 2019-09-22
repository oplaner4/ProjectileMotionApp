using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjectileMotionWeb.Models
{
    public class _FormQuantityGroupModel : BaseModel
    {
        public _FormQuantityGroupModel(MvcHtmlString editorForQuantity, MvcHtmlString labelForQuantity, MvcHtmlString validationMessageForQuantity, MvcHtmlString nameForQuantity = null, MvcHtmlString nameForUnit = null, string selectedUnit = null, Type unitClassType = null, Dictionary<string, double> predefinedDic = null)
        {
            EditorForQuantity = editorForQuantity;
            NameForQuantity = nameForQuantity;
            NameForUnit = nameForUnit;
            UnitClassType = unitClassType;
            PredefinedDic = predefinedDic;
            LabelForQuantity = labelForQuantity;
            ValidationMessageForQuantity = validationMessageForQuantity;
            HasPredefined = PredefinedDic != null && NameForQuantity != null;
            HasUnit = UnitClassType != null && NameForUnit != null;
            SelectedUnit = selectedUnit;
        }

        public MvcHtmlString EditorForQuantity { get; set; }

        public MvcHtmlString LabelForQuantity { get; set; }

        public MvcHtmlString ValidationMessageForQuantity { get; set; }

        public MvcHtmlString NameForQuantity { get; set; }

        public MvcHtmlString NameForUnit { get; set; }

        public Type UnitClassType { get; set; }

        public Dictionary<string, double> PredefinedDic { get; set; }

        public bool HasPredefined { get; private set; }

        public bool HasUnit { get; private set; }

        public string SelectedUnit { get; set; }
    }
}