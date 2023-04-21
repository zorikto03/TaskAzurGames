using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject StartUI;

    [SerializeField] int MaxCoins = 50;
    [SerializeField] int CoinsPerCoin = 1;
    [SerializeField] int GrassCost = 15;
    [SerializeField] TextMeshProUGUI textCoins;
    [SerializeField] RectTransform CoinPrefab;
    [SerializeField][Range(0.2f, 0.9f)] float minDuration = 0.2f;
    [SerializeField][Range(0.9f, 2f)] float maxDuration = 1.5f;

    [SerializeField] AudioSource Audio;

    int _counter;

    Vector3 targetPosition;
    Queue<RectTransform> queue;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = textCoins.rectTransform.position;
        queue = new Queue<RectTransform>();
        PrepareCoins();
    }


    public void PrepareCoins()
    {
        for (int i = 0; i < MaxCoins; i++)
        {
            var coin = Instantiate(CoinPrefab);
            coin.anchorMin = new Vector2(1, 1);
            coin.anchorMax = new Vector2(1, 1);
            coin.pivot = new Vector2(0.5f, 0.5f);
            coin.anchoredPosition = new Vector3(0, 0, 0);
            coin.transform.SetParent(transform);
            coin.gameObject.SetActive(false);
            queue.Enqueue(coin);
        }
    }

    public void Animate(Vector3 pos)
    {
        Audio.Play();

        for (int i = 0; i < CoinsPerCoin; i++)
        {
            if (queue.Count > 0)
            {
                var coin = queue.Dequeue();
                coin.gameObject.SetActive(true);
                coin.position = Camera.main.WorldToScreenPoint(pos); new Vector3(Screen.width / 2, 0);

                var duration = Random.Range(minDuration, maxDuration);

                coin.DOMove(targetPosition, duration).
                    OnComplete(() =>
                    {
                        _counter += GrassCost;
                        queue.Enqueue(coin);
                        coin.gameObject.SetActive(false);
                        VibrateCounter();
                    });
            }
        }
    }

    void VibrateCounter()
    {
        textCoins.text = _counter.ToString();
        var seq = DOTween.Sequence();
        seq.Append(textCoins.transform.DOScale(1.5f, 0.2f));
        seq.Append(textCoins.transform.DOScale(1f, 0.2f));
    }

    public void Touched()
    {
        StartUI.SetActive(false);
    }
}
