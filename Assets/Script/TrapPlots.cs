using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlots : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //�����ײ
        if (other.collider.CompareTag("Player"))
        {
            //���Ӵ���Playerʱ����ؿ���ʧ
            Destroy(gameObject);
        }
    }
}
