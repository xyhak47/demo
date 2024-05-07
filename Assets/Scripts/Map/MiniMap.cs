using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD.Map 
{ 
    public class MiniMap : MonoBehaviour
    {
        public Camera mapCamera;    //小地图相机
        public Transform player;    //人物
        public Transform minPlayerIcon;//小地图人物图标
        public Transform maxPlayerIcon;//大地图人物图标
        private float minMapSize;   //小地图大小
        public float maxMapSize;    //大地图大小
        public float minSize;       //小地图最小值
        public float maxSize;       //小地图最大值
        public GameObject minMap;   //小地图
        public GameObject maxMap;   //大地图
        private bool isMaxMap = false;//是否打开大地图

        private void Awake()
        {
            minMapSize = mapCamera.orthographicSize;
            player = GameObject.Find("Player").transform;
        }

        // Update is called once per frame
        private void Update()
        {
            if (isMaxMap)
            {
                //动态实时更新大地图相机位置及人物图标转向
                mapCamera.transform.position = new Vector3(player.position.x, mapCamera.transform.position.y, player.position.z);
                maxPlayerIcon.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
            }
            else
            {
                //动态实时更新小地图相机位置及人物图标转向
                mapCamera.transform.position = new Vector3(player.position.x, mapCamera.transform.position.y, player.position.z);
                minPlayerIcon.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
            }
        
        }

        /// <summary>
        /// 缩放小地图
        /// </summary>
        /// <param name="value">缩放尺度</param>
        public void ChangeMaxSize(float value)
        {
            minMapSize += value;
            minMapSize = Mathf.Clamp(minMapSize, minSize, maxSize);
            mapCamera.orthographicSize = minMapSize;
        }

        /// <summary>
        /// 打开大地图
        /// </summary>
        public void OpenMaxMap()
        {
            maxMap.gameObject.SetActive(true);
            minMap.gameObject.SetActive(false);
            mapCamera.orthographicSize = maxMapSize;
            isMaxMap = true;
        }


        public void OpenMinMap()
        {
            maxMap.gameObject.SetActive(false);
            minMap.gameObject.SetActive(true);
            mapCamera.orthographicSize = minMapSize;
            isMaxMap = false;
        }
    }
}
