using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using UnityEngine;

namespace VespersAssortedOuterWildsShaders
{
    public class MainBehaviour : ModBehaviour
    {
        RenderTexture cameraTexture;
        RenderTexture customTexture;     
        Camera customCamera;

        public float ASCIIScaleFactor;
        public float ASCIIBackBrightness;

        public float PixelScaleFactor;
        public bool PixelNoBlend;

        public int OilIntensity;
        public int OilRadius;
     
        public int EdgeDetectionRadius;

        public bool PixelShaderOn;
        public bool ASCIIShaderOn;
        public bool OilShaderOn;
        public bool EdgeDetectionShaderOn;
        public bool InvertShaderOn;
        public bool SynthWaveShaderOn;

        public ComputeShader ASCIIShader;
        public ComputeShader PixelShader;
        public ComputeShader OilShader;
        public ComputeShader EdgeDetectionShader;
        public ComputeShader InvertShader;
        public ComputeShader SynthWaveShader;

        public bool AllShadersOff { get => !ASCIIShaderOn && !PixelShaderOn && !OilShaderOn && !EdgeDetectionShaderOn && !InvertShaderOn && !SynthWaveShaderOn; }
        Camera NormalCamera { get => Camera.main; }
        public static MainBehaviour Instance { get; set; }
        private void Start()
        {
            Instance = this;

            //ModHelper.Console.WriteLine("Skipping splash screen...");
            //var titleScreenAnimation = FindObjectOfType<TitleScreenAnimation>();
            //titleScreenAnimation.SetValue("_fadeDuration", 0);
            //titleScreenAnimation.SetValue("_gamepadSplash", false);
            //titleScreenAnimation.SetValue("_introPan", false);
            //titleScreenAnimation.Invoke("FadeInTitleLogo");
            //ModHelper.Console.WriteLine("Done!");

            ModHelper.Events.Scenes.OnCompleteSceneChange += OnCompleteSceneChange;

            Textures();
            ComputeShaders();
            CameraWork();
        }

        private void OnCompleteSceneChange(OWScene oldScene, OWScene newScene)
        {
            CameraWork();
        }

        public override void Configure(IModConfig config)
        {
            PixelShaderOn = config.GetSettingsValue<bool>("Pixel shader");
            ASCIIShaderOn = config.GetSettingsValue<bool>("ASCII shader");
            OilShaderOn = config.GetSettingsValue<bool>("Oil shader");
            EdgeDetectionShaderOn = config.GetSettingsValue<bool>("Edge detection shader");
            InvertShaderOn = config.GetSettingsValue<bool>("Invert shader");
            SynthWaveShaderOn = config.GetSettingsValue<bool>("Synthwave shader");

            //PixelNoBlend = !config.GetSettingsValue<bool>("Pixel blend");
            PixelScaleFactor = config.GetSettingsValue<float>("Pixel scale");
            ASCIIScaleFactor = config.GetSettingsValue<float>("ASCII scale");
            //ASCIIBackBrightness = config.GetSettingsValue<float>("ASCII back brightness");
            OilIntensity = config.GetSettingsValue<int>("Oil intensity");
            OilRadius = config.GetSettingsValue<int>("Oil radius");
            EdgeDetectionRadius = config.GetSettingsValue<int>("Edge detection radius");
        }

        private void Textures()
        {
            ModHelper.Console.WriteLine("Creating textures...");
            customTexture = new RenderTexture(1920, 1080, 0);
            customTexture.enableRandomWrite = true;
            customTexture.filterMode = FilterMode.Point;
            customTexture.Create();

            cameraTexture = new RenderTexture(1920, 1080, 0);
            cameraTexture.enableRandomWrite = true;
            cameraTexture.filterMode = FilterMode.Point;
            cameraTexture.Create();
            ModHelper.Console.WriteLine("Done!");
        }

        //public bool AnyOtherShaderOn(string shaderOn)
        //{
        //    bool otherShaderIsOn = false;
        //    if(nameof(ASCIIShader) != shaderOn)
        //    {
        //        otherShaderIsOn = ASCIIShaderOn || otherShaderIsOn;
        //    }
        //    if(nameof(PixelShader) != shaderOn)
        //    {
        //        otherShaderIsOn = PixelShaderOn || otherShaderIsOn;
        //    }
        //    if(nameof(OilShader) != shaderOn)
        //    {
        //        otherShaderIsOn = OilShaderOn || otherShaderIsOn;
        //    }
        //    if(nameof(EdgeDetectionShader) != shaderOn)
        //    {
        //        otherShaderIsOn = EdgeDetectionShaderOn || otherShaderIsOn;
        //    }
        //    return otherShaderIsOn;
        //}

        private void ComputeShaders()
        {
            ModHelper.Console.WriteLine("Loading compute shaders...");

            var shaderbundle = ModHelper.Assets.LoadBundle("shaders");

            ASCIIShader = shaderbundle.LoadAsset<ComputeShader>("ASCIIShader");
            PixelShader = shaderbundle.LoadAsset<ComputeShader>("PixelShader");
            OilShader = shaderbundle.LoadAsset<ComputeShader>("OilShader");
            EdgeDetectionShader = shaderbundle.LoadAsset<ComputeShader>("EdgeDetectionShader");
            InvertShader = shaderbundle.LoadAsset<ComputeShader>("InvertShader");
            SynthWaveShader = shaderbundle.LoadAsset<ComputeShader>("SynthWaveShader");
            ModHelper.Console.WriteLine("Done!");
        }

        private void CameraWork()
        {
            if (NormalCamera == null) { return; }

            GameObject textureCameraGO = new GameObject();
            customCamera = textureCameraGO.AddComponent<Camera>();
            textureCameraGO.AddComponent<OWCamera>();

            textureCameraGO.transform.SetParent(NormalCamera.transform);
            textureCameraGO.transform.position = Vector3.zero;
            textureCameraGO.transform.rotation = Quaternion.identity;
            customCamera.CopyFrom(NormalCamera);
            customCamera.cullingMask = 1 << 5;
            NormalCamera.targetTexture = cameraTexture;
            ModHelper.Console.WriteLine("Done!");

            AddCustomRenderingToCamera(textureCameraGO);

        }
        private void AddCustomRenderingToCamera(GameObject camera)
        {
            camera.AddComponent<ShaderApplier>().Ready(cameraTexture, customTexture);
        }
    }
}
