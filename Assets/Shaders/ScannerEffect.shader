Shader "Hidden/ScannerEffect" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _Radius("Radius", Float) = 1.0
        _ScannerOriginPosition("Scanner Origin Position", Vector) = (0, 0, 0, 0)
        _ScanWidth("Scan Width", Float) = 1.0
        _ScanColor("Scan Color", Color) = (1,1,1,1)
        _OuterRadiusSmoothness("Outer Radius Smoothness", Float) = 2
        _InnerRadiusSmoothness("Inner Radius Smoothness", Float) = 2
    }
	
    SubShader {
        Tags {
			"RenderType"="Opaque"
		}
		
        LOD 100

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
            };

            sampler2D _MainTex;
            float4x4 _InverseViewMatrix;
            sampler2D _CameraDepthTexture;
            float _Radius;
            float4 _ScannerOriginPosition;
            float _ScanWidth;
            float4 _ScanColor;
            float _OuterRadiusSmoothness;
            float _InnerRadiusSmoothness;

            v2f vert (appdata v) {
                v2f o;
				
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
				return o;
            }

            float4 getFragWorldPos(float2 depthUV) {
                //get the eye space depth
                //eye space = view space
                //basically how far a pixel is in unity units
                float linDepth = LinearEyeDepth(tex2D(_CameraDepthTexture, depthUV));

                //grab values from projection matrix
                //the projection matrix transforms view space into projection space based
                //on the camera fov and frustum parameters, we take the values from this 
                //matrix that are responsible for that
                float2 projectionMultipliers = float2(unity_CameraProjection._11, unity_CameraProjection._22);

                //we trasform the uv coordinates form 0 - 1 to -1 - 1
                //then we divide it by the projection parameter we got to invert the projection 
                //of the vertices
                //finally multiply by the depth to obtain the position of the fragment in view space
                float3 vpos = float3((depthUV * 2 - 1) / projectionMultipliers, -1) * linDepth;

                //finally just multiply the view space by the inverse view matrix to get the world
                //space position of the fragment
                return mul(_InverseViewMatrix, float4(vpos, 1));
            }

            float ring(float4 wsPos) {
                float dist = distance(wsPos, _ScannerOriginPosition);

                if(dist < _Radius && dist > _Radius-_ScanWidth) {
                    float diff = 1 - (_Radius - dist) / _ScanWidth;
                    return diff;
                }

                return 0;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float4 scanCol;

                float4 wsPos = getFragWorldPos(i.uv);
                
                scanCol = ring(wsPos);

                return col + scanCol;
            }
            ENDCG
        }
    }
}
