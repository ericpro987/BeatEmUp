using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RondaController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<TextMeshProUGUI>().text = "Ronda: "+controller.rondaActual;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
