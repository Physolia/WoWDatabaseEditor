using System;
using System.Collections.Generic;
using System.ComponentModel;
using Generator.Equals;
using Newtonsoft.Json;

namespace WDE.DatabaseEditors.Data.Structs
{
    [Equatable]
    public partial class DatabaseColumnJson
    {
        [DefaultEquality]
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; } = "";
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "help")] 
        public string? Help { get; set; } = null;
        
        [CustomEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))] 
        [JsonProperty(PropertyName = "db_column_name")]
        [DefaultValue("")]
        public string DbColumnName { get; set; } = "";

        [DefaultEquality]
        [JsonProperty(PropertyName = "column_id")]
        public string? ColumnIdForUi { get; set; }

        [DefaultEquality]
        [JsonProperty(PropertyName = "foreign_table")]
        public string? ForeignTable { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "value_type")]
        [DefaultValue("")]
        public string ValueType { get; set; } = "";

        [CustomEquality(typeof(DefaultValueEqualityComparer))]
        [JsonProperty(PropertyName = "default")]
        public object? Default { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "autoincrement")]
        public bool AutoIncrement { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "read_only")]
        public bool IsReadOnly { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "can_be_null")]
        public bool CanBeNull { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "is_condition")]
        public bool IsConditionColumn { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "zero_is_blank")]
        public bool IsZeroBlank { get; set; }
        
        [DefaultEquality]
        [JsonProperty(PropertyName = "autogenerate_comment")]
        public string? AutogenerateComment { get; set; }

        [DefaultEquality]
        [JsonProperty(PropertyName = "meta")]
        public string? Meta { get; set; }

        [DefaultEquality]
        [JsonProperty(PropertyName = "preferred_width")]
        public float? PreferredWidth { get; set; }

        [IgnoreEquality]
        [JsonIgnore]
        public bool IsMetaColumn => !string.IsNullOrEmpty(Meta);
        
        [IgnoreEquality] [JsonIgnore] public bool IsTypeString => ValueType is "string" || ValueType.EndsWith("StringParameter");
        [IgnoreEquality] [JsonIgnore] public bool IsTypeLong => ValueType is "long" or "uint" or "int" || (ValueType.EndsWith("Parameter") && !IsTypeString);
        [IgnoreEquality] [JsonIgnore] public bool IsTypeFloat => ValueType is "float";
        [IgnoreEquality] [JsonIgnore] public bool IsUnixTimestamp => ValueType is "UnixTimestampParameter";
        
        class DefaultValueEqualityComparer : IEqualityComparer<object>
        {
            public static readonly DefaultValueEqualityComparer Default = new();

            public bool Equals(object? x, object? y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null && y != null || x != null && y == null)
                    return false;
                if (x is string s && y is string s2)
                    return s == s2;
                try
                {
                    var d1 = Convert.ToDouble(x);
                    var d2 = Convert.ToDouble(y);
                    return Math.Abs(d1 - d2) < 0.0001;
                }
                catch (Exception _)
                {
                    return false;
                }
            }

            public int GetHashCode(object obj) => 0;
        }
    }
}