using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChange : MonoBehaviour
{
    public Renderer rend;

    [SerializeField]
    public Color colorToTurnTo = Color.white;

    private void Start()
    {

        rend = GetComponent<Renderer>();

        rend.material.color = colorToTurnTo;
    }
}
