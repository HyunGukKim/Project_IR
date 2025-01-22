
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using GoogleSheet.Protocol.v2.Res;
using GoogleSheet.Protocol.v2.Req;
using UGS;
using System;
using UGS.IO;
using GoogleSheet;
using System.Collections.Generic;
using System.IO;
using GoogleSheet.Type;
using System.Reflection;
using UnityEngine;


namespace t_StageData
{
    [GoogleSheet.Attribute.TableStruct]
    public partial class Enemy_Group : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<Enemy_Group> loadedList, Dictionary<int, Enemy_Group> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "1Yle4VbHW9fCd_JEo05-J5YbCrHS9SnpZHQ9CEELmQNI"; // it is file id
        static string sheetID = "1903928438"; // it is sheet id
        static UnityFileReader reader = new UnityFileReader();

/* Your Loaded Data Storage. */
    
        public static Dictionary<int, Enemy_Group> Enemy_GroupMap = new Dictionary<int, Enemy_Group>();  
        public static List<Enemy_Group> Enemy_GroupList = new List<Enemy_Group>();   

        /// <summary>
        /// Get Enemy_Group List 
        /// Auto Load
        /// </summary>
        public static List<Enemy_Group> GetList()
        {{
           if (isLoaded == false) Load();
           return Enemy_GroupList;
        }}

        /// <summary>
        /// Get Enemy_Group Dictionary, keyType is your sheet A1 field type.
        /// - Auto Load
        /// </summary>
        public static Dictionary<int, Enemy_Group>  GetDictionary()
        {{
           if (isLoaded == false) Load();
           return Enemy_GroupMap;
        }}

    

/* Fields. */

		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt stage;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_0;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_0_per;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_1;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_1_per;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_2;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_2_per;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_3;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_3_per;
		public CodeStage.AntiCheat.ObscuredTypes.ObscuredInt Enemy_Count;
  

#region fuctions


        public static void Load(bool forceReload = false)
        {
            if(isLoaded && forceReload == false)
            {
#if UGS_DEBUG
                 Debug.Log("Enemy_Group is already loaded! if you want reload then, forceReload parameter set true");
#endif
                 return;
            }

            string text = reader.ReadData("t_StageData"); 
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                CommonLoad(result.jsonObject, forceReload); 
                if(!isLoaded)isLoaded = true;
            }
      
        }
 

        public static void LoadFromGoogle(System.Action<List<Enemy_Group>, Dictionary<int, Enemy_Group>> onLoaded, bool updateCurrentData = false)
        {      
                IHttpProtcol webInstance = null;
    #if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    webInstance = UnityEditorWebRequest.Instance as IHttpProtcol;
                }
                else 
                {
                    webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
                }
    #endif
    #if !UNITY_EDITOR
                     webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
    #endif
          
 
                var mdl = new ReadSpreadSheetReqModel(spreadSheetID);
                webInstance.ReadSpreadSheet(mdl, OnError, (data) => {
                    var loaded = CommonLoad(data.jsonObject, updateCurrentData); 
                    onLoaded?.Invoke(loaded.list, loaded.map);
                });
        }

               


    public static (List<Enemy_Group> list, Dictionary<int, Enemy_Group> map) CommonLoad(Dictionary<string, Dictionary<string, List<string>>> jsonObject, bool forceReload){
            Dictionary<int, Enemy_Group> Map = new Dictionary<int, Enemy_Group>();
            List<Enemy_Group> List = new List<Enemy_Group>();     
            TypeMap.Init();
            FieldInfo[] fields = typeof(Enemy_Group).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string, string, string)>(); 
            List<List<string>> rows = new List<List<string>>();
            var sheet = jsonObject["Enemy_Group"];

            foreach (var column in sheet.Keys)
            {
                string[] split = column.Replace(" ", null).Split(':');
                         string column_field = split[0];
                string   column_type = split[1];

                typeInfos.Add((column, column_field, column_type));
                          List<string> typeValues = sheet[column];
                rows.Add(typeValues);
            }

          // 실제 데이터 로드
                    if (rows.Count != 0)
                    {
                        int rowCount = rows[0].Count;
                        for (int i = 0; i < rowCount; i++)
                        {
                            Enemy_Group instance = new Enemy_Group();
                            for (int j = 0; j < typeInfos.Count; j++)
                            {
                                try
                                {
                                    var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                    //int, float, List<..> etc
                                    string type = typeInfos[j].type;
                                    if (type.StartsWith(" < ") && type.Substring(1, 4) == "Enum" && type.EndsWith(">"))
                                    {
                                         Debug.Log("It's Enum");
                                    }

                                    var readedValue = TypeMap.Map[typeInfo].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue);

                                }
                                catch (Exception e)
                                {
                                    if (e is UGSValueParseException)
                                    {
                                        Debug.LogError("<color=red> UGS Value Parse Failed! </color>");
                                        Debug.LogError(e);
                                        return (null, null);
                                    }

                                    //enum parse
                                    var type = typeInfos[j].type;
                                    type = type.Replace("Enum<", null);
                                    type = type.Replace(">", null);

                                    var readedValue = TypeMap.EnumMap[type].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue); 
                                }
                              
                            }
                            List.Add(instance); 
                            Map.Add(instance.stage, instance);
                        }
                        if(isLoaded == false || forceReload)
                        { 
                            Enemy_GroupList = List;
                            Enemy_GroupMap = Map;
                            isLoaded = true;
                        }
                    } 
                    return (List, Map); 
}


 

        public static void Write(Enemy_Group data, System.Action<WriteObjectResult> onWriteCallback = null)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Enemy_Group).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;
                string writeRule = null;
                if(type.IsEnum)
                {
                    writeRule = TypeMap.EnumMap[type.Name].Write(fields[i].GetValue(data));
                }
                else
                {
                    writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                } 
                datas[i] = writeRule; 
            }  
           
#if UNITY_EDITOR
if(Application.isPlaying == false)
{
                UnityPlayerWebRequest.Instance.WriteObject(new WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}
else
{
            UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}
#endif

#if !UNITY_EDITOR
   UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

#endif
        } 
          

 


#endregion

#region OdinInsepctorExtentions
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button("UploadToSheet")]
    public void Upload()
    {
        Write(this);
    }
 
    
#endif


 
#endregion
    public static void OnError(System.Exception e){
         UnityGoogleSheet.OnTableError(e);
    }
 
    }
}
        