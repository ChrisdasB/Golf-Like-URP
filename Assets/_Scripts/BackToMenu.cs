using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public static event Action SetMenuFlag;

    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GoBackToMenu);
    }

    private void GoBackToMenu()
    {
        SetMenuFlag.Invoke();
        print("Menuflaginvoked!");
    }

}
