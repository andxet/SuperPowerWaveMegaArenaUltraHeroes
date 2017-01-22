using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelController : MonoBehaviour
{
    public Image Background1;
    public Image Background2;
    public Image Message;
    public int direction = 1;
    List<Image> bars = new List<Image>();
    public int barsVelocity = 10;
    public float messageVelocity = 0.4f;
    public int ShowTime = 5;

    // Use this for initialization
    void Start()
    {
        bars.Add(Background1);
        bars.Add(Background2);
        Image Background3 = Background1;
        Background1 = Background2;
        Background2 = Background3;
        Invoke("UpdateMessage", messageVelocity);
        //Invoke("CloseMe", ShowTime);
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (Image Background in bars)
            Background.transform.Translate(Vector3.right * barsVelocity * direction);

        if (direction < 0)
        {
            if (Background1.transform.position.x <  -(Background1.rectTransform.rect.width * Background1.rectTransform.lossyScale.x))
            {
                Background1.transform.position = new Vector3(Background2.transform.position.x + Background2.rectTransform.rect.width * Background2.rectTransform.lossyScale.x, Background1.transform.position.y, Background1.transform.position.z);
                Image Background3 = Background1;
                Background1 = Background2;
                Background2 = Background3;
            }   
        }
        else         
        {
            if (Background1.transform.position.x > Screen.width)
            {
                Background1.transform.position = new Vector3(Background2.transform.position.x - Background1.rectTransform.rect.width * Background1.rectTransform.lossyScale.x, Background1.transform.position.y, Background1.transform.position.z);
                Image Background3 = Background1;
                Background1 = Background2;
                Background2 = Background3;
            }
        }
        /*Background.transform.Translate(Vector3.right * barsVelocity * direction);
        if (barsVelocity < 0)
        {
            if (Background.transform.position.x < Screen.width - Background.flexibleWidth)

}
            ||
            barsVelocity > 0 && Background.transform.position.x > 0)
            gameObject.SetActive(false);*/
        /*GameObject objToDelete = null;
        GameObject objToAdd = null;
        foreach (GameObject obj in bars)
        {
            obj.transform.Translate(Vector3.right * barsVelocity * direction);
            if (obj.transform.position.x >= 0 && bars.Count != 2)
            {
                objToAdd = Instantiate(Background);
                objToAdd.transform.position = obj.transform.position;
                //objToAdd.transform.Translate()
            }

        }*/

        /*if (objToAdd != null)
            bars.Add(objToAdd);
        if (objToDelete != null)
            bars.Remove(objToDelete);

        Image img = obj.GetComponent<Image>();
        if (img.transform.position.x >= 0 && bars.Count != 2)
            bars.Add(Instantiate(Background));
        if (img.transform.position.x >= Screen.width)
        {
            bars.Remove(obj);
            break;
        }*/
    }

    public void Init(PlayerPosition loosePlayer)
    {
        if (loosePlayer == PlayerPosition.LEFT)
        {
            direction = -1;
            //Background.transform.position = new Vector3(Screen.width, Background.transform.position.y, Background.transform.position.z);
            Image Background3 = Background1;
            Background1 = Background2;
            Background2 = Background3;
        }
        else
        {
            direction = 1;
        }        
    }

    public void CloseMe()
    {
        gameObject.SetActive(false);
    }

    public void UpdateMessage()
    {
        if (Message.transform.rotation.z < 0)
            Message.transform.rotation = Quaternion.Euler(0, 0, 5);            
        else
            Message.transform.rotation = Quaternion.Euler(0, 0, -5);
        Invoke("UpdateMessage", messageVelocity);
    }
}
