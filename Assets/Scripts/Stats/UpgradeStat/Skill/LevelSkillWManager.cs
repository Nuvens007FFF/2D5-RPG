using System;
using UnityEngine;

public class LevelSkillWManager : LevelSkillManagerBase
{
    private SkillManagerW _skillManagerW;
    public static event Action upgradeSkillComplete_W;
    public static event Action<string> MessageEvent_W;
    public static event Action SaveDataComplete_W;

    protected  void Start()
    {   
       _skillManagerW = GetComponentInParent<SkillManagerW>();
        SaveDataComplete_W += updateUi;
        updateUi();
    }
    private void OnDestroy()
    {
        SaveDataComplete_W -= updateUi;
    }
    public void updateUi()
    {
        if (_skillManagerW.lv < level)
        {   //Lock
            SetImageUI(0);
        }
        else if (_skillManagerW.lv > level)
        {
            //Upgraded
            SetImageUI(2);
        }
        else if (_skillManagerW.lv == level)
        {
            //Upgrade
            SetImageUI(1);
        }
    }
    public  string SetStatusForSkill()
    {   
        if (_skillManagerW == null) { return null; }
        if (_skillManagerW.lv < level)
        {   //Lock
            return imageList[0].name;
        }
        else if (_skillManagerW.lv > level)
        {   
            //Upgraded
            return imageList[2].name;
        }
        else if (_skillManagerW.lv == level)
        {
            //Upgrade
            return imageList[1].name;
        }
        return null;

    }
    public  void OnUpgrade()
    {
        if (SetStatusForSkill() == "Upgrade")
        {   
            var enoughCoin = CoinSystem.instance.GetCoin(coinRequired);
            if (enoughCoin)
            {
                if (upgradeSkillComplete_W != null) { upgradeSkillComplete_W(); }
                if (SaveDataComplete_W != null) { SaveDataComplete_W(); }
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
        if (MessageEvent_W != null) MessageEvent_W(message);
    }
}
