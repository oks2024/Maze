float2 HalfPixel;

float3 LightDirection;

float3 Color;

float3 CameraPosition;

// This is used to compute the world position.
float4x4 InvertViewProjection;


// Diffuse color, and specularIntensity in the alpha channel.
texture AlbedoMap;

// Normals, and specularPower in the alpha channel.
texture NormalMap;

// Depth.
texture DepthMap;

sampler albedoSampler = sampler_state
{
    Texture = (AlbedoMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    MipFilter = LINEAR;
};

sampler normalSampler = sampler_state
{
    Texture = (NormalMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    MipFilter = LINEAR;
};

sampler depthSampler = sampler_state
{
    Texture = (DepthMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = POINT;
    MinFilter = POINT;
    MipFilter = POINT;
};

struct VertexShaderInput
{
    float3 Position : POSITION0;

    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = float4(input.Position, 1);
    output.TexCoord = input.TexCoord - HalfPixel;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
   // Get normal data from the normal map.
    float4 normalData = tex2D(normalSampler, input.TexCoord);

    // Transform normal back to [-1, 1] range;
    float3 normal = 2.0f * normalData.xyz - 1.0f;
    
    // Get specularPower, and get it into [0, 255] range.
    float specularPower = normalData.a * 255;

    // Get specularIntensity from the colorMap.
    float specularIntensity = tex2D(albedoSampler, input.TexCoord).a;

    // Read depth.
    float depthVal = tex2D(depthSampler, input.TexCoord).r;

    // Compute screen space poition;
    float4 position;
    position.x = input.TexCoord.x * 2.0f - 1.0f;
    position.y = -(input.TexCoord.y * 2.0f - 1.0f);
    position.z = depthVal;
    position.w = 1.0f;

    // Transform to world space.
    position = mul(position, InvertViewProjection);
    position /= position.w;

    // Surface to light vector.
    float3 lightVector = -normalize(LightDirection);

    // Compute diffuse light.
    float NdL = max(0, dot(normal, lightVector));
    float3 diffuseLight = NdL * Color.rgb;

    // Reflection vector.
    float3 reflectionVector = normalize(reflect(lightVector, normal));

    // Camera to surface vector.
    float3 directionToCamera = normalize(CameraPosition - position);

    // Compute specular light.
    float specularLight = specularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), specularPower);

    return float4(diffuseLight.rgb, specularLight);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
