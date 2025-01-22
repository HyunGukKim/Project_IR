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
        // ���̺� ������
        UnityGoogleSheet.LoadAllData();

        //foreach (var data in t_StageData.Data.DataMap)
        //{
         
        //    Debug.Log(Number.GetNumber(BigDouble.Parse(data.Value.wave1HP)));
        //}

        Init();
        // �ڳ� ������ �ʱ�ȭ
        StaticManager.Backend.InitInGameData();
        //Queue�� ����� �Լ� ���������� ����
        NextStep(true, string.Empty, string.Empty, string.Empty);
    }

    private void Init()
    {
        _initializeStep.Clear();
        // Ʈ��������� �ҷ��� ��, �Ⱥҷ��� ��� ���� Get �Լ��� �ҷ����� �Լ� *�߿�*
        _initializeStep.Enqueue(() => { Debug.Log("Ʈ����� �õ�"); TransactionRead(NextStep); });
    }

    // �� �ڳ� �Լ��� ȣ���ϴ� BackendGameDataLoad���� ������ ����� ó���ϴ� �Լ�
    // �����ϸ� ���� �������� �̵�, �����ϸ� ���� UI�� ����.
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

    // Ʈ����� �б� ȣ�� �Լ�
    private void TransactionRead(Normal.AfterBackendLoadFunc func)
    {
        bool isSuccess = false;
        string className = GetType().Name;
        string functionName = MethodBase.GetCurrentMethod()?.Name;
        string errorInfo = string.Empty;

        //Ʈ����� ����Ʈ ����
        List<TransactionValue> transactionList = new List<TransactionValue>();

        // ���� ���̺� �����͸�ŭ Ʈ����� �ҷ�����
        foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
        {
            transactionList.Add(gameData.Value.GetTransactionGetValue());
        }

        // [�ڳ�] Ʈ����� �б� �Լ�
        Backend.GameData.TransactionReadV2(transactionList, callback => {
            try
            {
                Debug.Log($"Backend.GameData.TransactionReadV2 : {callback}");

                // �����͸� ��� �ҷ����� ���
                if (callback.IsSuccess())
                {
                    JsonData gameDataJson = callback.GetFlattenJSON()["Responses"];

                    int index = 0;

                    foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
                    {

                        _initializeStep.Enqueue(() => {
                            //ShowDataName(gameData.Key);
                            // �ҷ��� �����͸� ���ÿ��� �Ľ�
                            gameData.Value.BackendGameDataLoadByTransaction(gameDataJson[index++], NextStep);
                        });
                        //_maxLoadingCount++;

                    }
                    // �ִ� �۾� ���� ����
                    //loadingSlider.maxValue = _maxLoadingCount;
                    isSuccess = true;
                } else
                {
                    // Ʈ��������� �����͸� ã�� ���Ͽ� ������ �߻��Ѵٸ� ������ GetMyData�� ȣ��
                    foreach (var gameData in StaticManager.Backend.GameData.GameDataList)
                    {
                        _initializeStep.Enqueue(() => {
                            Debug.Log(gameData.Key);
                            //ShowDataName(gameData.Key);
                            // GetMyData ȣ��
                            gameData.Value.BackendGameDataLoad(NextStep);
                        });
                        //_maxLoadingCount++;
                    }
                    // �ִ� �۾� ���� ����
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
