using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD.Map 
{ 
    public class MiniMap : MonoBehaviour
    {
        public Camera mapCamera;    //С��ͼ���
        public Transform player;    //����
        public Transform minPlayerIcon;//С��ͼ����ͼ��
        public Transform maxPlayerIcon;//���ͼ����ͼ��
        private float minMapSize;   //С��ͼ��С
        public float maxMapSize;    //���ͼ��С
        public float minSize;       //С��ͼ��Сֵ
        public float maxSize;       //С��ͼ���ֵ
        public GameObject minMap;   //С��ͼ
        public GameObject maxMap;   //���ͼ
        private bool isMaxMap = false;//�Ƿ�򿪴��ͼ

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
                //��̬ʵʱ���´��ͼ���λ�ü�����ͼ��ת��
                mapCamera.transform.position = new Vector3(player.position.x, mapCamera.transform.position.y, player.position.z);
                maxPlayerIcon.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
            }
            else
            {
                //��̬ʵʱ����С��ͼ���λ�ü�����ͼ��ת��
                mapCamera.transform.position = new Vector3(player.position.x, mapCamera.transform.position.y, player.position.z);
                minPlayerIcon.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
            }
        
        }

        /// <summary>
        /// ����С��ͼ
        /// </summary>
        /// <param name="value">���ų߶�</param>
        public void ChangeMaxSize(float value)
        {
            minMapSize += value;
            minMapSize = Mathf.Clamp(minMapSize, minSize, maxSize);
            mapCamera.orthographicSize = minMapSize;
        }

        /// <summary>
        /// �򿪴��ͼ
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
