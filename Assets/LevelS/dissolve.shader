Shader "Unlit/dissolve" {
        Properties{
            _MainTex("Texture", 2D) = "white" {}
            _Color("Color", Color) = (1,1,1,1)
            _Distance("Distance", Range(0,100)) = 10
            _DissolveSpeed("Dissolve Speed", Range(0,1)) = 0.5
            _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
            _FogDensity("Fog Density", Range(0,1)) = 0.01
        }

            SubShader{
                Tags { "RenderType" = "Opaque" }

                Pass {
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include "UnityCG.cginc"

                    struct appdata {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                    };

                    struct v2f {
                        float4 vertex : SV_POSITION;
                        float2 uv : TEXCOORD0;
                        float4 worldPos : TEXCOORD1;
                        float3 viewDir : TEXCOORD2;
                    };

                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    float4 _Color;
                    float _Distance;
                    float _DissolveSpeed;
                    float4 _FogColor;
                    float _FogDensity;

                    v2f vert(appdata v) {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                        o.viewDir = UnityWorldSpaceViewDir(o.worldPos);
                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target {
                        // Calculate the distance from the camera to the object
                        float distance = length(i.worldPos - _WorldSpaceCameraPos);

                    // Calculate the dissolve factor based on the distance
                    float dissolveFactor = saturate((distance - _Distance) / _DissolveSpeed);

                    // Blend the texture and color based on the dissolve factor
                    fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;
                    fixed4 dissolveColor = lerp(texColor, _Color, dissolveFactor);

                    // Apply distance-based fog
                    float fogFactor = saturate(distance * _FogDensity);
                    dissolveColor.rgb = lerp(dissolveColor.rgb, _FogColor.rgb, fogFactor);

                    return dissolveColor;
                }
                ENDCG
            }
            }
                FallBack "Diffuse"
    }
