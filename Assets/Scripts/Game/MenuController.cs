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
            EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp().gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown().gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft().gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight().gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        }
    }
}
