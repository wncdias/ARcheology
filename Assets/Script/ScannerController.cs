using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    [SerializeField] private SpotController spot;
    [SerializeField] private float scanDuration = 4f;
    [SerializeField] GameObject scanUI;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            TrytoPutOnSpot(collision.gameObject);
        }
    }

    private void TrytoPutOnSpot(GameObject obj)
    {
        if (!spot.IsOccupied())
        {
            obj.transform.SetParent(spot.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            if (obj.TryGetComponent(out ObjectInteractor interactor))
            {
                interactor.SetLocked(true);
            }

            StartCoroutine(StartScanning(interactor));
        }
    }

    private IEnumerator StartScanning(ObjectInteractor interactor)
    {
        Debug.Log("Starting scan.");

        animator.SetBool("isScanning", true);

        scanUI.SetActive(false);

        interactor.SetLocked(true);

        yield return new WaitForSeconds(scanDuration); // Simulate scanning time
        Debug.Log("Scan complete.");

        animator.SetBool("isScanning", false);

        scanUI.SetActive(true);

        interactor.SetLocked(false);
        interactor.SetScanned(true);
    }
}
