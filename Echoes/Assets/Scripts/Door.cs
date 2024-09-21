using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private CircleCollider2D col;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && DynamiteCountHolder.Instance.dynamiteKeys > 0)
        {
            DynamiteCountHolder.Instance.DecreaseKeyByOne();
            Destroy(this.gameObject);
        }
    }

}
