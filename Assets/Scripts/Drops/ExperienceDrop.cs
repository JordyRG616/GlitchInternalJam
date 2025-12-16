using System;
using UnityEngine;

public class ExperienceDrop : Drop
{
    [SerializeField] private Color charOneColor;
    [SerializeField] private Color charTwoColor;
    [SerializeField] private SpriteRenderer visual;
    
    private int expAmount;
    private CharacterTag characterTag;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            if(playerCharacter.CharacterTag != characterTag || playerCharacter.Empowered)
                return;
            
            playerCharacter.ReceiveExp(expAmount);
            Collect();
        }
    }

    public void Setup(int amount, CharacterTag characterTag, Vector3 position)
    {
        expAmount = amount;
        this.characterTag = characterTag;

        visual.color = characterTag == CharacterTag.CharacterOne ? charOneColor : charTwoColor;
        
        transform.position = position;
        gameObject.SetActive(true);
    }

    protected override void Collect()
    {
        OnCollect.Fire(this);
        gameObject.SetActive(false);
    }
}
