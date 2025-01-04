using System.Collections;
using UnityEngine;

public class ShikuSolitaire : MonoBehaviour
{
    public Deck deck;
    public CameraSnapper camSnapper;
    public GameObject camSnapObject;
    public Transform shuffleTimerBar; 
    public static ShikuSolitaire Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        deck.cardAnimation.Stop();
        deck.StopShuffle();
        if (shuffleTimerBar != null)
        {
            shuffleTimerBar.localScale = new Vector3(0, shuffleTimerBar.localScale.y, shuffleTimerBar.localScale.z);
        }

        ShuffleDeck();
    }

    IEnumerator ShuffleLoop()
    {
        float shuffleDuration = 10f;
        deck.cardAnimation.Rewind();
        deck.cardAnimation.Play();
        deck.StartShuffle();

        if (shuffleTimerBar != null)
        {
            StartCoroutine(UpdateShuffleBar(shuffleDuration));
        }

        yield return new WaitForSeconds(shuffleDuration);
        deck.cardAnimation.Rewind();
        deck.cardAnimation.Stop();
        deck.StopShuffle();

        if (shuffleTimerBar != null)
        {
            shuffleTimerBar.localScale = new Vector3(0, shuffleTimerBar.localScale.y, shuffleTimerBar.localScale.z);
        }
    }

    IEnumerator UpdateShuffleBar(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scaleX = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            shuffleTimerBar.localScale = new Vector3(scaleX, shuffleTimerBar.localScale.y, shuffleTimerBar.localScale.z);
            yield return null;
        }
    }

    public void ShuffleDeck()
    {
        if (GameManager.instance.Money >= 50f)
        {
            StartCoroutine(ShuffleLoop());
        }
    }
}