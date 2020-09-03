Shader "Custom/Albatross_Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BottomLimit("Laser Bottom Limit Position", Vector) = (0,0,0,0)
        _LaserForwardDirection("Laser Forward Direction", Vector) = (0,0,0,0)
        _LaserUpDirection("Laser Up Direction", Vector) = (0,0,0,0)
        _FirstHitInfo("First Hit Info", Vector) = (0,0,0,0)
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag alpha
            // make fog work
            #pragma multi_compile_fog
            #pragma target 5.0
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 wpos : POSITION1;
            };

            sampler2D _MainTex;

            int numOfRayCasts;
            float laserWidth;
            static const float EPSILON = 1e-8;
            float4 bottomLimit;
            float4 _MainTex_ST;
            float4 _BottomLimit;
            float4 _LaserForwardDirection;
            float4 _LaserUpDirection;
            float4 _FirstHitInfo;
            float4 _HitInfos[4];

            bool isInHollowArea(float3 wPos)
            {
                float lengthA, lengthB, lengthP;
                float widthP = dot(wPos.xyz - _BottomLimit.xyz, normalize(_LaserUpDirection.xyz));
                int areaIndex = widthP / laserWidth * numOfRayCasts;
                lengthA = dot(_HitInfos[areaIndex].xyz - _BottomLimit.xyz, normalize(_LaserForwardDirection.xyz));
                // lengthB = dot(_HitInfos[areaIndex+1].xyz - _BottomLimit.xyz, normalize(_LaserForwardDirection.xyz));
                lengthP = dot(wPos.xyz - _BottomLimit.xyz, normalize(_LaserForwardDirection.xyz));
                if(_HitInfos[areaIndex].w == 1 && lengthP > lengthA)// || lengthP > lengthB)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                    if(isInHollowArea(i.wpos))
                    {
                        col = fixed4(0,0,0,0);//tex2D(_MainTex, i.uv)*0;
                    }
                    else
                    {
                        col = tex2D(_MainTex, i.uv);
                    }
                // if(i.wpos.x > 550)
                // {
                //     col = fixed4(0,0,0,0);
                // }
                // else
                // {
                //     col = tex2D(_MainTex, i.uv);
                // }
                return col;
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
            }
            ENDCG
        }
    }
}
