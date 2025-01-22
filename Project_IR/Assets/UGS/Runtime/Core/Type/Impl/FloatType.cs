using CodeStage.AntiCheat.ObscuredTypes;

namespace GoogleSheet.Type
{
    [Type(typeof(ObscuredFloat), new string[] { "float", "Float" })]
    public class FloatType : IType
    {
        public object DefaultValue => 0.0f;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            float f = 0.0f;
            ObscuredFloat ofloat = float.Parse(value);

            var b = float.TryParse(value, out f);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

                //return DefaultValue;
            }
            return ofloat;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
