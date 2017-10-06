# TexPax
Texture packer for making composite images (combined gray-scale images)

[Download current release for Windows (255kb)](https://raw.github.com/ChrisPikul510/texpax/master/dist/TexPax.exe)

## What is this?
It's a tiny texture packer (not spritesheet or atlas). It builds composite textures from input images. I use this for combining texture maps that come with PBR Materials. When you bake, or download them, usually there's a bunch of different gray-scale images. So I use this to combine the common maps like Metallic, Roughness, Ambient Occlusion into one combined texture. This way, instead of loading in 3+ textures separately you're only using 1. It simply maps each gray-scale image to a specific color channel (R/G/B/A).

As an added bonus, you can also combine diffuse/albedo with a separate alpha mask if needed. Or adding the height/displacement map to the alpha channel of your normal map.

![Alt text](/images/example.png?raw=true "Example")

## How does it work?
![Alt text](/images/tutorial.png?raw=true "Explanation")

You can click on the image icon for the given input type (Diffuse, Metallic, AO, etc.) to load in a texture. Or even easier and better, you can drag and drop them onto their respective slot. From there you can choose how to calculate the value if you are combining the Metallic, Roughness, Specular, and Ambien Occlusion maps. The default option is "A" for average, which combines the RGBA channels and get's an average from it. Or you can cycle through to only pick one color component channel. Next button is the output channel. Once we have the input value, this button maps it to the desired color component channel.

Each input slot as 2 buttons to the left of it. The top of the two is a input fill. This is used if you don't have a texture to load. Each slot type is a bit different. Cross icon means let the program handle it if needed, but you are hinting you don't want this. If each component slot of the output image has crosses with no texture it won't output that file at all. The other options for this tool is fill white, or fill black. When I say let the program handle it, I mean if something is needed to fill the composite map it'll pick a sane value for this. For alpha channel it chooses white (non-transparent), for normal map it picks blue (Z full).

The second of the 2 buttons is the inversion mode button. When active, it'll invert the input value. This only affects texture input, so if there isn't a texture but fill mode is on, it won't have an effect. __On normal map, this inversion works differently__, instead of inverting the whole image (fairly useless with normal maps) it will instead only invert the __green__ channel. I chose this because Blender outputs normal maps with a flipped green from what Unreal Engine uses. For everything else it works as expected.

On the far right of the app is a couple text input boxes. These are the suffix that'll be applied for the given output. The text input at the bottom left (labeled "Output Prefix") is as labeled, the file prefix. So combined they generate the file names. With the default prefix "Texture" and the default composite suffix "\_COMP" results in the output file "Texture_COMP".

The other settings on the bottom are things like output format, and the texture size. __Note, the texture size is set automatically__ when a texture file is loaded up. I currently __assume that your textures are the same size__, and don't check when you load them in. So if they aren't, I have no idea what would happen, but I assume the universe will impload on itself ending all life as we know it. As soon as the textures loaded the inputs become read-only with the size displayed.

The other buttons are self explanitory.
