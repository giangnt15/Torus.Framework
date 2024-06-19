using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        string Serialize<T>(T value);

        object Deserialize(string value, Type returnType);

        T Deserialize<T>(string value);
    }

    public class SystemTextJsonSerializer : IJsonSerializer
    {
        internal SystemTextJsonSerializer() {
        }

        public object Deserialize(string value, Type returnType)
        {
            return System.Text.Json.JsonSerializer.Deserialize(value, returnType);
        }

        public T Deserialize<T>(string value)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public string Serialize(object value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value);
        }

        public string Serialize<T>(T value)
        {
            return System.Text.Json.JsonSerializer.Serialize(value);
        }

    }

    public class NewtonSoftJsonSerializer : IJsonSerializer
    {
        internal NewtonSoftJsonSerializer() { }
        public object Deserialize(string value, Type returnType)
        {
            return JsonConvert.DeserializeObject(value, returnType);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
