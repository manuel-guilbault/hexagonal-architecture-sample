using System;
using Newtonsoft.Json;

namespace HexagonalArchitectureSample.Infrastructure
{
    public class ValueObjectJsonConverter<T>: JsonConverter<T>
    {
        private readonly Func<string, T> _fromString;
        private readonly Func<T, string> _toString;

        public ValueObjectJsonConverter(Func<string, T> fromString, Func<T, string> toString)
        {
            _fromString = fromString;
            _toString = toString;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            writer.WriteValue(_toString(value));
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String || reader.Value == null)
            {
                throw new JsonSerializationException($"Expected {JsonToken.String}, found {reader.TokenType}.");
            }

            return _fromString((string)reader.Value);
        }
    }
}
