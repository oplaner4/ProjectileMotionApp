using System.Collections.Generic;

namespace ProjectileMotionWeb.Models
{
    public class _MotionInformationModel : BaseModel
    {
        public _MotionInformationModel (Dictionary<string, string> information)
        {
            Information = information;
        }

        public Dictionary<string, string> Information { get; set; }
    }
}