
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        // 일단 바로 접속 
        GuestLogin();
    }

    private void GuestLogin()
    {
        // 게스트 로그인
        ObscuredString id = "test";
        ObscuredString pw = "1234";
        Backend.BMember.CustomSignUp(id, pw, (callback) => {
            Debug.Log($"Backend.BMember.CustomLogin : {callback}");
            try
            {
                if (callback.IsSuccess() == false)
                {
                    if (callback.GetStatusCode() == "401")
                    {
                        Debug.Log(callback.GetMessage());
                        if (callback.GetMessage().Contains("bad customId"))
                        {
                            //ShowAlertUI($"존재하지 않는 아이디입니다.");

                        } else if (callback.GetMessage().Contains("bad customPassword"))
                        {
                            //ShowAlertUI($"비밀번호가 올바르지 않습니다.");

                        } else if (callback.GetMessage().Contains("maintenance"))
                        {
                            //StaticManager.UI.AlertUI.OpenWarningUI("Error", "서버 점검중입니다.");

                        } else
                        {

                        }
                    } else if (callback.GetStatusCode() == "403")
                    { // 차단된 계정
                        //StaticManager.UI.AlertUI.OpenWarningUI("Error", callback.GetErrorCode().ToString());
                        return;
                    } else if (callback.GetStatusCode() == "409")
                    {
                        //ShowAlertUI($"로그인에 실패하였습니다\n{callback.ToString()}");
                        // 이미존재 하면 로그인 
                        Backend.BMember.CustomLogin(id, pw, callback => {
                            if (callback.IsSuccess() == false)
                            {
                                if (callback.GetStatusCode() == "401")
                                {
                                    if (callback.GetMessage().Contains("bad customId"))
                                    {
                                        //ShowAlertUI($"존재하지 않는 아이디입니다.");

                                    } else if (callback.GetMessage().Contains("bad customPassword"))
                                    {
                                        //ShowAlertUI($"비밀번호가 올바르지 않습니다.");

                                    } else if (callback.GetMessage().Contains("maintenance"))
                                    {
                                        //ShowAlertUI($"서버 점검중입니다.");

                                    } else
                                    {

                                    }
                                } else if (callback.GetStatusCode() == "403")
                                {
                                    //StaticManager.UI.AlertUI.OpenWarningUI("Error", callback.GetErrorCode().ToString());
                                    return;
                                }
                            } else
                            {
                                Debug.Log("로그인에 성공했습니다");
                                //GoNextScene();
                                StaticManager.Inst.ChangeScene("LoadingScene");
                            }
                        });
                        return;
                    } else
                    {
                        if (callback.GetStatusCode() == "403")
                        { // 차단된 계정
                            //StaticManager.UI.AlertUI.OpenWarningUI("Error", callback.GetErrorCode().ToString());
                            return;
                        }
                    }
                } else
                {
                    // 닉네임이 없을 경우, 닉네임 생성 UI 오픈
                    //_LoginUI_NickName.Open();
                }

            } catch (Exception e)
            {
                //ShowAlertUI(e.ToString());
            }
        });
    }
}
