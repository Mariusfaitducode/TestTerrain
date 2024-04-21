using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMesh : MonoBehaviour
{
    public GameObject meshObject;
    private MeshCollider meshCollider;

    public void AddCollider()
    {
        meshCollider = meshObject.AddComponent<MeshCollider >();
        
        //meshCollider.sharedMesh = lod
    }
}
