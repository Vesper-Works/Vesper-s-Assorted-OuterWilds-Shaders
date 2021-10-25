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
                MainBehaviour.Instance.ASCIIShader.SetFloat("backBrightness", MainBehaviour.Instance.ASCIIBackBrightness);
                MainBehaviour.Instance.ASCIIShader.Dispatch(0, 1920 / (8 * 8), 1100 / (8 * 8), 1);
            }
            if (MainBehaviour.Instance.PixelShaderOn)
            {
                MainBehaviour.Instance.PixelShader.SetTexture(0, "cameraTexture", cameraTexture);
                MainBehaviour.Instance.PixelShader.SetTexture(0, "pixelTexture", customTexture);
                MainBehaviour.Instance.PixelShader.SetFloat("scaleFactor", MainBehaviour.Instance.PixelScaleFactor);
                MainBehaviour.Instance.PixelShader.SetBool("noBlend", MainBehaviour.Instance.PixelNoBlend);
                MainBehaviour.Instance.PixelShader.Dispatch(0, 1920 / (int)Math.Min(MainBehaviour.Instance.PixelScaleFactor * MainBehaviour.Instance.PixelScaleFactor, 8 * 8),
                    1100 / (int)Math.Min(MainBehaviour.Instance.PixelScaleFactor * MainBehaviour.Instance.PixelScaleFactor, 8 * 8), 1);
            }
            if (MainBehaviour.Instance.OilShaderOn)
            {
                MainBehaviour.Instance.OilShader.SetTexture(0, "inputTexture", cameraTexture);
                MainBehaviour.Instance.OilShader.SetTexture(0, "outputTexture", customTexture);
                MainBehaviour.Instance.OilShader.SetInt("radius", MainBehaviour.Instance.OilRadius);
                MainBehaviour.Instance.OilShader.SetInt("intensity", MainBehaviour.Instance.OilIntensity);
                MainBehaviour.Instance.OilShader.Dispatch(0, 1920 / 32, 1100 / 32, 1);
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
