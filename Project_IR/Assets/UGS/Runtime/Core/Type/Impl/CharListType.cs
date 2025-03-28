﻿using System.Collections.Generic;
using System.Diagnostics;

namespace GoogleSheet.Type
{
    [Type(Type: typeof(List<char>), TypeName: new string[] { "list<char>", "List<Char>" })]
    public class CharListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {


            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            var list = new System.Collections.Generic.List<char>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(char.Parse(data));
            }
            else
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<char>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}