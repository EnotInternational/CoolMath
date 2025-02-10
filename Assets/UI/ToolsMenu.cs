using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ToolsMenu : MonoBehaviour
{
    int countPages =0;
    public Image fonLevels;
    public Button LeftMenu;
    public GameObject[] Pages; 
    bool pressLeftMenu;
    bool pressMenu;
    public RectTransform slider;
    Vector2 v;
    public GameObject panel;

    void Start()
    {
       v = slider.anchorMax;
    }
    public void Menu()
    {
        pressMenu = true;
    }

    public void leftMenu()
    {
        pressLeftMenu = true;
    }

    void Update()
    {
    if(pressLeftMenu==true)
        {
            v.x += 0.01f;
            slider.anchorMax = v;
            if (slider.anchorMax.x >= 0.09f)
            {
                pressLeftMenu = false;
            }
        }    
    if(pressMenu==true)
        {
            v.x -= 0.01f;
            slider.anchorMax = v;
            if (slider.anchorMax.x <= 0)
            {
                pressMenu = false;
            }
        }   
    
    
    
    
    }
    public void Volume(Button volum)
    {
        Image img = volum.GetComponent<Image>();
        Debug.Log(img.sprite.name);
        if (img.sprite.name=="no-sound")
        {
            img.sprite = Resources.Load<Sprite>("volume");
        }
        else
        {
            img.sprite = Resources.Load<Sprite>("no-sound");
        }
    }



}
