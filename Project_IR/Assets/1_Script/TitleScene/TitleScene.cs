
using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        // �ϴ� �ٷ� ���� 
        GuestLogin();
    }

    private void GuestLogin()
    {
        // �Խ�Ʈ �α���
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
                            //ShowAlertUI($"�������� �ʴ� ���̵��Դϴ�.");

                        } else if (callback.GetMessage().Contains("bad customPassword"))
                        {
                            //ShowAlertUI($"��й�ȣ�� �ùٸ��� �ʽ��ϴ�.");

                        } else if (callback.GetMessage().Contains("maintenance"))
                        {
                            //StaticManager.UI.AlertUI.OpenWarningUI("Error", "���� �������Դϴ�.");

                        } else
                        {

                        }
                    } else if (callback.GetStatusCode() == "403")
                    { // ���ܵ� ����
                        //StaticManager.UI.AlertUI.OpenWarningUI("Error", callback.GetErrorCode().ToString());
                        return;
                    } else if (callback.GetStatusCode() == "409")
                    {
                        //ShowAlertUI($"�α��ο� �����Ͽ����ϴ�\n{callback.ToString()}");
                        // �̹����� �ϸ� �α��� 
                        Backend.BMember.CustomLogin(id, pw, callback => {
                            if (callback.IsSuccess() == false)
                            {
                                if (callback.GetStatusCode() == "401")
                                {
                                    if (callback.GetMessage().Contains("bad customId"))
                                    {
                                        //ShowAlertUI($"�������� �ʴ� ���̵��Դϴ�.");

                                    } else if (callback.GetMessage().Contains("bad customPassword"))
                                    {
                                        //ShowAlertUI($"��й�ȣ�� �ùٸ��� �ʽ��ϴ�.");

                                    } else if (callback.GetMessage().Contains("maintenance"))
                                    {
                                        //ShowAlertUI($"���� �������Դϴ�.");

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
                                Debug.Log("�α��ο� �����߽��ϴ�");
                                //GoNextScene();
                                StaticManager.Inst.ChangeScene("LoadingScene");
                            }
                        });
                        return;
                    } else
                    {
                        if (callback.GetStatusCode() == "403")
                        { // ���ܵ� ����
                            //StaticManager.UI.AlertUI.OpenWarningUI("Error", callback.GetErrorCode().ToString());
                            return;
                        }
                    }
                } else
                {
                    // �г����� ���� ���, �г��� ���� UI ����
                    //_LoginUI_NickName.Open();
                }

            } catch (Exception e)
            {
                //ShowAlertUI(e.ToString());
            }
        });
    }
}
