Shader "Custom/TerrainShaderTry2"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        
        const static int maxColorCount = 8;
        const static float epsilon = 1E-4;

        int baseColorCount;
        float3 baseColors[maxColorCount];
        float baseStartHeights[maxColorCount];
        float baseBlends[maxColorCount];

        float minHeight;
        float maxHeight;

        struct Input
        {
            float3 worldPos;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        //in CGPROGRAM language, you have to define the function before you call it
        float inverseLerp(float a, float b, float value){
            //saturate clamps values to 0-1
            return saturate((value-a)/(b-a));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            o.Albedo = heightPercent;
            
            
            for (int i = 0; i < baseColorCount; i++){
                float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, (heightPercent - baseStartHeights[i]));
                o.Albedo = (o.Albedo*(1-drawStrength)) + (drawStrength*baseColors[i]);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
