using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectCoin : MonoBehaviour
{
    public System.Action OnEndEffect;

    [SerializeField]
    private UnityEngine.UI.Text coinTxt;
    [SerializeField]
    private ParticleSystem particle;

    public void SetUp(int valueCoin)
    {
        particle.gameObject.SetActive(true);
        coinTxt.text = "+"+valueCoin.ToString();
        coinTxt.transform.position = Vector3.zero;
        Color32 newColor = coinTxt.color;
        newColor.a = 255;
        coinTxt.color = newColor;
        coinTxt.DOKill();
        coinTxt.transform.DOMoveY(coinTxt.transform.position.y + 6f, 2.8f);
        newColor.a = 100;
        coinTxt.DOColor(newColor, 2.8f);
    }

    private void Update()
    {
        if (!particle.IsAlive())
        {
            if (OnEndEffect != null)
                OnEndEffect();
            gameObject.SetActive(false);
            coinTxt.transform.position = Vector3.zero;
            Color32 newColor = coinTxt.color;
            newColor.a = 255;
            coinTxt.color = newColor;
        }
    }
}
