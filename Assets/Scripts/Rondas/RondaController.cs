using UnityEngine;

public class RondaController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int rondaActual;
    public int enemicsActuals;
    void Start()
    {
        rondaActual = 1;
        enemicsActuals = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(int enemicsRonda)
    {
        enemicsActuals = enemicsRonda;
        rondaActual++;
    }
}
