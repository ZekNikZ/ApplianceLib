using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();

        //StartCoroutine(MyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MyCoroutine()
    {
        while (true)
        {
            Animator.SetBool("IsActive", true);
            yield return new WaitForSeconds(3);
            Animator.SetBool("IsActive", false);
            yield return new WaitForSeconds(3);
        }
    }
}
