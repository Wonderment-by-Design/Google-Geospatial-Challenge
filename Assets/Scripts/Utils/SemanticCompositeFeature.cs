using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SemanticCompositeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        public Material SemanticMaterial;
        public Material CompositeMaterial;

        [Tooltip("Semantic texture width")]
        public int TextureWidth = 256;
        [Tooltip("Semantic texture height")]
        public int TextureHeight = 144;

        [Range(0.1f, 1f)]
        public float BlurDownscale = 0.25f;
        [Range(0f, 1f)]
        public float BlurAmount = 1f;
        [Range(1f, 4f)]
        public float BlurPasses = 2f;

        [Range(0f, 1f)]
        public float AccumulateAmount = 0.5f;

        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    class CustomRenderPass : ScriptableRenderPass
    {
        private CustomPassSettings _passSettings;

        private Material _semanticMaterial;
        private Material _compositeMaterial;

        private RenderTexture _semanticPostRT;
        private RenderTexture _semanticPreviousRT;

        private RenderTargetIdentifier _colorBuffer, _compositeTempBuffer;

        private int _gpuTexturePreID = Shader.PropertyToID("_GPUTexturePre");
        private int _semanticTexturePreID = Shader.PropertyToID("_SemanticTexturePre");
        private int _semanticTexturePostID = Shader.PropertyToID("_SemanticTexturePost");
        private int _compositeTempBufferID = Shader.PropertyToID("_CompositeTempBuffer");

        public CustomRenderPass(CustomPassSettings passSettings)
        {
            _passSettings = passSettings;

            _semanticMaterial = passSettings.SemanticMaterial;
            _compositeMaterial = passSettings.CompositeMaterial;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Semantic Setup
            _semanticPostRT = RenderTexture.GetTemporary(_passSettings.TextureWidth, _passSettings.TextureHeight, 0);
            _semanticPreviousRT = RenderTexture.GetTemporary(_passSettings.TextureWidth, _passSettings.TextureHeight, 0);

            // Camera
            _colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;

            // Composite Setup
            RenderTextureDescriptor compositeDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(_compositeTempBufferID, compositeDescriptor, FilterMode.Bilinear);
            _compositeTempBuffer = new RenderTargetIdentifier(_compositeTempBufferID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_passSettings != null)
            {
                CommandBuffer cmd = CommandBufferPool.Get();

                // SEMANTIC POST EFFECT
                if (_semanticMaterial && Shader.GetGlobalTexture(_semanticTexturePreID))
                {
                    RenderTexture _semanticTemp1RT = RenderTexture.GetTemporary(_passSettings.TextureWidth, _passSettings.TextureHeight, 0);
                    RenderTexture _semanticTemp2RT = RenderTexture.GetTemporary(_passSettings.TextureWidth, _passSettings.TextureHeight, 0);
                    RenderTexture _semanticBlurRT = RenderTexture.GetTemporary(_passSettings.TextureWidth, _passSettings.TextureHeight, 0);

                    // Pass 0 Horizontal Blur
                    // Pass 1 Vertical Blur
                    int w = Mathf.FloorToInt((float)_passSettings.TextureWidth / _passSettings.BlurDownscale);
                    int h = Mathf.FloorToInt((float)_passSettings.TextureHeight / _passSettings.BlurDownscale);

                    Vector2 horizontal = new Vector2(1f / w, 0f);
                    Vector2 vertical = new Vector2(0f, 1f / h);

                    _semanticMaterial.SetVector("_Horizontal", horizontal);
                    Blit(cmd, Shader.GetGlobalTexture(_semanticTexturePreID), _semanticTemp1RT, _semanticMaterial, 0);
                    _semanticMaterial.SetVector("_Vertical", vertical);
                    Blit(cmd, _semanticTemp1RT, _semanticTemp2RT, _semanticMaterial, 1);
                    _semanticTemp1RT.DiscardContents();

                    if(_passSettings.BlurPasses > 1)
                    {
                        for (int i = 1; i < _passSettings.BlurPasses; i++)
                        {
                            _semanticMaterial.SetVector("_Horizontal", horizontal);
                            Blit(cmd, _semanticTemp2RT, _semanticTemp1RT, _semanticMaterial, 0);
                            _semanticTemp2RT.DiscardContents();

                            _semanticMaterial.SetVector("_Vertical", vertical);
                            Blit(cmd, _semanticTemp1RT, _semanticTemp2RT, _semanticMaterial, 1);
                            _semanticTemp1RT.DiscardContents();
                        }
                    }

                    Blit(cmd, _semanticTemp2RT, _semanticBlurRT);

                    // Pass 2 Accumulate + Composite 
                    _semanticMaterial.SetTexture("_BlurTex", _semanticBlurRT);
                    _semanticMaterial.SetFloat("_BlurAmount", _passSettings.BlurAmount);
                    _semanticMaterial.SetTexture("_PreviousTex", _semanticPreviousRT);
                    _semanticMaterial.SetFloat("_AccumulateAmount", _passSettings.AccumulateAmount);

                    Blit(cmd, Shader.GetGlobalTexture(_semanticTexturePreID), _semanticPostRT, _semanticMaterial, 2);
                    Blit(cmd, _semanticPostRT, _semanticPreviousRT);

                    // Release temp RT's
                    RenderTexture.ReleaseTemporary(_semanticTemp1RT);
                    RenderTexture.ReleaseTemporary(_semanticTemp2RT);
                    RenderTexture.ReleaseTemporary(_semanticBlurRT);
                }

                // COMPOSITE EFFECT
                if (_compositeMaterial && Shader.GetGlobalTexture(_gpuTexturePreID))
                {
                    _compositeMaterial.SetTexture(_semanticTexturePostID, _semanticPostRT);

                    Blit(cmd, _colorBuffer, _compositeTempBuffer, _compositeMaterial);
                    Blit(cmd, _compositeTempBuffer, _colorBuffer);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new ArgumentNullException("cmd");

            RenderTexture.ReleaseTemporary(_semanticPostRT);
            RenderTexture.ReleaseTemporary(_semanticPreviousRT);
            cmd.ReleaseTemporaryRT(_compositeTempBufferID);
        }
    }

    public CustomPassSettings PassSettings = new CustomPassSettings();

    private CustomRenderPass _customRenderPass;

    public override void Create()
    {
        _customRenderPass = new CustomRenderPass(PassSettings);

        _customRenderPass.renderPassEvent = PassSettings.RenderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_customRenderPass);
    }
}


