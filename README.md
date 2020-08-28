# Facepunch.Steamworks
[Another fucking c# Steamworks implementation](https://wiki.facepunch.com/steamworks/)

## Installation:
1. Adding plugin

**Easy way:**
Add 
 `"facepunch.steamworks": "https://github.com/kamyker/Facepunch.Steamworks.git"` 
to 
`<your_project>/Packages/manifest.json` 
dependencies

**Git way:**
Clone repo as submodule to `<your_project>/Packages/Facepunch.Steamworks`

2. Set proper Scription Define Symbols in Unity Player settings:

Win x64:

`PLATFORM_WIN64;PLATFORM_WIN;PLATFORM_64`

Win x32:

`PLATFORM_WIN32;PLATFORM_WIN;PLATFORM_32`

Linux or Mac x32:

`PLATFORM_POSIX32;PLATFORM_POSIX;PLATFORM_32`

Linux or Mac x64:

`PLATFORM_POSIX64;PLATFORM_POSIX;PLATFORM_64`

If you are using Mac or Linux and build isn't working check libraries unity settings in `Packages\Facepunch.Steamworks\UnityPlugin\redistributable_bin`

## More

More in main repo: https://github.com/Facepunch/Facepunch.Steamworks
