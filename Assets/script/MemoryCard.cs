using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour {

    [SerializeField] private SceneController controller;
    [SerializeField] private GameObject cardBack;   // Переменная в панели Инспектор

    private int _id;
    public int id
    {
        get { return _id; }     // Функция чтения
    }

    public void SetCard(int id , Sprite image)   // Открытый метод для передачи указанному объекту новый спрайт
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()       // Функция вызывается при клике на Object
    {
        if (cardBack.activeSelf && controller.canReveal)        // Проверка свойств canReveal котроллера, для гарантии открытия только 2-х карт
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);      // Уведомление контроллера об открытии карты
        }
    }

    public void Unreveal()      // SceneController скрывает карту вернув "рубашку"
    {
        cardBack.SetActive(true);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
