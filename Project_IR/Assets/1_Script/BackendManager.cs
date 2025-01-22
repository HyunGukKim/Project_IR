using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    // ���� ���� ���� �����͸� ��Ƴ��� Ŭ����
    public class BackendGameData
    {
        public readonly UserData UserData = new(); // UserData ���̺� ������
        public readonly InvenData InvenData = new(); // InvenData ���̺� ������


        public readonly Dictionary<string, GameData>
            GameDataList = new Dictionary<string, GameData>();

        public BackendGameData()
        {
            GameDataList.Add("���� ����", UserData);
            GameDataList.Add("�κ��丮 ����", InvenData);
        }
    }

    public BackendGameData GameData = new(); // ���� ���� Ŭ���� ����

    public void Init()
    {
        var initializeBro = Backend.Initialize(true);

        // �ʱ�ȭ ������
        if (initializeBro.IsSuccess())
        {
            Debug.Log("�ڳ� �ʱ�ȭ�� �Ϸ�Ǿ����ϴ�.");
            //CreateSendQueueMgr();
            SetErrorHandler();
        }
        //�ʱ�ȭ ���н�
        else
        {
            //StaticManager.UI.AlertUI.OpenErrorUI(GetType().Name, MethodBase.GetCurrentMethod()?.ToString(), initializeBro.ToString());
        }
    }

    // �ε������� �Ҵ��� �ڳ� ���� Ŭ���� �ʱ�ȭ
    public void InitInGameData()
    {
        GameData = new();
    }

    // ������Ʈ�� �߻��� ���Ŀ� ȣ�⿡ ���� ������ ��ȯ���ִ� �븮�� �Լ�
    public delegate void AfterUpdateFunc(BackendReturnObject callback);

    // ���� �ٲ� �����Ͱ� �ִ��� üũ�� �ٲ� �����͵��� �ٷ� ���� Ȥ�� Ʈ����ǿ� ���� ������ �����ϴ� �Լ�
    public void UpdateAllGameData(AfterUpdateFunc afterUpdateFunc)
    {
        // �������嵵 ������
        //ServerSave.Inst.LocalSave();

        //StaticManager.UI.OnBlock();
        string info = string.Empty;

        // �ٲ� �����Ͱ� � �ִ��� üũ
        List<GameData> gameDatas = new List<GameData>();

        foreach (var gameData in GameData.GameDataList)
        {
            if (gameData.Value.IsChangedData)
            {
                info += gameData.Value.GetTableName() + "\n";
                gameDatas.Add(gameData.Value);
            }
        }

        if (gameDatas.Count <= 0)
        {
            // ������Ʈ�� ����� �������� �ʽ��ϴ�.
        } else if (gameDatas.Count == 1)
        {

            //�ϳ���� ã�Ƽ� �ش� ���̺� ������Ʈ
            foreach (var gameData in gameDatas)
            {
                if (gameData.IsChangedData)
                {
                    gameData.Update(callback => {

                        //�����Ұ�� ������ ���� ���θ� false�� ����
                        if (callback.IsSuccess())
                        {
                            gameData.IsChangedData = false;
                        } else
                        {
                            //SendBugReport(GetType().Name, MethodBase.GetCurrentMethod()?.ToString(), callback.ToString() + "\n" + info);
                        }
                        Debug.Log($"UpdateV2 : {callback}\n������Ʈ ���̺� : \n{info}");
                        if (afterUpdateFunc == null)
                        {

                        } else
                        {
                            afterUpdateFunc(callback); // ������ �븮�� �Լ� ȣ��
                        }
                    });
                }
            }
        } else
        {
            // 2�� �̻��̶�� Ʈ����ǿ� ��� ������Ʈ
            // �� 10�� �̻��̸� Ʈ����� ���� ����
            List<TransactionValue> transactionList = new List<TransactionValue>();

            // ����� �����͸�ŭ Ʈ����� �߰�
            foreach (var gameData in gameDatas)
            {
                transactionList.Add(gameData.GetTransactionUpdateValue());
            }

            Backend.GameData.TransactionWriteV2(transactionList, callback => {
                Debug.Log($"Backend.BMember.TransactionWriteV2 : {callback}");

                if (callback.IsSuccess())
                {
                    foreach (var data in gameDatas)
                    {
                        data.IsChangedData = false;
                    }
                } else
                {
                    //SendBugReport(GetType().Name, MethodBase.GetCurrentMethod()?.ToString(), callback.ToString() + "\n" + info);
                }

                Debug.Log($"TransactionWriteV2 : {callback}\n������Ʈ ���̺� : \n{info}");

                if (afterUpdateFunc == null)
                {

                } else
                {

                    afterUpdateFunc(callback);  // ������ �븮�� �Լ� ȣ��
                }
            });
        }
        //StaticManager.UI.OffBlock();
    }



    // ��� �ڳ� �Լ����� ���� �߻� ��, �� ������ ���� ȣ�����ִ� �ڵ鷯
    private void SetErrorHandler()
    {
        Backend.ErrorHandler.InitializePoll(true);

        //// ���� ���� ���� �߻� ��
        //Backend.ErrorHandler.OnMaintenanceError = () => {
        //    Debug.Log("���� ���� �߻�!!!");
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("���� ���� ��", "���� ���� �������Դϴ�.\nŸ��Ʋ�� ���ư��ϴ�.");
        //};
        //// 403 ���� �߻���
        //Backend.ErrorHandler.OnTooManyRequestError = () => {
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("���������� �ൿ ����", "���������� �ൿ�� �����Ǿ����ϴ�.\nŸ��Ʋ�� ���ư��ϴ�.");
        //};
        //// �׼�����ū ���� �� �������� ��ū ���� ��
        //Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("�ٸ� ��� ���� ����", "�ٸ� ��⿡�� �α����� �����Ǿ����ϴ�.\nŸ��Ʋ�� ���ư��ϴ�.");
        //};
    }

    void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
            Backend.ErrorHandler.Poll();
        }
    }
}
