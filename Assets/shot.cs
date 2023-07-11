using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{

    public GameObject hitEffect;
    [SerializeField] float attackDamage = 5f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<Health>().UpdateHealth(-attackDamage);
        if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<Enemy>().UpdateHealth(-attackDamage);
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);

    }


}
