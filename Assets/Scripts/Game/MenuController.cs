using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Selectable selectableOnUp = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (selectableOnUp != null)
            {
                EventSystem.current.SetSelectedGameObject(selectableOnUp.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Selectable selectableOnDown = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (selectableOnDown != null)
            {
                EventSystem.current.SetSelectedGameObject(selectableOnDown.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Selectable selectableOnLeft = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
            if (selectableOnLeft != null)
            {
                EventSystem.current.SetSelectedGameObject(selectableOnLeft.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Selectable selectableOnRight = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
            if (selectableOnRight != null)
            {
                EventSystem.current.SetSelectedGameObject(selectableOnRight.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            UnityEngine.UI.Button button = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }
}
