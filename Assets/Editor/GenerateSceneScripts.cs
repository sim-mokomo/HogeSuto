using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UniRx;

public class GenerateSceneScripts : EditorWindow {

    [MenuItem("Editors/Generate Scene Scripts")]
    public static void OpenWindow()
    {
        GetWindow<GenerateSceneScripts>();
    }

    private Object scriptSaveDirectory;
    private List<string> saveFileNameList;
    private ReactiveProperty<int> fileNum = new ReactiveProperty<int>();
    private const string fileExtension = ".cs";

    private void OnEnable()
    {
        saveFileNameList = new List<string>();
        fileNum.Subscribe(num =>
       {
            
            int fileNumSub = Mathf.Abs(num - saveFileNameList.Count);
           if (num > saveFileNameList.Count)
           {
               for (int i = 0; i < fileNumSub; i++)
               {
                   saveFileNameList.Add("");
               }
           }
            else if (saveFileNameList.Count > num)
            {
               saveFileNameList.RemoveRange(index: num, count: fileNumSub);
            }
                                       
            saveFileNameList.Capacity = num;
       });
    }

    private void OnGUI()
    {
        scriptSaveDirectory = EditorGUILayout.ObjectField(label: "ファイル保存先", obj: scriptSaveDirectory, objType: typeof(Object),allowSceneObjects: false, options: null) ;

        int prevFileNum = fileNum.Value;
        fileNum.Value = EditorGUILayout.IntField(label: "生成ファイル数", value: fileNum.Value);
        fileNum.Value = Mathf.Max(fileNum.Value, 1);
        if (prevFileNum != fileNum.Value)
            fileNum.Publish(fileNum.Value);

        for (int i = 0; i < saveFileNameList.Count ; i++)
        {
            string fileName = saveFileNameList[i];
            fileName = EditorGUILayout.TextField(label: string.Format("生成ファイル名{0}",i), text: fileName, options: null);
            saveFileNameList[i] = fileName;
        }


        if(GUILayout.Button("生成"))
        {

            saveFileNameList = saveFileNameList.Where(filename => string.IsNullOrEmpty(filename) == false).ToList();
            foreach(string fileName in saveFileNameList)
            {
                string savePath = AssetDatabase.GetAssetPath(scriptSaveDirectory);
                savePath =  System.IO.Path.Combine(savePath, fileName+fileExtension);
                
                StringBuilder contentBuilder = new StringBuilder();
                contentBuilder.AppendLine("using System.Collections;");
                contentBuilder.AppendLine("using System.Collections.Generic;");
                contentBuilder.AppendLine("using UnityEngine;");
                contentBuilder.AppendLine("");

                Debug.Log(fileName);
                contentBuilder.AppendLine(string.Format("public class {0} : MonoBehaviour ",fileName));
                contentBuilder.AppendLine("{");
                contentBuilder.AppendLine("public void Init() { ");
                contentBuilder.AppendLine("}");
                contentBuilder.AppendLine("public void Move() { ");
                contentBuilder.AppendLine("}");

                contentBuilder.AppendLine("}");
                
                string fileContent = contentBuilder.ToString();
                
                File.WriteAllText(path: savePath, contents: fileContent, encoding: System.Text.Encoding.UTF8);
            }
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }
    }

}
