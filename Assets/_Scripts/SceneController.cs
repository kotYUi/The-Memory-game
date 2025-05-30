using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;
    [SerializeField] TMP_Text scoreLabel;

    private MemoryCard firstRevealde;
    private MemoryCard secondRevealde;

    private int score = 0;

    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    public bool canReveal
    {
        get
        {
            return secondRevealde == null;
        }
    }
    
    private void Start()
    {
        Vector3 startPos = originalCard.transform.position;
        int[] numbers = {0, 0, 1, 1, 2, 2, 3, 3};
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    } 

    public void Restart()
    {
        SceneManager.LoadScene("_main");
    }
    
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealde == null)
        {
            firstRevealde = card;
        }
        else
        {
            secondRevealde = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (firstRevealde.Id == secondRevealde.Id)
        {
            score++;
            scoreLabel.text = "Score:" + score.ToString();
        }
        else
        {
            yield return new WaitForSeconds(1f);
            firstRevealde.Unrevealed();
            secondRevealde.Unrevealed();
        }
        firstRevealde = null;
        secondRevealde = null;
    }
}
