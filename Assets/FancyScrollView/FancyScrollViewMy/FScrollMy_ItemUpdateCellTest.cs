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
    

    public class FScrollMy_ItemUpdateCellTest : MonoBehaviour {

        [SerializeField] Text message;
        [SerializeField] Image image;
        [SerializeField] Button button;
        public FScrollMy_Item FItem;

    
        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="data"></param>
        /// <param name="cellobj"></param>
        public void UpdateCellContent(int dataIndex, FScrollMy_ItemData itemData, GameObject cellObj) {
            message.text = itemData.TagStr;

            var selected = FItem.Context.SelectedIndex == dataIndex;
            image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
            if (this.FItem.Context.OnUpdateCellContent != null) {
                this.FItem.Context.OnUpdateCellContent(dataIndex, itemData, cellObj);
            }
        }
        public void ClickedCell(int dataIndex, FScrollMy_ItemData data, GameObject cellobj) {
            if (this.FItem.Context.OnCellSelect != null) {
                this.FItem.Context.OnCellSelect(dataIndex);
            }
            if (this.FItem.Context.OnCellClicked != null) {
                this.FItem.Context.OnCellSelect(dataIndex);
            }
        }
        #region mb初始
        private void Awake() {
            Debug.Log("awake");
            this.FItem = this.GetComponent<FScrollMy_Item>();
            this.button.onClick.AddListener(() => {
                this.FItem.OnNotifyClickedCell();
            });
        }
        void OnEnable() {
            Debug.Log("Awake-> *OnEnable->Property->Start");

        }
        void Start() {

        }
  
        #endregion


    }

}
