using Asm;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public class SlotField : MonoBehaviour
    {
        [SerializeField]
        private ArgTokenColorMap argTokenColorMap = null;
        [SerializeField]
        private GameObject tokenPrefab = null;

        private Argument arg;
        private GameObject token;
        private List<RectTransform> slotRTs;

        private TMP_InputField inputField;

        void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Init(Argument arg, List<RectTransform> slotRTs)
        {
            this.arg = arg;
            this.slotRTs = slotRTs;
            slotRTs.Add(GetComponent<RectTransform>());

            if (arg.type == Argument.Type.IMMEDIATE) {
                inputField.text = arg.val.ToString();
            } else {
                InsertToken(arg);
                inputField.text = "0";
            }
            inputField.onEndEdit.AddListener((string val) => arg.val = int.Parse(val));
        }

        public void InsertToken(Argument arg, GameObject transferToken = null)
        {
            Debug.Assert(arg.type != Argument.Type.IMMEDIATE);
            Debug.Assert(token == null);

            inputField.interactable = false;
            if (transferToken == null) {
                token = Instantiate(tokenPrefab, transform, false);
                token.GetComponent<Draggable>().Init(slotRTs);

                // Configure token text
                TextMeshProUGUI tm = token.GetComponentInChildren<TextMeshProUGUI>();
                if (arg.type == Argument.Type.REGISTER) {
                    tm.text = "R" + arg.val.ToString();
                } else {
                    tm.text = arg.label.name;
                }

                // Configure token color
                ArgTokenColorMap.Type tokenType = arg.type == Argument.Type.REGISTER
                    ? ArgTokenColorMap.Type.REGISTER
                    : arg.label.type == Label.Type.BRANCH
                    ? ArgTokenColorMap.Type.BRANCH_LABEL
                    : ArgTokenColorMap.Type.CONST_LABEL;
                token.GetComponent<Image>().color = argTokenColorMap.map[tokenType];

                // Resize to fit the preferred width
                RectTransform rt = GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(tm.GetPreferredValues(tm.text).x, rt.sizeDelta.y);
                RectTransform tokenRT = token.GetComponent<RectTransform>();
                tokenRT.sizeDelta = rt.sizeDelta;
            } else {
                token = transferToken;
                token.transform.SetParent(transform, false);
            }
        }

        public void ReleaseToken()
        {
            Debug.Assert(token != null);

            token = null;
            arg.type = Argument.Type.IMMEDIATE;
            arg.val = 0;
            inputField.interactable = true;
            inputField.text = arg.val.ToString();
        }
    }
}