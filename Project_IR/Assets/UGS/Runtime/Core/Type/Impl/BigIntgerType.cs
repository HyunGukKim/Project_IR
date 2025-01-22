using System.Numerics;

namespace GoogleSheet.Type {
    [Type(Type: typeof(BigInteger), TypeName: new string[] { "bigInteger", "BigInteger", "biginteger","bint" })]
    public class BigIntgerType : IType {

        public object DefaultValue => 0;
        public object Read(string value) {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            BigInteger @bigInteger = 0;
            var b = BigInteger.TryParse(value, out @bigInteger);
            if (b == false) {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return @bigInteger;
        }

        public string Write(object value) {
            return value.ToString();
        }
    }
}