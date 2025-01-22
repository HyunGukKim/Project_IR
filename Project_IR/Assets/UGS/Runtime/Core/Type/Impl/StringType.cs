using System;
using CodeStage.AntiCheat.ObscuredTypes;

namespace GoogleSheet.Type
{
    [Type(typeof(ObscuredString), new string[] { "string", "String" })]
    public class StringType : IType
    {
        public object DefaultValue => string.Empty;
        public object Read(string value)
        {
            ObscuredString sValue = value;
            return sValue;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

