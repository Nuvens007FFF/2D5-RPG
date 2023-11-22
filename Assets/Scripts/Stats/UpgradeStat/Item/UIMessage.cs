using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    public TextMeshProUGUI message_tmp;
    [SerializeField] private float duration = 0.5f;
    public Vector3 punchVector = new Vector3(9,15,0);

    private void Start()
    {
        InventoryManager.MessageEvent += TakeMessage;
        StoreManager.MessageStoreEvent += TakeMessage;
        UpdateRequiredForSlot.SendMessageEvent += TakeMessage;
        LevelSkillQManager.MessageEvent += TakeMessage;
        LevelSkillEManager.MessageEvent_E += TakeMessage;
        LevelSkillWManager.MessageEvent_W += TakeMessage;
        LevelSkillRManager.MessageEvent_R += TakeMessage;
    }
        private void OnDestroy()
    {
        InventoryManager.MessageEvent -= TakeMessage;
        StoreManager.MessageStoreEvent -= TakeMessage;
        UpdateRequiredForSlot.SendMessageEvent -= TakeMessage;
        LevelSkillQManager.MessageEvent -= TakeMessage;
        LevelSkillEManager.MessageEvent_E -= TakeMessage;
        LevelSkillWManager.MessageEvent_W -= TakeMessage;
        LevelSkillRManager.MessageEvent_R -= TakeMessage;
    }
    void TakeMessage( string message)
    {
        message_tmp.text = message;
        //animation
        // message_tmp.transform.DOShakeScale(0.5f, strength: new Vector3(duration, duration, 0f), vibrato: 3, randomness: 0).OnComplete(()=>Invoke("OfMessage",1f));
        message_tmp.transform.DOPunchPosition(punchVector, duration, vibrato: 10, elasticity: 1f).OnComplete(() => Invoke("OfMessage", 1f)); 
    }
    void OfMessage()
    {
        message_tmp.text = null;
    }
}
