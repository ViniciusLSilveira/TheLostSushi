using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public static StateManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    public string m_Filename = "save.sav";
    public bool m_UseCrypto;
    private string m_LevelName;

    private List<ISerializable> m_Data = new List<ISerializable>();

    private byte[] m_Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
    private byte[] m_IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    private void Start() {
        Load();
    }

    public void Register(ISerializable data) {
        m_Data.Add(data);
    }

    public void PersistentSave(string levelName)
    {
        string tempFile = m_Filename;
        m_Filename = "save.sav";
        Save();
        m_Filename = tempFile;
    }

    private string ReadSaveFile()
    {
        string path = $"{Application.dataPath}/../{m_Filename}";
        string json = string.Empty;
        if (File.Exists(path)) {
            if (m_UseCrypto) {
                byte[] decryptedData = File.ReadAllBytes(path);
                json = Decrypt(decryptedData);
            }
            else {
                StreamReader reader = new StreamReader(path);
                json = reader.ReadToEnd();
                reader.Close();
            }
        }

        return json;
    }

    public void Save() {
        string path = $"{Application.dataPath}/../{m_Filename}";
        JObject data = new JObject();
        foreach (var obj in m_Data)
            data.Add(obj.GetKey(), obj.Serialize());

        if (m_UseCrypto) {
            File.WriteAllBytes(path, Encrypt(data.ToString()));
        }
        else {
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(data.ToString());
            writer.Close();
        }
    }

    private byte[] Encrypt(string json) {
        AesManaged aes = new AesManaged();
        aes.Padding = PaddingMode.Zeros;

        var encryptor = aes.CreateEncryptor(m_Key, m_IV);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        StreamWriter writer = new StreamWriter(cryptoStream);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();

        cryptoStream.Close();

        byte[] data = memoryStream.ToArray();
        memoryStream.Close();

        return data;
    }

    public void Load() {

        string json = ReadSaveFile();

        if (json == string.Empty) return;

        JObject data = JObject.Parse(json);
        foreach (var obj in m_Data)
            obj.Deserialize(data[obj.GetKey()].ToString());
    }

    private string Decrypt(byte[] data) {
        AesManaged aes = new AesManaged();
        aes.Padding = PaddingMode.Zeros;

        var decryptor = aes.CreateDecryptor(m_Key, m_IV);
        MemoryStream memoryStream = new MemoryStream(data);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        StreamReader reader = new StreamReader(cryptoStream);
        string json = reader.ReadToEnd();

        reader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return json;
    }
}