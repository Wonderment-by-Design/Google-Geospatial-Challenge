Shader "Custom/ARCoreCustomBackground"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _EnvironmentDepth("Texture", 2D) = "black" {}
        _Semantic("Texture", 2D) = "black" {}
    }

    SubShader
    {
        Name "ARCore Background (Before Opaques)"
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
            "ForceNoShadowCasting" = "True"
        }

        Pass
        {
            Name "AR Camera Background (ARCore)"
            Cull Off
            ZTest Always
            ZWrite On
            Lighting Off
            LOD 100
            Tags
            {
                "LightMode" = "Always"
            }

            GLSLPROGRAM

            #pragma multi_compile_local __ ARCORE_ENVIRONMENT_DEPTH_ENABLED

            #pragma only_renderers gles3

            #include "UnityCG.glslinc"

#ifdef SHADER_API_GLES3
#extension GL_OES_EGL_image_external_essl3 : require
#endif // SHADER_API_GLES3

            // Device display transform is provided by the AR Foundation camera background renderer.
            uniform mat4 _UnityDisplayTransform;

#ifdef VERTEX
            varying vec2 textureCoord;
            //varying vec2 textureCoord2;

            void main()
            {
#ifdef SHADER_API_GLES3
                // Transform the position from object space to clip space.
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

                // Remap the texture coordinates based on the device rotation.
                textureCoord = (_UnityDisplayTransform * vec4(gl_MultiTexCoord0.x, 1.0f - gl_MultiTexCoord0.y, 1.0f, 0.0f)).xy;

#endif // SHADER_API_GLES3
            }
#endif // VERTEX

#ifdef FRAGMENT
            varying vec2 textureCoord;
            uniform samplerExternalOES _MainTex;
            uniform float _UnityCameraForwardScale;

            uniform sampler2D _Semantic;

#ifdef ARCORE_ENVIRONMENT_DEPTH_ENABLED
            uniform sampler2D _EnvironmentDepth;
#endif // ARCORE_ENVIRONMENT_DEPTH_ENABLED

#if defined(SHADER_API_GLES3) && !defined(UNITY_COLORSPACE_GAMMA)
            float GammaToLinearSpaceExact (float value)
            {
                if (value <= 0.04045F)
                    return value / 12.92F;
                else if (value < 1.0F)
                    return pow((value + 0.055F)/1.055F, 2.4F);
                else
                    return pow(value, 2.2F);
            }

            vec3 GammaToLinearSpace (vec3 sRGB)
            {
                // Approximate version from http://chilliant.blogspot.com.au/2012/08/srgb-approximations-for-hlsl.html?m=1
                return sRGB * (sRGB * (sRGB * 0.305306011F + 0.682171111F) + 0.012522878F);

                // Precise version, useful for debugging, but the pow() function is too slow.
                // return vec3(GammaToLinearSpaceExact(sRGB.r), GammaToLinearSpaceExact(sRGB.g), GammaToLinearSpaceExact(sRGB.b));
            }

#endif // SHADER_API_GLES3 && !UNITY_COLORSPACE_GAMMA

            float ConvertDistanceToDepth(float d)
            {
                d = _UnityCameraForwardScale > 0.0 ? _UnityCameraForwardScale * d : d;

                float zBufferParamsW = 1.0 / _ProjectionParams.y;
                float zBufferParamsY = _ProjectionParams.z * zBufferParamsW;
                float zBufferParamsX = 1.0 - zBufferParamsY;
                float zBufferParamsZ = zBufferParamsX * _ProjectionParams.w;

                // Clip any distances smaller than the near clip plane, and compute the depth value from the distance.
                return (d < _ProjectionParams.y) ? 1.0f : ((1.0 / zBufferParamsZ) * ((1.0 / d) - zBufferParamsW));
            }

            void main()
            {
#ifdef SHADER_API_GLES3
                vec3 result = texture(_MainTex, textureCoord).xyz;
                float depth = 1.0;               

#ifdef ARCORE_ENVIRONMENT_DEPTH_ENABLED
                float distance = texture(_EnvironmentDepth, textureCoord).x;
                depth = ConvertDistanceToDepth(distance);
#endif // ARCORE_ENVIRONMENT_DEPTH_ENABLED

                depth = 0.0;

                vec3 semantic = texture(_Semantic, textureCoord).xyz;
                float semanticLabel = round(semantic.r * 255.0);
                if(int(semanticLabel) == 1 || int(semanticLabel) == 4 || int(semanticLabel) == 5 || int(semanticLabel) == 6) // 1 = sky, 4 = road, 5 = sidewalk, 6 = terrain
                {
                    depth = 1.0;
                } 

#ifndef UNITY_COLORSPACE_GAMMA
                result = GammaToLinearSpace(result);
#endif // !UNITY_COLORSPACE_GAMMA

                gl_FragColor = vec4(result, 1.0);
                gl_FragDepth = depth;
#endif // SHADER_API_GLES3
            }

#endif // FRAGMENT
            ENDGLSL
        }
    }

    FallBack Off
}
                
// Semantic labels        
//     case 0: col = _Unlabeled; break;
//     case 1: col = _Sky; break;
//     case 2: col = _Building; break;
//     case 3: col = _Tree; break;
//     case 4: col = _Road; break;
//     case 5: col = _Sidewalk; break;
//     case 6: col = _Terrain; break;
//     case 7: col = _Structure; break;
//     case 8: col = _Object; break;
//     case 9: col = _Vehicle; break;
//     case 10: col = _Person; break;
//     case 11: col = _Water; break;