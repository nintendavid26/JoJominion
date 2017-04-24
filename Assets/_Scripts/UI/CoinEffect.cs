using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinEffect : MonoBehaviour
{

    public int amount;
    public Text text;
    public float y;
    public float speed;
    public Image s;
    public float fade;

    // Use this for initialization
    void Start()
    {
        text.text = "+" + amount;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);
        Color C = new Color(s.color.r, s.color.g, s.color.b, s.color.a);
        C.a -= fade;
        s.color = C;
        C = new Color(text.color.r, text.color.g, text.color.b, text.color.a);
        C.a -= fade;
        text.color = C;
        if (s.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public static CoinEffect Make(int amount, Vector2 loc, CoinEffect effect)
    {
        CoinEffect h = Instantiate(effect, Camera.main.WorldToScreenPoint(loc), Quaternion.identity, FindObjectOfType<Canvas>().transform) as CoinEffect;
        h.amount = amount;
        return h;

    }
}
