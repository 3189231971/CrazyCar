﻿using LitJson;
using System.Text;
using UnityEngine;
using QFramework;

public class ApplyAvatarCommand : AbstractCommand {
    private readonly int mCurAid;

    public ApplyAvatarCommand(int curAid) {
        mCurAid = curAid;
    }

    protected override void OnExecute() {
        StringBuilder sb = new StringBuilder();
        JsonWriter w = new JsonWriter(sb);
        w.WriteObjectStart();
        w.WritePropertyName("aid");
        w.Write(mCurAid);
        w.WriteObjectEnd();
        Debug.Log("++++++ " + sb.ToString());
        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        CoroutineController.Instance.StartCoroutine(this.GetSystem<INetworkSystem>().POSTHTTP(url: this.GetSystem<INetworkSystem>().HttpBaseUrl + RequestUrl.changeAvatarUrl,
            data: bytes, token: this.GetModel<IGameModel>().Token.Value,
            succData: (data) => {
                this.GetModel<IUserModel>().Aid.Value = (int)data["aid"];
                this.SendEvent(new UpdataAvatarUIEvent(this.GetModel<IUserModel>().Aid));
                WarningAlertInfo alertInfo = new WarningAlertInfo("Successfully Set");
                UIController.Instance.ShowPage(new ShowPageInfo(UIPageType.WarningAlert, UILevelType.Alart, alertInfo));
            },
            code: (code) => {
                if (code == 423) {
                    WarningAlertInfo alertInfo = new WarningAlertInfo("Did not have");
                    UIController.Instance.ShowPage(new ShowPageInfo(UIPageType.WarningAlert, UILevelType.Alart, alertInfo));
                }
            }));
    }
}