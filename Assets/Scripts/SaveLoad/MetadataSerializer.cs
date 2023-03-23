using System;
using Newtonsoft.Json;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Reflection;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using System.Linq;

namespace Contaquest.Metaverse.Data
{
    public class MetadataSerializer : PersistentGenericSingleton<MetadataSerializer>
    {
        private const string propertiesPrefix = ",\"properties\":";

        #if UNITY_EDITOR
        #region Tests
        [Header("Serialization")]
        [TabGroup("RobotPartData")] public NFTData[] nftDatas;
        [TabGroup("RobotPartData")] public RobotPart[] robotPartDatas;
        [TabGroup("RobotPartData")] public TextAsset[] robotPartDataJsonFiles;

        [TabGroup("Founders Token")] public FoundersToken tokenData;

        [TabGroup("CSV")] public string csvFile = "/Data/Metadata/Metadata.csv";


        [Button]
        [TabGroup("Founders Token")]
        public void TestSerializeToken()
        {
            NFTData nftData = (nftDatas.Length <= 1 && nftDatas[1] != null) ? nftData = nftDatas[1] : new NFTData("", "", "");

            string serializedString = Serialize(new MetaData(nftData, tokenData));
            File.WriteAllText(Application.dataPath + $"/Data/Metadata/{nftData.name.Replace(" ", string.Empty)}.json", serializedString);
        }

        [Button]
        [TabGroup("RobotPartData")]
        public void TestSerializeRobotData()
        {
            for (int i = 0; i < robotPartDatas.Length; i++)
            {
                NFTData nftData = (i < nftDatas.Length && nftDatas[i] != null) ? nftDatas[i] : new NFTData("", "", "");
                string serializedString = Serialize(new MetaData(nftData, robotPartDatas[i]));
                File.WriteAllText(Application.dataPath + $"/Data/Metadata/{nftData.name.Replace(" ", string.Empty)}.json", serializedString);
            }
        }

        [Button]
        [TabGroup("RobotPartData")]
        public void TestDeSerializeRobotData()
        {
            robotPartDatas = new RobotPart[robotPartDataJsonFiles.Length];
            nftDatas = new NFTData[robotPartDatas.Length];
            for (int i = 0; i < robotPartDataJsonFiles.Length; i++)
            {
                string serializedString = robotPartDataJsonFiles[i].text;//File.ReadAllText(Application.dataPath + "/Testing/serializedData.json");
                //Debug.Log($"Deserializing {testJson[i].name}");

                MetaData metaData = Deserialize(serializedString, robotPartDataJsonFiles[i].name);
                robotPartDatas[i] = metaData.itemData as RobotPart;
                nftDatas[i] = metaData.nftData;
            }
        }

        [Button]
        [TabGroup("CSV")]
        public void SerializeToCSV()
        {
            List<MetaDataSimple> metaDataSimples = new List<MetaDataSimple>();
            for (int i = 0; i < nftDatas.Length; i++)
            {
                MetaDataSimple metaDataSimple = robotPartDatas[i].GetMetaDataSimple();
                metaDataSimple.Name = nftDatas[i].name;
                metaDataSimple.Description = nftDatas[i].description;
                metaDataSimple.Image = nftDatas[i].image;
                metaDataSimples.Add(metaDataSimple);
            }
            metaDataSimples.Add(new MetaDataSimple());


            IEnumerable records = metaDataSimples;
            var writer = new StreamWriter(Application.dataPath + csvFile);
            using (writer)
            {
                CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                using (csv)
                {
                    csv.WriteRecords(records);
                }
            }
        }

        [Button]
        [TabGroup("CSV")]
        public void DeSerializeFromCSV()
        {
            List<MetaDataSimple> records = new List<MetaDataSimple>();

            StreamReader reader = new StreamReader(Application.dataPath + csvFile);
            using (reader)
            {
                CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                using (csv)
                {
                    records = csv.GetRecords<MetaDataSimple>().ToList();
                }
            }
            IEnumerable<MetaData> metaDatas = records.Select((arg) => arg.GetMetaData());
            nftDatas = metaDatas.Select((arg) => arg.nftData).ToArray();
            robotPartDatas = metaDatas.Select((arg) => arg.itemData as RobotPart).ToArray();
        }

        #endregion
        #endif

        #region Serialization
        public string Serialize(MetaData metaData)
        {
            metaData.itemData.InitializeValuesBeforeSerialization();

            string nftDataString = JsonConvert.SerializeObject(metaData.nftData);
            string itemDataString = JsonConvert.SerializeObject(metaData.itemData);

            string propertiesString = propertiesPrefix + itemDataString;

            int insertionIndex = nftDataString.Length - 1;
            string serializedString = nftDataString.Insert(insertionIndex, propertiesString);

            return serializedString;
        }

        #endregion

        #region Deserialization and Data Interpretation
        public MetaData Deserialize(string jsonMetadata, string file = "")
        {
            int propertyStartIndex = GetPropertyStartIndex(jsonMetadata);

            NFTData nftData = GetNFTDataFromString(jsonMetadata, propertyStartIndex);
            Item itemData = GetItemDataFromString(jsonMetadata, propertyStartIndex, file);

            if(itemData != null)
                itemData.nftData = nftData;
            if(nftData != null)
            nftData.itemData = itemData;

            MetaData metaData = new MetaData(nftData, itemData);
            return metaData;
        }
        private int GetPropertyStartIndex(string jsonMetadata)
        {
            return jsonMetadata.IndexOf(propertiesPrefix, 38, StringComparison.OrdinalIgnoreCase);
        }

        private NFTData GetNFTDataFromString(string jsonMetadata, int propertyStartIndex)
        {
            if(propertyStartIndex == -1)
            {
                Debug.LogError("Could not find NFT Data in metadata: " + jsonMetadata);
            }
            string nftDataString = jsonMetadata.Remove(propertyStartIndex, jsonMetadata.Length - propertyStartIndex - 1);
            if (nftDataString[nftDataString.Length - 1] != '}')
                nftDataString += '}';
            //NFTData nFTData = JsonUtility.FromJson<NFTData>(nftDataString);
            NFTData nFTData = null;
            try
            {
                nFTData = JsonConvert.DeserializeObject<NFTData>(nftDataString);
            }
            catch
            {
                Debug.LogWarning("Failed to deserialize NFT string: " + nftDataString);
                return null;
            }
            return nFTData;
        }

        private Item GetItemDataFromString(string jsonMetadata, int propertyStartIndex, string file = "")
        {
            int propertiesPrefixLength = propertiesPrefix.Length;

            string itemDataString = jsonMetadata.Remove(jsonMetadata.Length - 1).Remove(0, propertyStartIndex + propertiesPrefixLength);
            if (itemDataString[itemDataString.Length - 2] == '}')
                itemDataString = itemDataString.Remove(itemDataString.Length - 1);
            string variableName = "{\"Item Type\":\"";
            int variableNameLength = variableName.Length;

            int endIndex = itemDataString.IndexOf(',', variableNameLength);
            string typeString = itemDataString.Substring(variableNameLength, endIndex - variableNameLength - 1);

            typeString = typeString.Replace(" ", string.Empty);
            typeString = "Contaquest.Metaverse.Data." + typeString;

            Type type = GetTypeFromString(typeString);

            if (type == null)
            {
                Debug.LogWarning($"The Itemtype inside the Json Metadata {file} is invalid: " + typeString);
                return null;
            }
            Item itemData = JsonConvert.DeserializeObject(itemDataString, type) as Item;
            if (itemData != null)
                itemData.Initialize();
            else
            {
                Debug.Log(itemDataString);
                Debug.LogWarning($"{file} has invalid metadata");
            }
            return itemData;
        }

        private Type GetTypeFromString(string typeString)
        {
            Assembly assembly = typeof(Item).Assembly;
            return assembly.GetType(typeString);
        }

        #endregion
    }
}