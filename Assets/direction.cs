using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class direction : MonoBehaviour
{
    public Transform player;
    

    private void Update()
    {
        Vector2 dir = player.position;
        dir.y += 1;
        GetComponent<Rigidbody2D>().position = dir;
        GetComponent<Rigidbody2D>().rotation = player.rotation.z;
    }

   
    

}
