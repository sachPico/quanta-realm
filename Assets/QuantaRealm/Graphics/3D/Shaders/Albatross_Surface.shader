Shader "Custom/Albatross_Surface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BottomLimit("Laser Bottom Limit Position", Vector) = (0,0,0,0)
        _LaserForwardDirection("Laser Forward Direction", Vector) = (0,0,0,0)
        _LaserUpDirection("Laser Up Direction", Vector) = (0,0,0,0)
        _FirstHitInfo("First Hit Info", Vector) = (0,0,0,0)
        _CutOut("Cutout", Range(0,1)) = 0.5
        _IsLineRenderer("Is Line Renderer", Range(0,1)) = 0
    }
    SubShader
    {
        // Tags { "RenderType"="Transparent" }
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        CGPROGRAM
        #include "UnityCG.cginc"
        // #pragma shader_feature
        #pragma surface surf Standard fullforwardshadows alphatest:_CutOut
        #pragma target 5.0

        // StructuredBuffer<float4> hitInfoBuffer;
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };
        struct HitInfo
        {
            float4 hitPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        int numOfRayCasts;
        float laserWidth;
        static const float EPSILON = 1e-8;
        float4 bottomLimit;
        float4 _BottomLimit;
        float4 _LaserForwardDirection;
        float4 _LaserUpDirection;
        float4 _FirstHitInfo;
        float4 _HitInfos[16];
        int _IsLineRenderer;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        bool isInHollowArea(float3 wPos)
        {
            float lengthA, lengthB, lengthP;
            float widthP = dot(wPos.xyz - _BottomLimit.xyz, _LaserUpDirection.xyz);
            float regionRatio = widthP / laserWidth * numOfRayCasts;
            int areaIndex = (int) regionRatio;
            int upOrDown = (int)((regionRatio-areaIndex)/0.5);

            lengthA = dot(_HitInfos[areaIndex].xyz - _BottomLimit.xyz, _LaserForwardDirection.xyz);
            lengthP = dot(wPos.xyz - _BottomLimit.xyz, _LaserForwardDirection.xyz);
            if(_HitInfos[areaIndex].w == 1 && lengthP > lengthA)// || lengthP > lengthB)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            // IN.wpos = mul(unity_ObjectToWorld, IN.vertex).xyz;
            // o.Albedo = c.rgb;
            if(_IsLineRenderer==1)
            {
                if(isInHollowArea(IN.worldPos+(_LaserUpDirection*laserWidth/2)))
                {
                    // o.Alpha = 0;
                    o.Albedo = fixed3(0,0,0);
                    o.Alpha = 0;
                }
                else
                {
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }
            }
            else
            {
                if(isInHollowArea(IN.worldPos))
                {
                    // o.Alpha = 0;
                    o.Albedo = fixed3(0,0,0);
                    o.Alpha = 0;
                }
                else
                {
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }
            }
        }
        ENDCG
    }
    FallBack "Transparent/VertexLit"
}
