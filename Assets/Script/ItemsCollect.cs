using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCollect : MonoBehaviour
{
    public int addScore;
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
            //�÷�����
            ScoreUI.itemScore = ScoreUI.itemScore + addScore;
            //���Ӵ���Playerʱ��Ʒ��ʧ
            Destroy(gameObject);
        }
    }
}
