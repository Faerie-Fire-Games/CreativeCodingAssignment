using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    void FixedUpdate()
    {
        gameObject.transform.position = player.position;
    }
}
