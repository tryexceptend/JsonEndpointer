using System.Text.Json;
using System.Text.Json.Extensions;

namespace JsonEndpointerTests
{
    public class JsonEndpointerUnitTests
    {
        [Fact]
        public void GetKeyToStringTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Null(root.GetKeyToString("name2"));
            Assert.Null(root.GetKeyToString("testType.value_array_int"));
            Assert.Equal("test_json", root.GetKeyToString("name"));
            Assert.Equal("1234", root.GetKeyToString("testType.value_int"));
            Assert.Equal("1234,567", root.GetKeyToString("testType.value_decimal"));
            Assert.Equal("-1234", root.GetKeyToString("testType.value_int_less_zero"));
            Assert.Equal("-1234,567", root.GetKeyToString("testType.value_decimal_less_zero"));
            Assert.Equal("false", root.GetKeyToString("testType.value_bool_false"));
            Assert.Equal("true", root.GetKeyToString("testType.value_bool_true"));
        }
        [Fact]
        public void GetKeyToBooleanTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(()=>root.GetKeyToBoolean("name2"));
            Assert.Throws<FormatException>(()=>root.GetKeyToBoolean("testType.value_array_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToBoolean("name"));
            Assert.True(root.GetKeyToBoolean("testType.value_int"));
            Assert.True(root.GetKeyToBoolean("testType.value_decimal"));
            Assert.True(root.GetKeyToBoolean("testType.value_int_less_zero"));
            Assert.True(root.GetKeyToBoolean("testType.value_decimal_less_zero"));
            Assert.False(root.GetKeyToBoolean("testType.value_int_0"));
            Assert.False(root.GetKeyToBoolean("testType.value_bool_false"));
            Assert.True(root.GetKeyToBoolean("testType.value_bool_true"));
            Assert.False(root.GetKeyToBoolean("testType.value_bool_false_string"));
            Assert.True(root.GetKeyToBoolean("testType.value_bool_true_string"));
        }
        [Fact]
        public void GetKeyToByteTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToByte("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("name2"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_array_int"));
            Assert.Equal(123, root.GetKeyToByte("testType.value_byte"));
            Assert.Equal(123, root.GetKeyToByte("testType.value_byte_string"));
            Assert.Equal(0, root.GetKeyToByte("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_int_less_zero"));
            Assert.Throws<OverflowException>(() => root.GetKeyToByte("testType.value_int_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_true_string"));
        }
        [Fact]
        public void GetKeyToSByteTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToSByte("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToSByte("name2"));
            Assert.Throws<FormatException>(() => root.GetKeyToSByte("testType.value_array_int"));
            Assert.Equal(123, root.GetKeyToSByte("testType.value_byte"));
            Assert.Equal(-123, root.GetKeyToSByte("testType.value_sbyte_less_zero"));
            Assert.Equal(123, root.GetKeyToSByte("testType.value_byte_string"));
            Assert.Equal(-123, root.GetKeyToSByte("testType.value_sbyte_string_less_zero"));
            Assert.Equal(0, root.GetKeyToByte("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_int_less_zero"));
            Assert.Throws<OverflowException>(() => root.GetKeyToByte("testType.value_int_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToByte("testType.value_bool_true_string"));
        }
        [Fact]
        public void GetKeyToInt16Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("name2"));
            Assert.Equal(123, root.GetKeyToInt16("testType.value_byte"));
            Assert.Equal(-123, root.GetKeyToInt16("testType.value_sbyte_less_zero"));
            Assert.Equal(123, root.GetKeyToInt16("testType.value_byte_string"));
            Assert.Equal(-123, root.GetKeyToInt16("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt16("testType.value_int"));
            Assert.Equal(-1234, root.GetKeyToInt16("testType.value_int_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt16("testType.value_int_string"));
            Assert.Equal(-1234, root.GetKeyToInt16("testType.value_int_string_less_zero"));
            Assert.Equal(0, root.GetKeyToInt16("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt16("testType.value_array_int"));
            Assert.Throws<OverflowException>(() => root.GetKeyToInt16("testType.value_int32"));
            Assert.Throws<OverflowException>(() => root.GetKeyToInt16("testType.value_int32_less_zero"));
        }
        [Fact]
        public void GetKeyToInt32Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("name2"));
            Assert.Equal(123, root.GetKeyToInt32("testType.value_byte"));
            Assert.Equal(-123, root.GetKeyToInt32("testType.value_sbyte_less_zero"));
            Assert.Equal(123, root.GetKeyToInt32("testType.value_byte_string"));
            Assert.Equal(-123, root.GetKeyToInt32("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt32("testType.value_int"));
            Assert.Equal(-1234, root.GetKeyToInt32("testType.value_int_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt32("testType.value_int_string"));
            Assert.Equal(-1234, root.GetKeyToInt32("testType.value_int_string_less_zero"));
            Assert.Equal(0, root.GetKeyToInt32("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt32("testType.value_array_int"));
            Assert.Equal(123456, root.GetKeyToInt32("testType.value_int32"));
            Assert.Equal(-123456, root.GetKeyToInt32("testType.value_int32_less_zero"));
        }
        [Fact]
        public void GetKeyToInt64Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("name2"));
            Assert.Equal(123, root.GetKeyToInt64("testType.value_byte"));
            Assert.Equal(-123, root.GetKeyToInt64("testType.value_sbyte_less_zero"));
            Assert.Equal(123, root.GetKeyToInt64("testType.value_byte_string"));
            Assert.Equal(-123, root.GetKeyToInt64("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt64("testType.value_int"));
            Assert.Equal(-1234, root.GetKeyToInt64("testType.value_int_less_zero"));
            Assert.Equal(1234, root.GetKeyToInt64("testType.value_int_string"));
            Assert.Equal(-1234, root.GetKeyToInt64("testType.value_int_string_less_zero"));
            Assert.Equal(0, root.GetKeyToInt64("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToInt64("testType.value_array_int"));
            Assert.Equal(3147483647, root.GetKeyToInt64("testType.value_int64"));
            Assert.Equal(-3147483647, root.GetKeyToInt64("testType.value_int64_less_zero"));
        }
        [Fact]
        public void GetKeyToUInt16Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("name2"));
            Assert.Equal(123, root.GetKeyToUInt16("testType.value_byte"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_sbyte_less_zero"));
            Assert.Equal(123, root.GetKeyToUInt16("testType.value_byte_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt16("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234, root.GetKeyToUInt16("testType.value_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_int_less_zero"));
            Assert.Equal(1234, root.GetKeyToUInt16("testType.value_int_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt16("testType.value_int_string_less_zero"));
            Assert.Equal(0, root.GetKeyToUInt16("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_array_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_int32"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt16("testType.value_int32_less_zero"));
        }
        [Fact]
        public void GetKeyToUInt32Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("name2"));
            Assert.Equal(123u, root.GetKeyToUInt32("testType.value_byte"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_sbyte_less_zero"));
            Assert.Equal(123u, root.GetKeyToUInt32("testType.value_byte_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt32("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234u, root.GetKeyToUInt32("testType.value_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_int_less_zero"));
            Assert.Equal(1234u, root.GetKeyToUInt32("testType.value_int_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt32("testType.value_int_string_less_zero"));
            Assert.Equal(0u, root.GetKeyToUInt32("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_array_int"));
            Assert.Equal(123456u, root.GetKeyToUInt32("testType.value_int32"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt32("testType.value_int32_less_zero"));
        }
        [Fact]
        public void GetKeyToUInt64Test()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("name"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("name2"));
            Assert.Equal(123d, root.GetKeyToUInt64("testType.value_byte"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_sbyte_less_zero"));
            Assert.Equal(123d, root.GetKeyToUInt64("testType.value_byte_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt64("testType.value_sbyte_string_less_zero"));
            Assert.Equal(1234d, root.GetKeyToUInt64("testType.value_int"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_int_less_zero"));
            Assert.Equal(1234d, root.GetKeyToUInt64("testType.value_int_string"));
            Assert.Throws<OverflowException>(() => root.GetKeyToUInt64("testType.value_int_string_less_zero"));
            Assert.Equal(0d, root.GetKeyToUInt64("testType.value_int_0"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_decimal"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_decimal_less_zero"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_bool_false"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_bool_false_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_bool_true"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_bool_true_string"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_array_int"));
            Assert.Equal(3147483647d, root.GetKeyToUInt64("testType.value_int64"));
            Assert.Throws<FormatException>(() => root.GetKeyToUInt64("testType.value_int64_less_zero"));
        }
        [Fact]
        public void GetKeyToObjectTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            ObjTest objTest = root.GetKeyToObject<ObjTest>("testType.obj");
            Assert.NotNull(objTest);
        }
        [Fact]
        public void GetKeyToArrayTest()
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            int[] objTest = root.GetKeyToArray<int>("testType.value_array_int");
            Assert.NotNull(objTest);
        }
        private readonly string json = "{" +
            "\"name\":\"test_json\"," +
            "\"testType\":{"+
                "\"value_byte\":123," +
                "\"value_sbyte_less_zero\":-123," +
                "\"value_byte_string\":\"123\"," +
                "\"value_sbyte_string_less_zero\":\"-123\"," +
                "\"value_int\":1234," +
                "\"value_int_less_zero\":-1234," +
                "\"value_int_string\":\"1234\"," +
                "\"value_int_string_less_zero\":\"-1234\"," +
                "\"value_int_0\":0," +
                "\"value_decimal\":1234.567," +
                "\"value_decimal_less_zero\":-1234.567," +
                "\"value_bool_false\":false," +
                "\"value_bool_false_string\":\"false\"," +
                "\"value_bool_true\":true," +
                "\"value_bool_true_string\":\"true\"," +
                "\"value_array_int\":[1,2,3]," +
                "\"value_int32\":123456," +
                "\"value_int32_less_zero\":-123456," +
                "\"value_int64\":3147483647," +
                "\"value_int64_less_zero\":-3147483647," +
                "\"obj\":{"+
                    "\"name\":\"TestObj\","+
                    "\"value\":123" +
                    "}" +
                "}" +
            "}";
        public class ObjTest
        {
            public string name { get; set; }
            public int value { get; set; }
        }
    }
}