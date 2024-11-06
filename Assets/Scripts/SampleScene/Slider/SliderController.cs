using UnityEngine;

public class SliderController : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void FixedUpdate()
    {
            transform.rotation = Quaternion.identity;
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

}
