using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour, IInteractable
{
    private bool isHeld = false;
    private bool isLocked = false;
    private bool isScanned = false;

    [SerializeField] private SOObjectInfo objectInfo;

    [SerializeField] private float infoDisplayHeight = 0.3f;

    [SerializeField] private ParticleSystem currentEffect;

    public void OnInteract()
    {
        //Debug.Log("Interagindo com o cubo");

        if (isLocked) return;

        if (HoldingManager.Instance.TryPickUp(gameObject))
        {
            isHeld = true;
            ShowObjectInfo();
        }
        else if (isHeld)
        {
            HoldingManager.Instance.Drop();
            isHeld = false;
            HideObjectInfo();
        }
    }

    public void StopInteract()
    {
        Debug.Log("Parando interação com o cubo");
    }

    void Update()
    {

        if (InputHandler.TryRayCastHit(out RaycastHit hitObject))
        {
            if (hitObject.transform == this.transform)
            {
                OnInteract();
            }
        }
    }

    private void ShowObjectInfo()
    {
        if (objectInfo == null || isScanned == false) return;

        var infoController = FindObjectOfType<ObjectInfoController>();

        if (infoController != null)
        {
            infoController.SetObjectInfo(objectInfo);
            infoController.SetVisible(true);

            infoController.transform.SetParent(transform);
            infoController.transform.localPosition = new Vector3(0, infoDisplayHeight, 0);
        }

    }

    private void HideObjectInfo()
    {
        var infoController = FindObjectOfType<ObjectInfoController>();

        if (infoController != null)
        {
            infoController.SetVisible(false);
            infoController.transform.SetParent(null);
            //stop particle effect
            currentEffect.Stop();
        }
    }

    public void SetLocked(bool locked = true)
    {
        isLocked = locked;
    }
    public void SetScanned(bool scanned = true)
    {
        isScanned = scanned;
    }
}
