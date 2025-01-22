namespace GoogleSheet.Type {
    [Type(Type: typeof(bool), TypeName :new string[] { "bool", "Bool" })]
    public class BoolType : IType {
          
        public object DefaultValue => true;
        /// <summary>
        /// value recevied from google sheet.
        /// </summary> 
        bool _bool = true;
        public object Read(string value) {
            if (value.Contains("FALSE") || value.Contains("false")) {
                _bool = false;
                return _bool;
            } else {
                _bool = true;
            }
            return _bool;
        }

        /// <summary>
        /// value write to google sheet
        /// </summary> 
        public string Write(object value) {
            return value.ToString();
        
        }
    }
}