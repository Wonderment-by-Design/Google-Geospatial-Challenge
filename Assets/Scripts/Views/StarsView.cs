using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsView : MonoBehaviour
{
    [SerializeField]
    Image[] stars;
    Color starYellow = new Color(1f, 0.93f, 0.32f);

    public void SetStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            stars[i].color = starYellow;
        }
    }

    public void ResetStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = Color.white;
        }
    }
}
