using BackEnd;
using BreakInfinity;
using LitJson;
using System;
using System.Collections.Generic;
using System.Reflection;
using UGS;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private delegate void BackendLoadStep();
    private readonly Queue<BackendLoadStep> _initializeStep = new Queue<BackendLoadStep>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 테이블 가져옴
        UnityGoogleSheet.LoadAllData();

        //foreach (var data in t_StageData.Data.DataMap)
        //{
         
        //    Debug.Log(Number.GetNumber(BigDouble.Parse(data.Value.wave1HP)));
        //}

        Init();
        // 뒤끝 데이터 초기화
        StaticManager.Backend.InitInGameData();
        //Queue에 저장된 함수 순차적으로 실행
        NextStep(true, string.Empty, string.Empty, string.Empty);
    }

    private void Init()
    {
        _initializeStep.Clear();
        // 트랜잭션으로 불러온 후, 안불러질 경우 각자 Get 함수로 불러오는 함수 *중요*
        _initializeStep.Enqueue(() => { Debug.Log("트랜잭션 시도"); TransactionRead(NextStep); });
    }

    // 각 뒤끝 함수를 호출하는 BackendGameDataLoad에서 실행한 결과를 처리하는 함수
    // 성공하면 다음 스텝으로 이동, 실패하면 에러 UI를 띄운다.
    private void NextStep(bool isSuccess, string className, string funcName, string errorInfo)
    {
        if (isSuccess == true)
        {
            if (_initializeStep.Count > 0)
            {
                _initializeStep.Dequeue().Invoke();
            } else
            {
                StaticManager.Inst.ChangeScene("MainScene");
            }
        } else
        {
            //StaticManager.UI.AlertUI.OpenErrorUI(className, funcName, errorInfo);
        }
    }

    // 트랜잭션 읽기 호출 함수
    private void TransactionRead(Normal.AfterBackendLoadFunc func)
    {
        bool isSuccess = false;
        string className = GetType().Name;
        string functionName = MethodBase.GetCurrentMethod()?.Name;
        string errorInfo = string.Empty;

        //트랜잭션 리스트 생성
        List<TransactionValue> transactionList = new List<TransactionValue>();

        // 게임 테이블 데이터만큼 트랜잭션 불러오기
        foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
        {
            transactionList.Add(gameData.Value.GetTransactionGetValue());
        }

        // [뒤끝] 트랜잭션 읽기 함수
        Backend.GameData.TransactionReadV2(transactionList, callback => {
            try
            {
                Debug.Log($"Backend.GameData.TransactionReadV2 : {callback}");

                // 데이터를 모두 불러왔을 경우
                if (callback.IsSuccess())
                {
                    JsonData gameDataJson = callback.GetFlattenJSON()["Responses"];

                    int index = 0;

                    foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
                    {

                        _initializeStep.Enqueue(() => {
                            //ShowDataName(gameData.Key);
                            // 불러온 데이터를 로컬에서 파싱
                            gameData.Value.BackendGameDataLoadByTransaction(gameDataJson[index++], NextStep);
                        });
                        //_maxLoadingCount++;

                    }
                    // 최대 작업 개수 증가
                    //loadingSlider.maxValue = _maxLoadingCount;
                    isSuccess = true;
                } else
                {
                    // 트랜잭션으로 데이터를 찾지 못하여 에러가 발생한다면 개별로 GetMyData로 호출
                    foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
                    {
                        _initializeStep.Enqueue(() => {
                            Debug.Log(gameData.Key);
                            //ShowDataName(gameData.Key);
                            // GetMyData 호출
                            gameData.Value.BackendGameDataLoad(NextStep);
                        });
                        //_maxLoadingCount++;
                    }
                    // 최대 작업 개수 증가
                    //loadingSlider.maxValue = _maxLoadingCount;
                    isSuccess = true;
                }
            } catch (Exception e)
            {
                errorInfo = e.ToString();
            } finally
            {
                func.Invoke(isSuccess, className, functionName, errorInfo);
            }
        });
    }

}
