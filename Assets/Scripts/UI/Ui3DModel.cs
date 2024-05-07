using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD.UI
{
    public class Ui3DModel : MonoBehaviour
    {
        public List<GameObject> AllObjMaster = new List<GameObject>();

        internal void ActiveMaster(string name)
        {
            CancelMaster();
            foreach(GameObject _o in AllObjMaster)
            {
                if (_o && _o.name.Equals(name))
                {
                    _o.SetActive(true);
                }
            }
        }

        internal void CancelMaster()
        {
            foreach(GameObject _o in AllObjMaster)
            {
                if(_o)
                    _o.SetActive(false);
            }
        }
    }
}