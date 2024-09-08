using Comfort.Common;
using EFT;
using EFT.UI;
using JehreeDevTools.Common;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JehreeDevTools.Modules.Teleport
{
    internal class TeleportPointInteractable : CustomInteractableBase
    {
        public string Name { get; set; }
        public Vector3 TeleportPosition { get; set; }

        public void Init(string name, Vector3 teleportPosition)
        {
            Name = name;
            TeleportPosition = teleportPosition;

            Actions.AddRange(GetActions());
        }

        private List<ActionsTypesClass> GetActions()
        {
            List<ActionsTypesClass> actions = new List<ActionsTypesClass>();

            actions.Add(new ActionsTypesClass
            {
                Name = "Copy Name",
                Action = () =>
                {
                    GUIUtility.systemCopyBuffer = '"' + Name + '"';
                    Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TradeOperationComplete);
                }
            });
            actions.Add(new ActionsTypesClass
            {
                Name = "Delete",
                Action = () =>
                {
                    JDTComponentBase.GetPlayerComponent<TeleportController>().MapData.DeleteTeleport(Name);
                    Die();
                    Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuWeaponDisassemble);
                }
            });

            return actions;
        }

        public void HandleDeathEvent(bool born, string name)
        {
            if (!born && name == Name) Die();
        }

        public static GameObject Create(string name, Vector3 teleportPosition)
        {
            GameObject rootObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            TeleportPointInteractable interactable = rootObj.AddComponent<TeleportPointInteractable>();
            interactable.Init(name, teleportPosition);
            JDTComponentBase.GetPlayerComponent<TeleportController>().MapData.BirthDeathEvent += interactable.HandleDeathEvent;

            Renderer renderer = rootObj.GetComponent<Renderer>();
            renderer.enabled = true;
            renderer.material.color = Color.blue;

            rootObj.name = interactable.Name + "_gameobject";
            rootObj.layer = LayerMask.NameToLayer("Interactive");

            rootObj.transform.position = new Vector3(interactable.TeleportPosition.x, interactable.TeleportPosition.y + 1f, interactable.TeleportPosition.z);
            rootObj.transform.localScale = rootObj.transform.localScale * 0.4f;

            GameObject tmpObj = new GameObject(name + "_tmp");
            TextMeshPro tmp = tmpObj.AddComponent<TextMeshPro>();
            tmpObj.transform.parent = rootObj.transform;
            tmp.text = interactable.Name;
            tmp.fontSize = 4;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.transform.position = rootObj.transform.position + new Vector3(0, 0.5f, 0);
            tmpObj.AddComponent<LookAtTarget>().Init(Singleton<GameWorld>.Instance.MainPlayer.gameObject, invertLook:true);

            return rootObj;
        }

        public void Die()
        {
            gameObject.transform.position = new Vector3(0, 0, -9999);
            Destroy(gameObject, 3);
        }
    }
}
