using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class AssetsUpdateState : AbstractState<LaunchStates, Launch>, IController {
    public AssetsUpdateState(FSM<LaunchStates> fsm, Launch target) : base(fsm, target) {
    }

    public override void OnEnter() {
        this.SendCommand(new InitDllCommand());
        this.RegisterEvent<FinishDownloadResEvent>(OnFinishDownloadRes);
        UIController.Instance.ShowPage(new ShowPageInfo(UIPageType.DownloadResUI, UILevelType.UIPage));
    }
    
    private void OnFinishDownloadRes(FinishDownloadResEvent e) {
        if (e.isFinish) {
            mFSM.ChangeState(LaunchStates.InitConfig);
        } else {
            mFSM.ChangeState(LaunchStates.ExitGameState);
        }
    }

    public override void OnExit() {
        this.UnRegisterEvent<FinishDownloadResEvent>(OnFinishDownloadRes);
    }

    public IArchitecture GetArchitecture() {
        return CrazyCar.Interface;
    }
}
