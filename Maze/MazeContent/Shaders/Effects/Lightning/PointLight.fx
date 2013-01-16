float4x4 World;
float4x4 View;
float4x4 Projection;

float2 HalfPixel;
float3 Color;

// Position of the camera, used for specular light.
float3 CameraPosition;

// Matrix used to compute world position.
float4x4 InvertViewProjection;

float3 LightPosition;

float LightRadius;

float LightIntensity;

// Diffuse color, specular intensity in the alpha channel.
texture AlbedoMap;

// Normal map, specular power in the alpha channel.
texture NormalMap;

// Depth;
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
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
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
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(float4(input.Position, 1.0f), World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.ScreenPosition = output.Position;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // Obtain screen position.
	input.ScreenPosition.xy /= input.ScreenPosition.w;

	// Obtain texture coordinates corresponding to the current pixel.
	// The screen coordinates are in [-1, 1]*[1, -1].
	// The texture coordinates need to be in [0, 1]*[0, 1].
	float2 texCoord = 0.5f * (float2(input.ScreenPosition.x, -input.ScreenPosition.y) + 1);

	// Align texels to pixels
	texCoord -= HalfPixel;

	// Get normals data.
	float4 normalData = tex2D(normalSampler, texCoord);

	// Transform normal back to [0, 1].
	float3 normal = 2.0f * normalData.xyz - 1.0f;

	// Get specular power from the normal data.
	float specularPower = normalData.a * 255;

	// Get specular intensity from Albedo.
	float specularIntensity = tex2D(albedoSampler, texCoord).a;
	
	// Get Depth.
	float depth = tex2D(depthSampler, texCoord).r;

	// Compute screen space position.
	float4 position;
	position.xy = input.ScreenPosition.xy;
	position.z = depth;
	position.w = 1.0;

	// Transform to world space.
	position = mul(position, InvertViewProjection);
	position /= position.w;

	// Surface to light vector.
	float3 lightVector = LightPosition - position;

	// Compute attenuation based on distance (linear).
	float attenuation = saturate(1.0f - length(lightVector)/LightRadius);

	// Normalize for next operations.
	lightVector = normalize(lightVector);

	// Compute diffuse light.
	float NdL = max(0, dot(normal, lightVector));
	
	float3 diffuseLight = NdL * Color.rgb;

	// Reflection vector.
	float3 reflectionVector = normalize(reflect(-lightVector, normal));

	// Camera to surface vector.
	float3 directionToCamera = normalize(CameraPosition - position);

	// Compute specular light.
	float specularLight = specularIntensity * pow(saturate(dot(reflectionVector, directionToCamera)), specularPower);

	// Return value, take account of light intensity and attenuation.
	return attenuation * LightIntensity * float4(diffuseLight.rgb, specularLight);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
