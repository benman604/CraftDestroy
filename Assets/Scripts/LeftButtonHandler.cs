using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LeftButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if(this.gameObject.name == "L")
        {
            Title.move = 5;
        }
        else
        {
            Title.move = -5;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Title.move = 0f;
    }
}