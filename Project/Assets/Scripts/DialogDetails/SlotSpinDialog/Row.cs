using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] private float maxRow = 4.37f;
    [SerializeField] private float minRow = 0;
    [SerializeField] private float defaultInterval = 0.0025f;
    [SerializeField] private int maxSlot = 8;
    [SerializeField] private float cellHeight = 0.72f;

    public void SetUp()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, cellHeight * (int)PrizeSlotName.Seven, 0);
    }

    public void RotatePlay(PrizeSlotConfigData prize, Action callback = null)
    {
        StopCoroutine("Rotate");
        StartCoroutine(Rotate(prize, callback));
    }

    private IEnumerator Rotate(PrizeSlotConfigData prize, Action callback = null)
    {
        Vector3 curPos = transform.localPosition;
        float timeInterval = defaultInterval;
        int stepMoveSlow = 60;
        int moveWithConstTimeInterval = 20;
        // Move with const timeInterval
        for (int i = 0; i < moveWithConstTimeInterval + stepMoveSlow; i++)
        {
            yield return new WaitForSeconds(timeInterval);
            curPos.y += cellHeight;
            if (curPos.y >= maxRow)
            {
                curPos.y = minRow;
                transform.localPosition = curPos;
            }
            transform.localPosition = curPos;

            // move slowly
            if (i >= stepMoveSlow)
            {
                if (i > Mathf.RoundToInt(stepMoveSlow * 0.25f))
                    timeInterval = 0.005f;
                if (i > Mathf.RoundToInt(stepMoveSlow * 0.5f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(stepMoveSlow * 0.75f))
                    timeInterval = 0.02f;
                if (i > Mathf.RoundToInt(stepMoveSlow * 0.95f))
                    timeInterval = 0.03f;
            }
        }

        float prizePos = cellHeight * (int)prize.prizeName;
        transform.DOLocalMoveY(prizePos, 0.5f).SetEase(Ease.OutElastic).OnComplete(()=> {
            if (callback != null)
                callback();
        });
    }
}
