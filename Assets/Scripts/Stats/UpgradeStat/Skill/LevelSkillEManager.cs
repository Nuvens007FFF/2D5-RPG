using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkillEManager : LevelSkillManagerBase
{
    private SkillManagerE _skillManagerE;
    public static event Action upgradeSkillComplete_E;
    public static event Action<string> MessageEvent_E;
    public static event Action SaveDataComplete_E;

    protected void Start()
    {
        _skillManagerE = GetComponentInParent<SkillManagerE>();
        SaveDataComplete_E += updateUi;
        updateUi();
    }
    private void OnDestroy()
    {
        SaveDataComplete_E -= updateUi;
    }
    public  void updateUi()
    {
        if (_skillManagerE.lv < level)
        {   //Lock
            SetImageUI(0);
        }
        else if (_skillManagerE.lv > level)
        {
            //Upgraded
            SetImageUI(2);
        }
        else if (_skillManagerE.lv == level)
        {
            //Upgrade
            SetImageUI(1);
        }
    }
    public string SetStatusForSkill()
    {
        if (_skillManagerE == null) { return null; }
        if (_skillManagerE.lv < level)
        {   //Lock
            return imageList[0].name;
        }
        else if (_skillManagerE.lv > level)
        {
            //Upgraded
            return imageList[2].name;
        }
        else if (_skillManagerE.lv == level)
        {
            //Upgrade
            return imageList[1].name;
        }
        return null;

    }
    public void OnUpgrade()
    {
        if (SetStatusForSkill() == "Upgrade")
        {
            var enoughCoin = CoinSystem.instance.GetCoin(coinRequired);
            if (enoughCoin)
            {
                if (upgradeSkillComplete_E != null) { upgradeSkillComplete_E(); }
                if (SaveDataComplete_E != null) { SaveDataComplete_E(); }
            }
            else
            {
                var message = "Bạn không đủ tiền để nâng cấp kỹ năng này !";
                SendMessageEvent(message);
            }
        }
        else if (SetStatusForSkill() == "Upgraded")
        {
            var message = "Bạn đã nâng cấp kỹ năng lên cấp này rồi !";
            SendMessageEvent(message);
        }
        else if (SetStatusForSkill() == "Lock")
        {
            var message = "Bạn chưa đủ cấp độ để mở khóa kỹ năng này !";
            SendMessageEvent(message);
        }
    }
    private void SendMessageEvent(string message)
    {
        if (MessageEvent_E != null) MessageEvent_E(message);
    }
}
