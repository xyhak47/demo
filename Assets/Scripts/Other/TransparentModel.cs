using System.Collections.Generic;
using UnityEngine;

public class TransparentModel : MonoBehaviour
{
    public static TransparentModel Instance;
    private void Awake()
    {
        Instance = this;
    }
    public Transform[] parentObj;
    public Material transparentMaterial;
    Material[] MatOld;
    Transform[] othersTransform;
    Dictionary<string, Material[]> dic = new Dictionary<string, Material[]>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in parentObj)
        {
            foreach (Transform child in item.GetComponentsInChildren<Transform>())
            {
                var temp = GetGameObjectPath(child);
                dic.Add(temp, child.GetComponent<Renderer>().materials);
            }
        }
    }

    public string GetGameObjectPath(Transform obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.parent.gameObject.transform;
            path = "/" + obj.name + path;
        }
        return path;
    }
    private void ModTransparent(Transform tran)
    {
        Material[] Mat = new Material[tran.GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < Mat.Length; i++)
        {
            Mat[i] = transparentMaterial;
        }
        tran.GetComponent<Renderer>().materials = Mat;
    }
    public void RecoveryTransparent(Transform tran)
    {
        var name = GetGameObjectPath(tran);
        MatOld = dic[name];
        tran.GetComponent<Renderer>().materials = MatOld;
    }
    public void TransparentOthers(string partName)
    {
        for (int i = 0; i < parentObj.Length; i++)
        {
            foreach (Transform child in parentObj[i].GetComponentsInChildren<Transform>())
            {
                if (partName == child.name)
                {
                    othersTransform = child.GetComponentsInChildren<Transform>();
                    continue;
                }
                int count = 0;
                if (othersTransform != null)
                {
                    foreach (var item in othersTransform)
                    {
                        var temp1 = GetGameObjectPath(item);
                        var temp2 = GetGameObjectPath(child);
                        if (temp1 == temp2)
                        {
                            count = 1;
                            break;
                        }
                    }
                }
                //¸ÄÍ¸Ã÷
                if (count == 0)
                {
                    ModTransparent(child);
                }
            }
        }
    }
    public void RecoveryTransparent()
    {
        for (int i = 0; i < parentObj.Length; i++)
        {

            foreach (Transform child in parentObj[i].GetComponentsInChildren<Transform>())
            {
                RecoveryTransparent(child);
            }
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            TransparentModel.Instance.TransparentOthers(GameObject.Find("Buchongshebei").gameObject.name);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TransparentModel.Instance.RecoveryTransparent();
        }*/
    }
}
