using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    // 게임 정보 관리 데이터만 모아놓은 클래스
    public class BackendGameData
    {
        public readonly UserData UserData = new(); // UserData 테이블 데이터
        public readonly InvenData InvenData = new(); // InvenData 테이블 데이터


        public readonly Dictionary<string, GameData>
            GameDataList = new Dictionary<string, GameData>();

        public BackendGameData()
        {
            GameDataList.Add("유저 정보", UserData);
            GameDataList.Add("인벤토리 정보", InvenData);
        }
    }

    public BackendGameData GameData = new(); // 게임 모음 클래스 생성

    public void Init()
    {
        var initializeBro = Backend.Initialize(true);

        // 초기화 성공시
        if (initializeBro.IsSuccess())
        {
            Debug.Log("뒤끝 초기화가 완료되었습니다.");
            //CreateSendQueueMgr();
            SetErrorHandler();
        }
        //초기화 실패시
        else
        {
            //StaticManager.UI.AlertUI.OpenErrorUI(GetType().Name, MethodBase.GetCurrentMethod()?.ToString(), initializeBro.ToString());
        }
    }

    // 로딩씬에서 할당할 뒤끝 정보 클래스 초기화
    public void InitInGameData()
    {
        GameData = new();
    }

    // 업데이트가 발생한 이후에 호출에 대한 응답을 반환해주는 대리자 함수
    public delegate void AfterUpdateFunc(BackendReturnObject callback);

    // 값이 바뀐 데이터가 있는지 체크후 바뀐 데이터들은 바로 저장 혹은 트랜잭션에 묶어 저장을 진행하는 함수
    public void UpdateAllGameData(AfterUpdateFunc afterUpdateFunc)
    {
        // 로컬저장도 같이함
        //ServerSave.Inst.LocalSave();

        //StaticManager.UI.OnBlock();
        string info = string.Empty;

        // 바뀐 데이터가 몇개 있는지 체크
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
            // 업데이트할 목록이 존재하지 않습니다.
        } else if (gameDatas.Count == 1)
        {

            //하나라면 찾아서 해당 테이블만 업데이트
            foreach (var gameData in gameDatas)
            {
                if (gameData.IsChangedData)
                {
                    gameData.Update(callback => {

                        //성공할경우 데이터 변경 여부를 false로 변경
                        if (callback.IsSuccess())
                        {
                            gameData.IsChangedData = false;
                        } else
                        {
                            //SendBugReport(GetType().Name, MethodBase.GetCurrentMethod()?.ToString(), callback.ToString() + "\n" + info);
                        }
                        Debug.Log($"UpdateV2 : {callback}\n업데이트 테이블 : \n{info}");
                        if (afterUpdateFunc == null)
                        {

                        } else
                        {
                            afterUpdateFunc(callback); // 지정한 대리자 함수 호출
                        }
                    });
                }
            }
        } else
        {
            // 2개 이상이라면 트랜잭션에 묶어서 업데이트
            // 단 10개 이상이면 트랜잭션 실패 주의
            List<TransactionValue> transactionList = new List<TransactionValue>();

            // 변경된 데이터만큼 트랜잭션 추가
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

                Debug.Log($"TransactionWriteV2 : {callback}\n업데이트 테이블 : \n{info}");

                if (afterUpdateFunc == null)
                {

                } else
                {

                    afterUpdateFunc(callback);  // 지정한 대리자 함수 호출
                }
            });
        }
        //StaticManager.UI.OffBlock();
    }



    // 모든 뒤끝 함수에서 에러 발생 시, 각 에러에 따라 호출해주는 핸들러
    private void SetErrorHandler()
    {
        Backend.ErrorHandler.InitializePoll(true);

        //// 서버 점검 에러 발생 시
        //Backend.ErrorHandler.OnMaintenanceError = () => {
        //    Debug.Log("점검 에러 발생!!!");
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("서버 점검 중", "현재 서버 점검중입니다.\n타이틀로 돌아갑니다.");
        //};
        //// 403 에러 발생시
        //Backend.ErrorHandler.OnTooManyRequestError = () => {
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("비정상적인 행동 감지", "비정상적인 행동이 감지되었습니다.\n타이틀로 돌아갑니다.");
        //};
        //// 액세스토큰 만료 후 리프레시 토큰 실패 시
        //Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
        //    StaticManager.UI.AlertUI.OpenErrorUIWithText("다른 기기 접속 감지", "다른 기기에서 로그인이 감지되었습니다.\n타이틀로 돌아갑니다.");
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
