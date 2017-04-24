using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCard : MonoBehaviour {

    public Text Name;
    public Image image;
    public Text Cost;
    public Text Effect;
    public RectTransform r;
	public void UpdateDisplay (Card card) {
        Name.text = card.Name;
        image.sprite = card.sprite();
        Cost.text = card.Cost + "";
        Effect.text = card.EffectDes;
        Vector2 R = new Vector2(r.localPosition.x, r.localPosition.y);
        R.y = card.InfoImgY;
        r.localPosition = R;
        r.sizeDelta = new Vector2(r.sizeDelta.x, card.InfoImgH);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Close();
        }
    }

    public void Close()
    {
        GameController.Controller.ShowingCardInfo = false;
        gameObject.SetActive(false);
    }
}
