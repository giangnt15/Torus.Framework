using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torus.Framework.Core.Serialization
{
    public class JsonSerializerFactory
    {
        private static readonly SystemTextJsonSerializer _systemTextJsonSerializer;
        private static readonly NewtonSoftJsonSerializer _newtonSoftJsonSerializer;

        static JsonSerializerFactory()
        {
            _systemTextJsonSerializer = new SystemTextJsonSerializer();
            _newtonSoftJsonSerializer = new NewtonSoftJsonSerializer();
        }

        public static IJsonSerializer GetSerializer<T>() where T : IJsonSerializer
        {
            if (_systemTextJsonSerializer.GetType() == typeof(T))
            {
                return _systemTextJsonSerializer;
            }
            else if (_newtonSoftJsonSerializer.GetType() == typeof(T))
            {
                return _newtonSoftJsonSerializer;
            }
            throw new InvalidOperationException("Can't find appropriate serializer");
        }
    }
}
