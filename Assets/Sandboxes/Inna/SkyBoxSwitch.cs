using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    public Material alternateSkybox;
    public Material defaultSkybox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger, switching to alternate skybox.");
            RenderSettings.skybox = alternateSkybox;
            DynamicGI.UpdateEnvironment(); // updates lighting
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger, switching to default skybox.");
            RenderSettings.skybox = defaultSkybox;
            DynamicGI.UpdateEnvironment();
        }
    }
}
