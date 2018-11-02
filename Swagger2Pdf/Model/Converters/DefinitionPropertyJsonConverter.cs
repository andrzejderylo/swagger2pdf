using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Swagger2Pdf.Model.Converters
{
    //public class DefinitionPropertyJsonConverter : JsonConverter
    //{
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        var jObject = JToken.ReadFrom(reader);
    //        var jObjectType = jObject["type"]?.ToString();
    //        if (jObjectType == "array")
    //        {
    //            var arrayType = jObject.ToObject<ArrayProperty>();
    //            return arrayType;
    //        }
    //
    //        if (jObjectType != null)
    //        {
    //            return jObject.ToObject<Property>();
    //        }
    //
    //        var defRef = jObject.ToObject<DefinitionReference>();
    //        return defRef;
    //    }
    //
    //    public override bool CanConvert(Type objectType) => true;
    //
    //    public override bool CanWrite => false;
    //
    //    public override bool CanRead => true;
    //
    //}
}