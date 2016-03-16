using UnityEngine;

public class GradientBackground : MonoBehaviour {

  public Color topColor = Color.blue;
  public Color bottomColor = Color.white;
  public int gradientLayer = 7;
  private Camera camera;

  public float breakpoint1 = 0.33f;
  public float breakpoint2 = 0.66f;

  void Awake() {
    camera = GetComponent<Camera>();
    gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
    if (!camera) {
      Debug.LogError("Must attach GradientBackground script to the camera");
      return;
    }

    camera.clearFlags = CameraClearFlags.Depth;
    camera.cullingMask = camera.cullingMask & ~(1 << gradientLayer);
    Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
    gradientCam.depth = camera.depth - 1;
    gradientCam.cullingMask = 1 << gradientLayer;

    Mesh mesh = new Mesh();

    float edge1 = 0.577f;
    float edge2 = -0.577f;

    float break1 = edge1 - (edge1 - edge2) * breakpoint1;
    float break2 = edge1 - (edge1 - edge2) * breakpoint2;

    mesh.vertices = new Vector3[8] {
      new Vector3(-1f, edge1, 1f), new Vector3(1f, edge1, 1f),    // 0 1
      new Vector3(-1f, break1, 1f), new Vector3(1f, break1, 1f),    // 2 3
      new Vector3(-1f, break2, 1f), new Vector3(1f, break2, 1f),  // 4 5
      new Vector3(-1f, edge2, 1f), new Vector3(1f, edge2, 1f)   // 6 7
    };

    mesh.colors = new Color[8] { topColor, topColor, topColor, topColor, bottomColor, bottomColor, bottomColor, bottomColor };

    mesh.triangles = new int[18] {
      0, 1, 2,
      1, 3, 2,
      2, 3, 4,
      3, 5, 4,
      4, 5, 6,
      5, 7, 6
    };

    Material mat = new Material(Shader.Find("Vertex Color Only"));
    GameObject gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

    ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
    gradientPlane.GetComponent<Renderer>().material = mat;
    gradientPlane.layer = gradientLayer;
  }

}
