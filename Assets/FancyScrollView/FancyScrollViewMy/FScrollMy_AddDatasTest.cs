namespace FancyScrollView {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using Text = TMPro.TMP_Text;
    using InputField = TMPro.TMP_InputField;
    using Dropdown = TMPro.TMP_Dropdown;
    using ItemData = FScrollMy_ItemData;
    using Context = FScrollMy_Context;


    public class FScrollMy_AddDatasTest : MonoBehaviour {

        public FScrollMy ScrollView;
        [Header("--Debug-------------")]
        public bool Debug_UseTestData = true;
        public int Debug_TestDataCount = 20;
        public void Debug_AddTestData() {
            var items = Enumerable.Range(0, Debug_TestDataCount)
            .Select(i => new ItemData() { TagStr = "Cell " + i })
            .ToArray();

            this.ScrollView.UpdateData(items);
            this.ScrollView.SelectCell(0);
        }


        #region mb初始
        private void Awake() {
            //Debug.Log("awake");
        }
        void OnEnable() {
            //Debug.Log("Awake-> *OnEnable->Property->Start");

        }
        void Start() {
#if DEBUG && UNITY_EDITOR
            if (Debug_UseTestData) {
                //只有在完成初始化后 在start状态才能添加数据
                this.StartCoroutine(TestData());
            }
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("type", "hostport");
            map.Add("display", "NameDisplay");
            map.Add("id", "<#ID:123>");
            //
            string js = LitJson.JsonMapper.ToJson(map);
            Debug.Log(js);
#endif
        }
        IEnumerator TestData() {
            //
            //yield return null;
            this.Debug_AddTestData();            
            yield break;
        }
        #endregion


    }


}
