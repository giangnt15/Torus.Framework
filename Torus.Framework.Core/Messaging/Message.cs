using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torus.Framework.Core.Serialization;

namespace Torus.Framework.Core.Messaging
{
    public class Message : IMessage
    {
        public Guid Id { get; set; }

        public string ToJson()
        {
            return JsonSerializerFactory.GetSerializer<SystemTextJsonSerializer>().Serialize(this);
        }

        public static T FromJson<T>(string json) where T : IMessage
        {
            return JsonSerializerFactory.GetSerializer<SystemTextJsonSerializer>().Deserialize<T>(json);
        }
    }
}
