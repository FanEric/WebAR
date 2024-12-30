using UnityEngine;
using System.Collections;
public class ScaleAndRotate : MonoBehaviour
{
    private Touch oldTouch1;  //�ϴδ�����1(��ָ1)
    private Touch oldTouch2;  //�ϴδ�����2(��ָ2)
    void Update()
    {
        //û�д��������Ǵ�����Ϊ0
        if (Input.touchCount <= 0)
        {
            return;
        }
        //���㴥���� ˮƽ������ת
        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;
            transform.Rotate(Vector3.down * deltaPos.x, Space.World);//��Y�������ת
            //transform.Rotate(Vector3.right * deltaPos.y, Space.World);//��X�������ת
            //transform.Rotate(Vector3.right * deltaPos.y, Space.World);//��X�������ת���������ǻ�����д��Z�������ת
        }
        //��㴥��, �Ŵ���С
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //��2��տ�ʼ�Ӵ���Ļ, ֻ��¼����������
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }
        //�����ϵ����������µ��������룬���Ҫ�Ŵ�ģ�ͣ���СҪ����ģ��
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        //��������֮�Ϊ����ʾ�Ŵ����ƣ� Ϊ����ʾ��С����
        float offset = newDistance - oldDistance;
        //�Ŵ����ӣ� һ�����ذ� 0.01������(100�ɵ���)
        float scaleFactor = offset / 100f;
        Vector3 localScale = transform.localScale;
        Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                    localScale.y + scaleFactor,
                                    localScale.z + scaleFactor);
        //��ʲô����½�������
        if (scale.x >= 0.05f && scale.y >= 0.05f && scale.z >= 0.05f)
        {
            transform.localScale = scale;
        }
        //��ס���µĴ����㣬�´�ʹ��
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }
}