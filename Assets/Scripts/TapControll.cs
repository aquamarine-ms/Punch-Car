using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapControll : MonoBehaviour , IController
{
    
    [SerializeField] private PlayerController playerController;
    
    void Start()
    {
        Controller = playerController;
    }

    void Update()
    {
#if UNITY_EDITOR
        GetClickStandalone();
        #else        
    GetClick();

#endif
    }

    private void GetClick()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.position.x > (float)Screen.width / 2)
            {
                Right();
            }
            else
            {
                Left();
            }
        }
    }

    private void GetClickStandalone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x > (float)Screen.width / 2)
            {
                Right();
            }
            else
            {
                Left();
            }
        }
    }

    public PlayerController Controller { get; set; }

    public void Left()
    {
        Controller.Left();
    }

    public void Right()
    {
        Controller.Right();
    }

    public void Up()
    {
        throw new System.NotImplementedException();
    }

    public void Down()
    {
        throw new System.NotImplementedException();
    }
}

