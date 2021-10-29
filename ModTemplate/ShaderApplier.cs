using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VespersAssortedOuterWildsShaders
{
    class ShaderApplier : MonoBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture customTexture;
        bool running;

        public void Ready(RenderTexture cameraTexture, RenderTexture customTexture)
        {
            this.cameraTexture = cameraTexture;
            this.customTexture = customTexture;
            running = true;
        }
        private void Update()
        {
            if (!running || Keyboard.current["h"].IsPressed()) { return; }

            if (MainBehaviour.Instance.ASCIIShaderOn)
            {
                MainBehaviour.Instance.ASCIIShader.SetTexture(0, "cameraTexture", cameraTexture);
                MainBehaviour.Instance.ASCIIShader.SetTexture(0, "asciiTexture", customTexture);
                MainBehaviour.Instance.ASCIIShader.SetFloat("scaleFactor", MainBehaviour.Instance.ASCIIScaleFactor);
                MainBehaviour.Instance.ASCIIShader.SetFloat("backBrightness", 0.1f);
                MainBehaviour.Instance.ASCIIShader.Dispatch(0, 1920 / (8 * 8), 1100 / (8 * 8), 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }
            if (MainBehaviour.Instance.PixelShaderOn)
            {
                MainBehaviour.Instance.PixelShader.SetTexture(0, "cameraTexture", cameraTexture);
                MainBehaviour.Instance.PixelShader.SetTexture(0, "pixelTexture", customTexture);
                MainBehaviour.Instance.PixelShader.SetFloat("scaleFactor", MainBehaviour.Instance.PixelScaleFactor);
                MainBehaviour.Instance.PixelShader.SetBool("noBlend", true);
                MainBehaviour.Instance.PixelShader.Dispatch(0, 1920 / (int)Math.Min(MainBehaviour.Instance.PixelScaleFactor * MainBehaviour.Instance.PixelScaleFactor, 8 * 8),
                    1100 / (int)Math.Min(MainBehaviour.Instance.PixelScaleFactor * MainBehaviour.Instance.PixelScaleFactor, 8 * 8), 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }
            if (MainBehaviour.Instance.OilShaderOn)
            {
                MainBehaviour.Instance.OilShader.SetTexture(0, "inputTexture", cameraTexture);
                MainBehaviour.Instance.OilShader.SetTexture(0, "outputTexture", customTexture);
                MainBehaviour.Instance.OilShader.SetInt("radius", MainBehaviour.Instance.OilRadius);
                MainBehaviour.Instance.OilShader.SetInt("intensity", MainBehaviour.Instance.OilIntensity);
                MainBehaviour.Instance.OilShader.Dispatch(0, 1920 / 32, 1100 / 32, 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }
            if (MainBehaviour.Instance.EdgeDetectionShaderOn)
            {
                MainBehaviour.Instance.EdgeDetectionShader.SetTexture(0, "inputTexture", cameraTexture);
                MainBehaviour.Instance.EdgeDetectionShader.SetTexture(0, "outputTexture", customTexture);
                MainBehaviour.Instance.EdgeDetectionShader.SetInt("radius", MainBehaviour.Instance.EdgeDetectionRadius);
                MainBehaviour.Instance.EdgeDetectionShader.Dispatch(0, 1920 / 32, 1100 / 32, 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }
            if (MainBehaviour.Instance.InvertShaderOn)
            {
                MainBehaviour.Instance.InvertShader.SetTexture(0, "inputTexture", cameraTexture);
                MainBehaviour.Instance.InvertShader.SetTexture(0, "outputTexture", customTexture);
                MainBehaviour.Instance.InvertShader.Dispatch(0, 1920 / 32, 1100 / 32, 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }
            if (MainBehaviour.Instance.SynthWaveShaderOn)
            {
                MainBehaviour.Instance.SynthWaveShader.SetTexture(0, "inputTexture", cameraTexture);
                MainBehaviour.Instance.SynthWaveShader.SetTexture(0, "outputTexture", customTexture);
                MainBehaviour.Instance.SynthWaveShader.SetInt("radius", 1);
                MainBehaviour.Instance.SynthWaveShader.SetFloat("edgeSensitivity", 0.1f);
                MainBehaviour.Instance.SynthWaveShader.Dispatch(0, 1920 / 32, 1100 / 32, 1);
                Graphics.CopyTexture(customTexture, cameraTexture);
            }

        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!running)
            {
                return;
            }
            if (Keyboard.current["h"].IsPressed() || MainBehaviour.Instance.AllShadersOff)
            {
                Graphics.Blit(cameraTexture, destination);
            }
            else
            {
                Graphics.Blit(customTexture, destination);
            }

        }
    }
}
