using UnityEngine;

public class ChangerColor : MonoBehaviour
{
    public void ChangeColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }
}