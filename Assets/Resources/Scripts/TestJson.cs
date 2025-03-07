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

//            FileStream fs = new FileStream(sheet.Key + ".Json", FileMode.Create);   // sheet,Key 아마 시트명으로 명명한듯
//        StreamWriter sw = new StreamWriter(FSM, Encoding.Unicode);
//        // KeyValuePair : Dictionary에서 Key & Value 모두 받아온다는 

//        if(sw != null)
//        {
//            //JsonWriter writer = new JsonWriter(sw);       // LitJSon사용 추청
//            writer.WriteObjectStart();                      // 최초 중괄호
//            StreamWriter.WritePropertyName(sheet.Key);      // 시트명
//            writer.WriteArrayStart();                       // 대괄호


//            foreach(KeyValuePair)
//            {
//                writer.WriteObjectStart();                  // 중괄호

//                writer.WriteObjectEnd();                    // 항상 닫아줘야 함
//            }

//            writer.WriteArrayEnd();                         // 항상 닫아줘야 함(대괄호)
//            writer.WriteObjectEnd();                        // 항상 닫아줘야 함(최초 중괄호)

//            // 파일을 열었다면 무조건 닫는걸 먼저 만듦
//            //sw.Close();
//        }
//    }

//}
