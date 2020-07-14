using Newtonsoft.Json.Linq;
public interface ISerializable
{
    string GetKey();
    JObject Serialize();
    void Deserialize(string json);
}
