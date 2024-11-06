using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RondaText : MonoBehaviour
{
    [SerializeField] private RondaController rnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator changeText()
    {
        this.GetComponent<TextMeshProUGUI>().text = "Ronda: " + rnd.rondaActual;
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        this.gameObject.SetActive(false);
    }
}
