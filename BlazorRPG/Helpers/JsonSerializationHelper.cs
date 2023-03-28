using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BlazorRPG.Helpers
{
    public static class JsonSerializationHelper
    {
        /// <summary>
        /// Deserializes the specified type and returns a list of items.
        /// Reads the json file from the assembly's resource manifest.
        /// </summary>
        /// <typeparam name="T">Type of entity to deserialize.</typeparam>
        /// <param name="resourceNamespace">Full namespace path to the resource file.</param>
        /// <returns>List of entities deserialized.</returns>
        public static IList<T> DeserializeResourceStream<T>(string resourceNamespace)
        {
            try
            {
                var assembly = typeof(T).GetTypeInfo().Assembly;
                var resourceStream = assembly.GetManifestResourceStream(resourceNamespace);
                StreamReader reader;
                using (reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    var elements = JsonSerializer.Deserialize<IList<T>>(json);
                    return elements;
                }
            }
            catch (JsonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error trying to load embedded resource for: {resourceNamespace}.", ex);
            }
        }
    }
}
