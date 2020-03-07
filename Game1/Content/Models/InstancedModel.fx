//-----------------------------------------------------------------------------
// InstancedModel.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// Camera settings.
//float4x4 World;
float4x4 View;
float4x4 Projection;
float DiffuseLight = 1.0;
float3 CamPos = float3(0, 0, 1);
float3 LightDir = float3(0, -1, 0);



const float ShadeX = 0.8;
const float ShadeY = 1.0;
const float ShadeZ = 0.6;

const float4 IMC1 = float4(1, 0, 0, 0);
const float4 IMC2 = float4(0, 1, 0, 0);
const float4 IMC3 = float4(0, 0, 1, 0);




//const float2 FUp = float2(0,1);
//const float2 FDown = float2(0,-1);
//const float2 FLeft = float2(-1,0);
//const float2 FRight = float2(1,0);


//const float4 IMC4 = float4(1000, 0, 1000, 1);

//texture TTexture;


sampler Sampler = sampler_state
{
	Texture = (Texture);
};


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};


struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Color : COLOR0;
	float Spec : COLOR1;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : NORMAL0;
	float3 CamDir : NORMAL1;

	//float3 lightingResult : NORMAL0;
};


// Vertex shader helper function shared between the two techniques.
VertexShaderOutput VertexShaderCommon(VertexShaderInput input, float4 instancePos:BLENDWEIGHT0)
{
	VertexShaderOutput output;

	// Apply the world and camera matrices to compute the output position.

	float4x4 instanceTransform = float4x4(IMC1, IMC2, IMC3, instancePos);

	float4 worldPosition = mul(input.Position, instanceTransform);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	//float3 worldNormal = mul(input.Normal, instanceTransform);

   //float diffuseAmount = max(-dot(worldNormal, LightDirection1), -dot(worldNormal, LightDirection2));

	float diffuseAmount = (abs(input.Normal.x) * ShadeX) + (abs(input.Normal.y) * ShadeY) + (abs(input.Normal.z) * ShadeZ);



	//float3 lightingResult = saturate((diffuseAmount * DiffuseLight) + AmbientLight);
	float yD = clamp(1 - (abs(CamPos.y - worldPosition.y) / 1500), 0.4, 1);
	float xD = clamp(1 - ((abs(CamPos.x - worldPosition.x) + abs(CamPos.z - worldPosition.z)) / 4000), 0, 1);

	float dif = diffuseAmount * DiffuseLight;

	float D = (yD * yD) * xD;
	output.Spec = D * 0.05;

	yD = clamp(D, 0.6, 1.0);

	output.Color = clamp(dif * yD, 0.4, 1);

	output.Normal = input.Normal;
	output.CamDir = normalize(input.Position.xyz - CamPos);


	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}




// Both techniques share this same pixel shader.
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

	//float4 texel = tex2D(Sampler, input.TextureCoordinate);

	//return float4(saturate(AmbientLightColor + (texel.xyz * DiffuseColor * input.lightingResult )), texel.w);

//* distance(Origin, input.TextureCoordinate

/*
	float4 C = tex2D(Sampler, input.TextureCoordinate);

	float4 U1 = tex2D(Sampler, input.TextureCoordinate + FUp);
	float4 D1 = tex2D(Sampler, input.TextureCoordinate + FDown);

	float4 L1 = tex2D(Sampler, input.TextureCoordinate + FLeft);
	float4 R1 = tex2D(Sampler, input.TextureCoordinate + FRight);

	float4 texel = (((((U1 + D1) * 0.5) + C ) * 0.5) + ((((L1 + R1) * 0.5) + C ) * 0.5)) * 0.5;*/

	//float4 texel = tex2D(Sampler, input.TextureCoordinate);
	//float Rad = max(min(((input.TextureCoordinate.x + input.TextureCoordinate.y)*0.4), 1.2),0.8);

	//float4 TxRad = float4(texel.x * Rad, texel.y * Rad, texel.z * Rad, texel.w);


//float IOAngle=dot(input.Normal,LightDir)

//* dot(LightDir, input.Normal) 
	//float3 r = normalize( input.Normal - LightDir);
	//float dotProduct = -dot(input.CamDir,r);
	//float spec = max(dotProduct*0.5f, 0) * length(input.Color);
	float4 pix = tex2D(Sampler, input.TextureCoordinate);


	float i = (dot(LightDir,input.Normal));
	float r = (dot(input.CamDir, input.Normal));
	float theta = (1 - abs(i - r)) * 0.2;

	float spec = theta * theta + input.Spec;

	//float spec = (1 - abs(i - r)) * max(0, i) / 4;




	//return  float4(pix.x + spec, pix.y + spec, pix.z + spec, pix.w);

	return  float4(pix.x * input.Color.x + spec, pix.y * input.Color.y + spec, pix.z * input.Color.z + spec, pix.w);


}


// Hardware instancing technique.
technique HardwareInstancing
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderCommon();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}


