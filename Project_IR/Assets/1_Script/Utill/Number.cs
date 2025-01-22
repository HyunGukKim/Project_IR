using BreakInfinity;
using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using System.Text;
using UnityEngine;

/// <summary>
/// 숫자 넣으면 자릿수 적용해서 리턴하는 클래스
/// </summary>
public static class Number
{
    private static StringBuilder _stringBuilder = new StringBuilder();
    private static int _length = 0;
    private static int _index = 0;
    public static string GetNumber(string value) {
        if (value == "0") { return "0"; }

        // 1000 (4자리) 보다 작으면 그대로 리턴
        if (value.Length < 4) { return value; }

        _length = ((value.Length - 4) / 3) + 1;
        _stringBuilder.Length = 0;

        while (--_length >= 0) {
            _stringBuilder.Insert(0, Convert.ToChar(65 + (_length % 26)));
            _length = _length / 26;
        }

        _index = value.Length % 3 == 0 ? 3 : value.Length % 3;
        _stringBuilder.Insert(0, value.Substring(_index, 2));
        _stringBuilder.Insert(0, ".");
        _stringBuilder.Insert(0, value.Substring(0, _index));

        return _stringBuilder.ToString();
    }

    private static string _value;

    public static string GetNumber(BigDouble value) {
        if (value == 0) { return "0"; }
        _value = value.ToString("F0");

        // 1000 (4자리) 보다 작으면 그대로 리턴
        if (_value.Length < 4) { return _value.ToString(); }
        
        _length = ((_value.Length - 4) / 3) + 1;
        _stringBuilder.Length = 0;

        while (--_length >= 0) {
            _stringBuilder.Insert(0, Convert.ToChar(65 + (_length % 26)));
            _length = _length / 26;
        }

        _index = _value.Length % 3 == 0 ? 3 : _value.Length % 3;
        _stringBuilder.Insert(0, _value.Substring(_index, 2));
        _stringBuilder.Insert(0, ".");
        _stringBuilder.Insert(0, _value.Substring(0, _index));

        return _stringBuilder.ToString();
    }

    public static string GetNumberNoFloat(string value) {
        if (value == "0") { return "0"; }

        // 1000 (4자리) 보다 작으면 그대로 리턴
        if (value.Length < 4) { return value; }

        _length = ((value.Length - 4) / 3) + 1;
        _stringBuilder.Length = 0;

        while (--_length >= 0) {
            _stringBuilder.Insert(0, Convert.ToChar(65 + (_length % 26)));
            _length = _length / 26;
        }

        _index = value.Length % 3 == 0 ? 3 : value.Length % 3;
        _stringBuilder.Insert(0, value.Substring(0, _index));

        return _stringBuilder.ToString();
    }
}