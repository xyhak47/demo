using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class URPDrawRenderObject : MonoBehaviour
{
    [Tooltip("Íâ±ß¿òÑÕÉ«")]
    public Color OutLineColor = Color.green;

    [HideInInspector]
    public Material material;

    public void Awake()
    {
     
      
    }

    public void Start()
    {

    }


    void OnEnable()
    {
        URPDrawRenderPass.AddTarget(this);
    }

    void OnDisable()
    {
        URPDrawRenderPass.RemoveTarget(this);
    }
}