using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorkMove : MonoBehaviour
{
   public IEnumerator OnMouseDown()    //ʹ��Э��
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(transform.position);//��ά��������ת��Ļ����
        //�������Ļ����תΪ��ά���꣬�ټ�������λ�������֮��ľ���
        var offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetScreenPos.z));

        while (Input.GetMouseButton(0))
        {
            //�����λ�ö�ά����תΪ��ά����
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetScreenPos.z);
            //�����ת������ά������ת������������+���������λ�õ�ƫ����
            var targetPos = Camera.main.ScreenToWorldPoint(mousePos) + offset;
            transform.position = targetPos;
            yield return new WaitForFixedUpdate();//ѭ��ִ��
        }
    }

}
