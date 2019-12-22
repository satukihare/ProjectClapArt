using UnityEngine;


public class BlendExpression : MonoBehaviour
{
    private Animator _blendTree;
    private int _expressionIndex;

    [SerializeField, Range(-1f, 1f)]
    public float Blending_x = 0f;
    [SerializeField, Range(-1, 1f)]
    public float Blending_y = 0f;

    float FaceEmotion_X, FaceEmotion_Y;

    float Emotion_Diff_X, Emotion_Diff_Y;

    [SerializeField, Range(0f, 60f)]
    float Diff_Frame;

    [SerializeField, Range(0f, 1f)]
    public float ExpressionWeight = 1f;

    void Start()
    {
        _blendTree = GetComponent<Animator>();

        _expressionIndex = _blendTree.GetLayerIndex("Face");
        FaceEmotion_X = Blending_x;
        FaceEmotion_Y = Blending_y;

    }

    void Update()
    {
        //Fail getting animator.
        if (_blendTree == null)
        {
            return;
        }

        if (!(FaceEmotion_X == Blending_x && FaceEmotion_Y == Blending_y))
        {
            //フレームごとの補間処理
            FaceEmotion_X += Emotion_Diff_X;
            FaceEmotion_Y += Emotion_Diff_Y;
            
        }

        //Setting Blend Param and Weights.
        _blendTree.SetFloat("Emotion_X",Blending_x);
        _blendTree.SetFloat("Emotion_Y",Blending_y);

        if (_expressionIndex != -1)
            _blendTree.SetLayerWeight(_expressionIndex, ExpressionWeight);

    }

    public void ChangeFace(float x ,float y)
    {
        Blending_x = x;
        Blending_y = y;

        Emotion_Diff_X = (  Blending_x - FaceEmotion_X) / Diff_Frame;
        Emotion_Diff_Y = (  Blending_y - FaceEmotion_Y) / Diff_Frame;
    }
}