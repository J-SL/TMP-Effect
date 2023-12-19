using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterOperate : MonoBehaviour
{
    private enum OperationMode { VertexColor,VertexJitter,VertexWave};

    //private OperationMode operationMode = OperationMode.VertexColor;

    private TextMeshPro m_TextMeshPro;

    private TMP_TextInfo m_textInfo;

    private TextContainer m_textContainer;

    private struct VertexAnim
    {
        public float angleRange;
        public float angle;
        public float speed;
    }

    private void Awake()
    {
        m_TextMeshPro = GetComponent<TextMeshPro>();
        //m_TextMeshPro.text = "This is a simple test to changing vertex color pre character";
        //settings
        //m_TextMeshPro.enableWordWrapping = true;

        //m_textContainer = gameObject.AddComponent<TextContainer>();
        //m_textContainer.width = 35f;

        m_TextMeshPro.ForceMeshUpdate();
    }

    private void Start()
    {
        if (isRefreshColor)
            StartCoroutine(AnimateVertexColor());
        if(isJitter)
            StartCoroutine(AnimateVertexJitter());
        if(isWave)
            StartCoroutine(AnimateVertexWave());
    }
    #region AnimateVertexColor
    [SerializeField]
    private bool AnimateColor;
    [SerializeField]
    private bool isRefreshColor = true;
    [SerializeField]
    private float refreshColorTime=0.05f;
    [SerializeField]
    [Range(0, 255)]
    private byte transparency=127;
    [SerializeField]
    private bool isSolidColor=true;

    IEnumerator AnimateVertexColor()
    {
        int charaterCount = m_TextMeshPro.textInfo.characterCount;
        int currentCharater = 0;

        Color32[] newVertexColors = m_TextMeshPro.mesh.colors32;
        Color32 c0 = m_TextMeshPro.color;
        c0.a = 255;
        Color32 c1 = c0;

        while(isRefreshColor)
        {
            currentCharater = (currentCharater + 1) % charaterCount;
            int vertexIndex = m_TextMeshPro.textInfo.characterInfo[currentCharater].vertexIndex;

            if (m_TextMeshPro.textInfo.characterInfo[currentCharater].character == 32)
                continue;

            if (currentCharater == 0)
                c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                    (byte)Random.Range(0, 255), transparency);

            c1 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
                    (byte)Random.Range(0, 255), transparency);
            if (isSolidColor)
            {
                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c0;
            }
            else
            {
                newVertexColors[vertexIndex + 0] = c1;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c1;
                newVertexColors[vertexIndex + 3] = c0;
            }     

            m_TextMeshPro.mesh.colors32 = newVertexColors;

            yield return new WaitForSeconds(refreshColorTime);
        }
        if (isRefreshColor == false) yield break;
    }
    #endregion

    #region AnimateJitter
    [SerializeField]
    private bool AnimateJitter;
    [SerializeField]
    private bool isJitter;
    [SerializeField]
    private bool isCustomJitter;
    [SerializeField]
    [Range(0,1)]
    private float customX, customY;
    [SerializeField]
    [Range(0.1f,9.9f)]
    private float rotateExtent=1;

    IEnumerator AnimateVertexJitter()
    {
        int charaterCount = m_TextMeshPro.textInfo.characterCount;
        Matrix4x4 matrix;
        Vector3[] vertices;

        int loopCount = 0;

        VertexAnim[] vertexAnims= new VertexAnim[charaterCount];

        for(int i=0;i< charaterCount; i++)
        {
            vertexAnims[i].angleRange = Random.Range(10f, 25f);
            vertexAnims[i].speed = Random.Range(1f, 3f);
            //vertexAnims[i].speed = 1.5f;
        }

        while(isJitter)
        {
            m_TextMeshPro.ForceMeshUpdate();
            vertices = m_TextMeshPro.mesh.vertices;

            int characterCount = m_TextMeshPro.textInfo.characterCount;

            for(int i=0;i<characterCount;i++)
            {
                VertexAnim vertAnim = vertexAnims[i];
                TMP_CharacterInfo charInfo = m_TextMeshPro.textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int vertexIndex = charInfo.vertexIndex;


                Vector3 offset;
                if (isCustomJitter)
                {
                    Vector2 charRotatePoint = new Vector2(customX+ 
                        (vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2,
                        customY+ (vertices[vertexIndex + 0].y + vertices[vertexIndex + 2].y) / 2);
                    offset = charRotatePoint;
                }
                else
                {
                    Vector2 charMidTopline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2
                          , charInfo.topRight.y);
                    offset = charMidTopline;
                }

                vertices[vertexIndex + 0] += -offset;
                vertices[vertexIndex + 1] += -offset;
                vertices[vertexIndex + 2] += -offset;
                vertices[vertexIndex + 3] += -offset;

                vertAnim.angle = Mathf.SmoothStep(-vertAnim.angleRange, vertAnim.angleRange,
                    Mathf.PingPong(loopCount / 25 * vertAnim.speed, 1f*rotateExtent));

                matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, vertexAnims[i].angle), Vector3.one);

                vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;

                vertexAnims[i] = vertAnim;
            }

            loopCount += 1;

            m_TextMeshPro.mesh.vertices = vertices;

            yield return null;
        }

        if (isJitter == false) yield break;
    }
    #endregion

    #region AnimateVertexWave
    [SerializeField]
    private bool AnimateWave;
    [SerializeField]
    private bool isWave;
    [SerializeField]
    [Range(0, 2)]
    private float height=0.5f;
    IEnumerator AnimateVertexWave()
    {
        Vector3[] vertices;

        int loopCount = 0;

        VertexAnim[] vertexAnims = new VertexAnim[1024];

        for (int i = 0; i < 1024; i++)
        {
            vertexAnims[i].angleRange = Random.Range(10f, 25f);
            vertexAnims[i].speed = Random.Range(1f, 3f);
        }

        while (isWave)
        {
            m_TextMeshPro.ForceMeshUpdate();
            vertices = m_TextMeshPro.mesh.vertices;

            int characterCount = m_TextMeshPro.textInfo.characterCount;

            for (int i = 0; i < characterCount; i++)
            {
                VertexAnim vertAnim = vertexAnims[i];
                TMP_CharacterInfo charInfo = m_TextMeshPro.textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int vertexIndex = charInfo.vertexIndex;

                Vector3 offset = new Vector3(0f, height*Mathf.Sin((loopCount / 25) * vertAnim.speed)
                    * vertAnim.angleRange, 0f);

                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;

                vertexAnims[i] = vertAnim;
            }
            loopCount += 1;

            m_TextMeshPro.mesh.vertices = vertices;

            yield return null;
        }
    }
    #endregion
}
