using System.Linq;
using System.Collections.Generic;

namespace System.Text.Json.Extensions
{
    /// <summary>
    /// Позволяет полчить из json текста значение какого то конкретного ключа
    /// <example>
    /// json file:
    ///     {[
    ///         {
    ///         "following": false,
    ///         "id": "1393174363",
    ///         "screen_name": "Project",
    ///         "name": "Test Project",
    ///         "protected": false,
    ///         "count": 32289,
    ///         "formatted_followers_count": "32.3K followers",
    ///         "age_gated": false
    ///         }
    ///     ]}
    /// [0].name - вернет "Project"
    /// 
    /// json file:
    ///     {
    ///     "data":[
    ///         {"_id": "5f45164f577207ff493f984a", "timestamp": 1598362949, "name": "file1.txt"},
    ///         {"_id": "5f45162d577207ff493f9848", "timestamp": 1598362756, "name": "file2.txt"},
    ///         {"_id": "5f451607577207ff493f9846", "timestamp": 1598362727, "name": "file3.txt"}
    ///         ]
    ///     }
    /// data[0].timestamp - вернет 1598362949
    /// </example>
    /// </summary>
    public static class JsonEndpointer
    {
        /// <summary>
        /// Символы исключения, которых не может быть в имени ключа
        /// </summary>
        private readonly static string breackChar = ",<>?^:()=!%\\|/*+-";
        /// <summary>
        /// Описание элемента для текущего шага парсинга
        /// </summary>
        class ElmProperty
        {
            /// <summary>
            /// Название элемента
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Тип:
            /// 1 - просто значение
            /// 2 - массив
            /// </summary>
            public int Type { get; set; }
            /// <summary>
            /// Список индексов в массиве, если массив многоразмерный
            /// </summary>
            public List<int> Positions { get; set; }
            /// <summary>
            /// Конструктор по имени и типу
            /// </summary>
            /// <param name="name">Имя</param>
            /// <param name="type">Тип элемента</param>
            public ElmProperty(string name, int type)
            {
                Name = name;
                Type = type;
                Positions = new List<int>();
            }
            /// <summary>
            /// Конструктор по имени, типу и позициям
            /// </summary>
            /// <param name="name">Имя</param>
            /// <param name="type">Тип элемента</param>
            /// <param name="positions">Позиции</param>
            public ElmProperty(string name, int type, List<int> positions)
            {
                Name = name;
                Type = type;
                Positions = positions;
            }
        }
        /// <summary>
        /// Разбивает строку на массив элементов
        /// </summary>
        /// <param name="path">Путь к значению в json документе</param>
        /// <returns></returns>
        private static List<ElmProperty> DelimiterProperty(string path)
        {
            if (path == null || path == "") { throw new ArgumentNullException(nameof(path)); }
            List<ElmProperty> result = new List<ElmProperty>();
            string[] elems = path.Split(new char[] { '.' });
            foreach (string elem in elems)
            {
                for (int i = 0; i < elem.Length; i++)
                {
                    if (breackChar.IndexOf(elem[i]) >= 0)
                    {
                        throw new Exception("Invalid name param: " + elem);
                    }
                }
                var arrsplit = elem.Split(new char[] { '[' });
                if (arrsplit != null && arrsplit.Length > 1)
                {
                    ElmProperty elemAdd = new ElmProperty(arrsplit[0], 2, new List<int>());
                    for (int i = 1; i < arrsplit.Length; i++)
                    {
                        try
                        {
                            int pos = int.Parse(arrsplit[i].Trim(']'));
                            elemAdd.Positions.Add(pos);
                        }
                        catch
                        {
                            throw new Exception("Invalid array index in param: " + elem);
                        }
                    }
                    result.Add(elemAdd);
                }
                else
                {
                    result.Add(new ElmProperty(elem, 1));
                }
            }
            return result;
        }
        /// <summary>
        /// Вернет элемент документа, лежащего по пути <paramref name="path"/>
        /// </summary>
        /// <param name="root">Элемент с которого надо вести поиск</param>
        /// <param name="path">Путь к элементу в документе</param>
        /// <returns></returns>
        public static JsonElement GetKeyByPath(JsonElement root, string path)
        {
            JsonElement firstElem = root;
            var properties = DelimiterProperty(path);
            if (properties == null)
            {
                return root;
            }
            foreach (var prop in properties)
            {
                switch (prop.Type)
                {
                    case 1:
                        firstElem.TryGetProperty(prop.Name, out firstElem);
                        break;
                    case 2:
                        if (prop.Name != "")
                        {
                            firstElem.TryGetProperty(prop.Name, out firstElem);
                        }
                        for (int p = 0; p < prop.Positions.Count; p++)
                        {
                            var arrElem = firstElem.EnumerateArray();
                            int i = 0;
                            foreach (var elm in arrElem)
                            {
                                if (i == prop.Positions[p])
                                {
                                    firstElem = elm;
                                    break;
                                }
                                i++;
                            }
                            if (i >= arrElem.Count())
                            {
                                throw new KeyNotFoundException("Not found array index for json key: " + path);
                            }
                        }
                        break;
                    default:
                        throw new KeyNotFoundException("Not found type json key: " + path);
                }
            }
            return firstElem;
        }
        /// <summary>
        /// Вернет значение ключа, приведя его к строковому типу
        /// </summary>
        /// <param name="root">Первый элемент json документа</param>
        /// <param name="path">Путь к ключу</param>
        /// <returns>Строку, если значение ключа можно привести к строке,
        ///  иначе null.</returns>
        public static string? GetKeyToString(this JsonElement root, string path) 
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => res.GetString(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Number => res.GetDecimal().ToString(),
                _ => null,
            };
        }
        /// <summary>
        /// Вернет логическое значение ключа, если такое возможно.
        /// Для числовых значений отдает true если значение != 0.
        /// </summary>
        /// <param name="root">Первый элемент json документа</param>
        /// <param name="path">Путь к ключу</param>
        /// <returns>Логическое значение ключа. Иначе исключение</returns>
        /// <exception cref="FormatException">Если не получается привести значение к логическому типу.</exception>
        public static bool GetKeyToBoolean(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => bool.Parse(res.GetString()),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Number => res.GetDecimal()!=0,
                _ => throw new FormatException("Cannot convert the key format to the boolean type: " + path)
            };
        }

        public static byte GetKeyToByte(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => byte.Parse(res.GetString()),
                JsonValueKind.Number => res.GetByte(),
                _ => throw new FormatException("Cannot convert the key format to the byte type: " + path)
            };
        }

        public static sbyte GetKeyToSByte(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => sbyte.Parse(res.GetString()),
                JsonValueKind.Number => res.GetSByte(),
                _ => throw new FormatException("Cannot convert the key format to the sbyte type: " + path)
            };
        }

        public static Int16 GetKeyToInt16(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Int16.Parse(res.GetString()),
                JsonValueKind.Number => res.GetInt16(),
                _ => throw new FormatException("Cannot convert the key format to the Int16 type: " + path)
            };
        }
        public static Int32 GetKeyToInt32(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Int32.Parse(res.GetString()),
                JsonValueKind.Number => res.GetInt32(),
                _ => throw new FormatException("Cannot convert the key format to the Int32 type: " + path)
            };
        }
        public static Int64 GetKeyToInt64(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Int64.Parse(res.GetString()),
                JsonValueKind.Number => res.GetInt64(),
                _ => throw new FormatException("Cannot convert the key format to the Int64 type: " + path)
            };
        }
        public static UInt16 GetKeyToUInt16(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => UInt16.Parse(res.GetString()),
                JsonValueKind.Number => res.GetUInt16(),
                _ => throw new FormatException("Cannot convert the key format to the UInt16 type: " + path)
            };
        }
        public static UInt32 GetKeyToUInt32(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => UInt32.Parse(res.GetString()),
                JsonValueKind.Number => res.GetUInt32(),
                _ => throw new FormatException("Cannot convert the key format to the UInt32 type: " + path)
            };
        }
        public static UInt64 GetKeyToUInt64(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => UInt64.Parse(res.GetString()),
                JsonValueKind.Number => res.GetUInt64(),
                _ => throw new FormatException("Cannot convert the key format to the UInt64 type: " + path)
            };
        }
        public static Single GetKeyToSingle(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Single.Parse(res.GetString()),
                JsonValueKind.Number => res.GetSingle(),
                _ => throw new FormatException("Cannot convert the key format to the Single type: " + path)
            };
        }
        public static Decimal GetKeyToDecimal(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Decimal.Parse(res.GetString()),
                JsonValueKind.Number => res.GetDecimal(),
                _ => throw new FormatException("Cannot convert the key format to the Decimal type: " + path)
            };
        }
        public static Double GetKeyToDouble(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => Double.Parse(res.GetString()),
                JsonValueKind.Number => res.GetDouble(),
                _ => throw new FormatException("Cannot convert the key format to the Double type: " + path)
            };
        }
        public static DateTime GetKeyToDateTime(this JsonElement root, string path)
        {
            long dt = 0;
            try
            {
                dt = root.GetKeyToInt64(path);
            }
            catch { }
            if (dt != 0)
            {
                if (dt <= 253402300799)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(dt).DateTime;
                }
                else
                {
                    return DateTimeOffset.FromUnixTimeSeconds(dt / 1000).DateTime;
                }
            }
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => DateTime.Parse(res.GetString()),
                _ => throw new FormatException("Cannot convert the key format to the DateTime type: " + path)
            };
        }
        public static DateTimeOffset GetKeyToDateTimeOffset(this JsonElement root, string path)
        {
            long dt = 0;
            try
            {
                dt = root.GetKeyToInt64(path);
            }
            catch { }
            if (dt != 0)
            {
                if (dt <= 253402300799)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(dt);
                }
                else
                {
                    return DateTimeOffset.FromUnixTimeSeconds(dt / 1000);
                }
            }
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.String => DateTimeOffset.Parse(res.GetString()),
                _ => throw new FormatException("Cannot convert the key format to the DateTimeOffset type: " + path)
            };
        }
        public static int GetKeyToArrayLength(this JsonElement root, string path)
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.Array => res.GetArrayLength(),
                _ => throw new FormatException("Cannot convert the key format to the ArrayLength type: " + path)
            };
        }

        public static T GetKeyToObject<T>(this JsonElement root, string path) where T : class?
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.Object => JsonSerializer.Deserialize<T>(res),
                _ => null
            };
        }
        public static T[] GetKeyToArray<T>(this JsonElement root, string path) 
        {
            var res = GetKeyByPath(root, path);
            return res.ValueKind switch
            {
                JsonValueKind.Array => JsonSerializer.Deserialize<T[]>(res),
                _ => null
            };
        }
    }
}
