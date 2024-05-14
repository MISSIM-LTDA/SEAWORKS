using UnityEngine;
using UnityEngine.UI;

public class CamerasControl : MonoBehaviour
{
    [SerializeField] private GameObject[] cameras;
    public GameObject actualCamera;

    private Toggle[] camerasToggles;
    private Toggle[] auxCamerasToggles;

    public GameObject auxCamera;
    private RenderTexture auxCameraRenderTexture;
    private void Start()
    {
        camerasToggles = transform.Find("Cameras Toggles").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < camerasToggles.Length; i++){
            int j = i;
            camerasToggles[i].onValueChanged.AddListener(delegate {
                ChangeCameras(camerasToggles[j].isOn, j, cameras[j]);
            });
        }

        cameras[0].SetActive(true);
        actualCamera = cameras[0];
        camerasToggles[0].isOn = true;

        auxCamerasToggles = transform.Find("Aux Cameras Toggles").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < auxCamerasToggles.Length; i++){
            int j = i;
            int k = j + 1;
            auxCamerasToggles[i].onValueChanged.AddListener(delegate {
                AuxCameras(auxCamerasToggles[j].isOn, j, cameras[k]);
            });
        }

        auxCameraRenderTexture = Resources.Load<RenderTexture>("Extra Cam Render Texture");
    }
    public void ChangeCameras(bool change,int index,GameObject camera) 
    {
        if (actualCamera == camera){
            camerasToggles[index].isOn = true;
        }

        else if (change){
            camera.SetActive(true);
            actualCamera = camera;

            for (int i = 0; i < camerasToggles.Length; i++){
                if (i != index) {
                    camerasToggles[i].isOn = false;
                    cameras[i].SetActive(false);
                }
            }
        }
    }
    public void AuxCameras(bool change, int index, GameObject camera)
    {  
        if (change) 
        {
            for(int i=0;i<auxCamerasToggles.Length;i++) {
                if (i != index) { auxCamerasToggles[i].isOn = false; }
            }

            auxCamera = Instantiate(camera);
            auxCamera.transform.SetParent(camera.transform.parent);
            auxCamera.transform.SetPositionAndRotation(camera.transform.position, camera.transform.rotation);

            auxCamera.GetComponent<Camera>().targetTexture = auxCameraRenderTexture;

            auxCamera.SetActive(true);
        }

        else {
            Destroy(auxCamera);
        }
    }
}
