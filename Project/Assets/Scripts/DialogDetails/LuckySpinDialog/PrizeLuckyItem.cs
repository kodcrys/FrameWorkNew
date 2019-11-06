using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeLuckyItem : MonoBehaviour
{
    private Image iconImg;
    private Text prizeTxt;
    [SerializeField]
    private int valueCost;

    private void Awake()
    {
        iconImg = GetComponentInChildren<Image>();
        prizeTxt = GetComponentInChildren<Text>();
    }

    public void SetUp(PrizeLuckySpinConfigData config)
    {
        // TODO: change sprite of image
        //Debug.LogError(config);
        prizeTxt.text = config.value.ToString();
        valueCost = config.value;
    }
}
