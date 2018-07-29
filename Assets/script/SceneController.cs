using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    public const int gridRows = 2;      // Значения указывают кол-во ячеек сетки и их расположение друг от друга
    public const int gridCols = 4;      //
    public const float offsetX = 3f;    //
    public const float offsetY = 3.5f;  //
    private int _score = 0;
    [SerializeField] private MemoryCard originalCard;   // Ссылка для карты в сцене
    [SerializeField] private Sprite[] images;           // Массив для ссылок на ресурсы-спрайты
    [SerializeField] private TextMesh scoreLabel;

    // Use this for initialization
    void Start()
    {
        Vector3 startPos = originalCard.transform.position;    // Положения первой (оригинальной) карты, все остольные координаты копий отсчитываются от неё

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };            // Объявление массива с парными индификаторами для 4-х спрайтов

        numbers = ShuffleArray(numbers);                       // Вызвоф функции, перемешивающий элементы массива

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)                 // Вложенные циклы для задания столбцов и строк сетки
            {
                MemoryCard card;                               // Ссылка для контейнера для исходной карты или её копий
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];                       // Получаем ID из перемешанного списка
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetX * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);    // Смещение по осям X and Y
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)       // Алгоритм перемешивания Кнута (Фишера-Йетса)
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

    
    public bool canReveal
    {
        get
        { return _secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)               // Сохрание карт в одной из переменных
        {
            _firstRevealed = card;              //
        }                               
        else
        {
            _secondRevealed = card;          //
            StartCoroutine(CheckMatch());   //     
        }
    }
    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)         // Сравнение ID открытых карт
        {
            _score++;                                       // Увеличение счета, если ID совпадают
            scoreLabel.text = "Score: " + _score;          //
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            _firstRevealed.Unreveal();                       // Закрытие несовпадающих карт
            _secondRevealed.Unreveal();                     //
        }
        _firstRevealed = null;                             // Очистка переменных
        _secondRevealed = null;                           // 
    }

    public void Restart()
    {
        Application.LoadLevel("card");      // Загрузка сцены
    }
}

