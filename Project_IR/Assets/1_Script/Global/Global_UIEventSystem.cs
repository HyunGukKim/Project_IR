﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eUIEventType {
    None,

    NormalTurnStartData,

}

/// <summary>
/// delegate 함수 리스트를 들고있는 구조체.
/// </summary>
public struct DelegateList {
    private List<Delegate> _funcList;
    public List<Delegate> GetEventList() { return _funcList; }

    public void AddFunction<T>(T func) {
        if (null == _funcList) {
            _funcList = new List<Delegate>();
        }

        _funcList.Add(func as Delegate);
    }


    public void RemoveFunction<T>(T func) {
        if (null == _funcList) {
            return;
        }

        _funcList.Remove(func as Delegate);
    }

    public bool IsEmpty() {
        if (null == _funcList) {
            return true;
        }

        if (_funcList.Count <= 0) {
            return true;
        }

        return false;
    }
}

public class Global_UIEventSystem {

    // delegate 변수로 사용할 것들.

    public delegate void EventFunc();
    public delegate void EventFunc<T>(T param1);
    public delegate void EventFunc<T1, T2>(T1 param1, T2 param2);
    public delegate void EventFunc<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
    public delegate void EventFunc<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
    public delegate void EventFunc<T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);

    // 실제 ui event 함수들을 들고있는 dictionary

    private static Dictionary<int, DelegateList> _uiEventTable = null;



    ////////////////////////////////////////////////////////////////////////////////////////////////

    // 주의 : 씬이 바뀔 때 ui event 는 모두 클리어되어야 한다.

    public static void ClearUIEvent() {

        if (null != _uiEventTable) {
            _uiEventTable.Clear();
        }

        Debug.Log("UI Event Cleared");
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////

    // UI 이벤트 호출 부분. 원하는 이벤트 이름과 인자로 전달할 내용을 전달하면 된다.

    public static void CallUIEvent(eUIEventType eventType) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }

        // 해당 이벤트에 달린 모든 함수를 실행.
        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        for (int idx = 0; idx < eventList.Count; idx++) {
            (eventList[idx] as EventFunc)();
        }
    }

    public static void CallUIEvent<T>(eUIEventType eventType, T param) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }



        // 해당 이벤트에 달린 모든 함수를 실행.

        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        foreach (EventFunc<T> func in eventList) {
            func(param);
        }
    }

    public static void CallUIEvent<T1, T2>(eUIEventType eventType, T1 param1, T2 param2) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }



        // 해당 이벤트에 달린 모든 함수를 실행.

        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        foreach (EventFunc<T1, T2> func in eventList) {
            func(param1, param2);
        }

    }

    public static void CallUIEvent<T1, T2, T3>(eUIEventType eventType, T1 param1, T2 param2, T3 param3) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }



        // 해당 이벤트에 달린 모든 함수를 실행.

        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        foreach (EventFunc<T1, T2, T3> func in eventList) {
            func(param1, param2, param3);
        }

    }

    public static void CallUIEvent<T1, T2, T3, T4>(eUIEventType eventType, T1 param1, T2 param2, T3 param3, T4 param4) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }



        // 해당 이벤트에 달린 모든 함수를 실행.

        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        foreach (EventFunc<T1, T2, T3, T4> func in eventList) {
            func(param1, param2, param3, param4);
        }

    }

    public static void CallUIEvent<T1, T2, T3, T4, T5>(eUIEventType eventType, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        if (false == _uiEventTable.ContainsKey(parseType)) {
            Debug.Log("등록되지 않은 이벤트입니다 : " + eventType);
            return;
        }



        // 해당 이벤트에 달린 모든 함수를 실행.

        List<Delegate> eventList = _uiEventTable[parseType].GetEventList();
        foreach (EventFunc<T1, T2, T3, T4, T5> func in eventList) {
            func(param1, param2, param3, param4, param5);
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    // UI 이벤트 등록 부분. 인자 개수에 따라서 다른 RegisterUIEvent 함수로 등록하면 된다.

    public static void RegisterUIEvent(eUIEventType eventType, EventFunc func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc>(func);

            _uiEventTable.Add(parseType, list);
        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc>(func);
        }
    }



    public static void RemoveUIEvent(eUIEventType eventType, EventFunc func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;



        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc>(func);
        }
    }

    public static void RegisterUIEvent<T>(eUIEventType eventType, EventFunc<T> func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc<T>>(func);

            _uiEventTable.Add(parseType, list);
        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc<T>>(func);
        }
    }



    public static void RemoveUIEvent<T>(eUIEventType eventType, EventFunc<T> func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;



        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc<T>>(func);
        }
    }

    public static void RegisterUIEvent<T1, T2>(eUIEventType eventType, EventFunc<T1, T2> func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc<T1, T2>>(func);

            _uiEventTable.Add(parseType, list);

        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc<T1, T2>>(func);
        }

    }

    public static void RemoveUIEvent<T1, T2>(eUIEventType eventType, EventFunc<T1, T2> func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc<T1, T2>>(func);
        }
    }


    public static void RegisterUIEvent<T1, T2, T3>(eUIEventType eventType, EventFunc<T1, T2, T3> func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc<T1, T2, T3>>(func);

            _uiEventTable.Add(parseType, list);

        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc<T1, T2, T3>>(func);
        }
    }

    public static void RemoveUIEvent<T1, T2, T3>(eUIEventType eventType, EventFunc<T1, T2, T3> func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc<T1, T2, T3>>(func);
        }
    }

    public static void RegisterUIEvent<T1, T2, T3, T4>(eUIEventType eventType, EventFunc<T1, T2, T3, T4> func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc<T1, T2, T3, T4>>(func);

            _uiEventTable.Add(parseType, list);

        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc<T1, T2, T3, T4>>(func);
        }
    }

    public static void RemoveUIEvent<T1, T2, T3, T4>(eUIEventType eventType, EventFunc<T1, T2, T3, T4> func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc<T1, T2, T3, T4>>(func);
        }
    }

    public static void RegisterUIEvent<T1, T2, T3, T4, T5>(eUIEventType eventType, EventFunc<T1, T2, T3, T4, T5> func) {
        if (null == _uiEventTable) {
            _uiEventTable = new Dictionary<int, DelegateList>();
        }

        int parseType = (int)eventType;



        // 이벤트가 아얘 없었으면 새로 할당해주어야 한다.

        if (false == _uiEventTable.ContainsKey(parseType)) {
            DelegateList list = new DelegateList();
            list.AddFunction<EventFunc<T1, T2, T3, T4, T5>>(func);

            _uiEventTable.Add(parseType, list);

        } else {

            // 이벤트에 함수 추가.

            _uiEventTable[parseType].AddFunction<EventFunc<T1, T2, T3, T4, T5>>(func);
        }
    }

    public static void RemoveUIEvent<T1, T2, T3, T4, T5>(eUIEventType eventType, EventFunc<T1, T2, T3, T4, T5> func) {
        if (null == _uiEventTable) {
            return;
        }

        int parseType = (int)eventType;

        // 이벤트가 있어야 지워줄 것도 있다..

        if (true == _uiEventTable.ContainsKey(parseType)) {

            _uiEventTable[parseType].RemoveFunction<EventFunc<T1, T2, T3, T4, T5>>(func);
        }
    }
}
