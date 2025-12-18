using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickThreeBox : ReactiveScreenElement
{
    public Signal OnChoiceMade = new Signal();
    
    [SerializeField] private Image icon;
    [Space]
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body;

    private WeaponUpgrade currentUpgrade = null;


    private void Start()
    {
        RegisterReaction(EventTriggerType.PointerEnter, e => transform.localScale = Vector3.one * 1.1f);
        RegisterReaction(EventTriggerType.PointerExit, e => transform.localScale = Vector3.one);

        RegisterReaction(EventTriggerType.PointerUp, ChooseThis);
    }

    private void ChooseThis(PointerEventData obj)
    {
        currentUpgrade.Apply();
        currentUpgrade = null;
        OnChoiceMade.Fire();
    }

    public void ReceiveUpgrade(WeaponUpgrade upgrade)
    {
        currentUpgrade = upgrade;

        icon.sprite = upgrade.weaponIcon;
        header.text = upgrade.upgradeName;
        body.text = upgrade.upgradeDescription;
    }
}
