using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageTextUI : MonoBehaviour
{
    private TextMeshPro TxtDamage;
    private Transform rect;

    public void InitializeDamageText(int damageAmount, bool isCritic, float yPos)
    {
        TxtDamage = GetComponent<TextMeshPro>();
        rect = GetComponent<Transform>();

        // ÀÏ¹Ý°ø°Ý : Èò»ö±Û¾¾, »¡¸® »ç¶óÁü
        // Å©¸®Æ¼ÄÃ : ÁÖÈ²»ö±Û¾¾, ÃµÃµÈ÷ »ç¶óÁü

        TxtDamage.text = damageAmount.ToString();

        this.gameObject.SetActive(true);

        if (isCritic)
        {
            TxtDamage.color = new Color32(255, 102, 2,255);
            TxtDamage.fontSize = 4;

            rect.DOMoveY(yPos + 0.8f, 0.8f).SetEase(Ease.InOutQuad).OnComplete(() => this.gameObject.SetActive(false));
        } else
        {
            TxtDamage.color = Color.white;
            TxtDamage.fontSize = 3;

            rect.DOMoveY(yPos + 0.6f, 0.6f).SetEase(Ease.InOutQuad).OnComplete(() => this.gameObject.SetActive(false));
        }
    }
}
