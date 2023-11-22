using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSkillQManager : LevelSkillManagerBase
{
    public static event Action upgradeSkillComplete;
    public static event Action<string> MessageEvent;
    public static event Action SaveDataComplete;

    private SKillManagerQ _skillManagerQ;
    protected virtual void Start()
    {
        _skillManagerQ = GetComponentInParent<SKillManagerQ>();
        SaveDataComplete += updateUi;
        updateUi();
    }
    private void OnDestroy()
    {
        SaveDataComplete -= updateUi;
    }

    public void updateUi()
    {
        if (_skillManagerQ.lv < level)
        {   //Lock
            SetImageUI(0);
        }
        else if (_skillManagerQ.lv > level)
        {
            //Upgraded
            SetImageUI(2);
        }
        else if (_skillManagerQ.lv == level)
        {
            //Upgrade
            SetImageUI(1);
        }
    }
    public virtual string SetStatusForSkill()
    {
        if (_skillManagerQ == null) { return null; }
        if (_skillManagerQ.lv < level)
        {   //Lock
            return imageList[0].name;
        }
        else if (_skillManagerQ.lv > level)
        {
            //Upgraded
            return imageList[2].name;
        }
        else if (_skillManagerQ.lv == level)
        {
            //Upgrade
            return imageList[1].name;
        }
            return null;

    }
    public virtual void OnUpgrade()
    {
        if (SetStatusForSkill() == "Upgrade")
        {
            var enoughCoin =  CoinSystem.instance.GetCoin(coinRequired);
            if (enoughCoin)
            {
                if (upgradeSkillComplete != null) { upgradeSkillComplete(); }
                if (SaveDataComplete != null) { SaveDataComplete(); }
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
        if (MessageEvent != null) MessageEvent(message);
    }
}
