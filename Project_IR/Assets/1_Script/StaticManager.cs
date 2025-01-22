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

    // 씬 변경 시 페이드아웃되면서 씬 전환 씬변경 어떤식으로 할지는 생각해봐야지
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void WithdrawAccount()
    {
        // 모든 데이터 리셋
        // Backend.GameData.UserData.InitializeData();


        Backend.UpdateAllGameData((callback) => {
            UnityEngine.Debug.Log("꺼짐");
            PlayerPrefs.DeleteAll();
            Application.Quit();
        });


        //즉시 탈퇴
        //BackEnd.Backend.BMember.WithdrawAccount(0, callback  => {
        //    PlayerPrefs.DeleteAll();
        //    // 이후 처리
        //    Application.Quit();
        //});
    }
}
