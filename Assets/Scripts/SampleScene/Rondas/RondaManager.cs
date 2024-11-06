using UnityEngine;

public class RondaManager : MonoBehaviour
{
    [SerializeField] public RondaController rnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rnd.enemicsActuals = 5;
        rnd.rondaActual = 1;
    }

    // Update is called once per frame
    public void Reset(int enemicsRonda)
    {
        rnd.enemicsActuals = enemicsRonda;
        rnd.rondaActual++;
    }
}
