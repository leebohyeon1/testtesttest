using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cartoon : MonoBehaviour
{
    public GameObject[] cartoon;
    public GameObject credit;
    public float Speed;
    public bool ismove;
    private void Start()
    {
        ismove = false; 
    }
    void Update()
    {
       if (ismove)
        {
            credit.transform.Translate(Vector3.up * Speed * Time.deltaTime);
            if (credit.transform.GetComponent<RectTransform>().anchoredPosition.y >= 3100)
            {
                Speed = 0;
                StartCoroutine(load());
            }
        }
    }
    public void move()
    {
       
       ismove=true;
    }
    public IEnumerator load()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("EntryScene");
    }
}
