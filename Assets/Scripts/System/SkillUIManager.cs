using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public Button skillQ;
    public Button skillW;
    public Button skillE;
    public Button skillR;
    public GameObject skillQ2;
    public GameObject skillE2;

    // Set these values based on your game design
    private float skillQCooldown = 1.5f;
    private float skillWCooldown = 10f;
    private float skillECooldown = 1.5f;
    private float skillRMaxEnergy = 100f;
    private bool UnlockSkill4 = false;
    public bool isRready = false;
    public bool QECombo = false;
    public bool EQCombo = false;

    private float skillQTimer;
    private float skillWTimer;
    private float skillETimer;
    public float skillREnergy;
    public Text skillQCooldownText;
    public Text skillWCooldownText;
    public Text skillECooldownText;
    public Text skillREnergyText;

    // Start is called before the first frame update
    void Start()
    {
        skillQTimer = 0f;
        skillWTimer = 0f;
        skillETimer = 0f;
        skillREnergy = 0f;
        skillQCooldown = UpdateStatCharacter.instance.CoolDownQ;
        skillECooldown = UpdateStatCharacter.instance.CoolDownE;
        UnlockSkill4 = UpdateStatCharacter.instance.UnLockSkillR;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldowns();
        UpdateCooldownText();
        UpdateSkillCombo();
    }

    void UpdateCooldowns()
    {
        skillQTimer -= Time.deltaTime;
        skillWTimer -= Time.deltaTime;
        skillETimer -= Time.deltaTime;

        // Update button interactability based on cooldowns
        skillQ.interactable = skillQTimer <= 0f;
        skillW.interactable = skillWTimer <= 0f;
        skillE.interactable = skillETimer <= 0f;
        skillR.interactable = (isRready && UnlockSkill4);
    }

    void UpdateSkillCombo()
    {
        if(EQCombo)
        {
            skillQ2.SetActive(true);
        }
        else
        {
            skillQ2.SetActive(false);
        }
        if (QECombo)
        {
            skillE2.SetActive(true);
        }
        else
        {
            skillE2.SetActive(false);
        }
    }

    void UpdateCooldownText()
    {
        // Display the remaining cooldown time on the skillR button
        skillQCooldownText.text = skillQTimer > 0f ? skillQTimer.ToString("F1") : "";
        skillWCooldownText.text = skillWTimer > 0f ? skillWTimer.ToString("F1") : "";
        skillECooldownText.text = skillETimer > 0f ? skillETimer.ToString("F1") : "";
        skillREnergyText.text = skillREnergy < skillRMaxEnergy ? skillREnergy.ToString("F1") : "";
    }

    public void SkillQUsed()
    {
        skillQTimer = skillQCooldown;
        Debug.Log("Q Used");
    }

    public void SkillWUsed()
    {
        skillWTimer = skillWCooldown;
        Debug.Log("W Used");
    }

    public void SkillEUsed()
    {
        skillETimer = skillECooldown;
        Debug.Log("E Used");
    }
}
