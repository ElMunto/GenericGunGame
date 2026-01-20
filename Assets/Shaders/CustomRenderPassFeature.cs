using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPassFeature : ScriptableRendererFeature
{
    [SerializeField] private Material material;

    class CustomRenderPass : ScriptableRenderPass
    {
        [SerializeField] private Material material;
        private int tempTextureID = -1;
        private RenderTargetIdentifier tempTexture;
        private RenderTargetIdentifier sourceTexture; // CHANGED from RTHandle

        public CustomRenderPass(Material material) : base()
        {
            this.material = material;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // CHANGED: Use cameraColorTarget for compatibility
            sourceTexture = renderingData.cameraData.renderer.cameraColorTarget;
            tempTextureID = Shader.PropertyToID("_TempTexture");
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            cmd.GetTemporaryRT(tempTextureID, descriptor, FilterMode.Bilinear);
            tempTexture = new RenderTargetIdentifier(tempTextureID);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("Full ScreenRender Feature");
            commandBuffer.Blit(sourceTexture, tempTexture, material);
            commandBuffer.Blit(tempTexture, sourceTexture);
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (tempTextureID != -1)
                cmd.ReleaseTemporaryRT(tempTextureID);
        }
    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(this.material);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


