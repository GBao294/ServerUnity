using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    //public GameObject hitEffect;
    [SerializeField] float attackDamage = 5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 5f);
        //Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<Health>().UpdateHealth(-attackDamage);
    }
}
