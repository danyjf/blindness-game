using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour {
    [SerializeField]
    private Material scannerMaterial;

    [SerializeField]
    private Transform scannerOrigin;

    [SerializeField]
    private float scanDuration = 3.0f;

    [SerializeField]
    private float scanSpeed = 30.0f;

    private Camera _camera;
    private bool scanning = false;
    private float scannerRadius = 0.0f;

    private void OnEnable() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        if(!scanning && Input.GetKeyDown(KeyCode.Mouse0)) {
            StartCoroutine("Scan");
        }
    }

    IEnumerator Scan() {
        scanning = true;
        scannerRadius = 0;

        yield return new WaitForSeconds(scanDuration);
    
        scanning = false;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if(scanning) {
            scannerMaterial.SetFloat("_UsingScanner", 1);
            scannerMaterial.SetMatrix("_InverseViewMatrix", _camera.worldToCameraMatrix.inverse);
            scannerMaterial.SetVector("_ScannerOriginPosition", scannerOrigin.position);

            scannerRadius += scanSpeed * Time.deltaTime;
            scannerMaterial.SetFloat("_Radius", scannerRadius);

            Graphics.Blit(src, dest, scannerMaterial);
        }else {
            Graphics.Blit(src, dest);
        }
    }
}
