//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using UnityEngine;

//public class TestJson : MonoBehaviour
//{
//    public void SaveJSonFile()
//    {
//        foreach (KeyValuePair)

//            FileStream fs = new FileStream(sheet.Key + ".Json", FileMode.Create);   // sheet,Key �Ƹ� ��Ʈ������ ����ѵ�
//        StreamWriter sw = new StreamWriter(FSM, Encoding.Unicode);
//        // KeyValuePair : Dictionary���� Key & Value ��� �޾ƿ´ٴ� 

//        if(sw != null)
//        {
//            //JsonWriter writer = new JsonWriter(sw);       // LitJSon��� ��û
//            writer.WriteObjectStart();                      // ���� �߰�ȣ
//            StreamWriter.WritePropertyName(sheet.Key);      // ��Ʈ��
//            writer.WriteArrayStart();                       // ���ȣ


//            foreach(KeyValuePair)
//            {
//                writer.WriteObjectStart();                  // �߰�ȣ

//                writer.WriteObjectEnd();                    // �׻� �ݾ���� ��
//            }

//            writer.WriteArrayEnd();                         // �׻� �ݾ���� ��(���ȣ)
//            writer.WriteObjectEnd();                        // �׻� �ݾ���� ��(���� �߰�ȣ)

//            // ������ �����ٸ� ������ �ݴ°� ���� ����
//            //sw.Close();
//        }
//    }

//}
