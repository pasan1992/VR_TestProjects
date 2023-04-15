using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSword : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer m_swordMat;
    public GameObject m_sword;
    void Start()
    {
        m_swordMat = m_sword.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        m_swordMat.material.SetColor("_Color", Color.white);
    }

    public void startAttack()
    {
       m_swordMat.material.SetColor("_Color", Color.red);
    }
}
