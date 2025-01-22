using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticManager : MonoBehaviour
{
    public static StaticManager Inst { get; private set; }

    public static BackendManager Backend { get; private set; }
    //public static GlobalUI UI { get; private set; }

    public Stopwatch sw = new Stopwatch();

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
        DontDestroyOnLoad(this.gameObject);

        Backend = GetComponentInChildren<BackendManager>();
        //UI = GetComponentInChildren<GlobalUI>();

        //UI.Init();
        Backend.Init();
    }

    // �� ���� �� ���̵�ƿ��Ǹ鼭 �� ��ȯ ������ ������� ������ �����غ�����
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void WithdrawAccount()
    {
        // ��� ������ ����
        // Backend.GameData.UserData.InitializeData();


        Backend.UpdateAllGameData((callback) => {
            UnityEngine.Debug.Log("����");
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });


        //��� Ż��
        //BackEnd.Backend.BMember.WithdrawAccount(0, callback  => {
        //    PlayerPrefs.DeleteAll();
        //    // ���� ó��
        //    Application.Quit();
        //});
    }
}
