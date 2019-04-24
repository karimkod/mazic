using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFogOfwarSetter : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    public Transform PlayerTransform { set { playerTransform = value; } }
    [SerializeField]
    Renderer fogOfWarRenderer;


    Vector4[] array;
    int currentIndex; 

    // Use this for initialization
    void Awake () {
        array = new Vector4[10];
        currentIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //fogOfWarRenderer.material.SetVector("_Player1_Pos", playerTransform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            array[currentIndex] = playerTransform.position;
            currentIndex++;
            fogOfWarRenderer.material.SetVectorArray("_lampePosition",array);
            fogOfWarRenderer.material.SetFloat("_lampeNumber", currentIndex);

        }
	}

    
}
