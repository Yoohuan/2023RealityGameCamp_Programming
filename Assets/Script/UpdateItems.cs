using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpdateItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //���ʰȡ
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControl.playerState = PlayerControl.State.stateUpdate;
            //���Ӵ���Playerʱ��Ʒ��ʧ
            Destroy(gameObject);
        }
    }
}
