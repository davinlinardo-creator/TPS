using UnityEngine;

public class Itemspin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 10 * Time.deltaTime, 0); // Rotate around Y-axis at 10 degrees per second
    }
}
