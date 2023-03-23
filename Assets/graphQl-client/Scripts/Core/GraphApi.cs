using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace GraphQlClient.Core
{
    public class QueryResponse<T>
    {
    }

    [CreateAssetMenu(fileName = "Api Reference", menuName = "GraphQlClient/Api Reference")]
    public class GraphApi : ScriptableObject
    {
        public string url;

        public List<Query> queries;

        public List<Query> mutations;

        public List<Query> subscriptions;

        private string _introspection;

        public Introspection.SchemaClass schemaClass;

        private string _authToken;

        private string _queryEndpoint;
        private string _mutationEndpoint;
        private string _subscriptionEndpoint;

        private UnityWebRequest _request;

        public bool loading;

        public void SetAuthToken(string auth)
        {
            _authToken = auth;
        }

        public Query GetQueryByName(string queryName, Query.Type type)
        {
            List<Query> querySearch;
            switch (type)
            {
                case Query.Type.Mutation:
                    querySearch = mutations;
                    break;
                case Query.Type.Query:
                    querySearch = queries;
                    break;
                case Query.Type.Subscription:
                    querySearch = subscriptions;
                    break;
                default:
                    querySearch = queries;
                    break;
            }

            return querySearch.Find(aQuery => aQuery.name == queryName);
        }

        public async Task<string> Post(Query query)
        {
            if (String.IsNullOrEmpty(query.query))
                query.CompleteQuery();
            using (var result = await HttpHandler.PostAsync(url, query.query, _authToken))
            {
                Debug.Log(result.downloadHandler.text);
                return result.downloadHandler.text;
            }
        }

        public async Task<UnityWebRequest> Post(string queryString)
        {
            using(var result = await HttpHandler.PostAsync(url, queryString, _authToken))
            {
                return result;
            }
        }

        public async Task<string> Post(string queryName, Query.Type type)
        {
            var query = GetQueryByName(queryName, type);
            return await Post(query);
        }

        public async Task<ClientWebSocket> Subscribe(Query query, string socketId = "1", string protocol = "graphql-ws")
        {
            if (String.IsNullOrEmpty(query.query))
                query.CompleteQuery();
            return await HttpHandler.WebsocketConnect(url, query.query, _authToken, socketId, protocol);
        }

        public async Task<ClientWebSocket> Subscribe(string queryName, Query.Type type, string socketId = "1",
            string protocol = "graphql-ws")
        {
            var query = GetQueryByName(queryName, type);
            return await Subscribe(query, socketId, protocol);
        }

        public async void CancelSubscription(ClientWebSocket cws, string socketId = "1")
        {
            await HttpHandler.WebsocketDisconnect(cws, socketId);
        }

        public async Task<T> GetQueryResult<T>(string queryName, [CanBeNull] object arguments, T output)
        {
            Query query = GetQueryByName(queryName, Query.Type.Query);

            if (query == null)
            {
                throw new Exception($"Error, can't find query {queryName}");
            }

            if (arguments != null) query.SetArgs(arguments);

            string queryResult = await Post(query);
            
#if UNITY_EDITOR
            // if(queryResult.Contains("errors") || queryResult.Contains("\"data\": null"))
                Debug.Log("Query result: " + queryResult);
#endif

            T deserializedResult = JsonConvert.DeserializeObject<T>(queryResult, new JsonSerializerSettings
                                                                                {
                                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                                });
            return deserializedResult;
        }

        public async Task<T> GetMutationResult<T>(string mutationName, [CanBeNull] object arguments, T output)
        {
            Query query = GetQueryByName(mutationName, Query.Type.Mutation);

            if (query == null)
            {
                throw new Exception($"Error, can't find query {mutationName}");
            }

            if (arguments != null) query.SetArgs(arguments);

            var queryResult = await Post(query);

            #if UNITY_EDITOR
            // if(queryResult.Contains("errors") || queryResult.Contains("\"data\": null"))
                Debug.Log("Query result: " + queryResult);
            #endif

            T deserializedResult = JsonConvert.DeserializeObject<T>(queryResult);
            return deserializedResult;
        }


        #region Utility

        private static string JsonToArgument(string jsonInput)
        {
            var jsonChar = jsonInput.ToCharArray();
            var indexes = new List<int>();
            jsonChar[0] = ' ';
            jsonChar[jsonChar.Length - 1] = ' ';
            for (var i = 0; i < jsonChar.Length; i++)
            {
                if (jsonChar[i] == '\"')
                {
                    if (indexes.Count == 2)
                        indexes = new List<int>();
                    indexes.Add(i);
                }

                if (jsonChar[i] == ':')
                {
                    jsonChar[indexes[0]] = ' ';
                    jsonChar[indexes[1]] = ' ';
                }
            }

            var result = new string(jsonChar);
            return result;
        }

        #endregion

#if UNITY_EDITOR

        #region Editor Use

        //Todo: Put schema file in proper location
        public async void Introspect()
        {
            loading = true;
            try
            {
                _request = await HttpHandler.PostAsync(url, Introspection.schemaIntrospectionQuery, _authToken);
            }
            catch
            {
                loading = false;
                throw;
            }

            EditorApplication.update += HandleIntrospection;
        }

        void HandleIntrospection()
        {
            if (!_request.isDone)
                return;
            EditorApplication.update -= HandleIntrospection;
            _introspection = _request.downloadHandler.text;
            File.WriteAllText(Application.dataPath + $"{Path.DirectorySeparatorChar}{name}schema.txt", _introspection);
            schemaClass = JsonConvert.DeserializeObject<Introspection.SchemaClass>(_introspection);
            if (schemaClass.data.__schema.queryType != null)
                _queryEndpoint = schemaClass.data.__schema.queryType.name;
            if (schemaClass.data.__schema.mutationType != null)
                _mutationEndpoint = schemaClass.data.__schema.mutationType.name;
            if (schemaClass.data.__schema.subscriptionType != null)
                _subscriptionEndpoint = schemaClass.data.__schema.subscriptionType.name;
            loading = false;
        }

        public void GetSchema()
        {
            if (schemaClass?.data != null) return;

            try
            {
                _introspection =
                    File.ReadAllText(Application.dataPath + $"{Path.DirectorySeparatorChar}{name}schema.txt");
                schemaClass = JsonConvert.DeserializeObject<Introspection.SchemaClass>(_introspection);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error on GetSchema: {e}");
                return;
            }

            if (schemaClass.data.__schema.queryType != null)
                _queryEndpoint = schemaClass.data.__schema.queryType.name;
            if (schemaClass.data.__schema.mutationType != null)
                _mutationEndpoint = schemaClass.data.__schema.mutationType.name;
            if (schemaClass.data.__schema.subscriptionType != null)
                _subscriptionEndpoint = schemaClass.data.__schema.subscriptionType.name;
        }


        public void CreateNewQuery()
        {
            GetSchema();
            if (queries == null)
                queries = new List<Query>();
            var query = new Query
                { fields = new List<Field>(), queryOptions = new List<string>(), type = Query.Type.Query };

            var queryType =
                schemaClass.data.__schema.types.Find((aType => aType.name == _queryEndpoint));
            for (var i = 0; i < queryType.fields.Count; i++)
            {
                query.queryOptions.Add(queryType.fields[i].name);
            }

            queries.Add(query);
        }

        public void CreateNewMutation()
        {
            GetSchema();
            mutations ??= new List<Query>();

            var mutation = new Query
                { fields = new List<Field>(), queryOptions = new List<string>(), type = Query.Type.Mutation };

            var mutationType =
                schemaClass?.data?.__schema?.types?.Find(aType => aType.name == _mutationEndpoint);
            if (mutationType == null)
            {
                Debug.Log("No mutations");
                return;
            }

            foreach (var field in mutationType.fields)
            {
                mutation.queryOptions.Add(field.name);
            }

            mutations.Add(mutation);
        }

        public void CreateNewSubscription()
        {
            GetSchema();
            if (subscriptions == null)
                subscriptions = new List<Query>();
            var subscription = new Query
                { fields = new List<Field>(), queryOptions = new List<string>(), type = Query.Type.Subscription };

            var subscriptionType =
                schemaClass.data.__schema.types.Find((aType => aType.name == _subscriptionEndpoint));
            if (subscriptionType == null)
            {
                Debug.Log("No subscriptions");
                return;
            }

            for (var i = 0; i < subscriptionType.fields.Count; i++)
            {
                subscription.queryOptions.Add(subscriptionType.fields[i].name);
            }

            subscriptions.Add(subscription);
        }

        public void EditQuery(Query query)
        {
            query.isComplete = false;
        }


        public bool CheckSubFields(string typeName)
        {
            var type = schemaClass?.data?.__schema?.types?.Find(aType => aType.name == typeName);

            return type?.fields != null && type.fields.Count != 0;
        }

        //ToDo: Do not allow addition of subfield that already exists
        public void AddField(Query query, string typeName, Field parent = null)
        {
            var type = schemaClass.data.__schema.types.Find(aType => aType.name == typeName);
            var subFields = type.fields;
            var parentIndex = query.fields.FindIndex(aField => aField == parent);
            var parentIndexes = new List<int>();
            if (parent != null)
            {
                parentIndexes = new List<int>(parent.parentIndexes) { parentIndex };
            }

            var fielder = new Field { parentIndexes = parentIndexes };

            foreach (var field in subFields)
            {
                fielder.possibleFields.Add((Field)field);
            }

            if (fielder.parentIndexes.Count == 0)
            {
                query.fields.Add(fielder);
            }
            else
            {
                var index = query.fields.FindLastIndex(aField =>
                    aField.parentIndexes.Count > fielder.parentIndexes.Count &&
                    aField.parentIndexes.Contains(fielder.parentIndexes.Last()));

                if (index == -1)
                {
                    index = query.fields.FindLastIndex(aField =>
                        aField.parentIndexes.Count > fielder.parentIndexes.Count &&
                        aField.parentIndexes.Last() == fielder.parentIndexes.Last());
                }

                if (index == -1)
                {
                    index = fielder.parentIndexes.Last();
                }

                index++;
                query.fields[parentIndex].hasChanged = false;
                query.fields.Insert(index, fielder);
            }
        }

        private string GetFieldType(Introspection.SchemaClass.Data.Schema.Type.Field field)
        {
            var newField = (Field)field;
            return newField.type;
        }


        public void GetQueryReturnType(Query query, string queryName)
        {
            var endpoint = query.type switch
            {
                Query.Type.Query => _queryEndpoint,
                Query.Type.Mutation => _mutationEndpoint,
                Query.Type.Subscription => _subscriptionEndpoint,
                _ => _queryEndpoint
            };

            var queryType =
                schemaClass.data.__schema.types.Find((aType => aType.name == endpoint));
            var field =
                queryType.fields.Find((aField => aField.name == queryName));

            query.returnType = GetFieldType(field);
        }

        public void DeleteQuery(List<Query> query, int index)
        {
            query.RemoveAt(index);
        }

        public void DeleteAllQueries()
        {
            queries = new List<Query>();
            mutations = new List<Query>();
            subscriptions = new List<Query>();
        }

        #endregion

#endif

        #region Classes

        [Serializable]
        public class Query
        {
            public string name;
            public Type type;
            public string query;
            public string queryString;
            public string returnType;
            private string _args;
            public List<string> queryOptions;
            public List<Field> fields;
            public bool isComplete;

            public enum Type
            {
                Query,
                Mutation,
                Subscription
            }

            public void SetArgs(object inputObject)
            {
                var json = JsonConvert.SerializeObject(inputObject, new EnumInputConverter());
                _args = JsonToArgument(json);
                CompleteQuery();
            }

            public void SetArgs(string inputString)
            {
                _args = inputString;
                CompleteQuery();
            }


            public void CompleteQuery()
            {
                isComplete = true;
                string data = null;
                string parent = null;
                Field previousField = null;
                for (var i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];
                    if (field.parentIndexes.Count == 0)
                    {
                        if (parent == null)
                        {
                            data += $"\n{GenerateStringTabs(field.parentIndexes.Count + 2)}{field.name}";
                        }
                        else
                        {
                            var count = previousField.parentIndexes.Count - field.parentIndexes.Count;
                            while (count > 0)
                            {
                                data += $"\n{GenerateStringTabs(count + 1)}}}";
                                count--;
                            }

                            data += $"\n{GenerateStringTabs(field.parentIndexes.Count + 2)}{field.name}";
                            parent = null;
                        }

                        previousField = field;
                        continue;
                    }

                    if (fields[field.parentIndexes.Last()].name != parent)
                    {
                        parent = fields[field.parentIndexes.Last()].name;

                        if (fields[field.parentIndexes.Last()] == previousField)
                        {
                            data += $"{{\n{GenerateStringTabs(field.parentIndexes.Count + 2)}{field.name}";
                        }
                        else
                        {
                            if (previousField != null)
                            {
                                var count = previousField.parentIndexes.Count - field.parentIndexes.Count;
                                while (count > 0)
                                {
                                    data += $"\n{GenerateStringTabs(count + 1)}}}";
                                    count--;
                                }
                            }

                            data += $"\n{GenerateStringTabs(field.parentIndexes.Count + 2)}{field.name}";
                        }

                        previousField = field;
                    }
                    else
                    {
                        data += $"\n{GenerateStringTabs(field.parentIndexes.Count + 2)}{field.name}";
                        previousField = field;
                    }

                    if (i == fields.Count - 1)
                    {
                        var count = previousField.parentIndexes.Count;
                        while (count > 0)
                        {
                            data += $"\n{GenerateStringTabs(count + 1)}}}";
                            count--;
                        }
                    }
                }

                var arg = String.IsNullOrEmpty(_args) ? "" : $"({_args})";
                string word;
                switch (type)
                {
                    case Type.Query:
                        word = "query";
                        break;
                    case Type.Mutation:
                        word = "mutation";
                        break;
                    case Type.Subscription:
                        word = "subscription";
                        break;
                    default:
                        word = "query";
                        break;
                }

                query = data == null
                    ? $"{word} {name}{{\n{GenerateStringTabs(1)}{queryString}{arg}\n}}"
                    : $"{word} {name}{{\n{GenerateStringTabs(1)}{queryString}{arg}{{{data}\n{GenerateStringTabs(1)}}}\n}}";
            }

            private string GenerateStringTabs(int number)
            {
                var result = "";
                for (var i = 0; i < number; i++)
                {
                    result += "    ";
                }

                return result;
            }
        }

        [Serializable]
        public class Field
        {
            public int index;

            public int Index
            {
                get => index;
                set
                {
                    type = possibleFields[value].type;
                    name = possibleFields[value].name;
                    if (value != index)
                        hasChanged = true;
                    index = value;
                }
            }

            public string name;
            public string type;
            public List<int> parentIndexes;
            public bool hasSubField;
            public List<PossibleField> possibleFields;

            public bool hasChanged;

            public Field()
            {
                possibleFields = new List<PossibleField>();
                parentIndexes = new List<int>();
                index = 0;
            }

            public void CheckSubFields(Introspection.SchemaClass schemaClass)
            {
                var t = schemaClass?.data?.__schema?.types?.Find((aType => aType.name == type));

                if (t?.fields == null || t.fields.Count == 0)
                {
                    hasSubField = false;
                    return;
                }

                hasSubField = true;
            }

            [Serializable]
            public class PossibleField
            {
                public string name;
                public string type;

                public static implicit operator PossibleField(Field field)
                {
                    return new PossibleField { name = field.name, type = field.type };
                }
            }

            public static explicit operator Field(Introspection.SchemaClass.Data.Schema.Type.Field schemaField)
            {
                var ofType = schemaField.type;
                string typeName;
                do
                {
                    typeName = ofType.name;
                    ofType = ofType.ofType;
                } while (ofType != null);

                return new Field { name = schemaField.name, type = typeName };
            }
        }

        #endregion
    }


    public class EnumInputConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var @enum = (Enum)value;
                var enumText = @enum.ToString("G");
                writer.WriteRawValue(enumText);
            }
        }
    }
}