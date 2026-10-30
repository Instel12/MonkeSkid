
using BepInEx;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;

namespace MonkeSkid
{
    [BepInPlugin("instel.MonkeSkid", "MonkeSkid", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            StartCoroutine(createButtons());
        }

        IEnumerator createButtons()
        {
            while (true)
            {
                CustomMapsLoadRoomMapButton[] buttons = FindObjectsOfType<CustomMapsLoadRoomMapButton>(true);

                foreach (CustomMapsLoadRoomMapButton button in buttons)
                {
                    if (button.GetComponent<HasMonkeSkid>() == null)
                    {
                        button.gameObject.AddComponent<HasMonkeSkid>();

                        GameObject copy = Instantiate(button.gameObject);

                        MonkeSkidButton buttonScript = copy.gameObject.AddComponent<MonkeSkidButton>();
                        buttonScript.pressedMaterial = button.pressedMaterial;
                        buttonScript.unpressedMaterial = button.unpressedMaterial;

                        copy.transform.SetParent(button.transform.parent, true);

                        copy.transform.position = button.transform.position;
                        copy.transform.localScale = button.transform.localScale;
                        copy.transform.rotation = button.transform.rotation;

                        copy.transform.position -= copy.transform.forward * 0.05f;

                        Destroy(copy.GetComponent<CustomMapsLoadRoomMapButton>());
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public class HasMonkeSkid : MonoBehaviour { }

    public class MonkeSkidButton : GorillaPressableButton
    {
        private float pressedTime = 0.2f;

        void Awake()
        {
            TextMeshPro tmp = new GameObject().AddComponent<TextMeshPro>();
            tmp.transform.SetParent(transform, true);
            tmp.text = "COPY LUAU";
            tmp.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            tmp.transform.localPosition = new Vector3(0, -0.51f, 0);
            tmp.transform.localScale = new Vector3(0.5f, 1, 1);
            tmp.color = Color.black;
            tmp.fontSize = 3;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(x => x.name.ToLower().Contains("utopium"));
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
            StartCoroutine(ButtonPressed_Local());

            GUIUtility.systemCopyBuffer = CustomGameMode.LuaScript;
        }

        private IEnumerator ButtonPressed_Local()
        {
            isOn = true;
            SetPressedMaterial();
            yield return new WaitForSeconds(pressedTime);
            SetUnpressedMaterial();
        }
    }
}
