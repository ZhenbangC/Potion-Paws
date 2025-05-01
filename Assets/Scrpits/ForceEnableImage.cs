using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ForceEnableImage : MonoBehaviour
{
    void Awake()
    {
        Image img = GetComponent<Image>();
        if (img != null && !img.enabled)
        {
            img.enabled = true;
        }
    }
}