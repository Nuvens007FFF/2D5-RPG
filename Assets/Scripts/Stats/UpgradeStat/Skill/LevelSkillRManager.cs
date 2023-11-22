using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkillRManager : LevelSkillManagerBase
{
    public static event Action upgradeSkillComplete_R;
    public static event Action<string> MessageEvent_R;
    public static event Action SaveDataComplete;

    private SkillManagerR _skillManagerR;
    protected virtual void Start()
    {
        _skillManagerR = GetComponentInParent<SkillManagerR>();
        SaveDataComplete += updateUi;
        updateUi();
    }

    public virtual void updateUi()
    {
        if (_skillManagerR.lv < level)
        {   //Lock
            SetImageUI(0);
        }
        else if (_skillManagerR.lv == level)
        {
            //Upgrade
            SetImageUI(1);
        }
    }
    public virtual string SetStatusForSkill()
    {
        if (_skillManagerR == null) { return null; }
        if (_skillManagerR.lv < level)
        {   //Lock
            return imageList[0].name;
        }
        else if (_skillManagerR.lv == level)
        {
            //Upgrade
            return imageList[1].name;
        }
        return null;

    }
    public virtual void OnUpgrade()
    {
        if (SetStatusForSkill() == "Lock")
        {
            var enoughCoin = CoinSystem.instance.GetCoin(coinRequired);
            if (enoughCoin)
            {
                if (upgradeSkillComplete_R != null) { upgradeSkillComplete_R(); }
                if (SaveDataComplete != null) { SaveDataComplete(); }
            }
            else
            {
                var message = "Bạn không đủ tiền để nâng cấp kỹ năng này !";
                SendMessageEvent(message);
            }
        }
        else if (SetStatusForSkill() == "UnLockSkill")
        {
            var message = "Bạn đã mở khóa kỹ năng này rồi !";
            SendMessageEvent(message);
        }
    }
    private void SendMessageEvent(string message)
    {
        if (MessageEvent_R != null) MessageEvent_R(message);
    }
}
