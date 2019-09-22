using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ProjectileMotionWeb.Models
{
    public class _FormResultUnitGroupModel : BaseModel
    {
        public _FormResultUnitGroupModel(MvcHtmlString labelForResultUnit, MvcHtmlString nameForResultUnit, Type unitClassType, string resultUnitName)
        {
            LabelForResultUnit = labelForResultUnit;
            NameForResultUnit = nameForResultUnit;
            UnitClassType = unitClassType;
            ResultUnitName = resultUnitName;
        }

        public MvcHtmlString LabelForResultUnit { get; set; }

        public MvcHtmlString NameForResultUnit { get; set; }

        public Type UnitClassType { get; set; }

        public string ResultUnitName { get; set; }

    }
}