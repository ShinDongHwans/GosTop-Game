using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float pixelSize = 1;
    public Color outlineColor = Color.black;
    public bool resolutionDependant = false;
    public int doubleResolution = 1024;

    private TextMesh textMesh;
    private MeshRenderer meshRenderer;

    float time;
    void Start() {
        time = Time.time;
        
        textMesh = GetComponent<TextMesh>();
        meshRenderer = GetComponent<MeshRenderer>();

        for(int i = 0; i < 8; i++) {
            GameObject outline = new GameObject("outline", typeof(TextMesh));
            outline.transform.parent = transform;
            outline.transform.localScale = new Vector3(1, 1, 1);
 
            MeshRenderer otherMeshRenderer = outline.GetComponent<MeshRenderer>();
            otherMeshRenderer.material = new Material(meshRenderer.material);
            otherMeshRenderer.receiveShadows = false;
            otherMeshRenderer.sortingLayerID = meshRenderer.sortingLayerID;
            otherMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;

            TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
            other.color = outlineColor;
            other.text = textMesh.text;
            other.alignment = textMesh.alignment;
            other.anchor = textMesh.anchor;
            other.characterSize = textMesh.characterSize;
            other.font = textMesh.font;
            other.fontSize = textMesh.fontSize;
            other.fontStyle = textMesh.fontStyle;
            other.richText = textMesh.richText;
            other.tabSize = textMesh.tabSize;
            other.lineSpacing = textMesh.lineSpacing;
            other.offsetZ = textMesh.offsetZ;
        }
    }

    void Update() {
        if(Time.time - time > 0.3f) {
            Destroy(gameObject);
        }
        transform.position += new Vector3(0, 0.01f, 0);
    }

    void LateUpdate() {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
 
        outlineColor.a = textMesh.color.a * textMesh.color.a;
 
        // copy attributes
        for (int i = 0; i < transform.childCount; i++) {
            TextMesh other = transform.GetChild(i).GetComponent<TextMesh>();
            bool doublePixel = resolutionDependant && (Screen.width > doubleResolution || Screen.height > doubleResolution);
            Vector3 pixelOffset = GetOffset(i) * (doublePixel ? 2.0f * pixelSize : pixelSize);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint + pixelOffset);
            other.transform.position = worldPoint;
        }
    }

    Vector3 GetOffset(int i) {
        switch (i % 8) {
            case 0: return new Vector3(0, 1, 0);
            case 1: return new Vector3(1, 1, 0);
            case 2: return new Vector3(1, 0, 0);
            case 3: return new Vector3(1, -1, 0);
            case 4: return new Vector3(0, -1, 0);
            case 5: return new Vector3(-1, -1, 0);
            case 6: return new Vector3(-1, 0, 0);
            case 7: return new Vector3(-1, 1, 0);
            default: return Vector3.zero;
        }
     }

    public const int DODGED = int.MaxValue;

    public static void Create(Vector2Int position, int value) {
        var gameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/Damage"),
            new Vector3(position.x + Random.Range(-0.1f, 0.1f), position.y + 0.4f + Random.Range(-0.15f, 0.15f), -4f),
            new Quaternion());
        var textMesh = gameObject.GetComponent<TextMesh>();
        if(value == DODGED) {
            textMesh.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            textMesh.text = "miss";
        } else if(value > 0) {
            textMesh.color = new Color(0f, 0.8f, 0f, 1f);
            textMesh.text = "+" + value;
        } else if(value < 0) {
            textMesh.color = new Color(1f, 0.2f, 0.2f, 1f);
            textMesh.text = "" + -value;
        }
    }
}
