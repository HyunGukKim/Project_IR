using GoogleSheet.Type;
using System;

namespace Hamster.ZG.Type {
    [Type(typeof(Int64), new string[] { "int64", "int64" })]
    public class Int64Type : IType {
        public object DefaultValue => 0;
        /// <summary>
        /// value recevied from google sheet.
        /// </summary> 
        public object Read(string value) {
            Int64 _int64 = 0;
            var b = Int64.TryParse(value, out _int64);
            if (b == false) {
                return DefaultValue;
            }
            return _int64;
        }

        /// <summary>
        /// value write to google sheet
        /// </summary> 
        public string Write(object value) {
            return value.ToString();
        
        }

        
    }
}