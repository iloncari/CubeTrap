using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FirstTimeSet : MonoBehaviour
{
    public Button btn;
    // Start is called before the first frame update
    void Start()
    {
        print(Screen.width + " " + Screen.height);

        float posx = ((Screen.width*1f) / 2f) + ((Screen.width*1f) / 2f) - 40f;
        float posy = ((Screen.height * 1f) / 2f) + 50f;
        Button enemy = Instantiate(btn, new Vector3(60f, 30f, 0), Quaternion.identity) as Button;
        enemy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
       // Instantiate(btn, new Vector3(-224f, -270.0f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
