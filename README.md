# Facepunch.Steamworks for Unity
[Another fucking c# Steamworks implementation](https://wiki.facepunch.com/steamworks/)

## Installation:
1. Adding plugin

**Easy way:**
In Unity click Window/Package Manager then:

 ![click top left plus icon](https://i.gyazo.com/2dc801f40193f36798812bcaff4ea2ee.png)
 
and copy/paste: `https://github.com/kamyker/Facepunch.Steamworks.Package.git` 

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
