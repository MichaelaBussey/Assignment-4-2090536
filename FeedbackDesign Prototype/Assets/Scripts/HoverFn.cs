using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverFn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text underline;
    public GameObject percent;
    private bool mouse_over = false;   
    //public string percent;
    //public float fadeTime;   
    //public bool displayInfo; 

    void Update()
    {
        if (mouse_over)
        {
            //Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        percent.SetActive(true);
        //percent.color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        percent.SetActive(false);
        //percent.color = Color.clear;
    }

    /*void Start()
    {
        underline = GameObject.Find("Text").GetComponent<Text>();
        percent.color = Color.clear;
    }

    void Update()
    {
        //FadeText(); 
    }*/

    /*private void OnMouseOver(Text underline)
    {
        percent.color = Color.blue;
    }

    private void OnMouseExit()
    {
        percent.color = Color.clear;
    }

    /*public void FadeText ()
    {
        if(displayInfo)
        {
            text.text = percent;
            text.color = Color.Lerp(text.color, Color.blue, fadeTime * Time.deltaTime);
        }
        else
        {
            text.color = Color.Lerp(text.color, Color.clear, fadeTime * Time.deltaTime);
        }*/
}

