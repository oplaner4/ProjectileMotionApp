using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectileMotionWeb.Helpers
{
    public class JsonSerializerHelper
    {
        public JsonSerializerHelper(object objToSerialize)
        {
            ObjToSerialize = objToSerialize;

        }

        public string Serialize ()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings() {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(ObjToSerialize, serializerSettings);
        }

        public object ObjToSerialize { get; private set; }
    }
}