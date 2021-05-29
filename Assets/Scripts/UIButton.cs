using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PlayerFollow player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion p = new Quaternion(0, 0, player.transform.rotation.z, player.transform.rotation.w);
        transform.rotation = p;
    }

}
